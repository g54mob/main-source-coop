using System.Text;
using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	public static class StaticInteractableId
	{
		private const uint FnvOffsetBasis = 2166136261u;

		private const uint FnvPrime = 16777619u;

		public static int String(string value)
		{
			uint num = 2166136261u;
			if (value != null)
			{
				foreach (uint num2 in value)
				{
					num = (num ^ (num2 & 0xFF)) * 16777619;
					num = (num ^ ((num2 >> 8) & 0xFF)) * 16777619;
				}
			}
			return (int)num;
		}

		public static int Position(Vector3 position, float precision = 0.1f)
		{
			int value = Mathf.RoundToInt(position.x / precision);
			int value2 = Mathf.RoundToInt(position.y / precision);
			int value3 = Mathf.RoundToInt(position.z / precision);
			return (int)HashInt(HashInt(HashInt(2166136261u, value), value2), value3);
		}

		public static int Combine(int a, int b)
		{
			return (int)HashInt((uint)a, b);
		}

		public static string SiblingIndexPath(Transform target, Transform anchor)
		{
			if (target == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			BuildPath(target, anchor, stringBuilder);
			return stringBuilder.ToString();
		}

		private static void BuildPath(Transform current, Transform anchor, StringBuilder sb)
		{
			if (!(current == null) && !(current == anchor))
			{
				BuildPath(current.parent, anchor, sb);
				if (sb.Length > 0)
				{
					sb.Append('/');
				}
				sb.Append(current.GetSiblingIndex());
			}
		}

		private static uint HashInt(uint hash, int value)
		{
			hash = (hash ^ (uint)(value & 0xFF)) * 16777619;
			hash = (hash ^ (((uint)value >> 8) & 0xFF)) * 16777619;
			hash = (hash ^ (((uint)value >> 16) & 0xFF)) * 16777619;
			hash = (hash ^ (((uint)value >> 24) & 0xFF)) * 16777619;
			return hash;
		}
	}
}
