using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace QFSW.QC
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
	public sealed class CommandPrefixAttribute : Attribute
	{
		public readonly string Prefix;

		public readonly bool Valid = true;

		private static readonly char[] _bannedAliasChars = new char[9] { ' ', '(', ')', '{', '}', '[', ']', '<', '>' };

		public CommandPrefixAttribute([CallerMemberName] string prefixName = "")
		{
			Prefix = prefixName;
			char[] bannedAliasChars = _bannedAliasChars;
			foreach (char c in bannedAliasChars)
			{
				if (Prefix.Contains(c))
				{
					string message = $"Development Processor Error: Command prefix '{Prefix}' contains the char '{c}' which is banned. Unexpected behaviour may occurr.";
					Debug.LogError(message);
					Valid = false;
					throw new ArgumentException(message, "prefixName");
				}
			}
		}
	}
}
