using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerSeparation : NetworkBehaviour
	{
		[Tooltip("Üst üste binmenin ne kadarını bu oyuncu çözer. 0.5 doğru değer: karşıdaki de kendi yarısını çözüyor, ikisi birden tam ayrılmayı bir kez yapıyor. 1 yapmak iki oyuncunun aynı çakışmayı iki kez itmesi demek, yani titreme.")]
		[SerializeField]
		[Range(0.1f, 1f)]
		private float resolveShare = 0.5f;

		[Tooltip("Bu kadarın altındaki çakışmalar yok sayılır - kapsüller sürekli birbirine değip duruyorken her kareye bir itme yazmak titremeden başka bir şey üretmiyor.")]
		[SerializeField]
		[Min(0f)]
		private float ignoreBelow = 0.02f;

		[Tooltip("Tek karede uygulanabilecek en büyük itme, metre/saniye. Bir şekilde tamamen üst üste binmiş iki oyuncunun birbirini haritanın öbür ucuna fırlatmasını engelliyor.")]
		[SerializeField]
		[Min(0.1f)]
		private float maxPushSpeed = 4f;

		private static readonly List<PlayerSeparation> all = new List<PlayerSeparation>();

		private CharacterController controller;

		private PlayerRagdoll ragdoll;

		private bool IsPushable
		{
			get
			{
				if (controller != null && controller.enabled)
				{
					if (!(ragdoll == null))
					{
						if (ragdoll.HasHumanoid)
						{
							return ragdoll.State == RagdollState.None;
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}

		public override void OnNetworkSpawn()
		{
			controller = GetComponent<CharacterController>();
			ragdoll = GetComponent<PlayerRagdoll>();
			all.Add(this);
		}

		public override void OnNetworkDespawn()
		{
			all.Remove(this);
		}

		private void LateUpdate()
		{
			if (!base.IsOwner || !IsPushable || all.Count < 2)
			{
				return;
			}
			Vector3 motion = Vector3.zero;
			foreach (PlayerSeparation item in all)
			{
				if (!(item == this) && !(item == null) && item.IsPushable)
				{
					motion += ResolveAgainst(item);
				}
			}
			if (!(motion.sqrMagnitude < 1E-06f))
			{
				float num = maxPushSpeed * Time.deltaTime;
				if (motion.magnitude > num)
				{
					motion = motion.normalized * num;
				}
				controller.Move(motion);
			}
		}

		private Vector3 ResolveAgainst(PlayerSeparation other)
		{
			if (!VerticallyOverlaps(other))
			{
				return Vector3.zero;
			}
			Vector3 vector = base.transform.position - other.transform.position;
			vector.y = 0f;
			float num = HorizontalRadius() + other.HorizontalRadius();
			float magnitude = vector.magnitude;
			float num2 = num - magnitude;
			if (num2 <= ignoreBelow)
			{
				return Vector3.zero;
			}
			return ((magnitude > 0.001f) ? (vector / magnitude) : DeterministicNudge(other)) * (num2 * resolveShare);
		}

		private Vector3 DeterministicNudge(PlayerSeparation other)
		{
			return new Vector3((base.OwnerClientId < other.OwnerClientId) ? 1f : (-1f), 0f, 0f);
		}

		private bool VerticallyOverlaps(PlayerSeparation other)
		{
			float num = base.transform.position.y + controller.center.y - controller.height * 0.5f;
			float num2 = num + controller.height;
			float num3 = other.transform.position.y + other.controller.center.y - other.controller.height * 0.5f;
			float num4 = num3 + other.controller.height;
			if (num < num4)
			{
				return num3 < num2;
			}
			return false;
		}

		private float HorizontalRadius()
		{
			return controller.radius * Mathf.Max(base.transform.lossyScale.x, base.transform.lossyScale.z);
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "PlayerSeparation";
		}
	}
}
