using System;
using System.Reflection;
using UnityEngine;

namespace Ami.Extension
{
	public class CustomDrawingMethod : PropertyAttribute
	{
		public MethodInfo Method;

		public CustomDrawingMethod(Type type, string method)
		{
			Method = type.GetMethod(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
	}
}
