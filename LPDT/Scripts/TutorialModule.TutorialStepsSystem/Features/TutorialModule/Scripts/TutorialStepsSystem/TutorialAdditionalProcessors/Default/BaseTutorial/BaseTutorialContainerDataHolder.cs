using System;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialContainerDataHolder
	{
		private TutorialContainerEntity _containerEntity;

		private GameObject _containerDeliverTracker;

		private BaseTutorialBoatEntity _boatEntity;

		public TutorialContainerEntity ContainerEntity
		{
			get
			{
				return _containerEntity;
			}
			set
			{
				_containerEntity = value;
				this.OnContainerPointGrabbableChanged?.Invoke(_containerEntity);
			}
		}

		public BaseTutorialBoatEntity BoatEntity
		{
			get
			{
				return _boatEntity;
			}
			set
			{
				_boatEntity = value;
				this.OnBoatEntityChanged?.Invoke(_boatEntity);
			}
		}

		public event Action<TutorialContainerEntity> OnContainerPointGrabbableChanged;

		public event Action<BaseTutorialBoatEntity> OnBoatEntityChanged;
	}
}
