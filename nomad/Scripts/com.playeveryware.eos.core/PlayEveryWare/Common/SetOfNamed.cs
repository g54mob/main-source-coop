using System;
using System.Collections.Generic;

namespace PlayEveryWare.Common
{
	public class SetOfNamed<T> : List<Named<T>> where T : IEquatable<T>, new()
	{
		private readonly string _defaultNamePattern;

		private Func<T, bool> _removePredicate;

		private readonly HashSet<string> _existingNames = new HashSet<string>();

		public SetOfNamed(string defaultNamePattern = "NamedItem")
		{
			_defaultNamePattern = defaultNamePattern;
		}

		public void SetRemovePredicate(Func<T, bool> removePredicate)
		{
			_removePredicate = removePredicate;
		}

		private string GetNewItemName()
		{
			int num = 1;
			int num2 = base.Count;
			string result = _defaultNamePattern;
			while (num <= num2)
			{
				int num3 = num + (num2 - num) / 2;
				string text = $"{_defaultNamePattern} ({num3})";
				if (ContainsName(text))
				{
					num = num3 + 1;
					continue;
				}
				result = text;
				num2 = num3 - 1;
			}
			return result;
		}

		public bool Add(T value = default(T))
		{
			T val = value;
			if (val == null)
			{
				value = (typeof(T).IsValueType ? default(T) : new T());
			}
			string newItemName = GetNewItemName();
			if (ContainsName(newItemName) || ContainsValue(value))
			{
				return false;
			}
			Named<T> named = new Named<T>(newItemName, value);
			named.NameChanged += OnItemNameChanged;
			_existingNames.Add(named.Name);
			Add(named);
			return true;
		}

		private void OnItemNameChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (sender is Named<T> named && !string.Equals(e.OldValue, e.NewValue))
			{
				if (ContainsName(e.NewValue))
				{
					named.TrySetName(e.OldValue, notify: false);
					return;
				}
				_existingNames.Remove(e.OldValue);
				_existingNames.Add(e.NewValue);
			}
		}

		private bool ContainsName(string name)
		{
			return _existingNames.Contains(name);
		}

		private bool ContainsValue(T item)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Value.Equals(item))
					{
						return true;
					}
				}
			}
			return false;
		}

		public new bool Remove(Named<T> item)
		{
			if (_removePredicate != null && !_removePredicate(item.Value))
			{
				return false;
			}
			item.NameChanged -= OnItemNameChanged;
			base.Remove(item);
			return true;
		}
	}
}
