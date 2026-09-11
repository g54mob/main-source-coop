using System;
using System.Reflection;
using Sirenix.Serialization;
using UnityEngine.Localization;

namespace Sirenix.OdinInspector.Modules.Localization
{
	public class LocalizedStringFormatter : ReflectionOrEmittedBaseFormatter<LocalizedString>
	{
		private static readonly FieldInfo m_LocalVariables_Field;

		static LocalizedStringFormatter()
		{
			m_LocalVariables_Field = typeof(LocalizedString).GetField("m_LocalVariables", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (m_LocalVariables_Field == null)
			{
				DefaultLoggers.DefaultLogger.LogError("Could not find field 'UnityEngine.LocalizedString.m_LocalVariables' - the internals of the Localization package have changed, and deserialization of Odin-serialized LocalizedString instances may be broken in some cases.");
			}
		}

		protected override LocalizedString GetUninitializedObject()
		{
			return new LocalizedString();
		}

		protected override void DeserializeImplementation(ref LocalizedString value, IDataReader reader)
		{
			base.DeserializeImplementation(ref value, reader);
			if (m_LocalVariables_Field != null && value != null)
			{
				object value2 = m_LocalVariables_Field.GetValue(value);
				if (value2 == null)
				{
					value2 = Activator.CreateInstance(m_LocalVariables_Field.FieldType);
					m_LocalVariables_Field.SetValue(value, value2);
				}
			}
		}
	}
}
