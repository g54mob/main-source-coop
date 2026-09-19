using UnityEngine;

namespace Mirror.Examples.CharacterSelection
{
	public class SceneCamera : NetworkBehaviour
	{
		[Header("Components")]
		[SerializeField]
		private CharacterSelection characterSelection;

		[SerializeField]
		private Transform cameraTarget;

		[Header("Diagnostics")]
		[ReadOnly]
		[SerializeField]
		private SceneReferencer sceneReferencer;

		[ReadOnly]
		[SerializeField]
		private Transform cameraObj;

		protected override void OnValidate()
		{
			base.OnValidate();
			Reset();
		}

		private void Reset()
		{
			characterSelection = GetComponent<CharacterSelection>();
			cameraTarget = base.transform.Find("CameraTarget");
			base.enabled = false;
		}

		public override void OnStartAuthority()
		{
			sceneReferencer = Object.FindAnyObjectByType<SceneReferencer>();
			cameraObj = sceneReferencer.cameraObject.transform;
			base.enabled = true;
		}

		public override void OnStopAuthority()
		{
			base.enabled = false;
		}

		private void Update()
		{
			if (Application.isFocused)
			{
				if ((bool)cameraObj && (bool)characterSelection)
				{
					characterSelection.floatingInfo.forward = cameraObj.transform.forward;
				}
				if ((bool)cameraObj && (bool)cameraTarget)
				{
					cameraObj.SetPositionAndRotation(cameraTarget.position, cameraTarget.rotation);
				}
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
