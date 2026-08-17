using System;
using System.Collections.Generic;
using System.Reflection;
using MapMagic.Nodes;
using MapMagic.Nodes.MatrixGenerators;

namespace MapMagic.Expose
{
	public static class Assigner
	{
		public static IUnit CopyAndAssign(IUnit unit, Exposed exp, Override ovd)
		{
			IUnit unit2 = unit.ShallowCopy();
			AssignOverrideToFields(unit2, exp, ovd);
			if (unit2 is IBiome { Override: not null } biome)
			{
				biome.Override = new Override(biome.Override);
				AssignOverrideToOverride(exp.EntriesById(unit.Id), biome.Override, ovd);
			}
			if (unit2 is IMultiLayer multiLayer)
			{
				bool flag = false;
				foreach (IUnit layer in multiLayer.Layers)
				{
					if (IsExposed(layer, exp))
					{
						flag = true;
					}
				}
				if (flag)
				{
					IList<IUnit> layers = multiLayer.Layers;
					IUnit[] array = new IUnit[layers.Count];
					layers.CopyTo(array, 0);
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = CopyAndAssign(array[i], exp, ovd);
					}
					multiLayer.Layers = array;
				}
			}
			return unit2;
		}

		public static bool IsExposed(IUnit unit, Exposed exp)
		{
			if (exp == null || exp.Count == 0)
			{
				return false;
			}
			if (exp.Contains(unit.Id))
			{
				return true;
			}
			if (unit is IMultiLayer multiLayer)
			{
				foreach (IUnit layer in multiLayer.Layers)
				{
					if (IsExposed(layer, exp))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static void AssignOverrideToFields(IUnit unit, Exposed exp, Override ovd)
		{
			Type type = unit.GetType();
			foreach (Exposed.Entry item in exp.EntriesById(unit.Id))
			{
				FieldInfo field = type.GetField(item.name);
				if (!(field == null))
				{
					if (field.FieldType != item.type && !field.FieldType.IsArray)
					{
						throw new Exception("Recorded field type doesn't match generator field type. Possibly generator has changed.");
					}
					Calculator.Vector vector = item.calculator.Calculate(ovd);
					object obj;
					if (item.channel == -1)
					{
						obj = vector.Convert(item.type);
					}
					else
					{
						object value = field.GetValue(unit);
						obj = vector.ConvertToChannel(value, item.channel, item.type);
					}
					if (item.arrIndex == -1)
					{
						field.SetValue(unit, obj);
						continue;
					}
					if (unit is Blend200 blend)
					{
						blend.layers[item.arrIndex].opacity = (float)obj;
						continue;
					}
					Array array = (Array)field.GetValue(unit);
					array.SetValue(obj, item.arrIndex);
					field.SetValue(unit, array);
				}
			}
		}

		private static void AssignOverrideToOverride(IEnumerable<Exposed.Entry> entries, Override ovdBase, Override ovdOverride)
		{
			foreach (Exposed.Entry entry in entries)
			{
				if (ovdBase.TryGetValue(entry.name, out var type, out var obj) && !(type != entry.type))
				{
					Calculator.Vector vector = entry.calculator.Calculate(ovdOverride);
					object value = ((entry.channel != -1) ? vector.ConvertToChannel(obj, entry.channel, entry.type) : vector.Convert(entry.type));
					ovdBase[entry.name] = value;
				}
			}
		}
	}
}
