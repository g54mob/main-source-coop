using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.Hazards
{
	[RequireComponent(typeof(Collider))]
	public class ContactDamageHazard : MonoBehaviour
	{
		[Tooltip("Damage dealt to the local player per contact (Once) or per interval (Continuous). Keep small.")]
		[Min(0f)]
		[SerializeField]
		private float damageAmount = 2f;

		[Tooltip("Once: damage a single time per contact (re-entering damages again). Continuous: damage repeatedly while touching, throttled by Damage Interval.")]
		[SerializeField]
		private ContactDamageCadence cadence;

		[Tooltip("Seconds between repeated damage ticks. Used only when Cadence is Continuous.")]
		[Min(0.01f)]
		[SerializeField]
		private float damageInterval = 1f;

		[Tooltip("Force the collider into trigger mode at Awake. Disable only for a custom setup.")]
		[SerializeField]
		private bool forceTriggerOnAwake = true;

		private bool _hasDamagedCurrentContact;

		private float _nextDamageTime;

		public float DamageAmount => damageAmount;

		public ContactDamageCadence Cadence => cadence;

		public float DamageInterval => damageInterval;

		private void Awake()
		{
			if (forceTriggerOnAwake)
			{
				Collider component = GetComponent<Collider>();
				if (component != null && !component.isTrigger)
				{
					component.isTrigger = true;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			TryDamage(other);
		}

		private void OnTriggerStay(Collider other)
		{
			TryDamage(other);
		}

		private void OnTriggerExit(Collider other)
		{
			if (cadence == ContactDamageCadence.Once)
			{
				NomadDrive.Features.Player.Player componentInParent = other.GetComponentInParent<NomadDrive.Features.Player.Player>();
				if (!(componentInParent == null) && componentInParent.isLocalPlayer)
				{
					_hasDamagedCurrentContact = false;
				}
			}
		}

		private void TryDamage(Collider other)
		{
			if ((cadence == ContactDamageCadence.Once && _hasDamagedCurrentContact) || (cadence == ContactDamageCadence.Continuous && Time.time < _nextDamageTime))
			{
				return;
			}
			NomadDrive.Features.Player.Player componentInParent = other.GetComponentInParent<NomadDrive.Features.Player.Player>();
			if (componentInParent == null || !componentInParent.isLocalPlayer)
			{
				return;
			}
			PlayerStatsManager component = componentInParent.GetComponent<PlayerStatsManager>();
			if (!(component == null))
			{
				component.ApplyDirectDamage(damageAmount);
				if (cadence == ContactDamageCadence.Once)
				{
					_hasDamagedCurrentContact = true;
				}
				else
				{
					_nextDamageTime = Time.time + Mathf.Max(0.01f, damageInterval);
				}
			}
		}
	}
}
