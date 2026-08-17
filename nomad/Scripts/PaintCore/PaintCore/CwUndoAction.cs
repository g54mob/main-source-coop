using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwUndoAction")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Undo Action")]
	public class CwUndoAction : MonoBehaviour
	{
		[SerializeField]
		private bool preUndoAll = true;

		[SerializeField]
		private bool preRedoAll = true;

		[SerializeField]
		public UnityEvent action;

		public bool PreUndoAll
		{
			get
			{
				return preUndoAll;
			}
			set
			{
				preUndoAll = value;
			}
		}

		public bool PreRedoAll
		{
			get
			{
				return preRedoAll;
			}
			set
			{
				preRedoAll = value;
			}
		}

		public UnityEvent Action
		{
			get
			{
				if (action == null)
				{
					action = new UnityEvent();
				}
				return action;
			}
		}

		protected virtual void OnEnable()
		{
			CwStateManager.OnPreUndoAll += HandlePreUndoAll;
			CwStateManager.OnPreRedoAll += HandlePreRedoAll;
		}

		protected virtual void OnDisable()
		{
			CwStateManager.OnPreUndoAll -= HandlePreUndoAll;
			CwStateManager.OnPreRedoAll -= HandlePreRedoAll;
		}

		private void HandlePreUndoAll()
		{
			if (preUndoAll && action != null)
			{
				action.Invoke();
			}
		}

		private void HandlePreRedoAll()
		{
			if (preRedoAll && action != null)
			{
				action.Invoke();
			}
		}
	}
}
