using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zorro.Core
{
	public abstract class DatabaseAsset<T, ObjectT> : SingletonAsset<T> where T : DatabaseAsset<T, ObjectT> where ObjectT : class
	{
		public List<ObjectT> Objects = new List<ObjectT>();

		public virtual void AddRuntimeEntry(ObjectT obj)
		{
			Objects.Add(obj);
		}

		public static ObjectT GetRandomEntry()
		{
			return SingletonAsset<T>.Instance.Objects[Random.Range(0, SingletonAsset<T>.Instance.Objects.Count)];
		}

		public static EntryType GetRandomEntry<EntryType>() where EntryType : ObjectT
		{
			EntryType[] array = SingletonAsset<T>.Instance.Objects.Where((ObjectT t) => t is EntryType).Cast<EntryType>().ToArray();
			return array[Random.Range(0, array.Length)];
		}
	}
}
