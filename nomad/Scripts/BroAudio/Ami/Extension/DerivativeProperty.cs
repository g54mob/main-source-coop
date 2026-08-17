using System;
using UnityEngine;

namespace Ami.Extension
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DerivativeProperty : PropertyAttribute
	{
		public bool IsEnd { get; private set; }

		public DerivativeProperty(bool isEnd = false)
		{
			IsEnd = isEnd;
		}
	}
}
