using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.QuotaModule.Scripts.PhysicsContainer
{
	public class QuotaContainerTrigger : MonoBehaviour
	{
		[SerializeField]
		private QuotaPhysicsContainer _quotaPhysicsContainer;

		[field: SerializeField]
		public bool IsQuotaCollectBlocked { get; set; }

		private void OnTriggerEnter(Collider other)
		{
			if (IsQuotaCollectBlocked)
			{
				return;
			}
			QuotaContainerItemData quotaContainerItemData = new QuotaContainerItemData();
			if (other.TryGetComponent<IPointGrabable>(out var component))
			{
				EnableCollisionIgnore(component);
				quotaContainerItemData.PointGrabable = component;
			}
			else
			{
				IPointGrabable componentInParent = other.GetComponentInParent<IPointGrabable>();
				if (componentInParent != null)
				{
					EnableCollisionIgnore(componentInParent);
					quotaContainerItemData.PointGrabable = componentInParent;
				}
			}
			if (other.TryGetComponent<MonoItem>(out var component2))
			{
				quotaContainerItemData.Item = component2;
				_quotaPhysicsContainer.AddQuotaItem(quotaContainerItemData);
				return;
			}
			MonoItem componentInParent2 = other.GetComponentInParent<MonoItem>();
			if (componentInParent2 != null)
			{
				quotaContainerItemData.Item = componentInParent2;
				_quotaPhysicsContainer.AddQuotaItem(quotaContainerItemData);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent<MonoItem>(out var component))
			{
				DisableCollisionIgnore(component);
				_quotaPhysicsContainer.RemoveQuotaItem(component);
				return;
			}
			MonoItem componentInParent = other.GetComponentInParent<MonoItem>();
			if (componentInParent != null)
			{
				DisableCollisionIgnore(componentInParent);
				_quotaPhysicsContainer.RemoveQuotaItem(componentInParent);
			}
		}

		private void EnableCollisionIgnore(IPointGrabable grabable)
		{
			grabable.AddIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
		}

		private void DisableCollisionIgnore(MonoItem item)
		{
			_quotaPhysicsContainer.GetQuotaItem(item)?.PointGrabable.RemoveIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
		}
	}
}
