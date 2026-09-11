using System;
using EasyTextEffects.Editor.MyBoxCopy.Tools.Internal;
using UnityEngine;

namespace EasyTextEffects.Editor.MyBoxCopy.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ConditionalFieldAttribute : PropertyAttribute
	{
		public readonly ConditionalData Data;

		public bool IsSet
		{
			get
			{
				if (Data != null)
				{
					return Data.IsSet;
				}
				return false;
			}
		}

		public ConditionalFieldAttribute(string fieldToCheck, bool inverse = false, params object[] compareValues)
		{
			Data = new ConditionalData(fieldToCheck, inverse, compareValues);
		}

		public ConditionalFieldAttribute(string[] fieldToCheck, bool[] inverse = null, params object[] compare)
		{
			Data = new ConditionalData(fieldToCheck, inverse, compare);
		}

		public ConditionalFieldAttribute(params string[] fieldToCheck)
		{
			Data = new ConditionalData(fieldToCheck);
		}

		public ConditionalFieldAttribute(bool useMethod, string method, bool inverse = false)
		{
			Data = new ConditionalData(useMethod, method, inverse);
		}
	}
}
