using System;
using System.Collections.Generic;
using PlayEveryWare.EpicOnlineServices.Utility;

namespace PlayEveryWare.Common
{
	public class Named<T> : IEquatable<Named<T>>, IComparable<Named<T>>, IEquatable<T> where T : IEquatable<T>
	{
		public T Value;

		public string Name { get; private set; }

		public event EventHandler<ValueChangedEventArgs<string>> NameChanged;

		public Named(string name, T value)
		{
			Name = name;
			Value = value;
		}

		public bool TrySetName(string newName, bool notify = true)
		{
			if (string.Equals(Name, newName))
			{
				return true;
			}
			string name = Name;
			Name = newName;
			if (notify)
			{
				this.NameChanged?.Invoke(this, new ValueChangedEventArgs<string>(name, newName));
			}
			return string.Equals(Name, newName);
		}

		public int CompareTo(Named<T> other)
		{
			return string.CompareOrdinal(Name, other.Name);
		}

		public bool Equals(T other)
		{
			if (Value == null && other == null)
			{
				return true;
			}
			if (Value != null)
			{
				return Value.Equals(other);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Named<T> other)
			{
				return Equals(other);
			}
			return false;
		}

		public bool Equals(Named<T> other)
		{
			if (other == null)
			{
				return false;
			}
			if (this != other)
			{
				return EqualityComparer<T>.Default.Equals(Value, other.Value);
			}
			return true;
		}

		public override int GetHashCode()
		{
			return HashUtility.Combine(Value);
		}

		public override string ToString()
		{
			return $"\"{Name}\" : ({Value})";
		}
	}
}
