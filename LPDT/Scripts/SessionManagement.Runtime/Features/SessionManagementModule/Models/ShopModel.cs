using Features.NetworkedModelCodegen.Scripts;

namespace Features.SessionManagementModule.Models
{
	[NetworkedModel(ModelScope.Shop, ModelOwnership.Shared)]
	public sealed class ShopModel : NetworkedModelBase
	{
		public Networked<int> Revision { get; } = new Networked<int>();
	}
}
