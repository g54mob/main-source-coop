using Fusion;
using UnityEngine;

namespace Features.ConsumeModule.Scripts
{
	public class ChangePlayerSpeedConsumable : MonoBehaviour, IConsumable
	{
		public void Apply(PlayerRef playerRef)
		{
			Debug.LogError("Change player speed consumable");
		}
	}
}
