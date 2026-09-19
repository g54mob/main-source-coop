using System;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core
{
	[Serializable]
	public class BeachLayout
	{
		[Tooltip("Designer-authored entry point offset from the beach origin.")]
		[SerializeField]
		private Vector3 _entryPointOffset;

		public Vector3 EntryPointOffset => _entryPointOffset;

		public Vector3 LocalToWorld(Pose pose, Vector3 localPosition)
		{
			return pose.position + pose.rotation * localPosition;
		}

		public Quaternion LocalToWorldRotation(Pose pose, Vector3 localEulerAngles)
		{
			return pose.rotation * Quaternion.Euler(localEulerAngles);
		}

		public BeachValidationResult Validate()
		{
			return new BeachValidationResult();
		}
	}
}
