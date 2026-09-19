using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NetworkedModelCodegen.Scripts
{
	public sealed class Networked<T>
	{
		private T _value;

		private Func<T, bool> _writer;

		public T Value
		{
			get
			{
				if (_writer == null)
				{
					return default(T);
				}
				return _value;
			}
			set
			{
				Write(value);
			}
		}

		public event Action<T> Changed;

		public void BindWriter(Func<T, bool> writer)
		{
			_writer = writer;
		}

		public void ApplyFromNetwork(T value)
		{
			if (!EqualityComparer<T>.Default.Equals(_value, value))
			{
				_value = value;
				this.Changed?.Invoke(value);
			}
		}

		private void Write(T value)
		{
			if (!EqualityComparer<T>.Default.Equals(_value, value))
			{
				if (_writer == null)
				{
					Debug.LogError("[NetworkedModel] wrote a networked cell whose shadow is not attached — out of scope, or before the owning scope finished attaching. The write was dropped. Open and await the owning scope before writing.");
				}
				else if (_writer(value))
				{
					_value = value;
					this.Changed?.Invoke(value);
				}
			}
		}
	}
}
