using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.PlayerStatesModule.Scripts.Views
{
	public abstract class DeathViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public float DeadToSpectatorDelay { get; private set; }
	}
}
