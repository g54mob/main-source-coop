using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;

namespace NomadDrive.Features.Objectives
{
	public class ObjectiveTriggerContext
	{
		public IInteractionManager InteractionManager;

		public IEquipmentManager EquipmentManager;

		public IPlayerService PlayerService;

		public IObjectivesService ObjectivesService;
	}
}
