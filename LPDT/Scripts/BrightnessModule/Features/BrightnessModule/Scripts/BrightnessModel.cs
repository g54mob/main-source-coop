using System;

namespace Features.BrightnessModule.Scripts
{
	public class BrightnessModel
	{
		private float _brightness;

		public float Brightness
		{
			get
			{
				return _brightness;
			}
			set
			{
				_brightness = value;
				this.OnBrightnessChanged?.Invoke();
			}
		}

		public event Action OnBrightnessChanged;
	}
}
