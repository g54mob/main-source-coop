using System;
using UnityEngine;

namespace Rewired.ComponentControls
{
	[Serializable]
	[AddComponentMenu("Rewired/Touch Controller")]
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	public sealed class TouchController : CustomController
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If true, disables mouse input when the Touch Controller script is enabled or GameObject is activated and re-enables mouse input when the script is disabled or GameObject is deactivated. This is useful for disabling Mouse Look controls when using touch controls in an FPS for example.")]
		private bool _disableMouseInputWhenEnabled = true;

		[CustomObfuscation(rename = false)]
		[Tooltip("If true, a Custom Controller will be populated with the data from this controller.")]
		[SerializeField]
		private bool _useCustomController = true;

		[NonSerialized]
		private bool ZONfyvTwmuSbXEYCxPhYKNWDJIaE;

		public bool disableMouseInputWhenEnabled
		{
			get
			{
				return _disableMouseInputWhenEnabled;
			}
			set
			{
				if (_disableMouseInputWhenEnabled != value)
				{
					_disableMouseInputWhenEnabled = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool useCustomController
		{
			get
			{
				return _useCustomController;
			}
			set
			{
				if (_useCustomController != value)
				{
					_useCustomController = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
					if (value)
					{
						ajhXJbgnqLHNDTtpxLqCuAgGrnVb();
					}
				}
			}
		}

		[CustomObfuscation(rename = false)]
		private TouchController()
		{
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDisable()
		{
			base.OnDisable();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && ReInput.isReady)
			{
				gbiGPNIAJVqgQnKFyeBCIIExwkrYA(true);
			}
		}

		internal override bool lpOPYPkfRAdylCMSLphTlIgUWIWy()
		{
			if (!OnInitialize())
			{
				return false;
			}
			if (ReInput.isReady)
			{
				ZONfyvTwmuSbXEYCxPhYKNWDJIaE = ReInput.controllers.Mouse.enabled;
				gbiGPNIAJVqgQnKFyeBCIIExwkrYA(false);
			}
			return true;
		}

		[CustomObfuscation(rename = false)]
		internal override bool GetUseCustomController()
		{
			return _useCustomController;
		}

		[CustomObfuscation(rename = false)]
		internal override void SetUseCustomController(bool value)
		{
			_useCustomController = value;
		}

		private void gbiGPNIAJVqgQnKFyeBCIIExwkrYA(bool P_0)
		{
			if (_disableMouseInputWhenEnabled)
			{
				if (P_0)
				{
					ReInput.controllers.Mouse.enabled = ZONfyvTwmuSbXEYCxPhYKNWDJIaE;
				}
				else
				{
					ReInput.controllers.Mouse.enabled = false;
				}
			}
		}

		private void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
		}

		private bool ajhXJbgnqLHNDTtpxLqCuAgGrnVb()
		{
			if (ReInput.isReady)
			{
				return true;
			}
			Logger.LogError("Rewired is not initialized. You must have an enabled Rewired Input Manager in the scene if using a Touch Controller. Custom Controller support will be disabled on this Touch Controller.");
			SetUseCustomController(value: false);
			return false;
		}
	}
}
