using System;
using System.Reflection;

namespace Fusion
{
	internal static class BehaviourCallbackOverrides
	{
		public static bool HasOverride(Type type, string methodName, params Type[] excludedDeclaringTypes)
		{
			MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				return false;
			}
			Type declaringType = method.DeclaringType;
			for (int i = 0; i < excludedDeclaringTypes.Length; i++)
			{
				if (declaringType == excludedDeclaringTypes[i])
				{
					return false;
				}
			}
			return true;
		}

		public static BehaviourCallbackFlags Compute(Type type)
		{
			BehaviourCallbackFlags behaviourCallbackFlags = BehaviourCallbackFlags.None;
			if (HasOverride(type, "FixedUpdateNetwork", typeof(SimulationBehaviour), typeof(NetworkBehaviour)))
			{
				behaviourCallbackFlags |= BehaviourCallbackFlags.FixedUpdateNetwork;
			}
			if (HasOverride(type, "Render", typeof(SimulationBehaviour)))
			{
				behaviourCallbackFlags |= BehaviourCallbackFlags.Render;
			}
			return behaviourCallbackFlags | ComputePreRender(type);
		}

		private static BehaviourCallbackFlags ComputePreRender(Type type)
		{
			MethodInfo method = type.GetMethod("PreRender", BindingFlags.Instance | BindingFlags.NonPublic);
			if (method == null)
			{
				return BehaviourCallbackFlags.None;
			}
			Type declaringType = method.DeclaringType;
			if (declaringType == typeof(SimulationBehaviour))
			{
				return BehaviourCallbackFlags.None;
			}
			if (declaringType == typeof(NetworkBehaviour))
			{
				return NetworkBehaviour.ChangeDetector.HasChangeCallbacks(type) ? BehaviourCallbackFlags.PreRender : BehaviourCallbackFlags.None;
			}
			return BehaviourCallbackFlags.PreRender;
		}
	}
}
