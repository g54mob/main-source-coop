using NomadDrive.Features.Player;

namespace NomadDrive.Features.Restoration
{
	public sealed class ToolInteractionSuppressor
	{
		private bool _suppressed;

		public bool IsSuppressed => _suppressed;

		public void SetSuppressed(IPlayerService playerService, bool suppress)
		{
			if (suppress != _suppressed && playerService != null && playerService.TryGetInteractionManager(out var manager))
			{
				if (suppress)
				{
					manager.DisableInteraction();
				}
				else
				{
					manager.EnableInteraction();
				}
				_suppressed = suppress;
			}
		}
	}
}
