using System;
using System.Collections.Generic;
using System.Reflection;
using Den.Tools;
using Den.Tools.Serialization;
using MapMagic.Nodes;
using UnityEngine;

namespace MapMagic.Expose
{
	[Serializable]
	public class Override : ISerializationCallbackReceiver
	{
		private struct ObjType
		{
			public object obj;

			public Type type;

			public ObjType(object obj, Type type)
			{
				this.obj = obj;
				this.type = type;
			}
		}

		[NonSerialized]
		private DictionaryOrdered<string, ObjType> dict = new DictionaryOrdered<string, ObjType>();

		[SerializeField]
		private string serializedDict;

		[SerializeField]
		private UnityEngine.Object[] unityObjects;

		public object this[string key]
		{
			get
			{
				return dict[key].obj;
			}
			set
			{
				dict[key] = new ObjType(value, value.GetType());
			}
		}

		public int Count => dict.Count;

		public Override()
		{
		}

		public Override(Override src)
		{
			dict = new DictionaryOrdered<string, ObjType>(src.dict);
		}

		public bool TryGetValue(string name, out Type type, out object obj)
		{
			if (dict.TryGetValue(name, out var value))
			{
				obj = value.obj;
				type = value.type;
				return true;
			}
			obj = null;
			type = null;
			return false;
		}

		public void Add(string name, Type type, object val)
		{
			dict.Add(name, new ObjType(val, type));
		}

		public void AddDefault(string name, Type type)
		{
			object val = null;
			if (type.IsValueType)
			{
				val = Activator.CreateInstance(type);
			}
			Add(name, type, val);
		}

		public void SetOrAdd(string name, Type type, object val)
		{
			if (dict.ContainsKey(name))
			{
				dict[name] = new ObjType(val, type);
			}
			else
			{
				dict.Add(name, new ObjType(val, type));
			}
		}

		public void SetIfContains(string name, Type type, object val)
		{
			if (dict.TryGetValue(name, out var value) && value.type == type)
			{
				dict[name] = new ObjType(val, type);
			}
		}

		public void RemoveAt(int num)
		{
			dict.RemoveAt(num);
		}

		public void Switch(int n1, int n2)
		{
			dict.Switch(n1, n2);
		}

		public bool Contains(string name)
		{
			return dict.Contains(name);
		}

		public void Clear()
		{
			dict.Clear();
		}

		public (string, Type, object) GetOverrideAt(int num)
		{
			string text = ((IList<string>)dict)[num];
			ObjType objType = dict[text];
			return (text, objType.type, objType.obj);
		}

		public void SetOverrideAt(int num, string name, Type type, object obj)
		{
			string text = ((IList<string>)dict)[num];
			if (name != text)
			{
				dict.RemoveAt(num);
				dict.Add(name, new ObjType(obj, type));
			}
			else
			{
				dict[text] = new ObjType(obj, type);
			}
		}

		public void AddExposed(Exposed.Entry entry, Graph graph = null)
		{
			Type type = ((entry.channel < 0) ? entry.type : ((!(entry.type == typeof(Coord))) ? typeof(float) : typeof(int)));
			object obj = null;
			if (graph != null)
			{
				obj = GetFieldValue(graph, entry.id, entry.name);
				Calculator.Vector vector = new Calculator.Vector(obj);
				if (entry.channel >= 0)
				{
					vector.Unify(entry.channel);
				}
				obj = vector.Convert(type);
			}
			while (true)
			{
				string text = entry.calculator.CheckOverrideAssign(this);
				if (text != null)
				{
					if (obj != null)
					{
						graph.defaults.Add(text, type, obj);
					}
					else
					{
						graph.defaults.AddDefault(text, type);
					}
					continue;
				}
				break;
			}
		}

		public void AddAllExposed(Exposed exp, Graph graph = null)
		{
			foreach (Exposed.Entry item in exp.AllEntries())
			{
				AddExposed(item, graph);
			}
		}

		public void RemoveAllUnused(Exposed exp)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (Exposed.Entry item in exp.AllEntries())
			{
				foreach (string item2 in item.calculator.AllRefenrences())
				{
					hashSet.Add(item2);
				}
			}
			List<string> list = new List<string>();
			foreach (string key in dict.Keys)
			{
				if (!hashSet.Contains(key))
				{
					list.Add(key);
				}
			}
			foreach (string item3 in list)
			{
				dict.Remove(item3);
			}
		}

		private static object GetFieldValue(Graph graph, ulong id, string name)
		{
			IUnit unit = null;
			foreach (IUnit item in graph.AllUnits())
			{
				if (item.Id == id)
				{
					unit = item;
					break;
				}
			}
			if (unit == null)
			{
				return null;
			}
			FieldInfo field = unit.GetType().GetField(name);
			if (field == null)
			{
				return null;
			}
			return field.GetValue(unit);
		}

		public void Sync(Override ovd)
		{
			for (int num = dict.Count - 1; num >= 0; num--)
			{
				string key = ((IList<string>)dict)[num];
				Type type = dict[key].type;
				if (!ovd.dict.Contains(key) || ovd.dict[key].type != type)
				{
					RemoveAt(num);
				}
			}
			for (int i = 0; i < ovd.dict.Count; i++)
			{
				string key2 = ((IList<string>)ovd.dict)[i];
				if (!dict.Contains(key2))
				{
					dict.Add(key2, ovd.dict[key2]);
				}
			}
		}

		public void OnBeforeSerialize()
		{
			serializedDict = Den.Tools.Serialization.Serializer.Serialize(dict, out unityObjects);
		}

		public void OnAfterDeserialize()
		{
			if (serializedDict != null && unityObjects != null)
			{
				dict = (DictionaryOrdered<string, ObjType>)Den.Tools.Serialization.Serializer.Deserialize(serializedDict, unityObjects);
			}
			dict.ReCreateDictionary();
		}
	}
}
