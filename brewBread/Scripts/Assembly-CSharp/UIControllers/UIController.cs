using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIControllers
{
	public class UIController : MonoBehaviour
	{
		[Header("UI Controller")]
		[SerializeField]
		protected UIManager.UIStates _state;

		[SerializeField]
		protected EventSystem _eventSystem;

		[SerializeField]
		private List<UIManager.UIStates> _skipExitStates;

		protected UIManager.UIStates _previousState;

		private bool _assigning;

		public UIManager.UIStates PrevState => _previousState;

		public EventSystem EventSystem => _eventSystem;

		public virtual void Start()
		{
			StaticInstance<UIManager>.Instance.AddToDictionary(_state, this);
			_previousState = StaticInstance<UIManager>.Instance.PreviousState;
		}

		public virtual void OnDestroy()
		{
			StaticInstance<UIManager>.Instance?.RemoveFromDictionray(_state);
		}

		public virtual void LateUpdate()
		{
			if (!(_eventSystem == null) && _eventSystem.currentSelectedGameObject == null && !_assigning)
			{
				StartCoroutine(AssignEventSystemSelectedWithDelay());
			}
		}

		private IEnumerator AssignEventSystemSelectedWithDelay()
		{
			_assigning = true;
			yield return new WaitForSeconds(0.5f);
			if (_eventSystem.currentSelectedGameObject == null)
			{
				_eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
			}
			_assigning = false;
		}

		public virtual void Enter()
		{
		}

		public virtual void Exit()
		{
		}

		protected bool SkipExit()
		{
			foreach (UIManager.UIStates skipExitState in _skipExitStates)
			{
				if (StaticInstance<UIManager>.Instance.CurrentState == skipExitState)
				{
					return true;
				}
			}
			return false;
		}

		public virtual void SendEvent(GetEnum getEnum)
		{
			StaticInstance<UIManager>.Instance.SendEvent(getEnum.State);
		}
	}
}
