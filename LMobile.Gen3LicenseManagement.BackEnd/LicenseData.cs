using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace LMobile.Gen3LicenseManagement {
	public sealed class LicenseData : Gen3LicenseData {
		private DateTime myExpiryDate;
		private int myLicenseCount;
		private string myUserData;
		private readonly ListOfNotNulls<LicenseModule> myModules;
		public LicenseData() {
			myModules = new ListOfNotNulls<LicenseModule>();
		}
		public DateTime ExpiryDate {
			get { return myExpiryDate; }
			set { myExpiryDate = value; }
		}
		public int LicenseCount {
			get { return myLicenseCount; }
			set { myLicenseCount = value; }
		}
		public string UserData {
			get { return myUserData; }
			set { myUserData = value; }
		}
		IList<Gen3LicenseModule> Gen3LicenseData.Modules {
			get { return new WrappedList<Gen3LicenseModule, LicenseModule>(myModules, module => module); }
		}
		public IList<LicenseModule> Modules {
			get { return myModules; }
		}
		public int GetModuleLicenseCount(string moduleID) {
			foreach (var module in myModules) {
				if (module.ModuleID == moduleID) {
					var count = module.LicenseCount;
					if (count.HasValue) return count.Value;
					return this.LicenseCount;
				}
			}
			return 0;
		}
		public bool IsModuleLicensed(string moduleID) {
			foreach (var module in myModules) {
				if (module.ModuleID == moduleID) {
					return true;
				}
			}
			return false;
		}
		internal static void Register(SerializationSchema schema) {
			schema.RegisterEntity<LicenseData>("license",
				(context, entity) => entity.Serialize(context));
		}
		private void Serialize(SerializationContext serialize) {
			serialize.Value("ExpiryDate", ref myExpiryDate);
			serialize.Value("LicenseCount", ref myLicenseCount);
			serialize.OptionalValue("UserData", ref myUserData);
			serialize.Collection("Modules", myModules);
		}
		public void Save(Stream output) {
			this.Save(output, LicenseSchema.Instance.Version);
		}
		public void Save(Stream output, int version) {
			var writer = new PortableBitStreamWriter(output);
			var serializer = new BinarySerializer(LicenseSchema.Instance, version);
			writer.Write(version);
			writer.Write(LZF.Compress(serializer.Serialize(this)));
		}
		public byte[] Save() {
			return this.Save(LicenseSchema.Instance.Version);
		}
		public byte[] Save(int version) {
			using (var output = new MemoryStream()) {
				this.Save(output, version);
				return output.ToArray();
			}
		}
		public string Encrypt(LicenseRequest request) {
			using (var stream = new MemoryStream()) {
				using (var writer = new EncryptedStream(stream, request.GetLicenseDataKey())) {
					this.Save(writer, request.Version);
				}
				return ByteString.ToString(stream.ToArray());
			}
		}

		public string EncryptPortable(string contractNumber, Guid projectGuid, string installationType, int version) {
			using (var stream = new MemoryStream()) {
				using (var writer = new EncryptedStream(stream, GetPortableLicenseDataKey(contractNumber, projectGuid, installationType))) {
					this.Save(writer, version);
				}
				return ByteString.ToString(stream.ToArray());
			}
		}

		internal static byte[] GetPortableLicenseDataKey(string contractNumber, Guid projectGuid, string installationType) {
			var sha = SHA256Managed.Create();
			var contractNumberKey = sha.ComputeHash(Encoding.UTF8.GetBytes(contractNumber));
			var projectGuidKey = sha.ComputeHash(projectGuid.ToByteArray());
			var installationTypeKey = sha.ComputeHash(Encoding.UTF8.GetBytes(installationType));
			var result = new byte[64];
			for (var index = 0; index != 16; ++index) {
				result[index * 2] = contractNumberKey[index];
				result[index * 2 + 1] = projectGuidKey[index];
			}
			for (var index = 16; index != 32; ++index) {
				result[index * 2] = contractNumberKey[index];
				result[index * 2 + 1] = installationTypeKey[index];
			}
			return result;
		}

	}
}
