using System;
using UnityEngine;

namespace Rewired.Utils.Attributes
{
	public class BitmaskAttribute : PropertyAttribute
	{
		public Type propType;

		public BitmaskAttribute(Type P_0)
		{
			propType = P_0;
		}
	}
}
