using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.DamageableTrackModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.WeaponModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class HealingFishBehaviour : WeaponMonoBase
	{
		[SerializeField]
		private float _healAmount = 10f;

		[SerializeField]
		private float _velocityToAttack = 10f;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Transform _attackOrigin;

		[SerializeField]
		private float _searchRange = 0.3f;

		[SerializeField]
		private LayerMask _targetLayers = -1;

		[SerializeField]
		private float _attackRange = 0.5f;

		private bool _waitingForCollision;

		private CancellationTokenSource _cts;

		public override bool TryToAttack()
		{
			if (!base.TryToAttack())
			{
				return false;
			}
			if (!_waitingForCollision)
			{
				WaitForCollisionAsync(_cts.Token).Forget();
			}
			return true;
		}

		protected new void Attack()
		{
			if (_simplePointGrabable.GrabbedByPlayers.Contains(base.Runner.LocalPlayer.PlayerId))
			{
				TryToAttack();
			}
		}

		private void FixedUpdate()
		{
			if (_rigidbody.linearVelocity.magnitude > _velocityToAttack)
			{
				Attack();
			}
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_cts = new CancellationTokenSource();
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_cts?.Cancel();
			_cts?.Dispose();
		}

		private async UniTaskVoid WaitForCollisionAsync(CancellationToken token)
		{
			_waitingForCollision = true;
			float elapsed = 0f;
			float timeout = 0.5f;
			try
			{
				while (elapsed < timeout)
				{
					token.ThrowIfCancellationRequested();
					elapsed += Time.deltaTime;
					if (Physics.OverlapSphere((_attackOrigin != null) ? _attackOrigin.position : base.transform.position, _searchRange, _targetLayers).Any((Collider c) => c.gameObject != base.gameObject))
					{
						AttackLogic();
						break;
					}
					await UniTask.Yield(PlayerLoopTiming.Update, token);
				}
			}
			catch (OperationCanceledException)
			{
				_waitingForCollision = false;
				return;
			}
			_waitingForCollision = false;
		}

		private void AttackLogic()
		{
			List<Collider> list = (from c in Physics.OverlapSphere((_attackOrigin != null) ? _attackOrigin.position : base.transform.position, _attackRange, _targetLayers)
				where c.gameObject != base.gameObject
				select c).ToList();
			List<IDamageable> list2 = new List<IDamageable>();
			foreach (Collider item in list)
			{
				if (!(item.attachedRigidbody == _rigidbody) && FindComponent<IDamageable>(item.gameObject, out var component) && !list2.Contains(component) && !component.IsFullHealth)
				{
					component.Heal(_healAmount, isSynchronize: true);
					list2.Add(component);
					UpdateUsageCountRPC();
				}
			}
		}

		private bool FindComponent<T>(GameObject other, out T component)
		{
			component = other.GetComponent<T>();
			T val = component;
			if (val == null)
			{
				component = other.GetComponentInParent<T>();
			}
			val = component;
			if (val == null)
			{
				component = other.GetComponentInChildren<T>();
			}
			return component != null;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
