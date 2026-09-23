using UnityEngine;

namespace Mimicraft.Gameplay
{
	[DisallowMultipleComponent]
	public class RagdollBoneImpact : MonoBehaviour
	{
		private RagdollImpactFeedback owner;

		internal void Bind(RagdollImpactFeedback feedback)
		{
			owner = feedback;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (owner != null)
			{
				owner.ReportBoneImpact(collision);
			}
		}
	}
}
