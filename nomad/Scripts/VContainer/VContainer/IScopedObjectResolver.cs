using System;

namespace VContainer
{
	public interface IScopedObjectResolver : IObjectResolver, IDisposable
	{
		IObjectResolver Root { get; }

		IScopedObjectResolver Parent { get; }
	}
}
