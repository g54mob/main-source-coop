using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootVisibilityRange : MonoBehaviour
	{
		[Header("Loot Visibility")]
		[Tooltip("Distance (m) this loot stays visible to clients. Overrides the LootInterestManagement default.")]
		[SerializeField]
		private float visRange = 80f;

		public float VisRange => visRange;

		public void SetVisRange(float range)
		{
			visRange = Mathf.Max(1f, range);
		}
	}
}
