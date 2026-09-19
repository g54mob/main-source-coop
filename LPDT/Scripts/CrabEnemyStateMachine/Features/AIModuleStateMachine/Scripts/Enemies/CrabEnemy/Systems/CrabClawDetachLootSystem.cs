using Cysharp.Threading.Tasks;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CrabClawDetachLootSystem : MonoSystem
	{
		[SerializeField]
		private NetworkBehaviour _crabMeatPrefab;

		[SerializeField]
		private Transform _leftClawSpawnPoint;

		[SerializeField]
		private Transform _rightClawSpawnPoint;

		[SerializeField]
		private float _detachImpulse = 4f;

		[SerializeField]
		private float _upImpulse = 2f;

		private CrabEnemyContext _context;

		private IItemSpawnService _itemSpawnService;

		private bool _isEnabled;

		private CrabClawSide _detachClaw;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(CrabEnemyContext context, IItemSpawnService itemSpawnService)
		{
			_context = context;
			_itemSpawnService = itemSpawnService;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_detachClaw = ((_context != null) ? _context.ActiveClaw : CrabClawSide.None);
			if (base.HasStateAuthority && !(_crabMeatPrefab == null) && _itemSpawnService != null)
			{
				SpawnDetachedClawMeatAsync().Forget();
			}
		}

		public override void Disable()
		{
			_isEnabled = false;
		}

		public override void Clear()
		{
			_detachClaw = CrabClawSide.None;
		}

		private async UniTaskVoid SpawnDetachedClawMeatAsync()
		{
			Transform transform = ResolveSpawnPoint();
			Vector3 position = ((transform != null) ? transform.position : base.transform.position);
			Quaternion rotation = ((transform != null) ? transform.rotation : Quaternion.identity);
			NetworkBehaviour networkBehaviour = await _itemSpawnService.SpawnItem(_crabMeatPrefab, position, rotation);
			if (!(networkBehaviour == null) && networkBehaviour.TryGetComponent<Rigidbody>(out var component))
			{
				Vector3 vector = ResolveOutwardDirection(position);
				component.AddForce(vector * _detachImpulse + Vector3.up * _upImpulse, ForceMode.Impulse);
			}
		}

		private Transform ResolveSpawnPoint()
		{
			if (_detachClaw == CrabClawSide.Left)
			{
				if (!(_leftClawSpawnPoint != null))
				{
					return _rightClawSpawnPoint;
				}
				return _leftClawSpawnPoint;
			}
			if (!(_rightClawSpawnPoint != null))
			{
				return _leftClawSpawnPoint;
			}
			return _rightClawSpawnPoint;
		}

		private Vector3 ResolveOutwardDirection(Vector3 spawnPosition)
		{
			Vector3 vector = ((_context != null && _context.NavMeshAgent != null) ? _context.NavMeshAgent.transform.position : base.transform.position);
			Vector3 vector2 = spawnPosition - vector;
			vector2.y = 0f;
			if (vector2.sqrMagnitude < 0.0001f)
			{
				Transform transform = ((_context != null && _context.NavMeshAgent != null) ? _context.NavMeshAgent.transform : base.transform);
				vector2 = ((_detachClaw == CrabClawSide.Left) ? (-transform.right) : transform.right);
			}
			return vector2.normalized;
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
