using System;
using System.Collections.Generic;
using Den.Tools;
using MapMagic.Nodes;
using UnityEngine;

namespace MapMagic.Expose
{
	[Serializable]
	public class Exposed : ISerializationCallbackReceiver
	{
		[Serializable]
		public class Entry : ISerializationCallbackReceiver
		{
			public ulong id;

			public string name;

			public int channel = -1;

			public int arrIndex = -1;

			public string expression;

			[NonSerialized]
			public Type type;

			[NonSerialized]
			public Calculator calculator;

			[SerializeField]
			private string serType;

			public Entry()
			{
			}

			public Entry(ulong id, string name, int channel, int arrIndex, string expression, Type type, Calculator calculator)
			{
				this.id = id;
				this.name = name;
				this.channel = channel;
				this.arrIndex = arrIndex;
				this.expression = expression;
				this.type = type;
				this.calculator = calculator;
			}

			public Entry(ulong id, string name, int channel, int arrIndex, string expression, Type type)
			{
				this.id = id;
				this.name = name;
				this.channel = channel;
				this.arrIndex = arrIndex;
				this.expression = expression;
				this.type = type;
				calculator = Calculator.Parse(expression);
			}

			public Entry(ulong id, string name, string expression, Type type, Calculator calculator)
			{
				this.id = id;
				this.name = name;
				channel = -1;
				this.expression = expression;
				this.type = type;
				this.calculator = calculator;
			}

			public Entry(ulong id, string name, string expression, Type type)
			{
				this.id = id;
				this.name = name;
				channel = -1;
				this.expression = expression;
				this.type = type;
				calculator = Calculator.Parse(expression);
			}

			public void OnBeforeSerialize()
			{
				serType = type.AssemblyQualifiedName;
			}

			public void OnAfterDeserialize()
			{
				if (serType != null)
				{
					type = Type.GetType(serType);
					calculator = Calculator.Parse(expression);
				}
			}
		}

		[NonSerialized]
		private Dictionary<ulong, Entry[]> idToExpressions = new Dictionary<ulong, Entry[]>();

		[SerializeField]
		private Entry[] serEntries;

		public Entry this[ulong id, string name, int channel = -1, int arrIndex = -1]
		{
			get
			{
				if (!idToExpressions.TryGetValue(id, out var value))
				{
					return null;
				}
				return value.FindMember((Entry e) => e.name == name && e.channel == channel && e.arrIndex == arrIndex);
			}
		}

		public Entry this[Entry entry]
		{
			set
			{
				Entry[] array = idToExpressions[entry.id];
				int num = array.Find((Entry e) => e.name == entry.name && e.channel == entry.channel && e.arrIndex == entry.arrIndex);
				if (num >= 0)
				{
					array[num] = value;
					return;
				}
				throw new Exception("Value is not contained in array");
			}
		}

		public Entry[] this[ulong id]
		{
			get
			{
				if (!idToExpressions.TryGetValue(id, out var value))
				{
					return value;
				}
				return null;
			}
		}

		public int Count
		{
			get
			{
				int num = 0;
				foreach (Entry[] value in idToExpressions.Values)
				{
					num += value.Length;
				}
				return num;
			}
		}

		public string GetExpression(ulong id, string name, int channel = -1, int arrIndex = -1)
		{
			return this[id, name, channel, arrIndex]?.expression;
		}

		public Calculator GetCalculator(ulong id, string name, int channel = -1, int arrIndex = -1)
		{
			return this[id, name, channel, arrIndex]?.calculator;
		}

		public Type GetType(ulong id, string name, int channel = 0)
		{
			return this[id, name, channel, -1]?.type;
		}

		public void Add(Entry entry, bool overwrite = false)
		{
			if (!idToExpressions.TryGetValue(entry.id, out var value))
			{
				value = new Entry[1] { entry };
				idToExpressions.Add(entry.id, value);
				return;
			}
			int num = value.Find((Entry e) => e.name == entry.name && e.channel == entry.channel && e.arrIndex == entry.arrIndex);
			if (num >= 0)
			{
				if (!overwrite)
				{
					throw new Exception("Can't add value since it's already in array");
				}
				value[num] = entry;
			}
			else
			{
				ArrayTools.Add(ref value, entry);
				idToExpressions[entry.id] = value;
			}
		}

		public void AddRange(Entry[] entries, bool overwrite = false)
		{
			foreach (Entry entry in entries)
			{
				Add(entry, overwrite);
			}
		}

		public void AddRange(Exposed other, bool overwrite = false)
		{
			foreach (Entry[] value in other.idToExpressions.Values)
			{
				foreach (Entry entry in value)
				{
					Add(entry, overwrite);
				}
			}
		}

