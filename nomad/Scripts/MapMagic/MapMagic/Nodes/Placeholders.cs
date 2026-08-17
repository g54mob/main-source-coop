using System;
using System.Collections.Generic;
using Den.Tools;
using MapMagic.Products;

namespace MapMagic.Nodes
{
	public static class Placeholders
	{
		[Serializable]
		public class SerObject
		{
			public string type;

			public string[] fields;

			public Serializer.Value[] values;

			public static explicit operator Serializer.Object(SerObject src)
			{
				return new Serializer.Object
				{
					type = src.type,
					fields = src.fields,
					values = src.values
				};
			}

			public static explicit operator SerObject(Serializer.Object src)
			{
				return new SerObject
				{
					type = src.type,
					fields = src.fields,
					values = src.values
				};
			}
		}

		public abstract class GenericPlaceholder : Generator, Serializer.ICustomSerialization
		{
			public string origType;

			[NonSerialized]
			public string[] origFields = new string[0];

			[NonSerialized]
			public Serializer.Value[] origValues = new Serializer.Value[0];

			public override void Generate(TileData data, StopToken stop)
			{
			}

			public void PreprocessBeforeDeserialize(Serializer.Object serObj, Serializer.Object[] allSerialized, object[] allDeserialized)
			{
				origType = serObj.type;
				List<string> list = new List<string>();
				List<Serializer.Value> list2 = new List<Serializer.Value>();
				for (int i = 0; i < serObj.values.Length; i++)
				{
					if (serObj.values[i].t != 255)
					{
						list.Add(serObj.fields[i]);
						list2.Add(serObj.values[i]);
					}
				}
				origFields = list.ToArray();
				origValues = list2.ToArray();
			}

			public void PostprocessAfterSerialize(Serializer.Object serObj, Dictionary<object, Serializer.Object> allSerialized)
			{
				serObj.type = origType;
				ArrayTools.Append(ref serObj.fields, origFields);
				ArrayTools.Append(ref serObj.values, origValues);
			}
		}

		[GeneratorMenu(name = "Unknown", iconName = "GeneratorIcons/Generator")]
		public class InletOutletPlaceholder : GenericPlaceholder, IInlet<object>, IUnit, IOutlet<object>
		{
		}

		[GeneratorMenu(name = "Unknown", iconName = "GeneratorIcons/Generator")]
		public class InletPlaceholder : GenericPlaceholder, IInlet<object>, IUnit
		{
		}

		[GeneratorMenu(name = "Unknown", iconName = "GeneratorIcons/Generator")]
		public class OutletPlaceholder : GenericPlaceholder, IOutlet<object>, IUnit
		{
		}

		[GeneratorMenu(name = "Unknown", iconName = "GeneratorIcons/Generator")]
		public class Placeholder : GenericPlaceholder
		{
		}

		public static bool IsInletType(Type type)
		{
			return type.GetInterfaces().Find((Type i) => i.GetGenericTypeDefinition() == typeof(IInlet<>)) >= 0;
		}
	}
}
