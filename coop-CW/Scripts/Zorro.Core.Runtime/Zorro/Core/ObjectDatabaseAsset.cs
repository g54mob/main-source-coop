using System.Collections.Generic;
using UnityEngine;
using Zorro.Core.CLI;

namespace Zorro.Core
{
	public abstract class ObjectDatabaseAsset<T, ObjectT> : DatabaseAsset<T, ObjectT> where T : DatabaseAsset<T, ObjectT> where ObjectT : Object
	{
		public static ObjectT GetObjectFromString(string inString)
		{
			foreach (ObjectT @object in SingletonAsset<T>.Instance.Objects)
			{
				if (@object.name.WithoutWhitespace() == inString)
				{
					return @object;
				}
			}
			return null;
		}

		public static string[] GetAllObjectNames()
		{
			int count = SingletonAsset<T>.Instance.Objects.Count;
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = SingletonAsset<T>.Instance.Objects[i].name;
			}
			return array;
		}

		public List<ParameterAutocomplete> GetAutocompleteOptions(string parameterText)
		{
			List<ParameterAutocomplete> list = new List<ParameterAutocomplete>();
			foreach (ObjectT @object in SingletonAsset<T>.Instance.Objects)
			{
				if (!(@object == null) && @object.name.ToLower().StartsWith(parameterText.ToLower()))
				{
					list.Add(new ParameterAutocomplete(@object.name.WithoutWhitespace()));
				}
			}
			return list;
		}
	}
}
