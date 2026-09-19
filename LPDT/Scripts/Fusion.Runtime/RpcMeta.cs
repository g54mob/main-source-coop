using System;
using System.Diagnostics.CodeAnalysis;
using Fusion;

internal readonly struct RpcMeta
{
	public readonly uint Key;

	public readonly Type DeclaringType;

	public readonly int Sources;

	public readonly int Targets;

	public readonly RpcInvokeDelegate Invoke;

	public RpcMeta(uint key, [NotNull] Type declaringType, int sources, int targets, RpcInvokeDelegate invoke)
	{
		Key = key;
		DeclaringType = declaringType;
		Sources = sources;
		Targets = targets;
		Invoke = invoke;
	}
}
