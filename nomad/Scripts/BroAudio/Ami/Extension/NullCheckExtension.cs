using UnityEngine;

namespace Ami.Extension
{
	public static class NullCheckExtension
	{
		public static MonoBehaviour CheckNull(this MonoBehaviour mono)
		{
			if ((bool)mono)
			{
				return mono;
			}
			return null;
		}
	}
}
