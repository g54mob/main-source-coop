using System;
using System.Diagnostics.CodeAnalysis;

namespace Fusion
{
	internal readonly struct RpcMeta
	{
		public readonly uint Key;

		public readonly Type DeclaringType;

		public readonly RpcSources Sources;

		public readonly RpcTargets Targets;

		public readonly RpcInvokeDelegate Invoke;

		public readonly bool IsTickAligned;

		public readonly RpcChannel Channel;

		public readonly RpcInvokeLocalMode LocalInvoke;

		public RpcMeta(uint key, [NotNull] Type declaringType, RpcSources sources, RpcTargets targets, RpcChannel channel, RpcInvokeLocalMode localInvoke, bool tickAligned, RpcInvokeDelegate invoke)
		{
			Key = key;
			DeclaringType = declaringType;
			Sources = sources;
			Targets = targets;
			Invoke = invoke;
			IsTickAligned = tickAligned;
			Channel = channel;
			LocalInvoke = localInvoke;
		}

		internal bool IsInstanceOfDeclaringType(NetworkBehaviour obj)
		{
			if (!DeclaringType.IsGenericTypeDefinition)
			{
				return DeclaringType.IsInstanceOfType(obj);
			}
			Type type = obj.GetType();
			while (type != null && type != typeof(SimulationBehaviour))
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == DeclaringType)
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
	}
}
