using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWH.Common.Input
{
	public abstract class InputProvider : MonoBehaviour
	{
		public static List<InputProvider> Instances = new List<InputProvider>();

		protected static int InstanceCount;

		public virtual void Awake()
		{
			Instances.Add(this);
			InstanceCount++;
		}

		public virtual void OnDestroy()
		{
			Instances.Remove(this);
			InstanceCount--;
		}

		public static int CombinedInput<T>(Func<T, int> selector) where T : InputProvider
		{
			int num = 0;
			for (int i = 0; i < InstanceCount; i++)
			{
				if (Instances[i] is T arg)
				{
					num += selector(arg);
				}
			}
			return num;
		}

		public static float CombinedInput<T>(Func<T, float> selector) where T : InputProvider
		{
			float num = 0f;
			for (int i = 0; i < InstanceCount; i++)
			{
				if (Instances[i] is T arg)
				{
					num += selector(arg);
				}
			}
			return num;
		}

		public static bool CombinedInput<T>(Func<T, bool> selector) where T : InputProvider
		{
			for (int i = 0; i < InstanceCount; i++)
			{
				if (Instances[i] is T arg && selector(arg))
				{
					return true;
				}
			}
			return false;
		}

		public static Vector2 CombinedInput<T>(Func<T, Vector2> selector) where T : InputProvider
		{
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < InstanceCount; i++)
			{
				if (Instances[i] is T arg)
				{
					zero += selector(arg);
				}
			}
			return zero;
		}
	}
}
