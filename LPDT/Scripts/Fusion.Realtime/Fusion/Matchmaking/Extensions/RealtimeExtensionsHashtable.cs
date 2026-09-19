using System.Collections;
using System.Collections.Generic;
using Photon.Client;

namespace Fusion.Matchmaking.Extensions
{
	internal static class RealtimeExtensionsHashtable
	{
		private static readonly StreamBuffer Buffer = new StreamBuffer(1024);

		private static readonly Protocol18 Protocol = new Protocol18();

		public static Dictionary<string, SessionProperty> ConvertToDictionaryProperty(this PhotonHashtable customProperties)
		{
			Dictionary<string, SessionProperty> dictionary = new Dictionary<string, SessionProperty>();
			foreach (DictionaryEntry customProperty in customProperties)
			{
				if (customProperty.Key is string key && SessionProperty.Support(customProperty.Value))
				{
					dictionary[key] = SessionProperty.Convert(customProperty.Value);
				}
			}
			return dictionary;
		}

		public static PhotonHashtable ConvertToHashtable(this Dictionary<string, SessionProperty> properties)
		{
			PhotonHashtable photonHashtable = new PhotonHashtable();
			foreach (KeyValuePair<string, SessionProperty> property in properties)
			{
				if (property.Key != null && property.Value != null)
				{
					photonHashtable[property.Key] = property.Value.PropertyValue;
				}
			}
			return photonHashtable;
		}

		public static int CalculateTotalSize(this PhotonHashtable hashtable)
		{
			Buffer.Position = 0;
			Protocol.Serialize(Buffer, hashtable, setType: true);
			return Buffer.Position;
		}
	}
}
