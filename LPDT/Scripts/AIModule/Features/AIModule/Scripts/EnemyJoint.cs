using System;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	[Serializable]
	public class EnemyJoint
	{
		[field: SerializeField]
		public FixedJoint Joint { get; private set; }

		[field: SerializeField]
		public Transform ReferenceTransform { get; private set; }

		[field: SerializeField]
		public EnemyType Type { get; private set; }

		[field: SerializeField]
		public bool IsBusy { get; private set; }

		public void MarkJointAsBusy()
		{
			IsBusy = true;
		}

		public void MarkJointAsFree()
		{
			IsBusy = false;
		}
	}
}
