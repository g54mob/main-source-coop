using System.IO;
using System.Linq;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSaveSlotTests : EvilSaveTestBase
	{
		public override string CategoryName => "Slots & Backup";

		public void RunAll()
		{
			ClearResults();
			TestCreateSlot();
			TestDeleteSlot();
			TestSlotExists();
			TestGetSlotIds();
			TestSlotMetadata();
			TestCopySlot();
			TestSwitchActiveSlot();
			TestBackupCreate();
			TestBackupRestore();
			TestGetSavePath();
			LogSummary("Slots & Backup");
		}

		private void TestCreateSlot()
		{
			CleanupSlot("test_create");
			EvilSave.CreateSlot("test_create", "Test Create Slot");
			AssertTrue("CreateSlot: exists", EvilSave.SlotExists("test_create"));
			SaveSlotMetadata slotMetadata = EvilSave.GetSlotMetadata("test_create");
			AssertTrue("CreateSlot: meta not null", slotMetadata != null);
			if (slotMetadata != null)
			{
				Assert("CreateSlot: meta.slotId", "test_create", slotMetadata.slotId);
				Assert("CreateSlot: meta.displayName", "Test Create Slot", slotMetadata.displayName);
			}
			CleanupSlot("test_create");
		}

		private void TestDeleteSlot()
		{
			CleanupSlot("test_delete");
			EvilSave.CreateSlot("test_delete");
			AssertTrue("DeleteSlot: exists before", EvilSave.SlotExists("test_delete"));
			EvilSave.DeleteSlot("test_delete");
			string slotDirectory = EvilSave.GetSlotDirectory("test_delete");
			AssertTrue("DeleteSlot: dir gone", !Directory.Exists(slotDirectory));
		}

		private void TestSlotExists()
		{
			CleanupSlot("test_exists_yes");
			EvilSave.CreateSlot("test_exists_yes");
			AssertTrue("SlotExists: true for created", EvilSave.SlotExists("test_exists_yes"));
			AssertTrue("SlotExists: false for missing", !EvilSave.SlotExists("test_exists_no_way"));
			CleanupSlot("test_exists_yes");
		}

		private void TestGetSlotIds()
		{
			CleanupSlot("test_ids_a");
			CleanupSlot("test_ids_b");
			EvilSave.CreateSlot("test_ids_a");
			EvilSave.CreateSlot("test_ids_b");
			string[] slotIds = EvilSave.GetSlotIds();
			AssertTrue("GetSlotIds: contains A", slotIds.Contains("test_ids_a"));
			AssertTrue("GetSlotIds: contains B", slotIds.Contains("test_ids_b"));
			CleanupSlot("test_ids_a");
			CleanupSlot("test_ids_b");
		}

		private void TestSlotMetadata()
		{
			CleanupSlot("test_meta");
			EvilSave.CreateSlot("test_meta", "My Save");
			SaveSlotMetadata slotMetadata = EvilSave.GetSlotMetadata("test_meta");
			AssertTrue("Metadata: not null", slotMetadata != null);
			if (slotMetadata != null)
			{
				Assert("Metadata: displayName", "My Save", slotMetadata.displayName);
				Assert("Metadata: version", EvilSaveSettings.Instance.SaveVersion, slotMetadata.saveVersion);
				AssertTrue("Metadata: createdAt not empty", !string.IsNullOrEmpty(slotMetadata.createdAt));
				AssertTrue("Metadata: lastSavedAt not empty", !string.IsNullOrEmpty(slotMetadata.lastSavedAt));
			}
			CleanupSlot("test_meta");
		}

		private void TestCopySlot()
		{
			CleanupSlot("test_copy_src");
			CleanupSlot("test_copy_dst");
			EvilSave.ActiveSlot = "test_copy_src";
			EvilSave.Clear("test_copy_src");
			EvilSave.Save("copy.data", 12345);
			EvilSave.SaveToDisk("test_copy_src");
			EvilSave.CopySlot("test_copy_src", "test_copy_dst");
			AssertTrue("CopySlot: dest exists", EvilSave.SlotExists("test_copy_dst"));
			EvilSave.ActiveSlot = "test_copy_dst";
			EvilSave.LoadFromDisk("test_copy_dst");
			Assert("CopySlot: data preserved", 12345, EvilSave.Load("copy.data", 0));
			CleanupSlot("test_copy_src");
			CleanupSlot("test_copy_dst");
		}

		private void TestSwitchActiveSlot()
		{
			CleanupSlot("test_switch_a");
			CleanupSlot("test_switch_b");
			EvilSave.ActiveSlot = "test_switch_a";
			EvilSave.Clear("test_switch_a");
			EvilSave.Save("slot.val", 111);
			EvilSave.SaveToDisk("test_switch_a");
			EvilSave.ActiveSlot = "test_switch_b";
			EvilSave.Clear("test_switch_b");
			EvilSave.Save("slot.val", 222);
			EvilSave.SaveToDisk("test_switch_b");
			EvilSave.ActiveSlot = "test_switch_a";
			EvilSave.LoadFromDisk("test_switch_a");
			Assert("SwitchSlot: A has 111", 111, EvilSave.Load("slot.val", 0));
			EvilSave.ActiveSlot = "test_switch_b";
			EvilSave.LoadFromDisk("test_switch_b");
			Assert("SwitchSlot: B has 222", 222, EvilSave.Load("slot.val", 0));
			CleanupSlot("test_switch_a");
			CleanupSlot("test_switch_b");
		}

		private void TestBackupCreate()
		{
			CleanupSlot("test_backup");
			EvilSave.ActiveSlot = "test_backup";
			EvilSave.Clear("test_backup");
			EvilSave.Save("backup.val", 100);
			EvilSave.SaveToDisk("test_backup");
			EvilSave.CreateBackup("test_backup");
			string[] backups = EvilSave.GetBackups("test_backup");
			AssertTrue("Backup: at least 1 created", backups.Length >= 1);
			if (backups.Length != 0)
			{
				AssertTrue("Backup: file exists", File.Exists(backups[0]));
			}
			CleanupSlot("test_backup");
		}

		private void TestBackupRestore()
		{
			CleanupSlot("test_restore");
			EvilSave.ActiveSlot = "test_restore";
			EvilSave.Clear("test_restore");
			EvilSave.Save("restore.val", 100);
			EvilSave.SaveToDisk("test_restore");
			EvilSave.CreateBackup("test_restore");
			EvilSave.Save("restore.val", 999);
			EvilSave.SaveToDisk("test_restore");
			Assert("Restore: before = 999", 999, EvilSave.Load("restore.val", 0));
			bool condition = EvilSave.RestoreBackup("test_restore");
			AssertTrue("Restore: success", condition);
			Assert("Restore: value back to 100", 100, EvilSave.Load("restore.val", 0));
			CleanupSlot("test_restore");
		}

		private void TestGetSavePath()
		{
			string savePath = EvilSave.GetSavePath("my_slot");
			AssertTrue("GetSavePath: contains slot id", savePath.Contains("my_slot"));
			AssertTrue("GetSavePath: ends with save.dat", savePath.EndsWith("save.dat"));
			string saveRootPath = EvilSaveSettings.Instance.GetSaveRootPath();
			AssertTrue("GetSavePath: under save root", savePath.StartsWith(saveRootPath));
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
