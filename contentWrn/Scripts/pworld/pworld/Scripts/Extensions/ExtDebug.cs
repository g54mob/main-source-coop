using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtDebug
	{
		public static void PDebug(this Vector3 me, string pretext = "")
		{
			Vector3 vector = me;
			Debug.Log(pretext + vector.ToString());
		}

		public static void PDebug(this bool me, string preText = "")
		{
			Debug.Log(preText + me);
		}

		public static void PDebug(this string me, string preText = "")
		{
			Debug.Log(preText + me);
		}

		public static void PDebug(this float me, string preText = "")
		{
			Debug.Log(preText + me);
		}

		public static void PDebug<T>(this T me, string preText = "")
		{
			T val = me;
			Debug.Log(preText + val);
		}

		public static void PDraw(this Vector3 me, Vector3 start, float mul = 1f, Color? c = null)
		{
			Debug.DrawLine(start, start + me * mul, c ?? Color.green);
		}
	}
}
