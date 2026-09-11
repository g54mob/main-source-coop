using System;
using System.Collections.Generic;
using UnityEngine;

namespace pworld.Scripts
{
	public class PRayCollisionDMG : MonoBehaviour
	{
		public float damage;

		private PRayCollision collision_g;

		public List<Func<PRaycastHit, bool>> DmgCriterias = new List<Func<PRaycastHit, bool>>();

		public Action OnDmg;

		private void Awake()
		{
			collision_g = GetComponent<PRayCollision>();
			PRayCollision pRayCollision = collision_g;
			pRayCollision.OnCollision = (Action<PRaycastHit>)Delegate.Combine(pRayCollision.OnCollision, new Action<PRaycastHit>(DoDmg));
		}

		private void OnDisable()
		{
			PRayCollision pRayCollision = collision_g;
			pRayCollision.OnCollision = (Action<PRaycastHit>)Delegate.Remove(pRayCollision.OnCollision, new Action<PRaycastHit>(DoDmg));
		}

		private void OnDestroy()
		{
			PRayCollision pRayCollision = collision_g;
			pRayCollision.OnCollision = (Action<PRaycastHit>)Delegate.Remove(pRayCollision.OnCollision, new Action<PRaycastHit>(DoDmg));
		}

		private bool CheckCriterias(PRaycastHit other)
		{
			foreach (Func<PRaycastHit, bool> dmgCriteria in DmgCriterias)
			{
				if (dmgCriteria != null && !dmgCriteria(other))
				{
					return false;
				}
			}
			return true;
		}

		private void DoDmg(PRaycastHit obj)
		{
			PAffectable componentInParent = obj.hit.collider.gameObject.GetComponentInParent<PAffectable>();
			if (componentInParent != null)
			{
				componentInParent.TakeDamage(damage, base.gameObject);
				OnDmg?.Invoke();
			}
		}
	}
}
