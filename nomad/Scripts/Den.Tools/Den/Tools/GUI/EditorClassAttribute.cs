using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools.GUI
{
	public sealed class EditorClassAttribute : Attribute
	{
		public static Dictionary<Type, Type> editors = new Dictionary<Type, Type>();

		public EditorClassAttribute(Type type)
		{
			Debug.Log(type);
		}
	}
}
