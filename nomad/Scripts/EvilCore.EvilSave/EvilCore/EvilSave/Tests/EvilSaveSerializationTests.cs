using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSaveSerializationTests : EvilSaveTestBase
	{
		public override string CategoryName => "Serialization";

		public void RunAll()
		{
			ClearResults();
			TestSerializeDeserializeInt();
			TestSerializeDeserializeString();
			TestSerializeDeserializeVector3();
			TestSerializeDeserializeList();
			TestSerializeDeserializeDictionary();
			TestSerializeDeserializeComplex();
			TestSaveRawLoadRaw();
			TestSaveDataBinaryRoundtrip();
			TestSaveDataMultipleEntries();
			LogSummary("Serialization");
		}

		private void TestSerializeDeserializeInt()
		{
			int actual = EvilSave.Deserialize<int>(EvilSave.Serialize(42));
			Assert("Serialize/Deserialize int", 42, actual);
		}

		private void TestSerializeDeserializeString()
		{
			string actual = EvilSave.Deserialize<string>(EvilSave.Serialize("EvilSave Test"));
			Assert("Serialize/Deserialize string", "EvilSave Test", actual);
		}

		private void TestSerializeDeserializeVector3()
		{
			Vector3 value = new Vector3(1.1f, 2.2f, 3.3f);
			Vector3 vector = EvilSave.Deserialize<Vector3>(EvilSave.Serialize(value));
			AssertFloatEqual("Serialize/Deserialize Vector3.x", value.x, vector.x);
			AssertFloatEqual("Serialize/Deserialize Vector3.y", value.y, vector.y);
			AssertFloatEqual("Serialize/Deserialize Vector3.z", value.z, vector.z);
		}

		private void TestSerializeDeserializeList()
		{
			List<int> list = EvilSave.Deserialize<List<int>>(EvilSave.Serialize(new List<int> { 1, 2, 3, 4, 5 }));
			AssertTrue("Serialize/Deserialize List not null", list != null);
			Assert("Serialize/Deserialize List.Count", 5, list?.Count ?? 0);
			if (list != null)
			{
				Assert("Serialize/Deserialize List[3]", 4, list[3]);
			}
		}

		private void TestSerializeDeserializeDictionary()
		{
			Dictionary<string, float> dictionary = EvilSave.Deserialize<Dictionary<string, float>>(EvilSave.Serialize(new Dictionary<string, float>
			{
				{ "a", 1.1f },
				{ "b", 2.2f }
			}));
			AssertTrue("Serialize/Deserialize Dict not null", dictionary != null);
			Assert("Serialize/Deserialize Dict.Count", 2, dictionary?.Count ?? 0);
			if (dictionary != null)
			{
				AssertFloatEqual("Serialize/Deserialize Dict[a]", 1.1f, dictionary["a"]);
			}
		}

		private void TestSerializeDeserializeComplex()
		{
			PlayerSaveData playerSaveData = EvilSave.Deserialize<PlayerSaveData>(EvilSave.Serialize(new PlayerSaveData
			{
				playerName = "Serialize Test",
				level = 99,
				experience = 50000f,
				position = new Vector3(10f, 20f, 30f),
				rotation = Quaternion.identity,
				inventory = new List<string> { "A", "B" },
				stats = new Dictionary<string, int> { { "hp", 100 } },
				state = TestIntEnum.Running
			}));
			AssertTrue("Serialize/Deserialize Complex not null", playerSaveData != null);
			if (playerSaveData != null)
			{
				Assert("Serialize/Deserialize Complex.name", "Serialize Test", playerSaveData.playerName);
				Assert("Serialize/Deserialize Complex.level", 99, playerSaveData.level);
				Assert("Serialize/Deserialize Complex.state", TestIntEnum.Running, playerSaveData.state);
				Assert("Serialize/Deserialize Complex.inv.Count", 2, playerSaveData.inventory?.Count ?? 0);
			}
		}

		private void TestSaveRawLoadRaw()
		{
			EvilSave.Clear();
			byte[] data = new byte[4] { 222, 173, 190, 239 };
			EvilSave.SaveRaw("test.raw", data);
			byte[] array = EvilSave.LoadRaw("test.raw");
			AssertTrue("SaveRaw/LoadRaw not null", array != null);
			Assert("SaveRaw/LoadRaw length", 4, (array != null) ? array.Length : 0);
			if (array != null)
			{
				Assert("SaveRaw[0]", (byte)222, array[0]);
				Assert("SaveRaw[3]", (byte)239, array[3]);
			}
			byte[] array2 = EvilSave.LoadRaw("test.raw.missing");
			AssertTrue("LoadRaw missing returns null", array2 == null);
		}

		private void TestSaveDataBinaryRoundtrip()
		{
			SaveData saveData = new SaveData();
			saveData.Version = 5;
			saveData.Set("key1", 42);
			saveData.Set("key2", "hello");
			saveData.Set("key3", new Vector3(1f, 2f, 3f));
			byte[] array = saveData.Serialize();
			AssertTrue("SaveData binary: has bytes", array != null && array.Length != 0);
			AssertTrue("SaveData binary: starts with EVIL", array.Length >= 4 && array[0] == 69 && array[1] == 86 && array[2] == 73 && array[3] == 76);
			SaveData saveData2 = SaveData.Deserialize(array);
			Assert("SaveData binary: version", 5, saveData2.Version);
			Assert("SaveData binary: entry count", 3, saveData2.Entries.Count);
			Assert("SaveData binary: key1", 42, saveData2.Get("key1", 0));
			Assert("SaveData binary: key2", "hello", saveData2.Get<string>("key2"));
			AssertFloatEqual("SaveData binary: key3.x", 1f, saveData2.Get<Vector3>("key3").x);
		}

		private void TestSaveDataMultipleEntries()
		{
			SaveData saveData = new SaveData
			{
				Version = 1
			};
			for (int i = 0; i < 100; i++)
			{
				saveData.Set($"batch_{i}", i);
			}
			SaveData saveData2 = SaveData.Deserialize(saveData.Serialize());
			Assert("SaveData 100 entries: count", 100, saveData2.Entries.Count);
			bool condition = true;
			for (int j = 0; j < 100; j++)
			{
				if (saveData2.Get($"batch_{j}", 0) != j)
				{
					condition = false;
					break;
				}
			}
			AssertTrue("SaveData 100 entries: all match", condition);
		}
	}
}
