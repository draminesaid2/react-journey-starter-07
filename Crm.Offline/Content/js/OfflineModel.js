window.Helper.Database.registerTable("Main_NumberingSequence", {
	SequenceName: { type: "string", key: true, computed: false },
	LastNumber: { type: "long" },
	Prefix: { type: "string" },
	Format: { type: "string" },
	Suffix: { type: "string" },
	NextLow: { type: "long" },
	MaxLow: { type: "long" }
});
window.Helper.Database.addIndex("Main_NumberingSequence", ["SequenceName"]);
window.Helper.Database.setTransactionId("Main_Note",
	function (userNote) {
		return new $.Deferred().resolve(userNote.ContactId).promise();
	});
window.Helper.Database.setTransactionId("Main_Note",
	function (userNote) {
		return new $.Deferred().resolve(userNote.Id).promise();
	});
window.Helper.Database.setTransactionId("Main_FileResource",
	function (fileResource) {
		return new $.Deferred().resolve([fileResource.Id, fileResource.ParentId]).promise();
	});
window.Helper.Database.setTransactionId("Main_LinkResource",
	function (linkResource) {
		return new $.Deferred().resolve(linkResource.ParentId).promise();
	});