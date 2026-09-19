using System;

namespace Photon.Voice
{
	public interface ObjectFactory<TType, TInfo> : IDisposable
	{
		TType New();

		TType New(TInfo info);

		bool Free(TType obj);

		bool Free(TType obj, TInfo info);
	}
}
