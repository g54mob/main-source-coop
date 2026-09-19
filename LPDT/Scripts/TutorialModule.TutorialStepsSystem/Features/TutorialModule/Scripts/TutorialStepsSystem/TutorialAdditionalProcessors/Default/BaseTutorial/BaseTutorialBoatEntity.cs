using Features.QuotaModule.Scripts.PhysicsContainer;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialBoatEntity : MonoBehaviour
	{
		[SerializeField]
		public Transform HingeController;

		[SerializeField]
		public Transform QuotaSubmitTip;

		[SerializeField]
		public QuotaContainerTrigger QuotaContainerTrigger;
	}
}
