using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Sensors
{
	public class MonkeyItemSensor
	{
		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyEnemySettings _enemySettings;

		private readonly MonkeyItemInteractionSettings _itemSettings;

		private readonly Collider[] _heldCoinOverlapBuffer;

		public MonkeyItemSensor(MonkeyEnemyContext context, MonkeyEnemySettings enemySettings, MonkeyItemInteractionSettings itemSettings)
		{
			_context = context;
			_enemySettings = enemySettings;
			_itemSettings = itemSettings;
			_heldCoinOverlapBuffer = new Collider[Mathf.Max(1, _itemSettings.HeldCoinOverlapBufferSize)];
		}

		public bool TryAcceptFallenItem(IItem item, Tick currentTick, int tickRate)
		{
			if (!IsValidPreferredItem(item))
			{
				return false;
			}
			if (IsHeldByPlayer(item))
			{
				return false;
			}
			Tick tick = new Tick
			{
				Raw = item.LastGrabTikRaw
			};
			Tick tick2 = tickRate * _itemSettings.MaxItemGrabTime;
			if (currentTick > (int)tick + (int)tick2)
			{
				return false;
			}
			Vector3 itemPosition = _context.GetItemPosition(item);
			if (itemPosition == Vector3.zero)
			{
				return false;
			}
			float num = Vector3.Distance(_context.transform.position, itemPosition);
			if (num > _itemSettings.ItemDetectionRadius)
			{
				return false;
			}
			if (_context.HasTargetItem)
			{
				Vector3 itemPosition2 = _context.GetItemPosition(_context.TargetItem);
				if (Vector3.Distance(_context.transform.position, itemPosition2) < num)
				{
					return false;
				}
			}
			return _context.TrySetTargetItem(item);
		}

		public bool TryAcquireHeldCoin()
		{
			if (_context.TargetItem != null)
			{
				return false;
			}
			int num = Physics.OverlapSphereNonAlloc(_context.transform.position, _itemSettings.HeldCoinDetectionRadius, _heldCoinOverlapBuffer);
			for (int i = 0; i < num; i++)
			{
				Collider collider = _heldCoinOverlapBuffer[i];
				MonoItem component;
				MonoItem item = (collider.TryGetComponent<MonoItem>(out component) ? component : collider.GetComponentInParent<MonoItem>());
				if (IsValidPreferredItem(item) && IsHeldByPlayer(item))
				{
					return _context.TrySetTargetItem(item);
				}
			}
			return false;
		}

		private bool IsValidPreferredItem(IItem item)
		{
			if (item != null && !item.IsDespawned && !item.IsConsumed && item.AvailableForEnemy)
			{
				return item.Type == _enemySettings.PreferredItemType;
			}
			return false;
		}

		private static bool IsHeldByPlayer(IItem item)
		{
			if (item is MonoItem monoItem && monoItem.TryGetComponent<IPointGrabable>(out var component))
			{
				return component.GrabbedByPlayers.Count > 0;
			}
			return false;
		}
	}
}
