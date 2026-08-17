using System;
using System.Collections.Generic;
using System.Reflection;

namespace Den.Tools.GUI
{
	public sealed class ValAttribute : Attribute
	{
		public string name;

		public string cat;

		public bool display = true;

		public bool isLeft;

		public int priority;

		public Type type;

		public bool allowSceneObject;

		public float min = -3.4028235E+38f;

		public float max = 3.4028235E+38f;

		public FieldInfo field;

		public PropertyInfo prop;

		public MethodInfo method;

		[NonSerialized]
		private static readonly Dictionary<Type, ValAttribute[]> attributesCaches = new Dictionary<Type, ValAttribute[]>();

		public ValAttribute()
		{
		}

		public ValAttribute(string name)
		{
			this.name = name;
		}

		public ValAttribute(string name, string cat)
		{
			this.name = name;
			this.cat = cat;
		}

		public ValAttribute(string name, float min, float max)
		{
			this.name = name;
			this.min = min;
			this.max = max;
		}

		public ValAttribute(string name, float min)
		{
			this.name = name;
			this.min = min;
		}

		public ValAttribute(string name, string cat, float min, float max)
		{
			this.name = name;
			this.cat = cat;
			this.min = min;
			this.max = max;
		}

		public static ValAttribute[] GetAttributes(Type type)
		{
			if (attributesCaches.TryGetValue(type, out var value))
			{
				return value;
			}
			List<ValAttribute> list = new List<ValAttribute>();
			FieldInfo[] fields = type.GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				if (Attribute.GetCustomAttribute(fields[i], typeof(ValAttribute)) is ValAttribute valAttribute)
				{
					valAttribute.field = fields[i];
					valAttribute.type = fields[i].FieldType;
					list.Add(valAttribute);
				}
			}
			PropertyInfo[] properties = type.GetProperties();
			for (int j = 0; j < properties.Length; j++)
			{
				if (Attribute.GetCustomAttribute(properties[j], typeof(ValAttribute)) is ValAttribute valAttribute2)
				{
					valAttribute2.prop = properties[j];
					valAttribute2.type = properties[j].PropertyType;
					list.Add(valAttribute2);
				}
			}
			MethodInfo[] methods = type.GetMethods();
			for (int k = 0; k < methods.Length; k++)
			{
				if (Attribute.GetCustomAttribute(methods[k], typeof(ValAttribute)) is ValAttribute valAttribute3)
				{
					valAttribute3.method = methods[k];
					list.Add(valAttribute3);
				}
			}
			value = list.ToArray();
			attributesCaches.Add(type, value);
			return value;
		}
	}
}
