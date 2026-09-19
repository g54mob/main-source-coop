using Features.GrabModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public interface IBaseTutorialLocationContainerEntity
	{
		float OpenAngle { get; }

		bool IsOpenedByPlayer { get; }

		IPointGrabable ContentGrabbable { get; }

		IPointGrabable DoorGrabbable { get; }

		SerializableDictionary<TipType, Transform> TipHolders { get; }
	}
}
