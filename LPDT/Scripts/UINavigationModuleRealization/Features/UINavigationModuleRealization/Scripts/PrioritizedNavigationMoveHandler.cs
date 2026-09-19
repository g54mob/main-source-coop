using System.Collections.Generic;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.UINavigationModuleRealization.Scripts
{
	public class PrioritizedNavigationMoveHandler : MonoBehaviour
	{
		[SerializeField]
		private MonoBehaviour _moveSource;

		[SerializeField]
		private List<PrioritizedNavigationDirectionEntry> _directionEntries = new List<PrioritizedNavigationDirectionEntry>();

		private ISelectableWithNavigationMoveCallbacks _moveCallbacks;

		private Selectable _selfSelectable;

		private void Awake()
		{
			RefreshSource();
			Bind();
		}

		private void OnDestroy()
		{
			Unbind();
		}

		public void RefreshSource()
		{
			if (_moveSource == null || !(_moveSource is ISelectableWithNavigationMoveCallbacks))
			{
				_moveSource = null;
				MonoBehaviour[] components = GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour monoBehaviour in components)
				{
					if (!(monoBehaviour == null) && !(monoBehaviour == this) && monoBehaviour is ISelectableWithNavigationMoveCallbacks)
					{
						_moveSource = monoBehaviour;
						break;
					}
				}
			}
			_moveCallbacks = _moveSource as ISelectableWithNavigationMoveCallbacks;
			_selfSelectable = ResolveSelectable(_moveSource);
		}

		private Selectable ResolveSelectable(MonoBehaviour moveSource)
		{
			if (moveSource is ISelectableWithNavigationMoveCallbacks selectableWithNavigationMoveCallbacks)
			{
				return selectableWithNavigationMoveCallbacks.GetSelectable();
			}
			return null;
		}

		private void Bind()
		{
			if (_moveCallbacks != null)
			{
				_moveCallbacks.BeforeMoveEvent += OnBeforeMove;
			}
		}

		private void Unbind()
		{
			if (_moveCallbacks != null)
			{
				_moveCallbacks.BeforeMoveEvent -= OnBeforeMove;
			}
		}

		private void OnBeforeMove(MoveDirection direction)
		{
			if (!(_selfSelectable == null))
			{
				PrioritizedNavigationDirectionEntry entryForDirection = GetEntryForDirection(direction);
				if (entryForDirection != null)
				{
					Selectable target = ResolveTarget(entryForDirection.Targets);
					ApplyNavigationForDirection(direction, target);
				}
			}
		}

		private PrioritizedNavigationDirectionEntry GetEntryForDirection(MoveDirection direction)
		{
			foreach (PrioritizedNavigationDirectionEntry directionEntry in _directionEntries)
			{
				if (directionEntry != null && directionEntry.Direction == direction)
				{
					return directionEntry;
				}
			}
			return null;
		}

		private Selectable ResolveTarget(IReadOnlyList<Selectable> targets)
		{
			if (targets == null)
			{
				return null;
			}
			foreach (Selectable target in targets)
			{
				if (IsNavigable(target))
				{
					return target;
				}
			}
			return null;
		}

		private bool IsNavigable(Selectable selectable)
		{
			if (selectable != null && selectable.IsActive() && selectable.interactable)
			{
				return selectable.gameObject.activeInHierarchy;
			}
			return false;
		}

		private void ApplyNavigationForDirection(MoveDirection direction, Selectable target)
		{
			Navigation navigation = _selfSelectable.navigation;
			switch (direction)
			{
			default:
				return;
			case MoveDirection.Right:
				navigation.selectOnRight = target;
				break;
			case MoveDirection.Left:
				navigation.selectOnLeft = target;
				break;
			case MoveDirection.Up:
				navigation.selectOnUp = target;
				break;
			case MoveDirection.Down:
				navigation.selectOnDown = target;
				break;
			}
			_selfSelectable.navigation = navigation;
		}
	}
}
