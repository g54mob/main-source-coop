using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class ComponentTypeReference
	{
		[SerializeField]
		[Tooltip("Full type name (e.g. NomadDrive.Features.Cooking.Pan) or assembly-qualified name.")]
		private string _typeName;

		private static readonly Dictionary<string, Type> _typeCache = new Dictionary<string, Type>();

		public string TypeName => _typeName;

		public bool IsAssigned => !string.IsNullOrEmpty(_typeName);

		public Type ResolvedType
		{
			get
			{
				if (string.IsNullOrEmpty(_typeName))
				{
					return null;
				}
				if (_typeCache.TryGetValue(_typeName, out var value))
				{
					return value;
				}
				Type type = Type.GetType(_typeName);
				if (type == null)
				{
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
					for (int i = 0; i < assemblies.Length; i++)
					{
						type = assemblies[i].GetType(_typeName);
						if (type != null)
						{
							break;
						}
					}
				}
				_typeCache[_typeName] = type;
				return type;
			}
		}

		public bool IsMatch(GameObject go)
		{
			if (go == null)
			{
				return false;
			}
			Type resolvedType = ResolvedType;
			if (resolvedType == null)
			{
				return false;
			}
			return go.GetComponent(resolvedType) != null;
		}

		public bool IsMatch(Component component)
		{
			if (component != null)
			{
				return IsMatch(component.gameObject);
			}
			return false;
		}
	}
}
