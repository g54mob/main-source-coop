using System;
using Features.GrabModule.Scripts;
using UnityEngine;

namespace Features.MagnetModule.Scripts
{
	public class MagnetCollisionHandler : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _targetLayers;

		public event Action<SimplePointGrabable> OnItemInRange;

		public event Action<SimplePointGrabable> OnItemInRangeExit;

		private void OnTriggerEnter(Collider other)
		{
			if (IsInLayerMask(other.gameObject, _targetLayers))
			{
				SimplePointGrabable component2;
				if (other.TryGetComponent<SimplePointGrabable>(out var component))
				{
					this.OnItemInRange?.Invoke(component);
				}
				else if (other.transform.parent.TryGetComponent<SimplePointGrabable>(out component2))
				{
					this.OnItemInRange?.Invoke(component2);
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (IsInLayerMask(other.gameObject, _targetLayers))
			{
				SimplePointGrabable component2;
				if (other.TryGetComponent<SimplePointGrabable>(out var component))
				{
					this.OnItemInRangeExit?.Invoke(component);
				}
				else if (other.transform.parent.TryGetComponent<SimplePointGrabable>(out component2))
				{
					this.OnItemInRangeExit?.Invoke(component2);
				}
			}
		}

		private bool IsInLayerMask(GameObject gameObject, LayerMask layerMask)
		{
			return (layerMask.value & (1 << gameObject.layer)) != 0;
		}
	}
}
