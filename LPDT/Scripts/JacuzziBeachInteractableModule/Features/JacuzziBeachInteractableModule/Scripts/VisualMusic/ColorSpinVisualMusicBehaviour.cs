using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts.VisualMusic
{
	public class ColorSpinVisualMusicBehaviour : VisualMusicBehaviour
	{
		[SerializeField]
		private float _colorChangeInterval = 0.6f;

		[SerializeField]
		private float _colorLerpSpeed = 4f;

		[SerializeField]
		private float _saturation = 0.9f;

		[SerializeField]
		private float _value = 1f;

		[Header("Spin")]
		[Tooltip("Axis the lights sweep around, in LightsRoot space. The sweep never crosses to the other side of it.")]
		[SerializeField]
		private Vector3 _spinAxis = Vector3.down;

		[Tooltip("Angle between the axis and the light direction. Stays below 90 so the cone never flips over.")]
		[SerializeField]
		private Vector2 _tiltAngleRange = new Vector2(20f, 60f);

		[SerializeField]
		private Vector2 _spinSpeedRange = new Vector2(30f, 180f);

		private float[] _tiltAngles;

		private float[] _spinSpeeds;

		private float[] _yawAngles;

		private Color[] _targetColors;

		private float _colorChangeElapsed;

		protected override void OnEffectStarted()
		{
			if (_tiltAngles == null || _tiltAngles.Length != base.Lights.Length)
			{
				_tiltAngles = new float[base.Lights.Length];
				_spinSpeeds = new float[base.Lights.Length];
				_yawAngles = new float[base.Lights.Length];
				_targetColors = new Color[base.Lights.Length];
			}
			_colorChangeElapsed = 0f;
			for (int i = 0; i < base.Lights.Length; i++)
			{
				_tiltAngles[i] = Mathf.Clamp(Random.Range(_tiltAngleRange.x, _tiltAngleRange.y), 0f, 89f);
				_spinSpeeds[i] = Random.Range(_spinSpeedRange.x, _spinSpeedRange.y);
				_yawAngles[i] = Random.Range(0f, 360f);
				_targetColors[i] = GetRandomColor();
			}
		}

		protected override void OnEffectTick(float deltaTime)
		{
			_colorChangeElapsed += deltaTime;
			bool flag = _colorChangeElapsed >= _colorChangeInterval;
			if (flag)
			{
				_colorChangeElapsed = 0f;
			}
			Vector3 vector = LightsRoot.TransformDirection(_spinAxis.normalized);
			Vector3 perpendicular = GetPerpendicular(vector);
			for (int i = 0; i < base.Lights.Length; i++)
			{
				if (flag)
				{
					_targetColors[i] = GetRandomColor();
				}
				Color b = Color.Lerp(base.Lights[i].color, _targetColors[i], _colorLerpSpeed * deltaTime);
				base.Lights[i].color = Color.Lerp(base.DefaultColors[i], b, base.Blend);
				_yawAngles[i] = Mathf.Repeat(_yawAngles[i] + _spinSpeeds[i] * base.Blend * deltaTime, 360f);
				Vector3 vector2 = Quaternion.AngleAxis(_tiltAngles[i], perpendicular) * vector;
				Quaternion b2 = Quaternion.LookRotation(Quaternion.AngleAxis(_yawAngles[i], vector) * vector2, perpendicular);
				Transform parent = base.Lights[i].transform.parent;
				Quaternion a = ((parent == null) ? base.DefaultLocalRotations[i] : (parent.rotation * base.DefaultLocalRotations[i]));
				base.Lights[i].transform.rotation = Quaternion.Slerp(a, b2, base.Blend);
			}
		}

		protected override void OnEffectStopped()
		{
		}

		private Color GetRandomColor()
		{
			return Random.ColorHSV(0f, 1f, _saturation, _saturation, _value, _value);
		}

		private Vector3 GetPerpendicular(Vector3 axis)
		{
			Vector3 rhs = ((Mathf.Abs(Vector3.Dot(axis, Vector3.up)) > 0.99f) ? Vector3.forward : Vector3.up);
			return Vector3.Cross(axis, rhs).normalized;
		}
	}
}
