/// <reference path="../../../../Content/js/NumberingService.js" />
;
(function(numberingService) {

	function calculateNextFormattedNumber(numberingSequence) {
		var d = new $.Deferred();

		var lastNumber = parseInt(numberingSequence.LastNumber);
		var nextLow = parseInt(numberingSequence.NextLow);
		var maxLow = parseInt(numberingSequence.MaxLow);
		var format = numberingSequence.Format;

		if (nextLow >= maxLow) {
			return d.reject("Failed to calculate next formatted number for [" + numberingSequence.SequenceName + "] because NextLow (" + numberingSequence.NextLow + ") >= MaxLow (" + numberingSequence.MaxLow + ")");
		}

		var id = (maxLow * lastNumber + nextLow).toString();
		var result = format.substring(0, format.length - id.length) + id;
		result = (numberingSequence.Prefix != null ? numberingSequence.Prefix : "") + result + (numberingSequence.Suffix != null ? numberingSequence.Suffix : "");
		window.database.Main_NumberingSequence.attachOrGet(numberingSequence);
		numberingSequence.NextLow = nextLow + 1;

		numberingSequence.save()
			.then(function() { d.resolve(result); })
			.fail(d.reject);

		return d.promise();
	};

	function updateNumberingSequence(numberingSequence) {
		return window.database.Main_NumberingSequence.filter(function(x) {
				return x.SequenceName == this.sequenceName;
			}, { sequenceName: numberingSequence.SequenceName })
			.take(1)
			.toArray()
			.pipe(function(results) {
				var result;
				if (results.length === 1) {
					result = window.database.Main_NumberingSequence.attachOrGet(results[0]);
				} else {
					result = window.database.Main_NumberingSequence.add(
					{
						SequenceName: numberingSequence.SequenceName,
						LastNumber: null,
						Prefix: numberingSequence.Prefix,
						Format: numberingSequence.Format,
						Suffix: numberingSequence.Suffix,
						NextLow: 0,
						MaxLow: numberingSequence.MaxLow,
						ItemStatus: window.ko.ItemStatus.Persisted
					});
				}
				if (result.LastNumber === null || result.NextLow > 0) {
					return $.get(window.Helper.Url.resolveUrl("~/NumberingSequence/GetNextHighValue/" + numberingSequence.SequenceName + ".json"))
						.pipe(function(highValue) {
							result.LastNumber = highValue;
							result.NextLow = 0;
						});
				}
				return null;
			});
	}

	numberingService.registerNumberingService = function() {
		var d = new $.Deferred();
		$.get(window.Helper.Url.resolveUrl("~/NumberingSequences.json"))
			.then(function(numberingSequences) {
				window.async.forEachSeries(numberingSequences,
					function(numberingSequence, cb) {
						var isHighLowNumberingSequence = numberingSequence.MaxLow !== null;
						if (!isHighLowNumberingSequence) {
							cb();
						} else {
							updateNumberingSequence(numberingSequence).then(cb).fail(d.reject);	
						}
					},
					function() {
						window.database.saveChanges().then(d.resolve).fail(d.reject);
					});
			})
			.fail(d.resolve);
		return d.promise();
	};

	var baseGetNextFormattedNumber = numberingService.getNextFormattedNumber;
	numberingService.getNextFormattedNumber = function (sequenceName) {
		if (window.Helper.Offline.status === "online") {
			return baseGetNextFormattedNumber(sequenceName);
		}
		var d = new $.Deferred();
		window.database.Main_NumberingSequence
			.find(sequenceName)
			.then(function(numberingSequence) {
				calculateNextFormattedNumber(numberingSequence).then(d.resolve).fail(d.reject);
			})
			.fail(function() {
				baseGetNextFormattedNumber(sequenceName).then(d.resolve).fail(d.reject);
			});
		return d.promise();
	};

})(window.NumberingService);