using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RSG.ThirdPerson.JoystickModule
{
	public class VisibleFloatingJoystick : JoystickBase
	{
		[SerializeField]
		private float _transparencyDividerWhenInnactive;

		[SerializeField]
		private List<Image> _imagesToChangeTransparencyOn;

		[SerializeField]
		private float _initialAlpha;

		private Vector2 _initialPosition;

		protected override string controlPathInternal { get; set; }

		public event Action OnPointerDownEvent;

		public event Action OnPointerUpEvent;

		protected override void Start()
		{
			base.Start();
			_background.gameObject.SetActive(value: true);
			_initialPosition = _handle.position;
			foreach (Image item in _imagesToChangeTransparencyOn)
			{
				item.color = new Color(item.color.r, item.color.g, item.color.b, _initialAlpha / _transparencyDividerWhenInnactive);
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			this.OnPointerDownEvent?.Invoke();
			Vector2 position = eventData.position;
			_background.position = new Vector2(position.x, position.y);
			foreach (Image item in _imagesToChangeTransparencyOn)
			{
				item.color = new Color(item.color.r, item.color.g, item.color.b, _initialAlpha);
			}
			base.OnPointerDown(eventData);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			this.OnPointerUpEvent?.Invoke();
			Vector2 initialPosition = _initialPosition;
			_background.position = new Vector2(initialPosition.x, initialPosition.y);
			foreach (Image item in _imagesToChangeTransparencyOn)
			{
				item.color = new Color(item.color.r, item.color.g, item.color.b, _initialAlpha / _transparencyDividerWhenInnactive);
			}
			base.OnPointerUp(eventData);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			foreach (Image item in _imagesToChangeTransparencyOn)
			{
				item.color = new Color(item.color.r, item.color.g, item.color.b, _initialAlpha / _transparencyDividerWhenInnactive);
			}
		}

		public bool IsInitialPositionInitialized()
		{
			return _initialPosition != Vector2.zero;
		}
	}
}
