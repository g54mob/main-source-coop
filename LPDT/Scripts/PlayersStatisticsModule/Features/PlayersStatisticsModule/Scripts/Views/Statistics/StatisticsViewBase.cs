using Features.PlayerItemViewModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Views.Statistics
{
	public class StatisticsViewBase : ViewBehaviour
	{
		[SerializeField]
		private Transform _container;

		[field: SerializeField]
		public PlayerItemViewBase PlayerItemViewBase { get; private set; }

		[field: SerializeField]
		public Animator WindowAnimator { get; private set; }

		public Transform GetPlayerItemsContainer()
		{
			return _container;
		}
	}
}
