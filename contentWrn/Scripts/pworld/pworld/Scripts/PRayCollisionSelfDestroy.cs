using System;
using System.Collections.Generic;
using UnityEngine;

namespace pworld.Scripts
{
	public class PRayCollisionSelfDestroy : MonoBehaviour
	{
		private PRayCollision collision_g;

		public List<Func<PRaycastHit, bool>> Criterias = new List<Func<PRaycastHit, bool>>();

		private void Awake()
		{
			collision_g = GetComponent<PRayCollision>();
			PRayCollision pRayCollision = collision_g;
			pRayCollision.OnCollisionLate = (Action<PRaycastHit>)Delegate.Combine(pRayCollision.OnCollisionLate, new Action<PRaycastHit>(Sepukku));
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
			PRayCollision pRayCollision = collision_g;
			pRayCollision.OnCollisionLate = (Action<PRaycastHit>)Delegate.Remove(pRayCollision.OnCollisionLate, new Action<PRaycastHit>(Sepukku));
		}

		private bool CheckCriterias(PRaycastHit other)
		{
			foreach (Func<PRaycastHit, bool> criteria in Criterias)
			{
				if (criteria != null && !criteria(other))
				{
					return false;
				}
			}
			return true;
		}

		private void Sepukku(PRaycastHit obj)
		{
			if (CheckCriterias(obj))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
