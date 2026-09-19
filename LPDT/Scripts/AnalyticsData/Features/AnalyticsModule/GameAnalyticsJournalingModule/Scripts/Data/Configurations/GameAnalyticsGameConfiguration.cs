using System;
using UnityEngine;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations
{
	[Serializable]
	public class GameAnalyticsGameConfiguration
	{
		[SerializeField]
		private string _selectedPlatformOrganization;

		[SerializeField]
		private string _selectedPlatformStudio;

		[SerializeField]
		private string _selectedPlatformGame;

		[SerializeField]
		private int _selectedPlatformGameID;

		[SerializeField]
		private int _selectedGame;

		[SerializeField]
		private string _gameKey;

		[SerializeField]
		private string _secretKey;

		[SerializeField]
		private int _selectedOrganization;

		[SerializeField]
		private int _selectedStudio;

		public string SelectedPlatformOrganization => _selectedPlatformOrganization;

		public string SelectedPlatformStudio => _selectedPlatformStudio;

		public string SelectedPlatformGame => _selectedPlatformGame;

		public int SelectedPlatformGameID => _selectedPlatformGameID;

		public int SelectedGame => _selectedGame;

		public string GameKey => _gameKey;

		public string SecretKey => _secretKey;

		public int SelectedOrganization => _selectedOrganization;

		public int SelectedStudio => _selectedStudio;
	}
}
