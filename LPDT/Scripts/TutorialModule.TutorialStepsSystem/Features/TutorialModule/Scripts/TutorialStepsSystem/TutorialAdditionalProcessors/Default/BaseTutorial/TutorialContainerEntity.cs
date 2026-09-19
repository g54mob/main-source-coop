using System.Collections.Generic;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Features.HingeModule.Scripts;
using Features.TutorialModule.Scripts.GuideModule;
using Global.SerializableDictionary;
using UnityEngine;
using UnityEngine.AI;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class TutorialContainerEntity : MonoBehaviour
	{
		private ICartItemsContainer _cartItemsContainer;

		[field: SerializeField]
		public GameObject ContainerItemsGrabberObject { get; private set; }

		[field: SerializeField]
		public NavMeshObstacle ContainerNavmeshObstacle { get; private set; }

		[field: SerializeField]
		public Transform ContainerTipPoint { get; private set; }

		[field: SerializeField]
		public List<BoatHingeControllerData> BoatHingeControllersData { get; private set; }

		[field: SerializeField]
		public Transform HandleGrabPoint { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<TipType, Transform> TipHolders { get; private set; }

		public ICartItemsContainer ContainerItemsGrabber => _cartItemsContainer;

		private void Awake()
		{
			_cartItemsContainer = ContainerItemsGrabberObject.GetComponent<ICartItemsContainer>();
		}
	}
}
