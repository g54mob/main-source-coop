using System;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	[Serializable]
	public class ScreenSetup
	{
		[SerializeField]
		private LoadingScreenScreenPreset[] _screenPresets = Array.Empty<LoadingScreenScreenPreset>();

		public LoadingScreenScreenPreset[] ScreenPresets => _screenPresets ?? Array.Empty<LoadingScreenScreenPreset>();
	}
}
