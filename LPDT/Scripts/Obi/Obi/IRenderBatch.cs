using System;

namespace Obi
{
	public interface IRenderBatch : IComparable<IRenderBatch>
	{
		bool TryMergeWith(IRenderBatch other);
	}
}
