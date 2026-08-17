using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace QFSW.QC
{
	[Serializable]
	public abstract class TypeFormatter : ISerializationCallbackReceiver
	{
		[SerializeField]
		[HideInInspector]
		private string _type;

		public Type Type { get; private set; }

		[Preserve]
		protected TypeFormatter(Type type)
		{
			Type = type;
		}

		public void OnAfterDeserialize()
		{
			Type = Type.GetType(_type, throwOnError: false);
			if (Type == null)
			{
				Type = QuantumParser.ParseType(_type.Split(',')[0]);
			}
		}

		public void OnBeforeSerialize()
		{
			if (Type != null)
			{
				_type = Type.AssemblyQualifiedName;
			}
		}
	}
}
