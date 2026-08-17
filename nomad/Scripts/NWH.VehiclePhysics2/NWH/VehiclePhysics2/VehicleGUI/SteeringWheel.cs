using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NWH.VehiclePhysics2.VehicleGUI
{
	public class SteeringWheel : MonoBehaviour
	{
		[Tooltip("    Maximum angle that the steering wheel can be turned to towards either side in degrees.")]
		public float maximumSteeringAngle = 200f;

		[Tooltip("    Speed at which wheel is returned to center in degrees per second.")]
		public float returnToCenterSpeed = 400f;

		public Graphic steeringWheelGraphic;

		private Vector2 _centerPoint;

		private RectTransform _rectT;

		private float _wheelAngle;

		private bool _wheelBeingHeld;

		private float _wheelPrevAngle;

		private void Start()
		{
			_rectT = steeringWheelGraphic.rectTransform;
			InitEventsSystem();
			UpdateRect();
		}

		private void Update()
		{
			if (!_wheelBeingHeld && !Mathf.Approximately(0f, _wheelAngle))
			{
				float num = returnToCenterSpeed * Time.deltaTime;
				if (Mathf.Abs(num) > Mathf.Abs(_wheelAngle))
				{
					_wheelAngle = 0f;
				}
				else if (_wheelAngle > 0f)
				{
					_wheelAngle -= num;
				}
				else
				{
					_wheelAngle += num;
				}
			}
			_rectT.localEulerAngles = Vector3.back * _wheelAngle;
		}

		private void UpdateRect()
		{
			Vector3[] array = new Vector3[4];
			_rectT.GetWorldCorners(array);
			for (int i = 0; i < 4; i++)
			{
				array[i] = RectTransformUtility.WorldToScreenPoint(null, array[i]);
			}
			Vector3 vector = array[0];
			Vector3 vector2 = array[2];
			float width = vector2.x - vector.x;
			float height = vector2.y - vector.y;
			Rect rect = new Rect(vector.x, vector2.y, width, height);
			_centerPoint = new Vector2(rect.x + rect.width * 0.5f, rect.y - rect.height * 0.5f);
		}

		public void DragEvent(BaseEventData eventData)
		{
			Vector2 position = ((PointerEventData)eventData).position;
			float num = Vector2.Angle(Vector2.up, position - _centerPoint);
			if (Vector2.Distance(position, _centerPoint) > 20f)
			{
				if (position.x > _centerPoint.x)
				{
					_wheelAngle += num - _wheelPrevAngle;
				}
				else
				{
					_wheelAngle -= num - _wheelPrevAngle;
				}
			}
			_wheelAngle = Mathf.Clamp(_wheelAngle, 0f - maximumSteeringAngle, maximumSteeringAngle);
			_wheelPrevAngle = num;
		}

		public float GetClampedValue()
		{
			return _wheelAngle / maximumSteeringAngle;
		}

		public void PressEvent(BaseEventData eventData)
		{
			Vector2 position = ((PointerEventData)eventData).position;
			_wheelBeingHeld = true;
			_wheelPrevAngle = Vector2.Angle(Vector2.up, position - _centerPoint);
		}

		public void ReleaseEvent(BaseEventData eventData)
		{
			DragEvent(eventData);
			_wheelBeingHeld = false;
		}

		private void InitEventsSystem()
		{
			EventTrigger eventTrigger = steeringWheelGraphic.gameObject.GetComponent<EventTrigger>();
			if (eventTrigger == null)
			{
				eventTrigger = steeringWheelGraphic.gameObject.AddComponent<EventTrigger>();
			}
			if (eventTrigger.triggers == null)
			{
				eventTrigger.triggers = new List<EventTrigger.Entry>();
			}
			EventTrigger.Entry entry = new EventTrigger.Entry();
			EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
			UnityAction<BaseEventData> call = PressEvent;
			triggerEvent.AddListener(call);
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback = triggerEvent;
			eventTrigger.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			triggerEvent = new EventTrigger.TriggerEvent();
			call = DragEvent;
			triggerEvent.AddListener(call);
			entry.eventID = EventTriggerType.Drag;
			entry.callback = triggerEvent;
			eventTrigger.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			triggerEvent = new EventTrigger.TriggerEvent();
			call = ReleaseEvent;
			triggerEvent.AddListener(call);
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback = triggerEvent;
			eventTrigger.triggers.Add(entry);
		}
	}
}
