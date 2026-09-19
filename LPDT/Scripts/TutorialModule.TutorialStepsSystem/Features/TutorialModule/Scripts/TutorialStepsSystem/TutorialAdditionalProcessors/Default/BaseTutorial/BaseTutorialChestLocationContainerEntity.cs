using Features.GrabModule.Scripts;
using Features.ItemSpawnerModule;
using Features.TutorialModule.Scripts.GuideModule;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialChestLocationContainerEntity : MonoBehaviour, IBaseTutorialLocationContainerEntity
	{
		[SerializeField]
		private SimplePointGrabable _chestGrabbable;

		[SerializeField]
		private ItemSpawnerBase _itemSpawnerBase;

		[SerializeField]
		private SerializableDictionary<TipType, Transform> _tipHolders;

		private IPointGrabable _contentGrabbable;

		public float OpenAngle => Mathf.Abs(Mathf.DeltaAngle(0f, _chestGrabbable.Rigidbody.rotation.eulerAngles.x));

		public bool IsOpenedByPlayer => _chestGrabbable.GrabbedByPlayersCount > 0;

		public IPointGrabable ContentGrabbable => _contentGrabbable;

		public IPointGrabable DoorGrabbable => _chestGrabbable;

		public SerializableDictionary<TipType, Transform> TipHolders => _tipHolders;

		private void OnEnable()
		{
			_itemSpawnerBase.Spawn();
			_itemSpawnerBase.OnItemSpawned += InitContentGrabbable;
		}

		private void OnDisable()
		{
			_itemSpawnerBase.OnItemSpawned -= InitContentGrabbable;
		}

		private void InitContentGrabbable(ItemSpawnerBase itemSpawnerBase)
		{
			itemSpawnerBase.SpawnedInstance.TryGetComponent<IPointGrabable>(out _contentGrabbable);
		}
	}
}
