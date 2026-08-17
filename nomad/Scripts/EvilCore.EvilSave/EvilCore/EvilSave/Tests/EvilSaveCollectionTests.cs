using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSaveCollectionTests : EvilSaveTestBase
	{
		public override string CategoryName => "Collections";

		public void RunAll()
		{
			ClearResults();
			EvilSave.Clear();
			TestIntArray();
			TestStringArray();
			TestEmptyArray();
			TestFloatArray();
			TestVector3Array();
			TestIntList();
			TestStringList();
			TestEmptyList();
			TestVector3List();
			TestNestedList();
			TestStringIntDictionary();
			TestIntFloatDictionary();
			TestEmptyDictionary();
			TestStringVector3Dictionary();
			TestIntHashSet();
			TestStringHashSet();
			TestEmptyHashSet();
			TestByteArrayList();
			LogSummary("Collections");
		}

		private void TestIntArray()
		{
			int[] array = new int[6] { 1, 2, 3, -4, 0, 2147483647 };
			EvilSave.Save("test.arr.int", array);
			int[] array2 = EvilSave.Load<int[]>("test.arr.int");
			AssertTrue("int[] not null", array2 != null);
			Assert("int[].Length", array.Length, (array2 != null) ? array2.Length : 0);
			if (array2 != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Assert($"int[{i}]", array[i], array2[i]);
				}
			}
		}

		private void TestStringArray()
		{
			string[] array = new string[4] { "hello", "world", "", "test123" };
			EvilSave.Save("test.arr.str", array);
			string[] array2 = EvilSave.Load<string[]>("test.arr.str");
			AssertTrue("string[] not null", array2 != null);
			Assert("string[].Length", array.Length, (array2 != null) ? array2.Length : 0);
			if (array2 != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Assert($"string[{i}]", array[i], array2[i]);
				}
			}
		}

		private void TestEmptyArray()
		{
			int[] value = new int[0];
			EvilSave.Save("test.arr.empty", value);
			int[] array = EvilSave.Load<int[]>("test.arr.empty");
			AssertTrue("int[] (empty) not null", array != null);
			Assert("int[] (empty).Length", 0, (array != null) ? array.Length : (-1));
		}

		private void TestFloatArray()
		{
			float[] array = new float[5] { 0.1f, -99.9f, 3.4028235E+38f, -3.4028235E+38f, 0f };
			EvilSave.Save("test.arr.float", array);
			float[] array2 = EvilSave.Load<float[]>("test.arr.float");
			AssertTrue("float[] not null", array2 != null);
			if (array2 != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					AssertFloatEqual($"float[{i}]", array[i], array2[i]);
				}
			}
		}

		private void TestVector3Array()
		{
			Vector3[] array = new Vector3[3]
			{
				Vector3.zero,
				Vector3.one,
				new Vector3(1.5f, -2.5f, 100f)
			};
			EvilSave.Save("test.arr.v3", array);
			Vector3[] array2 = EvilSave.Load<Vector3[]>("test.arr.v3");
			AssertTrue("Vector3[] not null", array2 != null);
			Assert("Vector3[].Length", array.Length, (array2 != null) ? array2.Length : 0);
			if (array2 != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					AssertFloatEqual($"Vector3[{i}].x", array[i].x, array2[i].x);
					AssertFloatEqual($"Vector3[{i}].y", array[i].y, array2[i].y);
					AssertFloatEqual($"Vector3[{i}].z", array[i].z, array2[i].z);
				}
			}
		}

		private void TestIntList()
		{
			List<int> list = new List<int> { 10, 20, 30, -40, 0 };
			EvilSave.Save("test.list.int", list);
			List<int> list2 = EvilSave.Load<List<int>>("test.list.int");
			AssertTrue("List<int> not null", list2 != null);
			Assert("List<int>.Count", list.Count, list2?.Count ?? 0);
			if (list2 != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Assert($"List<int>[{i}]", list[i], list2[i]);
				}
			}
		}

		private void TestStringList()
		{
			List<string> list = new List<string> { "alpha", "beta", "gamma", "" };
			EvilSave.Save("test.list.str", list);
			List<string> list2 = EvilSave.Load<List<string>>("test.list.str");
			AssertTrue("List<string> not null", list2 != null);
			Assert("List<string>.Count", list.Count, list2?.Count ?? 0);
			if (list2 != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Assert($"List<string>[{i}]", list[i], list2[i]);
				}
			}
		}

		private void TestEmptyList()
		{
			List<int> value = new List<int>();
			EvilSave.Save("test.list.empty", value);
			List<int> list = EvilSave.Load<List<int>>("test.list.empty");
			AssertTrue("List<int> (empty) not null", list != null);
			Assert("List<int> (empty).Count", 0, list?.Count ?? (-1));
		}

		private void TestVector3List()
		{
			List<Vector3> list = new List<Vector3>
			{
				Vector3.up,
				Vector3.right,
				new Vector3(-1f, -2f, -3f)
			};
			EvilSave.Save("test.list.v3", list);
			List<Vector3> list2 = EvilSave.Load<List<Vector3>>("test.list.v3");
			AssertTrue("List<Vector3> not null", list2 != null);
			Assert("List<Vector3>.Count", list.Count, list2?.Count ?? 0);
			if (list2 != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					AssertFloatEqual($"List<Vector3>[{i}].magnitude", list[i].magnitude, list2[i].magnitude);
				}
			}
		}

		private void TestNestedList()
		{
			List<List<int>> list = new List<List<int>>
			{
				new List<int> { 1, 2, 3 },
				new List<int> { 4, 5 },
				new List<int>()
			};
			EvilSave.Save("test.list.nested", list);
			List<List<int>> list2 = EvilSave.Load<List<List<int>>>("test.list.nested");
			AssertTrue("List<List<int>> not null", list2 != null);
			Assert("List<List<int>>.Count", list.Count, list2?.Count ?? 0);
			if (list2 != null)
			{
				Assert("nested[0].Count", 3, list2[0].Count);
				Assert("nested[1].Count", 2, list2[1].Count);
				Assert("nested[2].Count", 0, list2[2].Count);
				Assert("nested[0][2]", 3, list2[0][2]);
				Assert("nested[1][0]", 4, list2[1][0]);
			}
		}

		private void TestStringIntDictionary()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>
			{
				{ "health", 100 },
				{ "mana", 50 },
				{ "score", -1 }
			};
			EvilSave.Save("test.dict.str_int", dictionary);
			Dictionary<string, int> dictionary2 = EvilSave.Load<Dictionary<string, int>>("test.dict.str_int");
			AssertTrue("Dict<string,int> not null", dictionary2 != null);
			Assert("Dict<string,int>.Count", dictionary.Count, dictionary2?.Count ?? 0);
			if (dictionary2 == null)
			{
				return;
			}
			foreach (KeyValuePair<string, int> item in dictionary)
			{
				dictionary2.TryGetValue(item.Key, out var value);
				Assert("Dict[\"" + item.Key + "\"]", item.Value, value);
			}
		}

		private void TestIntFloatDictionary()
		{
			Dictionary<int, float> dictionary = new Dictionary<int, float>
			{
				{ 0, 0.5f },
				{ 1, 1.5f },
				{ 99, -100f }
			};
			EvilSave.Save("test.dict.int_float", dictionary);
			Dictionary<int, float> dictionary2 = EvilSave.Load<Dictionary<int, float>>("test.dict.int_float");
			AssertTrue("Dict<int,float> not null", dictionary2 != null);
			Assert("Dict<int,float>.Count", dictionary.Count, dictionary2?.Count ?? 0);
			if (dictionary2 == null)
			{
				return;
			}
			foreach (KeyValuePair<int, float> item in dictionary)
			{
				dictionary2.TryGetValue(item.Key, out var value);
				AssertFloatEqual($"Dict[{item.Key}]", item.Value, value);
			}
		}

		private void TestEmptyDictionary()
		{
			Dictionary<string, int> value = new Dictionary<string, int>();
			EvilSave.Save("test.dict.empty", value);
			Dictionary<string, int> dictionary = EvilSave.Load<Dictionary<string, int>>("test.dict.empty");
			AssertTrue("Dict (empty) not null", dictionary != null);
			Assert("Dict (empty).Count", 0, dictionary?.Count ?? (-1));
		}

		private void TestStringVector3Dictionary()
		{
			Dictionary<string, Vector3> value = new Dictionary<string, Vector3>
			{
				{
					"spawn",
					new Vector3(10f, 0f, 20f)
				},
				{
					"checkpoint",
					new Vector3(-5f, 100f, 50f)
				}
			};
			EvilSave.Save("test.dict.str_v3", value);
			Dictionary<string, Vector3> dictionary = EvilSave.Load<Dictionary<string, Vector3>>("test.dict.str_v3");
			AssertTrue("Dict<string,Vector3> not null", dictionary != null);
			if (dictionary != null)
			{
				AssertFloatEqual("Dict[spawn].x", 10f, dictionary["spawn"].x);
				AssertFloatEqual("Dict[checkpoint].y", 100f, dictionary["checkpoint"].y);
			}
		}

		private void TestIntHashSet()
		{
			HashSet<int> hashSet = new HashSet<int> { 1, 2, 3, 100, -50 };
			EvilSave.Save("test.hashset.int", hashSet);
			HashSet<int> hashSet2 = EvilSave.Load<HashSet<int>>("test.hashset.int");
			AssertTrue("HashSet<int> not null", hashSet2 != null);
			Assert("HashSet<int>.Count", hashSet.Count, hashSet2?.Count ?? 0);
			if (hashSet2 == null)
			{
				return;
			}
			foreach (int item in hashSet)
			{
				AssertTrue($"HashSet contains {item}", hashSet2.Contains(item));
			}
		}

		private void TestStringHashSet()
		{
			HashSet<string> hashSet = new HashSet<string> { "alpha", "beta", "gamma" };
			EvilSave.Save("test.hashset.str", hashSet);
			HashSet<string> hashSet2 = EvilSave.Load<HashSet<string>>("test.hashset.str");
			AssertTrue("HashSet<string> not null", hashSet2 != null);
			Assert("HashSet<string>.Count", hashSet.Count, hashSet2?.Count ?? 0);
			if (hashSet2 == null)
			{
				return;
			}
			foreach (string item in hashSet)
			{
				AssertTrue("HashSet contains \"" + item + "\"", hashSet2.Contains(item));
			}
		}

		private void TestEmptyHashSet()
		{
			HashSet<int> value = new HashSet<int>();
			EvilSave.Save("test.hashset.empty", value);
			HashSet<int> hashSet = EvilSave.Load<HashSet<int>>("test.hashset.empty");
			AssertTrue("HashSet (empty) not null", hashSet != null);
			Assert("HashSet (empty).Count", 0, hashSet?.Count ?? (-1));
		}

		private void TestByteArrayList()
		{
			List<byte[]> list = new List<byte[]>();
			list.Add(new byte[3] { 1, 2, 3 });
			list.Add(new byte[2] { 255, 0 });
			list.Add(new byte[0]);
			List<byte[]> value = list;
			EvilSave.Save("test.list.bytes", value);
			List<byte[]> list2 = EvilSave.Load<List<byte[]>>("test.list.bytes");
			AssertTrue("List<byte[]> not null", list2 != null);
			Assert("List<byte[]>.Count", 3, list2?.Count ?? 0);
			if (list2 != null)
			{
				Assert("List<byte[]>[0].Length", 3, list2[0].Length);
				Assert("List<byte[]>[1].Length", 2, list2[1].Length);
				Assert("List<byte[]>[2].Length", 0, list2[2].Length);
				Assert("List<byte[]>[0][0]", (byte)1, list2[0][0]);
				Assert("List<byte[]>[1][0]", (byte)255, list2[1][0]);
			}
		}
	}
}
