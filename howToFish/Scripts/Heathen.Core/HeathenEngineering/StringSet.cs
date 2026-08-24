using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace HeathenEngineering
{
	[Serializable]
	public class StringSet
	{
		public List<StringFieldValue> Values = new List<StringFieldValue>();

		public string GetValue(uint Id)
		{
			StringFieldValue stringFieldValue = Values.FirstOrDefault((StringFieldValue p) => p.Field.Id == Id);
			if (stringFieldValue != null)
			{
				return stringFieldValue.value;
			}
			return string.Empty;
		}

		public static byte[] Serialize(StringSet Library)
		{
			byte[] result = null;
			if (Library != null)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream();
				binaryFormatter.Serialize(memoryStream, Library);
				result = memoryStream.ToArray();
				memoryStream.Dispose();
			}
			return result;
		}

		public static StringSet Deserialize(byte[] Buffer)
		{
			StringSet result = null;
			if (Buffer != null && Buffer.Length != 0)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream(Buffer);
				result = binaryFormatter.Deserialize(memoryStream) as StringSet;
				memoryStream.Dispose();
			}
			return result;
		}
	}
}
