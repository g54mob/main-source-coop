using Features.NetworkedModelCodegen.Scripts;

namespace Features.SessionManagementModule.Models
{
	[NetworkedModel(ModelScope.Shop, ModelOwnership.Individual)]
	public sealed class ShopVoteModel : NetworkedModelBase
	{
		public Networked<bool> HasVotedToLeave { get; } = new Networked<bool>();
	}
}
