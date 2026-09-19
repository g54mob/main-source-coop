using System;
using Features.ProgressSavingModule.Scripts.PlayerInfo;
using UnityEngine;
using Zenject;

namespace Features.GoogleFormModule.Scripts
{
	public class GoogleFormOpenOnApplicationQuiteSystem : IInitializable, IDisposable
	{
		private readonly GoogleFormConfiguration _googleFormConfiguration;

		private readonly PlayerInfoModel _playerInfoModel;

		public GoogleFormOpenOnApplicationQuiteSystem(GoogleFormConfiguration googleFormConfiguration, PlayerInfoModel playerInfoModel)
		{
			_googleFormConfiguration = googleFormConfiguration;
			_playerInfoModel = playerInfoModel;
		}

		public void Initialize()
		{
			Application.quitting += OpenForm;
		}

		public void Dispose()
		{
			Application.quitting -= OpenForm;
		}

		private void OpenForm()
		{
			if (_googleFormConfiguration.IsAutoFormOnQuitEnabled && !Application.isEditor && !IsHarnessRun() && _playerInfoModel.GameLaunchCount == 1)
			{
				Application.OpenURL(_googleFormConfiguration.ExternalLinks[ExternalLinkType.FeedbackLink]);
			}
		}

		private static bool IsHarnessRun()
		{
			return Array.IndexOf(Environment.GetCommandLineArgs(), "-rsaHarness") >= 0;
		}
	}
}
