using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace QFSW.QC.UI
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class DraggableUI : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		[SerializeField]
		private RectTransform _dragRoot;

		[SerializeField]
		private QuantumConsole _quantumConsole;

		[SerializeField]
		private bool _lockInScreen = true;

		[SerializeField]
		private UnityEvent _onBeginDrag;

		[SerializeField]
		private UnityEvent _onDrag;

		[SerializeField]
		private UnityEvent _onEndDrag;

		private Vector2 _lastPos;

		private bool _isDragging;

		public void OnPointerDown(PointerEventData eventData)
		{
			_isDragging = (bool)_quantumConsole && (bool)_quantumConsole.KeyConfig && _quantumConsole.KeyConfig.DragConsoleKey.IsHeld();
			if (_isDragging)
			{
				_onBeginDrag.Invoke();
				_lastPos = eventData.position;
			}
		}

		public void LateUpdate()
		{
			if (!_isDragging)
			{
				return;
			}
			Transform transform = _dragRoot;
			if (!transform)
			{
				transform = base.transform as RectTransform;
			}
			Vector2 mousePosition = InputHelper.GetMousePosition();
			Vector2 vector = mousePosition - _lastPos;
			_lastPos = mousePosition;
			if (_lockInScreen)
			{
				Vector2 vector2 = new Vector2(Screen.width, Screen.height);
				if (mousePosition.x <= 0f || mousePosition.x >= vector2.x)
				{
					vector.x = 0f;
				}
				if (mousePosition.y <= 0f || mousePosition.y >= vector2.y)
				{
					vector.y = 0f;
				}
			}
			transform.Translate(vector);
			_onDrag.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (_isDragging)
			{
				_isDragging = false;
				_onEndDrag.Invoke();
			}
		}
	}
}
