using System;
using UnityEngine;

namespace pworld.Scripts
{
	public abstract class PCollision : MonoBehaviour
	{
		public Action<RaycastHit> OnCollision;

		public Action<RaycastHit> OnCollisionLate;
	}
}
