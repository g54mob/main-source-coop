using System;

namespace Photon.Voice
{
	public class MonoPInvokeCallbackAttribute : Attribute
	{
		private Type type;

		public MonoPInvokeCallbackAttribute(Type t)
		{
			type = t;
		}
	}
}
