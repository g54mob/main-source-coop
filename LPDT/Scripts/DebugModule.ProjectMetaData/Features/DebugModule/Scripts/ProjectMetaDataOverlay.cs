using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class ProjectMetaDataOverlay : MonoBehaviour
	{
		[SerializeField]
		private GameObject _projectVersionOverlay;

		[SerializeField]
		private GameObject _FPSCounterOverlay;

		[SerializeField]
		private GameObject _regionOverlay;

		private void Start()
		{
			if (!Application.isEditor && !Debug.isDebugBuild)
			{
				_FPSCounterOverlay?.SetActive(value: false);
				_regionOverlay?.SetActive(value: false);
			}
		}
	}
}
