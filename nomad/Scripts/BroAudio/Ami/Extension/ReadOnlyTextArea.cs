using System;
using UnityEngine;

namespace Ami.Extension
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ReadOnlyTextArea : PropertyAttribute
	{
		public readonly bool ReadOnly;

		public ReadOnlyTextArea(bool readOnly)
		{
			ReadOnly = readOnly;
		}
	}
}
