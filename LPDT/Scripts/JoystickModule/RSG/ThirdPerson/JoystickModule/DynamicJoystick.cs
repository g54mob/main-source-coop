using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace RSG.ThirdPerson.JoystickModule
{
	public class DynamicJoystick : JoystickBase
	{
		[FormerlySerializedAs("moveThreshold")]
		[SerializeField]
		private float _moveThreshold = 1f;

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
			MoveThreshold = _moveThreshold;
			base.Start();
			_background.gameObject.SetActive(value: false);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			_background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
			_background.gameObject.SetActive(value: true);
			base.OnPointerDown(eventData);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			_background.gameObject.SetActive(value: false);
			base.OnPointerUp(eventData);
		}

		protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
		{
			if (magnitude > _moveThreshold)
			{
				Vector2 vector = normalised * (magnitude - _moveThreshold) * radius;
				_background.anchoredPosition += vector;
			}
			base.HandleInput(magnitude, normalised, radius, cam);
		}
	}
}
