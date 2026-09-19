using Features.NetworkedModelCodegen.Scripts;

namespace Features.SessionManagementModule.Models
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Shared)]
	public sealed class LevelModel : NetworkedModelBase
	{
		public Networked<int> LevelId { get; } = new Networked<int>();

		public Networked<int> BellStrikeCount { get; } = new Networked<int>();

		public Networked<int> CountdownStartTick { get; } = new Networked<int>();
	}
}
