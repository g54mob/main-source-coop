using Fusion;
using UnityEngine;

namespace Features.ConsumeModule.Scripts
{
	public class ChangePlayerHandsGravityConsumable : MonoBehaviour, IConsumable
	{
		public void Apply(PlayerRef playerRef)
		{
			Debug.LogError("Change player hands gravity consumable");
		}
	}
}
