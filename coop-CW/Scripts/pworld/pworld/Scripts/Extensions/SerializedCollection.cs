using System;
using System.Collections;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	[Serializable]
	public class SerializedCollection<T> : IEnumerable
	{
		public T[] collection;

		public T this[int key]
		{
			get
			{
				return collection[key];
			}
			set
			{
				collection[key] = value;
			}
		}

		public SerializedCollection(T[] coll)
		{
			collection = coll;
			_ = default(Vector2) == default(Vector2);
		}

		public IEnumerator GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		public static implicit operator T[](SerializedCollection<T> SC)
		{
			return SC.collection;
		}

		public static implicit operator SerializedCollection<T>(T[] col)
		{
			return new SerializedCollection<T>(col);
		}
	}
}
