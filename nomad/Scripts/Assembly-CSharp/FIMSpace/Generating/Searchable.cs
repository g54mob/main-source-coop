using UnityEngine;

namespace FIMSpace.Generating
{
	public static class Searchable
	{
		public static bool CheckSubType = true;

		private static object choosed = null;

		public static bool IsSetted { get; private set; }

		public static void Choose(object value)
		{
			IsSetted = true;
			choosed = value;
		}

		public static T Get<T>(bool extensiveTypeMatch = false) where T : Object
		{
			if (choosed == null)
			{
				IsSetted = false;
				return null;
			}
			if ((!extensiveTypeMatch) ? (typeof(T) == choosed.GetType()) : ((!CheckSubType) ? (choosed.GetType() == typeof(T)) : choosed.GetType().IsSubclassOf(typeof(T))))
			{
				IsSetted = false;
				T result = (T)choosed;
				choosed = null;
				return result;
			}
			return null;
		}

		public static object Get()
		{
			if (IsSetted)
			{
				IsSetted = false;
				object result = choosed;
				choosed = null;
				return result;
			}
			return null;
		}
	}
}
