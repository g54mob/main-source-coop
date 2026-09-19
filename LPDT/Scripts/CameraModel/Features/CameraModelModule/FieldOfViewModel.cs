using System;
using UnityEngine;

namespace Features.CameraModelModule
{
	public class FieldOfViewModel
	{
		public const float MIN_FIELD_OF_VIEW = 60f;

		public const float MAX_FIELD_OF_VIEW = 90f;

		public const float DEFAULT_FIELD_OF_VIEW = 60f;

		private float _fieldOfView = 60f;

		public float FieldOfView
		{
			get
			{
				return _fieldOfView;
			}
			set
			{
				_fieldOfView = ((value < 60f) ? 60f : Mathf.Min(value, 90f));
				this.OnFieldOfViewChanged?.Invoke(_fieldOfView);
			}
		}

		public event Action<float> OnFieldOfViewChanged;
	}
}
