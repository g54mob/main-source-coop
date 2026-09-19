using System;
using System.Threading.Tasks;

namespace Fusion
{
	[Serializable]
	public abstract class FusionMppmCommand
	{
		public virtual bool NeedsAck => false;

		public virtual string PersistentKey => null;

		public virtual void Execute()
		{
		}

		public virtual Task ExecuteAsync()
		{
			Execute();
			return Task.CompletedTask;
		}
	}
}
