using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialTargetReachTracker : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _targetLayerMask;

		[SerializeField]
		private Collider _ownCollider;

		private const int MAX_OVERLAP_RESULTS = 16;

		private readonly Collider[] _overlapResults = new Collider[16];

		private bool _enabled;

		private BaseTutorialTargetEventClass _baseTutorialTargetEventClass;

		[field: SerializeField]
		public BaseTutorialTargetType TargetType { get; private set; }

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if (_enabled != value)
				{
					_enabled = value;
					if (_enabled)
					{
						CheckAlreadyInside();
					}
				}
			}
		}

		[Inject]
		public void InjectDependencies(BaseTutorialTargetEventClass baseTutorialTargetEventClass)
		{
			_baseTutorialTargetEventClass = baseTutorialTargetEventClass;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (Enabled && IsInLayerMask(other.gameObject.layer, _targetLayerMask) && VerifyTarget(other))
			{
				_baseTutorialTargetEventClass.Invoke(TargetType);
			}
		}

		protected virtual bool VerifyTarget(Collider other)
		{
			return true;
		}

		private void CheckAlreadyInside()
		{
			Bounds bounds = _ownCollider.bounds;
			int num = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, _overlapResults, Quaternion.identity, _targetLayerMask, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num; i++)
			{
				Collider other = _overlapResults[i];
				if (VerifyTarget(other))
				{
					_baseTutorialTargetEventClass.Invoke(TargetType);
					break;
				}
			}
		}

		private bool IsInLayerMask(int layer, LayerMask layerMask)
		{
			return (layerMask.value & (1 << layer)) != 0;
		}
	}
}
