using Features.NetworkedModelCodegen.Scripts;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data
{
	[NetworkedModel(ModelScope.Run, ModelOwnership.Shared)]
	public sealed class MonkeyPorterRunModel : NetworkedModelBase
	{
		public Networked<bool> IsOwned { get; } = new Networked<bool>();
	}
}