		public bool Remove(ulong id, string name, int channel, int arrIndex)
		{
			if (!idToExpressions.TryGetValue(id, out var value))
			{
				return false;
			}
			int num = value.Find((Entry e) => e.name == name && e.channel == channel && e.arrIndex == arrIndex);
			if (num >= 0)
			{
				ArrayTools.RemoveAt(ref value, num);
				idToExpressions[id] = value;
				return true;
			}
			return false;
		}

		public bool Remove(ulong id, string name, int arrIndex)
		{
			if (!idToExpressions.TryGetValue(id, out var value))
			{
				return false;
			}
			bool result = false;
			while (true)
			{
				int num = value.Find((Entry e) => e.name == name && e.arrIndex == arrIndex);
				if (num < 0)
				{
					break;
				}
				ArrayTools.RemoveAt(ref value, num);
				idToExpressions[id] = value;
				result = true;
			}
			return result;
		}

		public bool Remove(ulong id)
		{
			if (idToExpressions.ContainsKey(id))
			{
				idToExpressions.Remove(id);
				return true;
			}
			return false;
		}

		public bool Contains(ulong id)
		{
			return idToExpressions.ContainsKey(id);
		}

		public bool Contains(ulong id, string name)
		{
			if (!idToExpressions.TryGetValue(id, out var value))
			{
				return false;
			}
			return value.Contains((Entry e) => e.name == name);
		}

		public bool Contains(ulong id, string name, int channel, int arrIndex)
		{
			if (!idToExpressions.TryGetValue(id, out var value))
			{
				return false;
			}
			return value.Find((Entry e) => e.name == name && e.channel == channel && e.arrIndex == arrIndex) >= 0;
		}

		public IEnumerable<Entry> EntriesById(ulong id)
		{
			if (idToExpressions.TryGetValue(id, out var value))
			{
				Entry[] array = value;
				for (int i = 0; i < array.Length; i++)
				{
					yield return array[i];
				}
			}
		}

		public IEnumerable<Entry> EntiriesByReference(string reference)
		{
			foreach (Entry[] value in idToExpressions.Values)
			{
				Entry[] array = value;
				foreach (Entry entry in array)
				{
					if (entry.calculator.ContainsReference(reference))
					{
						yield return entry;
					}
				}
			}
		}

		public IEnumerable<Entry> AllEntries()
		{
			foreach (Entry[] value in idToExpressions.Values)
			{
				Entry[] array = value;
				for (int i = 0; i < array.Length; i++)
				{
					yield return array[i];
				}
			}
		}

		public IEnumerable<ulong> AllIds()
		{
			foreach (ulong key in idToExpressions.Keys)
			{
				yield return key;
			}
		}

		public IEnumerable<IUnit> AllUnits(Graph graph)
		{
			foreach (IUnit item in graph.AllUnits())
			{
				if (idToExpressions.ContainsKey(item.Id))
				{
					yield return item;
				}
			}
		}

		public void ReplaceIds(Dictionary<ulong, ulong> oldNewIds)
		{
			ulong[] array = oldNewIds.Keys.ToArray();
			ulong[] array2 = oldNewIds.Values.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				ulong key = array[i];
				ulong num = array2[i];
				if (idToExpressions.TryGetValue(key, out var value))
				{
					if (idToExpressions.ContainsKey(num))
					{
						throw new Exception("ReplaceIds: idToExpressions already contains this newid: " + num);
					}
					Entry[] array3 = value;
					for (int j = 0; j < array3.Length; j++)
					{
						array3[j].id = num;
					}
					idToExpressions.Remove(key);
					idToExpressions.Add(num, value);
				}
			}
		}

		public void RemoveUnused(Graph graph)
		{
			HashSet<ulong> hashSet = new HashSet<ulong>(idToExpressions.Keys);
			foreach (IUnit item in graph.AllUnits())
			{
				hashSet.Remove(item.Id);
			}
			foreach (ulong item2 in hashSet)
			{
				idToExpressions.Remove(item2);
			}
		}

		public IEnumerable<IUnit> UnitsByReference(Graph graph, string reference)
		{
			foreach (IUnit item in graph.AllUnits())
			{
				if (!idToExpressions.TryGetValue(item.Id, out var value))
				{
					continue;
				}
				Entry[] array = value;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].calculator.ContainsReference(reference))
					{
						yield return item;
						break;
					}
				}
			}
		}

		public void OnBeforeSerialize()
		{
			serEntries = new Entry[Count];
			int num = 0;
			foreach (Entry item in AllEntries())
			{
				serEntries[num] = item;
				num++;
			}
		}

		public void OnAfterDeserialize()
		{
			idToExpressions.Clear();
			if (serEntries != null)
			{
				Entry[] array = serEntries;
				foreach (Entry entry in array)
				{
					Add(entry);
				}
			}
		}
	}
}
