using Features.NetworkedModelCodegen.Scripts;

namespace Features.NetworkedCodegenProbeModule.Data
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Individual)]
	public class ProbeFlagModel : NetworkedModelBase
	{
		public Networked<float> Level { get; } = new Networked<float>();
	}
}
