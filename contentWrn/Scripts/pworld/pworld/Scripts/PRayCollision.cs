using System;
using UnityEngine;

namespace pworld.Scripts
{
	public class PRayCollision : MonoBehaviour
	{
		private bool collidedThisFrame;

		private PRaycastHit lastHit;

		private Vector3 lastPosition;

		public Action<PRaycastHit> OnCollision;

		public Action<PRaycastHit> OnCollisionLate;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
			collidedThisFrame = CheckFirst(out lastHit);
		}

		private void LateUpdate()
		{
			lastPosition = base.transform.position;
		}

		public bool CollidedThisFrame(out PRaycastHit hit)
		{
			if (collidedThisFrame)
			{
				hit = lastHit;
				return true;
			}
			hit = lastHit;
			return false;
		}

		private bool CheckFirst(out PRaycastHit hitInfo)
		{
			return Check(out hitInfo, -1);
		}

		private bool Check(out PRaycastHit hitInfo, LayerMask mask)
		{
			Ray ray = new Ray(lastPosition, base.transform.position - lastPosition);
			if (Physics.Raycast(ray, out var hitInfo2, Vector3.Distance(base.transform.position, lastPosition), mask))
			{
				hitInfo = new PRaycastHit();
				hitInfo.hit = hitInfo2;
				hitInfo.ray = ray;
				OnCollision?.Invoke(hitInfo);
				OnCollisionLate?.Invoke(hitInfo);
				return true;
			}
			hitInfo = null;
			return false;
		}
	}
}
