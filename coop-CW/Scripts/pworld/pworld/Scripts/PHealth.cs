using System;
using UnityEngine;

namespace pworld.Scripts
{
	public class PHealth : MonoBehaviour, PAffectable
	{
		[SerializeField]
		private float health = 100f;

		private bool dead;

		public Action OnDied;

		public Action OnDiedLate;

		public Action OnRessurected;

		public Action<float, GameObject> OnTakeDamage;

		public Action<float, GameObject> OnTakeDamageLate;

		public float StartHealth { get; private set; }

		public bool Paused { get; set; }

		public float Health
		{
			get
			{
				return health;
			}
			private set
			{
				health = value;
				if (health <= 0f && !dead)
				{
					dead = true;
					OnDied?.Invoke();
					OnDiedLate?.Invoke();
				}
				if (health >= 0f && dead)
				{
					dead = false;
					OnRessurected?.Invoke();
				}
			}
		}

		private void Awake()
		{
			StartHealth = Health;
		}

		void PAffectable.AddForce(Vector3 force)
		{
		}

		public bool TakeDamage(float dmg, GameObject damager)
		{
			if (Paused)
			{
				return false;
			}
			Health -= dmg;
			OnTakeDamage?.Invoke(dmg, damager);
			OnTakeDamageLate?.Invoke(dmg, damager);
			return true;
		}

		public void PauseForSeconds(float sec)
		{
		}
	}
}
