using System;
using Global.Modules.LocalizationModule.Scripts.Generated;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	[Serializable]
	public class LoadingScreenTipEntry
	{
		[SerializeField]
		private LocalizationKey _localizationKey;

		[SerializeField]
		private float _tipsShowingTime = 3f;

		public LocalizationKey LocalizationKey => _localizationKey;

		public float TipsShowingTime => _tipsShowingTime;
	}
}
