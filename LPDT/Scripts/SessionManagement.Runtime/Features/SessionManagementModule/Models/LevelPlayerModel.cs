using Features.NetworkedModelCodegen.Scripts;

namespace Features.SessionManagementModule.Models
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Individual)]
	public sealed class LevelPlayerModel : NetworkedModelBase
	{
		public Networked<int> OwnerSlot { get; } = new Networked<int>();
	}
}
