using Unity.Jobs;

namespace Obi
{
	public class BurstJobHandle : IObiJobHandle
	{
		public JobHandle jobHandle { get; set; }

		public void Complete()
		{
			jobHandle.Complete();
		}

		public void Release()
		{
			jobHandle = default(JobHandle);
		}
	}
}
