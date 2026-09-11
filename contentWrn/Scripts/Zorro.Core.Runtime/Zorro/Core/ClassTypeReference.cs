using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zorro.Core
{
	[Serializable]
	public sealed class ClassTypeReference : ISerializationCallbackReceiver
	{
		[SerializeField]
		[FormerlySerializedAs("_classRef")]
		private string classRef;

		private Type type;

		public Type Type
		{
			get
			{
				return type;
			}
			set
			{
				if (value != null && !value.IsClass)
				{
					throw new ArgumentException($"'{value.FullName}' is not a class type.", "value");
				}
				type = value;
				classRef = GetClassRef(value);
			}
		}

		public static string GetClassRef(Type type)
		{
			if (!(type != null))
			{
				return "";
			}
			return type.FullName + ", " + type.Assembly.GetName().Name;
		}

		public ClassTypeReference()
		{
		}

		public ClassTypeReference(string assemblyQualifiedClassName)
		{
			Type = ((!string.IsNullOrEmpty(assemblyQualifiedClassName)) ? Type.GetType(assemblyQualifiedClassName) : null);
		}

		public ClassTypeReference(Type type)
		{
			Type = type;
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (!string.IsNullOrEmpty(classRef))
			{
				type = Type.GetType(classRef);
				if (type == null)
				{
					Debug.LogWarning($"'{classRef}' was referenced but class type was not found.");
				}
			}
			else
			{
				type = null;
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		public static implicit operator string(ClassTypeReference typeReference)
		{
			return typeReference.classRef;
		}

		public static implicit operator Type(ClassTypeReference typeReference)
		{
			return typeReference.Type;
		}

		public static implicit operator ClassTypeReference(Type type)
		{
			return new ClassTypeReference(type);
		}

		public override string ToString()
		{
			if (!(Type != null))
			{
				return "(None)";
			}
			return Type.FullName;
		}
	}
}
