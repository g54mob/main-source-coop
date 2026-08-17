using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public struct SandboxId : IEquatable<SandboxId>
	{
		private string _value;

		private const string PreProductionEnvironmentRegex = "^p\\-[a-zA-Z\\d]{30}$";

		public string Value
		{
			readonly get
			{
				return _value;
			}
			set
			{
				if (value == _value || (string.IsNullOrEmpty(_value) && string.IsNullOrEmpty(value)))
				{
					return;
				}
				if (value == null)
				{
					_value = null;
					return;
				}
				string value2 = _value;
				_value = value.ToLower();
				if (!IsValid())
				{
					string text = "Invalid SandboxId: \"" + _value + "\".";
					if (value2 != null)
					{
						text = text + "Restoring to previous value of \"" + value2 + "\".";
					}
					Debug.LogWarning(text);
					_value = value2;
				}
			}
		}

		[JsonIgnore]
		public readonly bool IsEmpty => IsNullOrEmpty(this);

		public bool IsValid()
		{
			if (!Guid.TryParse(_value, out var _))
			{
				return Regex.IsMatch(_value, "^p\\-[a-zA-Z\\d]{30}$");
			}
			return true;
		}

		public static bool IsNullOrEmpty(string sandboxString)
		{
			if (!string.IsNullOrEmpty(sandboxString))
			{
				return Guid.Empty.ToString("N").Equals(sandboxString);
			}
			return true;
		}

		public static bool IsNullOrEmpty(SandboxId sandboxId)
		{
			return IsNullOrEmpty(sandboxId._value);
		}

		public static SandboxId FromString(string sandboxString)
		{
			return new SandboxId
			{
				Value = sandboxString
			};
		}

		public readonly bool Equals(SandboxId other)
		{
			return _value == other._value;
		}

		public override readonly bool Equals(object obj)
		{
			if (obj is SandboxId other)
			{
				return Equals(other);
			}
			return false;
		}

		public override readonly int GetHashCode()
		{
			return _value?.GetHashCode() ?? 0;
		}

		public override readonly string ToString()
		{
			return _value;
		}
	}
}
