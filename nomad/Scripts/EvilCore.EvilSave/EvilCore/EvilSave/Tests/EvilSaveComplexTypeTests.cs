using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSaveComplexTypeTests : EvilSaveTestBase
	{
		public override string CategoryName => "Complex Types";

		public void RunAll()
		{
			ClearResults();
			EvilSave.Clear();
			TestByteEnumSaveLoad();
			TestIntEnumSaveLoad();
			TestEnumDefault();
			TestSimpleSerializable();
			TestNestedSerializable();
			TestPlayerSaveData();
			TestStructData();
			TestListOfSerializableObjects();
			TestDictionaryWithSerializableValues();
			TestNullString();
			LogSummary("Complex Types");
		}

		private void TestByteEnumSaveLoad()
		{
			EvilSave.Save("test.enum.byte", TestByteEnum.Disabled);
			TestByteEnum actual = EvilSave.Load("test.enum.byte", TestByteEnum.None);
			Assert("Enum (byte) Disabled", TestByteEnum.Disabled, actual);
			EvilSave.Save("test.enum.byte.none", TestByteEnum.None);
			Assert("Enum (byte) None", TestByteEnum.None, EvilSave.Load("test.enum.byte.none", TestByteEnum.None));
		}

		private void TestIntEnumSaveLoad()
		{
			EvilSave.Save("test.enum.int", TestIntEnum.Dead);
			Assert("Enum (int) Dead", TestIntEnum.Dead, EvilSave.Load("test.enum.int", TestIntEnum.Idle));
			EvilSave.Save("test.enum.int.max", TestIntEnum.MaxState);
			Assert("Enum (int) MaxState", TestIntEnum.MaxState, EvilSave.Load("test.enum.int.max", TestIntEnum.Idle));
		}

		private void TestEnumDefault()
		{
			TestIntEnum actual = EvilSave.Load("test.enum.missing", TestIntEnum.Idle);
			Assert("Enum default", TestIntEnum.Idle, actual);
		}

		private void TestSimpleSerializable()
		{
			SimpleData simpleData = new SimpleData
			{
				id = 42,
				name = "TestPlayer",
				health = 85.5f,
				isAlive = true
			};
			EvilSave.Save("test.simple", simpleData);
			SimpleData simpleData2 = EvilSave.Load<SimpleData>("test.simple");
			AssertTrue("SimpleData not null", simpleData2 != null);
			if (simpleData2 != null)
			{
				Assert("SimpleData.id", simpleData.id, simpleData2.id);
				Assert("SimpleData.name", simpleData.name, simpleData2.name);
				AssertFloatEqual("SimpleData.health", simpleData.health, simpleData2.health);
				Assert("SimpleData.isAlive", simpleData.isAlive, simpleData2.isAlive);
			}
		}

		private void TestNestedSerializable()
		{
			NestedData nestedData = new NestedData
			{
				label = "Nested",
				inner = new SimpleData
				{
					id = 7,
					name = "Inner",
					health = 100f,
					isAlive = false
				},
				scores = new List<int> { 10, 20, 30 }
			};
			EvilSave.Save("test.nested", nestedData);
			NestedData nestedData2 = EvilSave.Load<NestedData>("test.nested");
			AssertTrue("NestedData not null", nestedData2 != null);
			if (nestedData2 != null)
			{
				Assert("NestedData.label", nestedData.label, nestedData2.label);
				AssertTrue("NestedData.inner not null", nestedData2.inner != null);
				if (nestedData2.inner != null)
				{
					Assert("NestedData.inner.id", 7, nestedData2.inner.id);
					Assert("NestedData.inner.name", "Inner", nestedData2.inner.name);
					Assert("NestedData.inner.isAlive", expected: false, nestedData2.inner.isAlive);
				}
				AssertTrue("NestedData.scores not null", nestedData2.scores != null);
				if (nestedData2.scores != null)
				{
					Assert("NestedData.scores.Count", 3, nestedData2.scores.Count);
					Assert("NestedData.scores[1]", 20, nestedData2.scores[1]);
				}
			}
		}

		private void TestPlayerSaveData()
		{
			PlayerSaveData playerSaveData = new PlayerSaveData
			{
				playerName = "NomadDriver",
				level = 15,
				experience = 12345.67f,
				position = new Vector3(100f, 5f, -200f),
				rotation = Quaternion.Euler(0f, 180f, 0f),
				inventory = new List<string> { "Wrench", "GasCan", "Apple", "" },
				stats = new Dictionary<string, int>
				{
					{ "kills", 42 },
					{ "deaths", 7 },
					{ "distance", 999999 }
				},
				state = TestIntEnum.Running
			};
			EvilSave.Save("test.player", playerSaveData);
			PlayerSaveData playerSaveData2 = EvilSave.Load<PlayerSaveData>("test.player");
			AssertTrue("PlayerSaveData not null", playerSaveData2 != null);
			if (playerSaveData2 != null)
			{
				Assert("Player.name", playerSaveData.playerName, playerSaveData2.playerName);
				Assert("Player.level", playerSaveData.level, playerSaveData2.level);
				AssertFloatEqual("Player.exp", playerSaveData.experience, playerSaveData2.experience);
				AssertFloatEqual("Player.pos.x", playerSaveData.position.x, playerSaveData2.position.x);
				AssertFloatEqual("Player.pos.z", playerSaveData.position.z, playerSaveData2.position.z);
				AssertFloatEqual("Player.rot.y", playerSaveData.rotation.y, playerSaveData2.rotation.y);
				Assert("Player.inventory.Count", 4, playerSaveData2.inventory?.Count ?? 0);
				if (playerSaveData2.inventory != null)
				{
					Assert("Player.inventory[0]", "Wrench", playerSaveData2.inventory[0]);
				}
				Assert("Player.stats.Count", 3, playerSaveData2.stats?.Count ?? 0);
				if (playerSaveData2.stats != null)
				{
					playerSaveData2.stats.TryGetValue("kills", out var value);
					Assert("Player.stats[kills]", 42, value);
				}
				Assert("Player.state", TestIntEnum.Running, playerSaveData2.state);
			}
		}

		private void TestStructData()
		{
			StructData value = new StructData
			{
				x = 10,
				y = 20,
				tag = "grid"
			};
			EvilSave.Save("test.struct", value);
			StructData structData = EvilSave.Load<StructData>("test.struct");
			Assert("Struct.x", value.x, structData.x);
			Assert("Struct.y", value.y, structData.y);
			Assert("Struct.tag", value.tag, structData.tag);
		}

		private void TestListOfSerializableObjects()
		{
			List<SimpleData> value = new List<SimpleData>
			{
				new SimpleData
				{
					id = 1,
					name = "A",
					health = 10f,
					isAlive = true
				},
				new SimpleData
				{
					id = 2,
					name = "B",
					health = 0f,
					isAlive = false
				},
				new SimpleData
				{
					id = 3,
					name = "C",
					health = 100f,
					isAlive = true
				}
			};
			EvilSave.Save("test.list.complex", value);
			List<SimpleData> list = EvilSave.Load<List<SimpleData>>("test.list.complex");
			AssertTrue("List<SimpleData> not null", list != null);
			Assert("List<SimpleData>.Count", 3, list?.Count ?? 0);
			if (list != null)
			{
				Assert("list[0].name", "A", list[0].name);
				Assert("list[1].isAlive", expected: false, list[1].isAlive);
				AssertFloatEqual("list[2].health", 100f, list[2].health);
			}
		}

		private void TestDictionaryWithSerializableValues()
		{
			Dictionary<string, SimpleData> value = new Dictionary<string, SimpleData>
			{
				{
					"player",
					new SimpleData
					{
						id = 1,
						name = "P1",
						health = 80f,
						isAlive = true
					}
				},
				{
					"enemy",
					new SimpleData
					{
						id = 2,
						name = "E1",
						health = 0f,
						isAlive = false
					}
				}
			};
			EvilSave.Save("test.dict.complex", value);
			Dictionary<string, SimpleData> dictionary = EvilSave.Load<Dictionary<string, SimpleData>>("test.dict.complex");
			AssertTrue("Dict<string,SimpleData> not null", dictionary != null);
			Assert("Dict.Count", 2, dictionary?.Count ?? 0);
			if (dictionary != null)
			{
				AssertTrue("Dict has 'player'", dictionary.ContainsKey("player"));
				Assert("Dict[player].name", "P1", dictionary["player"].name);
				Assert("Dict[enemy].isAlive", expected: false, dictionary["enemy"].isAlive);
			}
		}

		private void TestNullString()
		{
			EvilSave.Save<string>("test.null.str", null);
			string text = EvilSave.Load<string>("test.null.str");
			AssertTrue("Null string loads as null or empty", text == null || text == "", "Got: '" + text + "'");
		}
	}
}
