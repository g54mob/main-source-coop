using System;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	[CreateAssetMenu(fileName = "LoadingScreenSettings_default", menuName = "Configurations/SceneTransitions/LoadingScreenSettings")]
	public class LoadingScreenSettings : ScriptableObject
	{
		[SerializeField]
		private LoadingScreenPreset[] _presets = Array.Empty<LoadingScreenPreset>();

		[SerializeField]
		private ScreenSetup _screenSetup = new ScreenSetup();

		public LoadingScreenPreset[] Presets => _presets;

		public ScreenSetup ScreenSetup => _screenSetup;
	}
}
