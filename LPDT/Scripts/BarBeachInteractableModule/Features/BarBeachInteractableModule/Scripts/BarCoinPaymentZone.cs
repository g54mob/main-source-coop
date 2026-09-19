using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.BarBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BarCoinPaymentZone : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		private static readonly Collider[] OverlapBuffer = new Collider[24];

		[SerializeField]
		private BarBeachInteractableBehaviour _bar;

		[SerializeField]
		private Collider _trigger;

		[SerializeField]
		[Min(0.1f)]
		private float _pollIntervalSeconds = 0.15f;

		private bool _paymentInFlight;

		private float _pollTimer;

		public override void Spawned()
		{
			base.Spawned();
			if (_trigger != null)
			{
				_trigger.isTrigger = true;
			}
			_paymentInFlight = false;
		}

		public void StateAuthorityChanged()
		{
			_paymentInFlight = false;
			_pollTimer = 0f;
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Object.HasStateAuthority && !_paymentInFlight && !(_bar == null) && _bar.ServeStock <= 0)
			{
				_pollTimer -= base.Runner.DeltaTime;
				if (!(_pollTimer > 0f))
				{
					_pollTimer = _pollIntervalSeconds;
					TryAcceptCoinInZone();
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (base.Object.HasStateAuthority && !_paymentInFlight && !(_bar == null) && _bar.ServeStock <= 0 && TryGetCoinFromCollider(other, out var coin))
			{
				TryBeginPayment(coin);
			}
		}

		private void TryAcceptCoinInZone()
		{
			if (_trigger == null)
			{
				return;
			}
			Bounds bounds = _trigger.bounds;
			int num = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, OverlapBuffer, _trigger.transform.rotation, -1, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num; i++)
			{
				Collider collider = OverlapBuffer[i];
				if (!(collider == null) && TryGetCoinFromCollider(collider, out var coin) && TryBeginPayment(coin))
				{
					break;
				}
			}
		}

		private bool TryBeginPayment(IItem coin)
		{
			if (_paymentInFlight || coin == null || _bar == null)
			{
				return false;
			}
			if (!_bar.TryGetBartender(out var bartender) || (Object)(object)bartender == null)
			{
				return false;
			}
			_paymentInFlight = true;
			if (!bartender.TryBeginTakeCoin(coin, OnCoinConsumed))
			{
				_paymentInFlight = false;
				return false;
			}
			Debug.Log("[BarPay] take-coin started");
			return true;
		}

		private void OnCoinConsumed()
		{
			_paymentInFlight = false;
			if (_bar != null && base.Object != null && base.Object.HasStateAuthority)
			{
				_bar.TryRestockFromPayment();
			}
		}

		private static bool TryGetCoinFromCollider(Collider collider, out IItem coin)
		{
			coin = null;
			if (collider == null)
			{
				return false;
			}
			MonoItem componentInParent = collider.GetComponentInParent<MonoItem>();
			if (componentInParent == null || componentInParent.Type != ItemType.Coin)
			{
				return false;
			}
			coin = componentInParent;
			return true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
