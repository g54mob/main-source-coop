using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Views.Statistics
{
	public class StatisticsPlayerViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public TMP_Text DeathsText { get; private set; }

		[field: SerializeField]
		public TMP_Text KillsText { get; private set; }

		[field: SerializeField]
		public TMP_Text RevivesText { get; private set; }

		[field: SerializeField]
		public TMP_Text CentsText { get; private set; }
	}
}
