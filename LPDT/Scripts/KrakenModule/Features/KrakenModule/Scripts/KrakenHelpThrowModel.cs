using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.KrakenModule.Scripts
{
	public class KrakenHelpThrowModel : ILevelCleanup
	{
		private readonly KrakenHelpThrowConfiguration _configuration;

		private int _helpThrowsUsed;

		public int HelpThrowsUsed => _helpThrowsUsed;

		public int RemainingHelpThrows
		{
			get
			{
				if (!(_configuration == null))
				{
					return Mathf.Max(0, _configuration.MaxHelpDeadPartThrowsPerLevel - _helpThrowsUsed);
				}
				return 0;
			}
		}

		public KrakenHelpThrowModel(KrakenHelpThrowConfiguration configuration)
		{
			_configuration = configuration;
		}

		public bool TryConsumeHelpThrow()
		{
			if (RemainingHelpThrows <= 0)
			{
				return false;
			}
			_helpThrowsUsed++;
			return true;
		}

		public void Cleanup()
		{
			_helpThrowsUsed = 0;
		}
	}
}
