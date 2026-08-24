using System;
using System.Collections.Generic;

namespace HeathenEngineering
{
	public abstract class VariableReference<T> : VariableReference, IEquatable<T>, IEquatable<VariableReference<T>>
	{
		public T m_constantValue;

		public abstract IDataVariable<T> m_variable { get; }

		public T Value
		{
			get
			{
				if (Mode == VariableReferenceType.Referenced)
				{
					if (m_variable != null)
					{
						return m_variable.Value;
					}
					throw new NullReferenceException("Failed to get variable reference, mode Referenced requires a valid DataVariable be available, no variable found.");
				}
				return m_constantValue;
			}
			set
			{
				if (Mode == VariableReferenceType.Static)
				{
					return;
				}
				if (Mode == VariableReferenceType.Referenced)
				{
					if (m_variable == null)
					{
						throw new NullReferenceException("Failed to set variable reference, mode Referenced requires a valid DataVariable be available, no variable found.");
					}
					m_variable.SetValue(value);
				}
				else
				{
					m_constantValue = value;
				}
			}
		}

		public VariableReference(T value)
		{
			Mode = VariableReferenceType.Constant;
			m_constantValue = value;
		}

		public static implicit operator T(VariableReference<T> reference)
		{
			return reference.Value;
		}

		public static bool operator ==(VariableReference<T> a, VariableReference<T> b)
		{
			return EqualityComparer<T>.Default.Equals(a.Value, b.Value);
		}

		public static bool operator ==(VariableReference<T> a, T b)
		{
			return EqualityComparer<T>.Default.Equals(a.Value, b);
		}

		public static bool operator ==(T a, VariableReference<T> b)
		{
			return EqualityComparer<T>.Default.Equals(a, b.Value);
		}

		public static bool operator !=(VariableReference<T> a, VariableReference<T> b)
		{
			return !EqualityComparer<T>.Default.Equals(a.Value, b.Value);
		}

		public static bool operator !=(VariableReference<T> a, T b)
		{
			return !EqualityComparer<T>.Default.Equals(a.Value, b);
		}

		public static bool operator !=(T a, VariableReference<T> b)
		{
			return !EqualityComparer<T>.Default.Equals(a, b.Value);
		}

		public bool Equals(T other)
		{
			return this == other;
		}

		public bool Equals(VariableReference<T> other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return -1937169414 + EqualityComparer<T>.Default.GetHashCode(Value);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
	public abstract class VariableReference
	{
		public VariableReferenceType Mode;
	}
}
