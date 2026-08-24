using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace HeathenEngineering
{
	[CreateAssetMenu(menuName = "System Core/Application/String Library")]
	public class StringLibrary : ScriptableObject
	{
		public string DisplayName;

		public string ISOCode;

		public Sprite Icon;

		public StringSet Data = new StringSet();

		public void Apply()
		{
			StringFieldManager.ApplyStringSet(Data);
		}

		public void SerializeLanguageSet(string filePath)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(StringFieldDataModel));
			StringFieldDataModel stringFieldDataModel = new StringFieldDataModel();
			stringFieldDataModel.Name = DisplayName;
			stringFieldDataModel.Code = ISOCode;
			stringFieldDataModel.Records = new List<StringFieldRecord>();
			foreach (StringFieldValue value in Data.Values)
			{
				StringFieldRecord item = new StringFieldRecord
				{
					Id = value.Field.Id,
					Value = value.value
				};
				stringFieldDataModel.Records.Add(item);
			}
			FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
			xmlSerializer.Serialize(fileStream, stringFieldDataModel);
			fileStream.Close();
			fileStream.Dispose();
		}

		public void DeserializeLanguageSet(string filePath)
		{
			if (File.Exists(filePath) && StringFieldManager.activeManager != null)
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(StringFieldDataModel));
				FileStream fileStream = new FileStream(filePath, FileMode.Open);
				StringFieldDataModel stringFieldDataModel = xmlSerializer.Deserialize(fileStream) as StringFieldDataModel;
				fileStream.Close();
				fileStream.Dispose();
				DisplayName = stringFieldDataModel.Name;
				ISOCode = stringFieldDataModel.Code;
				List<StringField> list = new List<StringField>(StringFieldManager.activeManager.availableFields);
				list.Sort((StringField a, StringField b) => a.Id.CompareTo(b.Id));
				Data.Values.Clear();
				{
					foreach (StringField field in list)
					{
						StringFieldRecord stringFieldRecord = stringFieldDataModel.Records.FirstOrDefault((StringFieldRecord p) => p.Id == field.Id);
						if (stringFieldRecord != null)
						{
							Data.Values.Add(new StringFieldValue
							{
								Field = field,
								value = stringFieldRecord.Value
							});
						}
						else
						{
							StringFieldValue item = new StringFieldValue
							{
								Field = field,
								value = field.defaultValue
							};
							Data.Values.Add(item);
						}
					}
					return;
				}
			}
			if (!File.Exists(filePath))
			{
				Debug.LogError("Unable to deserialize file [" + filePath + "] no such file exists.");
			}
			if (StringFieldManager.activeManager == null)
			{
				Debug.LogError("Unable to deserialize file [" + filePath + "] there is no active String Field Manager.");
			}
		}
	}
}
