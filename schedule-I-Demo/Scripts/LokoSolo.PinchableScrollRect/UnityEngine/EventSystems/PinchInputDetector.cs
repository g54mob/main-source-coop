using UnityEngine.Events;

namespace UnityEngine.EventSystems
{
	[DisallowMultipleComponent]
	public class PinchInputDetector : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
	{
		private IPinchStartHandler[] pinchStartHandlers;

		private IPinchEndHandler[] pinchEndHandlers;

		private IPinchZoomHandler[] pinchZoomHandlers;

		private int touchCount;

		private bool pinching;

		private PointerEventData firstPointer;

		private PointerEventData secondPointer;

		private float previousDistance;

		private float delta;

		[SerializeField]
		protected UnityEvent onPinchStart;

		[SerializeField]
		protected UnityEvent onPinchEnd;

		protected virtual void Awake()
		{
			pinchStartHandlers = GetComponents<IPinchStartHandler>();
			pinchEndHandlers = GetComponents<IPinchEndHandler>();
			pinchZoomHandlers = GetComponents<IPinchZoomHandler>();
		}

		protected virtual void RegisterPointer(PointerEventData eventData)
		{
			touchCount++;
			if (firstPointer == null)
			{
				firstPointer = eventData;
			}
			else if (secondPointer == null)
			{
				secondPointer = eventData;
				CalculateDistanceDelta();
				if (touchCount >= 2)
				{
					pinching = true;
					FireOnPinchStart(new PinchEventData(secondPointer, firstPointer));
				}
			}
		}

		protected virtual void UnregisterPointer(PointerEventData eventData)
		{
			touchCount--;
			if (touchCount < 0)
			{
				Debug.LogError("PinchInputDetector - Touch Count Mismatch");
			}
			if (IsEqualPointer(firstPointer, eventData))
			{
				if (pinching)
				{
					pinching = false;
					FireOnPinchEnd(new PinchEventData(firstPointer, secondPointer));
				}
				if (secondPointer != null)
				{
					firstPointer = secondPointer;
					secondPointer = null;
				}
				else
				{
					firstPointer = null;
				}
			}
			else if (IsEqualPointer(secondPointer, eventData))
			{
				if (pinching)
				{
					pinching = false;
					FireOnPinchEnd(new PinchEventData(secondPointer, firstPointer));
				}
				secondPointer = null;
			}
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			RegisterPointer(eventData);
			if (touchCount != 1)
			{
				if (pinching)
				{
					eventData.Use();
				}
				else if (!IsEqualPointer(firstPointer, eventData) && !IsEqualPointer(secondPointer, eventData))
				{
					eventData.Use();
				}
			}
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			UnregisterPointer(eventData);
			if (touchCount != 0)
			{
				if (pinching)
				{
					eventData.Use();
				}
				else if (!IsEqualPointer(firstPointer, eventData) && !IsEqualPointer(secondPointer, eventData))
				{
					eventData.Use();
				}
			}
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (touchCount == 0)
			{
				return;
			}
			if (IsEqualPointer(firstPointer, eventData))
			{
				firstPointer = eventData;
				if (secondPointer != null)
				{
					CalculateDistanceDelta();
				}
				if (pinching)
				{
					eventData.Use();
					FireOnPinchZoom(new PinchEventData(firstPointer, secondPointer, delta));
				}
			}
			else if (IsEqualPointer(secondPointer, eventData))
			{
				secondPointer = eventData;
				if (firstPointer != null)
				{
					CalculateDistanceDelta();
				}
				if (pinching)
				{
					eventData.Use();
					FireOnPinchZoom(new PinchEventData(secondPointer, firstPointer, delta));
				}
			}
			else
			{
				eventData.Use();
			}
		}

		private bool IsEqualPointer(PointerEventData a, PointerEventData b)
		{
			if (a == null)
			{
				return false;
			}
			if (b == null)
			{
				return false;
			}
			return a.pointerId == b.pointerId;
		}

		protected virtual void CalculateDistanceDelta()
		{
			float num = Vector2.Distance(firstPointer.position, secondPointer.position);
			delta = num - previousDistance;
			previousDistance = num;
		}

		protected virtual void FireOnPinchStart(PinchEventData data)
		{
			if (onPinchStart != null)
			{
				onPinchStart.Invoke();
			}
			if (pinchStartHandlers != null)
			{
				for (int i = 0; i < pinchStartHandlers.Length; i++)
				{
					pinchStartHandlers[i].OnPinchStart(data);
				}
			}
		}

		protected virtual void FireOnPinchEnd(PinchEventData data)
		{
			if (onPinchEnd != null)
			{
				onPinchEnd.Invoke();
			}
			if (pinchEndHandlers != null)
			{
				for (int i = 0; i < pinchEndHandlers.Length; i++)
				{
					pinchEndHandlers[i].OnPinchEnd(data);
				}
			}
		}

		protected virtual void FireOnPinchZoom(PinchEventData data)
		{
			if (pinchZoomHandlers != null)
			{
				for (int i = 0; i < pinchZoomHandlers.Length; i++)
				{
					pinchZoomHandlers[i].OnPinchZoom(data);
				}
			}
		}
	}
}
