using UnityEngine;

namespace Synty.AnimationBaseLocomotion.Samples
{
	public class SampleObjectLockOn : MonoBehaviour
	{
		public Material _highlightMat;

		public Material _targetMat;

		private Transform _highlightOrb;

		private MeshRenderer _meshRenderer;

		private void Start()
		{
			_highlightOrb = base.transform.Find("TargetHighlight");
			_meshRenderer = _highlightOrb.GetComponent<MeshRenderer>();
			if (_meshRenderer == null)
			{
				Debug.LogError("This script requires a MeshRenderer component on the GameObject.");
			}
		}

		private void OnTriggerEnter(Collider otherCollider)
		{
			SamplePlayerAnimationController component = otherCollider.GetComponent<SamplePlayerAnimationController>();
			if (component != null)
			{
				component.AddTargetCandidate(base.gameObject);
			}
		}

		private void OnTriggerExit(Collider otherCollider)
		{
			SamplePlayerAnimationController component = otherCollider.GetComponent<SamplePlayerAnimationController>();
			if (component != null)
			{
				component.RemoveTarget(base.gameObject);
				Highlight(enable: false, targetLock: false);
			}
		}

		public void Highlight(bool enable, bool targetLock)
		{
			Material material = (targetLock ? _targetMat : _highlightMat);
			if (_highlightOrb != null)
			{
				_highlightOrb.gameObject.SetActive(enable);
				if (enable)
				{
					_meshRenderer.material = material;
				}
			}
		}
	}
}
