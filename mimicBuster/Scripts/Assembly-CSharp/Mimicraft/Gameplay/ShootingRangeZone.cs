using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class ShootingRangeZone : MonoBehaviour
	{
		[Tooltip("Buraya isabet edince gösterilecek puan. Kafa 10, gövde 5 gibi.")]
		[SerializeField]
		[Min(0f)]
		private int points = 5;

		public int Points => points;
	}
}
