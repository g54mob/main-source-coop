using System.Collections.Generic;
using QuickOutline.Scripts;
using UnityEngine;

namespace Features.GrabModule.Scripts
{
	public class OutlineSynchronizer : MonoBehaviour
	{
		[SerializeField]
		private Outline _referenceOutline;

		[SerializeField]
		private List<Outline> _outlines;

		private bool _lastState;

		private bool _initialized;

		protected virtual void Awake()
		{
			if (_referenceOutline != null)
			{
				Initialize(_referenceOutline);
			}
		}

		protected virtual void OnDestroy()
		{
			_initialized = false;
		}

		private void Update()
		{
			if (_initialized && _lastState != _referenceOutline.enabled)
			{
				ApplyReferenceOutlineEnabled();
				_lastState = _referenceOutline.enabled;
			}
		}

		public void Initialize(Outline referenceOutline)
		{
			_referenceOutline = referenceOutline;
			_lastState = _referenceOutline.enabled;
			ApplyReferenceOutlineEnabled();
			_initialized = true;
		}

		private void ApplyReferenceOutlineEnabled()
		{
			foreach (Outline outline in _outlines)
			{
				outline.enabled = _referenceOutline.enabled;
			}
		}
	}
}
