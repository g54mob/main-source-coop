using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts
{
	[CreateAssetMenu(fileName = "TransitionInstallerConfiguration_Default", menuName = "Configurations/Transition/TransitionInstallerConfiguration")]
	public class TransitionInstallerConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SceneTransitionCanvas SceneTransitionCanvas { get; private set; }

		[field: SerializeField]
		public SceneTransitionCamera SceneTransitionCamera { get; private set; }
	}
}
