using System.Collections.Generic;
using Features.GrabModule.Scripts;
using QuickOutline.Scripts;
using UnityEngine;

namespace Features.HingeModule.Scripts
{
	public class ChangeGrabbableOnJointBreak : MonoBehaviour
	{
		[SerializeField]
		private HingeDespawner _hingeDespawner;

		[SerializeField]
		private SimplePointGrabable _pointGrabbable;

		[SerializeField]
		private List<Transform> _handles = new List<Transform>();

		[SerializeField]
		private Outline _outline;

		private void OnEnable()
		{
			_hingeDespawner.OnBreakEvent += ChangeGrabbable;
		}

		private void OnDisable()
		{
			_hingeDespawner.OnBreakEvent -= ChangeGrabbable;
		}

		private void ChangeGrabbable()
		{
			_pointGrabbable.Handles = _handles;
			_pointGrabbable.Outline = _outline;
		}
	}
}
