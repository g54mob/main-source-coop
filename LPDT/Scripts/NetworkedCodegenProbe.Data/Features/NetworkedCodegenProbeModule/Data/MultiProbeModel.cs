using Features.NetworkedModelCodegen.Scripts;

namespace Features.NetworkedCodegenProbeModule.Data
{
	[NetworkedModel(ModelScope.Session, ModelOwnership.Shared)]
	public class MultiProbeModel : NetworkedModelBase
	{
		public Networked<int> Count { get; } = new Networked<int>();

		public Networked<long> BigScore { get; } = new Networked<long>();

		public Networked<short> SmallCount { get; } = new Networked<short>();

		public Networked<byte> Tier { get; } = new Networked<byte>();

		public Networked<float> Charge { get; } = new Networked<float>();

		public Networked<double> Precision { get; } = new Networked<double>();

		public Networked<bool> IsReady { get; } = new Networked<bool>();

		public Networked<ProbePhase> Phase { get; } = new Networked<ProbePhase>();

		public int FusionUpdateTicks { get; private set; }

		protected override void OnFusionUpdate()
		{
			FusionUpdateTicks++;
		}
	}
}
