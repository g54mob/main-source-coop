using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSaveTestGUI : MonoBehaviour
	{
		private const string ExampleSlot = "example_save";

		private Rect _windowRect = new Rect(20f, 20f, 580f, 500f);

		private Vector2 _scrollPos;

		private readonly List<string> _log = new List<string>();

		private void OnGUI()
		{
			if (Application.isPlaying)
			{
				_windowRect = GUI.Window(948271, _windowRect, DrawWindow, "EvilSave Examples (F9)");
			}
		}

		private void DrawWindow(int id)
		{
			GUIStyle style = new GUIStyle(GUI.skin.label)
			{
				richText = true,
				wordWrap = true
			};
			GUILayout.BeginHorizontal();
			GUILayout.Label("<b>Save Examples</b>", style);
			GUILayout.FlexibleSpace();
			GUILayout.Button("X", GUILayout.Width(22f));
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Save Primitives"))
			{
				SavePrimitives();
			}
			if (GUILayout.Button("Save Unity Types"))
			{
				SaveUnityTypes();
			}
			if (GUILayout.Button("Save Collections"))
			{
				SaveCollections();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Save Complex"))
			{
				SaveComplex();
			}
			if (GUILayout.Button("Save PlayerPrefs"))
			{
				SavePlayerPrefs();
			}
			if (GUILayout.Button("Save ALL"))
			{
				SaveAll();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(6f);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Write to Disk"))
			{
				WriteToDisk();
			}
			if (GUILayout.Button("Load from Disk"))
			{
				LoadFromDisk();
			}
			if (GUILayout.Button("Log All Keys"))
			{
				LogAllKeys();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Clear Memory"))
			{
				ClearMemory();
			}
			if (GUILayout.Button("Delete Slot"))
			{
				DeleteSlot();
			}
			if (GUILayout.Button("Clear Log"))
			{
				_log.Clear();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
			GUILayout.Label("<b>Active Slot:</b> " + EvilSave.ActiveSlot + "  |  <b>Save Path:</b> " + EvilSave.GetSavePath("example_save"), style);
			GUILayout.Space(4f);
			_scrollPos = GUILayout.BeginScrollView(_scrollPos);
			foreach (string item in _log)
			{
				GUILayout.Label(item, style);
			}
			GUILayout.EndScrollView();
			GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
		}

		private void Log(string msg)
		{
			_log.Add(msg);
		}

		private void SavePrimitives()
		{
			EvilSave.ActiveSlot = "example_save";
			EvilSave.Save("prim.bool", value: true);
			EvilSave.Save("prim.byte", (byte)255);
			EvilSave.Save("prim.int", 42);
			EvilSave.Save("prim.long", 9999999999L);
			EvilSave.Save("prim.float", 3.14159f);
			EvilSave.Save("prim.double", Math.PI);
			EvilSave.Save("prim.decimal", 123456.789m);
			EvilSave.Save("prim.char", 'Z');
			EvilSave.Save("prim.string", "Hello EvilSave!");
			EvilSave.Save("prim.string.unicode", "Merhaba çşğüö \ud83d\ude80");
			EvilSave.Save("prim.datetime", DateTime.UtcNow);
			EvilSave.Save("prim.bytes", new byte[5] { 0, 1, 127, 128, 255 });
			Log("<color=lime>[Primitives]</color> 12 key saved to memory");
		}

		private void SaveUnityTypes()
		{
			EvilSave.ActiveSlot = "example_save";
			EvilSave.Save("unity.vector2", new Vector2(1.5f, -2.7f));
			EvilSave.Save("unity.vector3", new Vector3(10f, -20f, 30f));
			EvilSave.Save("unity.vector4", new Vector4(1f, 2f, 3f, 4f));
			EvilSave.Save("unity.vector2int", new Vector2Int(-5, 10));
			EvilSave.Save("unity.vector3int", new Vector3Int(100, -200, 300));
			EvilSave.Save("unity.quaternion", Quaternion.Euler(45f, 90f, 180f));
			EvilSave.Save("unity.color", new Color(0.1f, 0.5f, 0.9f, 0.75f));
			EvilSave.Save("unity.color32", new Color32(255, 128, 0, 200));
			EvilSave.Save("unity.rect", new Rect(10f, 20f, 300f, 400f));
			EvilSave.Save("unity.bounds", new Bounds(Vector3.one, Vector3.one * 10f));
			EvilSave.Save("unity.layermask", (LayerMask)136);
			AnimationCurve value = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
			EvilSave.Save("unity.curve", value);
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[2]
			{
				new GradientColorKey(Color.red, 0f),
				new GradientColorKey(Color.blue, 1f)
			}, new GradientAlphaKey[2]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(0.5f, 1f)
			});
			EvilSave.Save("unity.gradient", gradient);
			Log("<color=lime>[Unity Types]</color> 13 key saved to memory");
		}

		private void SaveCollections()
		{
			EvilSave.ActiveSlot = "example_save";
			EvilSave.Save("col.intarray", new int[5] { 1, 2, 3, -4, 2147483647 });
			EvilSave.Save("col.stringarray", new string[3] { "hello", "world", "test" });
			EvilSave.Save("col.intlist", new List<int> { 10, 20, 30, -40 });
			EvilSave.Save("col.v3list", new List<Vector3>
			{
				Vector3.up,
				Vector3.right,
				Vector3.forward
			});
			EvilSave.Save("col.nestedlist", new List<List<int>>
			{
				new List<int> { 1, 2, 3 },
				new List<int> { 4, 5 },
				new List<int>()
			});
			EvilSave.Save("col.dict.strint", new Dictionary<string, int>
			{
				{ "health", 100 },
				{ "mana", 50 },
				{ "score", -1 }
			});
			EvilSave.Save("col.dict.strv3", new Dictionary<string, Vector3>
			{
				{
					"spawn",
					new Vector3(10f, 0f, 20f)
				},
				{
					"checkpoint",
					new Vector3(-5f, 100f, 50f)
				}
			});
			EvilSave.Save("col.hashset", new HashSet<int> { 1, 2, 3, 100, -50 });
			Log("<color=lime>[Collections]</color> 8 key saved to memory");
		}

		private void SaveComplex()
		{
			EvilSave.ActiveSlot = "example_save";
			EvilSave.Save("cpx.enum.byte", TestByteEnum.Disabled);
			EvilSave.Save("cpx.enum.int", TestIntEnum.Running);
			EvilSave.Save("cpx.simple", new SimpleData
			{
				id = 42,
				name = "NomadDriver",
				health = 85.5f,
				isAlive = true
			});
			EvilSave.Save("cpx.nested", new NestedData
			{
				label = "NestedExample",
				inner = new SimpleData
				{
					id = 7,
					name = "Inner",
					health = 100f,
					isAlive = false
				},
				scores = new List<int> { 10, 20, 30 }
			});
			EvilSave.Save("cpx.struct", new StructData
			{
				x = 10,
				y = 20,
				tag = "grid"
			});
			EvilSave.Save("cpx.player", new PlayerSaveData
			{
				playerName = "NomadDriver",
				level = 15,
				experience = 12345.67f,
				position = new Vector3(100f, 5f, -200f),
				rotation = Quaternion.Euler(0f, 180f, 0f),
				inventory = new List<string> { "Wrench", "GasCan", "Apple" },
				stats = new Dictionary<string, int>
				{
					{ "kills", 42 },
					{ "deaths", 7 }
				},
				state = TestIntEnum.Running
			});
			EvilSave.Save("cpx.simplelist", new List<SimpleData>
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
				}
			});
			EvilSave.Save("cpx.simpledict", new Dictionary<string, SimpleData>
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
			});
			Log("<color=lime>[Complex]</color> 8 key saved to memory");
		}

		private void SavePlayerPrefs()
		{
			EvilSave.Prefs.SetString("prefs.name", "NomadDriver");
			EvilSave.Prefs.SetInt("prefs.level", 15);
			EvilSave.Prefs.SetFloat("prefs.volume", 0.75f);
			EvilSave.Prefs.SetBool("prefs.fullscreen", value: true);
			EvilSave.Prefs.SetObject("prefs.obj", new PrefsTestObject
			{
				name = "Settings",
				value = 42
			});
			Log("<color=lime>[PlayerPrefs]</color> 5 key saved to PlayerPrefs");
		}

		private void SaveAll()
		{
			SavePrimitives();
			SaveUnityTypes();
			SaveCollections();
			SaveComplex();
			SavePlayerPrefs();
			Log("<color=yellow>--- All categories saved to memory ---</color>");
		}

		private void WriteToDisk()
		{
			EvilSave.ActiveSlot = "example_save";
			EvilSave.SaveToDisk("example_save");
			string savePath = EvilSave.GetSavePath("example_save");
			Log("<color=cyan>[Disk]</color> Written to: " + savePath);
		}

		private void LoadFromDisk()
		{
			EvilSave.ActiveSlot = "example_save";
			EvilSave.LoadFromDisk("example_save");
			string[] keys = EvilSave.GetKeys();
			Log($"<color=cyan>[Disk]</color> Loaded {keys.Length} keys from disk");
			string[] array = keys;
			foreach (string text in array)
			{
				string text2 = LoadAndFormat(text);
				Log("  <color=white>" + text + "</color> = " + text2);
			}
		}

		private void LogAllKeys()
		{
			EvilSave.ActiveSlot = "example_save";
			string[] keys = EvilSave.GetKeys();
			if (keys.Length == 0)
			{
				Log("<color=yellow>No keys in memory. Save something first or load from disk.</color>");
				return;
			}
			Log(string.Format("<color=cyan>[Keys]</color> {0} keys in slot '{1}':", keys.Length, "example_save"));
			string[] array = keys;
			foreach (string text in array)
			{
				string text2 = LoadAndFormat(text);
				Log("  <color=white>" + text + "</color> = " + text2);
			}
			string[] allKeys = EvilSave.Prefs.GetAllKeys();
			if (allKeys.Length != 0)
			{
				Log($"<color=cyan>[PlayerPrefs]</color> {allKeys.Length} keys:");
				array = allKeys;
				foreach (string text3 in array)
				{
					Log("  <color=white>" + text3 + "</color>");
				}
			}
		}

		private void ClearMemory()
		{
			EvilSave.ActiveSlot = "example_save";
			EvilSave.Clear("example_save");
			Log("<color=yellow>[Clear]</color> Memory cleared (disk file untouched)");
		}

		private void DeleteSlot()
		{
			EvilSave.DeleteSlot("example_save");
			Log("<color=red>[Delete]</color> Slot deleted from disk and memory");
		}

		private string LoadAndFormat(string key)
		{
			try
			{
				byte[] array = EvilSave.LoadRaw(key);
				if (array == null)
				{
					return "<null>";
				}
				if (TryLoad<bool>(key, out var value))
				{
					return value.ToString();
				}
				if (TryLoad<int>(key, out var value2))
				{
					return value2.ToString();
				}
				if (TryLoad<float>(key, out var value3))
				{
					return value3.ToString("F4");
				}
				if (TryLoad<long>(key, out var value4))
				{
					return value4.ToString();
				}
				if (TryLoad<double>(key, out var value5))
				{
					return value5.ToString("F6");
				}
				if (TryLoad<string>(key, out var value6))
				{
					return "\"" + value6 + "\"";
				}
				if (TryLoad<Vector3>(key, out var value7))
				{
					return value7.ToString("F2");
				}
				if (TryLoad<Quaternion>(key, out var value8))
				{
					return value8.eulerAngles.ToString("F1");
				}
				if (TryLoad<Color>(key, out var value9))
				{
					return $"RGBA({value9.r:F2},{value9.g:F2},{value9.b:F2},{value9.a:F2})";
				}
				return $"[{array.Length} bytes]";
			}
			catch
			{
				return "<error reading>";
			}
		}

		private bool TryLoad<T>(string key, out T value)
		{
			try
			{
				value = EvilSave.Load<T>(key);
				return value != null && !value.Equals(default(T));
			}
			catch
			{
				value = default(T);
				return false;
			}
		}
	}
}
