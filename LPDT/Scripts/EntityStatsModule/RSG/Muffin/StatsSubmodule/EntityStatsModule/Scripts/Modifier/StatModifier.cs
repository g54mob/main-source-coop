using System;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier
{
	public class StatModifier : IEquatable<StatModifier>
	{
		public ModifierType ModifierType { get; }

		public float Value { get; }

		public int Order { get; }

		public StatModifier(float value, ModifierType modifierType, int order)
		{
			Value = value;
			ModifierType = modifierType;
			Order = order;
		}

		public StatModifier(float value, ModifierType modifierType)
			: this(value, modifierType, (int)modifierType)
		{
		}

		public bool Equals(StatModifier other)
		{
			if ((object)other == null)
			{
				return false;
			}
			if ((object)this == other)
			{
				return true;
			}
			if (ModifierType == other.ModifierType && Value.Equals(other.Value))
			{
				return Order == other.Order;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((StatModifier)obj);
		}

		public override int GetHashCode()
		{
			return ((((int)ModifierType * 397) ^ Value.GetHashCode()) * 397) ^ Order;
		}

		public static bool operator ==(StatModifier left, StatModifier right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(StatModifier left, StatModifier right)
		{
			return !object.Equals(left, right);
		}
	}
}
