using System;
using UnityEngine;

namespace Features.RagdollModule.Scripts
{
	[Serializable]
	public class KinematicsAdjustData
	{
		public Rigidbody Rigidbody;

		public AuthorityKinematicsDetector AuthorityKinematicsDetector;
	}
}
