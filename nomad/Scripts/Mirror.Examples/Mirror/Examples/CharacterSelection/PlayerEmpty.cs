using UnityEngine;

namespace Mirror.Examples.CharacterSelection
{
	public class PlayerEmpty : NetworkBehaviour
	{
		private SceneReferencer sceneReferencer;

		public override void OnStartAuthority()
		{
			sceneReferencer = Object.FindAnyObjectByType<SceneReferencer>();
			sceneReferencer.GetComponent<Canvas>().enabled = true;
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
