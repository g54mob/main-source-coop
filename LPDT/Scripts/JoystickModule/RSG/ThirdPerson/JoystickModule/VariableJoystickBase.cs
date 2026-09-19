using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace RSG.ThirdPerson.JoystickModule
{
	public class VariableJoystickBase : JoystickBase
	{
		[FormerlySerializedAs("moveThreshold")]
		[SerializeField]
		private float _moveThreshold = 1f;

		[FormerlySerializedAs("joystickType")]
		[SerializeField]
		private JoystickType _joystickType;

		private Vector2 _fixedPosition = Vector2.zero;

		public float MoveThreshold
		{
			get
			{
				return _moveThreshold;
			}
			set
			{
				_moveThreshold = Mathf.Abs(value);
			}
		}

		protected override string controlPathInternal { get; set; }

		protected override void Start()
		{
			base.Start();
			_fixedPosition = _background.anchoredPosition;
			SetMode(_joystickType);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (_joystickType != JoystickType.Fixed)
			{
				_background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
				_background.gameObject.SetActive(value: true);
			}
			base.OnPointerDown(eventData);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			if (_joystickType != JoystickType.Fixed)
			{
				_background.gameObject.SetActive(value: false);
			}
			base.OnPointerUp(eventData);
		}

		protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
		{
			if (_joystickType == JoystickType.Dynamic && magnitude > _moveThreshold)
			{
				Vector2 vector = normalised * (magnitude - _moveThreshold) * radius;
				_background.anchoredPosition += vector;
			}
			base.HandleInput(magnitude, normalised, radius, cam);
		}

		public void SetMode(JoystickType joystickType)
		{
			_joystickType = joystickType;
			if (joystickType == JoystickType.Fixed)
			{
				_background.anchoredPosition = _fixedPosition;
				_background.gameObject.SetActive(value: true);
			}
			else
			{
				_background.gameObject.SetActive(value: false);
			}
		}
	}
}
