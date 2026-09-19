using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts.PhysicsContainer
{
	public class QuotaContainerLocationRegistrar : MonoBehaviour
	{
		[SerializeField]
		private Transform _dropPoint;

		[Tooltip("Where the monkey porter parks to throw loot in. Empty = nearest navmesh point to the drop point.")]
		[SerializeField]
		private Transform _porterApproachPoint;

		private QuotaContainerLocationModel _quotaContainerLocationModel;

		private Transform DropTransform
		{
			get
			{
				if (!(_dropPoint == null))
				{
					return _dropPoint;
				}
				return base.transform;
			}
		}

		[Inject]
		public void InjectDependencies(QuotaContainerLocationModel quotaContainerLocationModel)
		{
			_quotaContainerLocationModel = quotaContainerLocationModel;
		}

		private void OnEnable()
		{
			_quotaContainerLocationModel.SetContainer(DropTransform, _porterApproachPoint);
		}

		private void OnDisable()
		{
			_quotaContainerLocationModel.ClearContainer(DropTransform);
		}
	}
}
