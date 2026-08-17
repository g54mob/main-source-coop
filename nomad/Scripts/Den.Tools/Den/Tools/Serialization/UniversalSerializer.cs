using UnityEngine;

namespace Den.Tools.Serialization
{
	public class UniversalSerializer
	{
		private string data;

		public void Serialize(object obj)
		{
			data = JsonUtility.ToJson(obj, prettyPrint: true);
		}

		public void Deserialize(object obj)
		{
			JsonUtility.FromJsonOverwrite(data, obj);
		}

		public object Clone(object obj)
		{
			return null;
		}
	}
}
