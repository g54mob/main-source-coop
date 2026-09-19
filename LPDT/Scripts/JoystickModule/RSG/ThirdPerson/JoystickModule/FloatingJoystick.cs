using UnityEngine.EventSystems;

namespace RSG.ThirdPerson.JoystickModule
{
	public class FloatingJoystick : JoystickBase
	{
		protected override string controlPathInternal { get; set; }

		protected override void Start()
		{
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
	}
}
