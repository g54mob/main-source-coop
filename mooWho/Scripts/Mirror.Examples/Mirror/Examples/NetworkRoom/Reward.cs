using Mirror.Examples.Common;
using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(RandomColor))]
	public class Reward : NetworkBehaviour
	{
		[Header("Components")]
		public RandomColor randomColor;

		[Header("Diagnostics")]
		[ReadOnly]
		[SerializeField]
		private bool available;

		protected override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				Reset();
			}
		}

		private void Reset()
		{
			base.transform.position = new Vector3(0f, -1000f, 0f);
			if (randomColor == null)
			{
				randomColor = GetComponent<RandomColor>();
			}
		}

		public override void OnStartServer()
		{
			available = true;
		}

		[ServerCallback]
		private void OnTriggerEnter(Collider other)
		{
			if (NetworkServer.active && base.gameObject.activeSelf && other.CompareTag("Player") && available)
			{
				available = false;
				uint num = (uint)((randomColor.color.r + randomColor.color.g + randomColor.color.b) / 3);
				PlayerScore component = other.GetComponent<PlayerScore>();
				component.Networkscore = component.score + num;
				Spawner.RecycleReward(base.gameObject);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
