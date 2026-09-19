using Features.GrabModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialBellEntity : MonoBehaviour
	{
		[field: SerializeField]
		public Transform BellHandle { get; private set; }

		[field: SerializeField]
		public SimplePointGrabable ClapperGrabbable { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<TipType, Transform> TipHolders { get; set; }
	}
}
