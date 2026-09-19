using UnityEngine;

namespace Features.RagdollModule.Scripts
{
	public class RagdollTest : MonoBehaviour
	{
		private void OnCollisionEnter(Collision other)
		{
			Debug.LogError("BONE " + base.gameObject.name + " ENTERED COLLISION " + other.gameObject.name);
		}
	}
}
