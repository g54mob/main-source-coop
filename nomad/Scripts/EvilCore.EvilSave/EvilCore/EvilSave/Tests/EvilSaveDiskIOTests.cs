using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSaveDiskIOTests : EvilSaveTestBase
	{
		public override string CategoryName => "Disk I/O";

		public void RunAll()
		{
			RunAllAsync().Forget();
		}

		private async UniTaskVoid RunAllAsync()
		{
			ClearResults();
			await TestSyncSaveLoad();
			await TestAsyncSaveLoad();
			await TestCompression();
			await TestEncryption();
			await TestCompressionAndEncryption();
			await TestJsonFormat();
			await TestMultipleKeys();
			await TestSavePersistsAfterClear();
			LogSummary("Disk I/O");
		}

		private async UniTask TestSyncSaveLoad()
		{
			string text = "test_sync_io";
			CleanupSlot(text);
			EvilSave.ActiveSlot = text;
			EvilSave.Clear(text);
			EvilSave.Save("io.int", 12345);
			EvilSave.Save("io.string", "disk test");
			EvilSave.Save("io.vector", new Vector3(1f, 2f, 3f));
			EvilSave.SaveToDisk(text);
			EvilSave.Clear(text);
			AssertTrue("Sync: cleared", !EvilSave.HasKey("io.int"));
			EvilSave.LoadFromDisk(text);
			Assert("Sync: int restored", 12345, EvilSave.Load("io.int", 0));
			Assert("Sync: string restored", "disk test", EvilSave.Load<string>("io.string"));
			AssertFloatEqual("Sync: vector.x", 1f, EvilSave.Load<Vector3>("io.vector").x);
			CleanupSlot(text);
			await UniTask.CompletedTask;
		}

		private async UniTask TestAsyncSaveLoad()
		{
			string testSlot = "test_async_io";
			CleanupSlot(testSlot);
			EvilSave.ActiveSlot = testSlot;
			EvilSave.Clear(testSlot);
			EvilSave.Save("async.float", 99.99f);
			EvilSave.Save("async.list", new List<int> { 10, 20, 30 });
			await EvilSave.SaveToDiskAsync(testSlot);
			EvilSave.Clear(testSlot);
			await EvilSave.LoadFromDiskAsync(testSlot);
			AssertFloatEqual("Async: float restored", 99.99f, EvilSave.Load("async.float", 0f));
			List<int> list = EvilSave.Load<List<int>>("async.list");
			AssertTrue("Async: list not null", list != null);
			Assert("Async: list.Count", 3, list?.Count ?? 0);
			if (list != null)
			{
				Assert("Async: list[2]", 30, list[2]);
			}
			CleanupSlot(testSlot);
		}

		private async UniTask TestCompression()
		{
			EvilSaveSettings instance = EvilSaveSettings.Instance;
			bool enableCompression = instance.EnableCompression;
			bool enableEncryption = instance.EnableEncryption;
			string text = "test_comp_off";
			string text2 = "test_comp_on";
			CleanupSlot(text);
			CleanupSlot(text2);
			string text3 = new string('A', 10000);
			instance.EnableCompression = false;
			instance.EnableEncryption = false;
			EvilSave.ActiveSlot = text;
			EvilSave.Clear(text);
			EvilSave.Save("comp.data", text3);
			EvilSave.SaveToDisk(text);
			instance.EnableCompression = true;
			instance.EnableEncryption = false;
			EvilSave.ActiveSlot = text2;
			EvilSave.Clear(text2);
			EvilSave.Save("comp.data", text3);
			EvilSave.SaveToDisk(text2);
			string savePath = EvilSave.GetSavePath(text);
			string savePath2 = EvilSave.GetSavePath(text2);
			long num = (File.Exists(savePath) ? new FileInfo(savePath).Length : 0);
			long num2 = (File.Exists(savePath2) ? new FileInfo(savePath2).Length : 0);
			AssertTrue("Compressed < Uncompressed", num2 < num && num2 > 0, $"Uncompressed={num}B, Compressed={num2}B");
			EvilSave.Clear(text2);
			EvilSave.LoadFromDisk(text2);
			string text4 = EvilSave.Load<string>("comp.data");
			Assert("Compression: data restored length", text3.Length, text4?.Length ?? 0);
			instance.EnableCompression = enableCompression;
			instance.EnableEncryption = enableEncryption;
			CleanupSlot(text);
			CleanupSlot(text2);
			await UniTask.CompletedTask;
		}

		private async UniTask TestEncryption()
		{
			EvilSaveSettings instance = EvilSaveSettings.Instance;
			bool enableCompression = instance.EnableCompression;
			bool enableEncryption = instance.EnableEncryption;
			string encryptionPassword = instance.EncryptionPassword;
			string text = "test_encryption";
			CleanupSlot(text);
			instance.EnableCompression = false;
			instance.EnableEncryption = true;
			instance.EncryptionPassword = "TestPassword123!";
			EvilSave.ActiveSlot = text;
			EvilSave.Clear(text);
			EvilSave.Save("enc.secret", "TopSecretData");
			EvilSave.Save("enc.number", 42);
			EvilSave.SaveToDisk(text);
			string savePath = EvilSave.GetSavePath(text);
			AssertTrue("Encryption: file exists", File.Exists(savePath));
			if (File.Exists(savePath))
			{
				byte[] bytes = File.ReadAllBytes(savePath);
				string text2 = Encoding.UTF8.GetString(bytes);
				AssertTrue("Encryption: no plaintext 'EVIL' header", !text2.StartsWith("EVIL"), "Encrypted file should not start with EVIL magic");
				AssertTrue("Encryption: no plaintext 'TopSecretData'", !text2.Contains("TopSecretData"), "Encrypted file should not contain plaintext secret");
			}
			EvilSave.Clear(text);
			EvilSave.LoadFromDisk(text);
			Assert("Encryption: decrypted string", "TopSecretData", EvilSave.Load<string>("enc.secret"));
			Assert("Encryption: decrypted int", 42, EvilSave.Load("enc.number", 0));
			instance.EnableCompression = enableCompression;
			instance.EnableEncryption = enableEncryption;
			instance.EncryptionPassword = encryptionPassword;
			CleanupSlot(text);
			await UniTask.CompletedTask;
		}

		private async UniTask TestCompressionAndEncryption()
		{
			EvilSaveSettings instance = EvilSaveSettings.Instance;
			bool enableCompression = instance.EnableCompression;
			bool enableEncryption = instance.EnableEncryption;
			string encryptionPassword = instance.EncryptionPassword;
			string text = "test_comp_enc";
			CleanupSlot(text);
			instance.EnableCompression = true;
			instance.EnableEncryption = true;
			instance.EncryptionPassword = "CombinedTest!";
			EvilSave.ActiveSlot = text;
			EvilSave.Clear(text);
			List<string> list = new List<string>();
			for (int i = 0; i < 100; i++)
			{
				list.Add($"Item_{i}_" + new string('X', 50));
			}
			EvilSave.Save("compenc.list", list);
			EvilSave.SaveToDisk(text);
			EvilSave.Clear(text);
			EvilSave.LoadFromDisk(text);
			List<string> list2 = EvilSave.Load<List<string>>("compenc.list");
			AssertTrue("Comp+Enc: list not null", list2 != null);
			Assert("Comp+Enc: list count", 100, list2?.Count ?? 0);
			if (list2 != null)
			{
				Assert("Comp+Enc: list[50]", list[50], list2[50]);
			}
			instance.EnableCompression = enableCompression;
			instance.EnableEncryption = enableEncryption;
			instance.EncryptionPassword = encryptionPassword;
			CleanupSlot(text);
			await UniTask.CompletedTask;
		}

		private async UniTask TestJsonFormat()
		{
			EvilSaveSettings instance = EvilSaveSettings.Instance;
			SaveFormat format = instance.Format;
			bool enableCompression = instance.EnableCompression;
			bool enableEncryption = instance.EnableEncryption;
			string text = "test_json_fmt";
			CleanupSlot(text);
			instance.Format = SaveFormat.Json;
			instance.EnableCompression = false;
			instance.EnableEncryption = false;
			EvilSave.ActiveSlot = text;
			EvilSave.Clear(text);
			EvilSave.Save("json.val", 777);
			EvilSave.Save("json.str", "JsonTest");
			EvilSave.SaveToDisk(text);
			EvilSave.Clear(text);
			EvilSave.LoadFromDisk(text);
			Assert("JSON format: int restored", 777, EvilSave.Load("json.val", 0));
			Assert("JSON format: string restored", "JsonTest", EvilSave.Load<string>("json.str"));
			instance.Format = format;
			instance.EnableCompression = enableCompression;
			instance.EnableEncryption = enableEncryption;
			CleanupSlot(text);
			await UniTask.CompletedTask;
		}

		private async UniTask TestMultipleKeys()
		{
			string text = "test_multikey";
			CleanupSlot(text);
			EvilSave.ActiveSlot = text;
			EvilSave.Clear(text);
			for (int i = 0; i < 50; i++)
			{
				EvilSave.Save($"multi.key_{i}", i * 10);
			}
			EvilSave.SaveToDisk(text);
			EvilSave.Clear(text);
			EvilSave.LoadFromDisk(text);
			bool condition = true;
			for (int j = 0; j < 50; j++)
			{
				if (EvilSave.Load($"multi.key_{j}", 0) != j * 10)
				{
					condition = false;
					break;
				}
			}
			AssertTrue("50 keys: all restored correctly", condition);
			string[] keys = EvilSave.GetKeys();
			Assert("50 keys: GetKeys count", 50, keys.Length);
			CleanupSlot(text);
			await UniTask.CompletedTask;
		}

		private async UniTask TestSavePersistsAfterClear()
		{
			string text = "test_persist";
			CleanupSlot(text);
			EvilSave.ActiveSlot = text;
			EvilSave.Clear(text);
			EvilSave.Save("persist.val", 999);
			EvilSave.SaveToDisk(text);
			EvilSave.Clear(text);
			AssertTrue("Persist: cleared in memory", !EvilSave.HasKey("persist.val"));
			EvilSave.LoadFromDisk(text);
			Assert("Persist: restored from disk", 999, EvilSave.Load("persist.val", 0));
			CleanupSlot(text);
			await UniTask.CompletedTask;
		}

		private void CleanupSlot(string slotId)
		{
			string slotDirectory = EvilSave.GetSlotDirectory(slotId);
			if (Directory.Exists(slotDirectory))
			{
				Directory.Delete(slotDirectory, recursive: true);
			}
			EvilSave.Clear(slotId);
		}
	}
}
