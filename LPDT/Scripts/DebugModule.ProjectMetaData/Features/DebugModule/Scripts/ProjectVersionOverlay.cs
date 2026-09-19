using TMPro;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class ProjectVersionOverlay : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _projectVersionText;

		[SerializeField]
		private ProjectAdditionalVersionConfiguration _additionalVersionConfiguration;

		private void Start()
		{
			_projectVersionText.text = "Version: " + Application.version + "." + _additionalVersionConfiguration.AdditionalVersion;
		}
	}
}
