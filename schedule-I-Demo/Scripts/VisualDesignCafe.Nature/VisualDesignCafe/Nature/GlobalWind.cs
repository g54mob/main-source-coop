using UnityEngine;

namespace VisualDesignCafe.Nature
{
	[ExecuteAlways]
	public class GlobalWind : MonoBehaviour
	{
		[SerializeField]
		private WindSettings _windSettings = WindSettings.Calm;

		[SerializeField]
		private WindZone _sourceWindZone;

		[SerializeField]
		private Texture2D _gustNoise;

		[HideInInspector]
		[SerializeField]
		private int _selectedPreset;

		private Quaternion _cachedRotation;

		private float _cachedWindMain;

		private float _cachedWindPulseFrequency;

		private float _cachedWindTurbulence;

		private double _smoothWindOffset;

		private double _cachedTime;

		private Vector2 _windOffset;

		private Vector2 _prevWindOffset;

		private Vector2 _direction = new Vector2(0f, 1f);

		private Vector2 _directionVelocity;

		private float _strength;

		private float _strengthVelocity;

		private float _speed;

		private float _speedVelocity;

		private float _turbulence;

		private float _turbulenceVelocity;

		public static GlobalWind Instance { get; private set; }

		public WindSettings Settings
		{
			get
			{
				return _windSettings;
			}
			set
			{
				_windSettings = value;
				_windSettings.Apply();
				UpdateDirection(useCache: false);
			}
		}

		public WindZone Zone
		{
			get
			{
				return _sourceWindZone;
			}
			set
			{
				_sourceWindZone = value;
				if (value != null)
				{
					ValidateWindZone();
					CopyAndApply();
				}
			}
		}

		public Texture2D GustNoise
		{
			get
			{
				return _gustNoise;
			}
			set
			{
				_gustNoise = value;
				_windSettings.Apply(_gustNoise);
			}
		}

		public void SetFloatingOrigin(double x, double z)
		{
			double num = 0.02;
			Shader.SetGlobalVector("g_FloatingOriginOffset_Gust", new Vector4(Wrap(x, 1.0 / num), Wrap(z, 1.0 / num), 0f, 0f));
			double num2 = 0.0625;
			Shader.SetGlobalVector("g_FloatingOriginOffset_Ambient", new Vector4(Wrap(x, 1.0 / num2), Wrap(z, 1.0 / num2), 0f, 0f));
			double range = 2285.0;
			Shader.SetGlobalVector("g_FloatingOriginOffset_Turbulence", new Vector4(Wrap(x, range), Wrap(z, range), 0f, 0f));
		}

		private float Wrap(double value, double range)
		{
			while (value > range)
			{
				value -= range;
			}
			while (value < range)
			{
				value += range;
			}
			return (float)value;
		}

		public void UpdateTime(double time)
		{
			double num = time - _cachedTime;
			_cachedTime = time;
			Shader.SetGlobalVector("g_PrevSmoothTime", new Vector4((float)_smoothWindOffset * 6f, (float)_smoothWindOffset * 0.15f, (float)_smoothWindOffset * 3.5f, (float)_smoothWindOffset * 3.5f));
			_smoothWindOffset += num * (double)Settings.WindSpeed;
			Shader.SetGlobalVector("g_SmoothTime", new Vector4((float)_smoothWindOffset * 6f, (float)_smoothWindOffset * 0.15f, (float)_smoothWindOffset * 3.5f, (float)_smoothWindOffset * 3.5f));
			_direction = Vector2.SmoothDamp(_direction, Settings.WindDirection, ref _directionVelocity, 1f, 1f, (float)num);
			_turbulence = Mathf.SmoothDamp(_turbulence, Settings.Turbulence, ref _turbulenceVelocity, 1f, 1f, (float)num);
			_speed = Mathf.SmoothDamp(_speed, Settings.WindSpeed, ref _speedVelocity, 1f, 1f, (float)num);
			_strength = Mathf.SmoothDamp(_strength, Settings.WindStrength, ref _strengthVelocity, 1f, 1f, (float)num);
			_prevWindOffset = _windOffset;
			_windOffset += (float)num * _speed * _direction * 0.15f;
			Shader.SetGlobalVector("g_WindOffset", new Vector4(_windOffset.x, _windOffset.y, _prevWindOffset.x, _prevWindOffset.y));
			Shader.SetGlobalVector("g_WindDirection", new Vector4(_direction.x, 0f, _direction.y));
			Shader.SetGlobalVector("g_Wind", new Vector4(_speed, _strength));
			Shader.SetGlobalVector("g_Turbulence", new Vector4(_speed, _turbulence));
		}

		private void OnEnable()
		{
			Instance = this;
			ValidateWindZone();
			if (_sourceWindZone != null)
			{
				CopyFromWindZone();
			}
			else
			{
				UpdateDirection(useCache: false);
			}
			_windSettings.Apply(_gustNoise);
		}

		private void Update()
		{
			if (_sourceWindZone != null && WindZoneHasChanged())
			{
				CopyAndApply();
			}
			if (Application.isPlaying)
			{
				UpdateTime(Time.time);
			}
			UpdateDirection(useCache: true);
		}

		private void CopyAndApply()
		{
			CacheWindZoneProperties();
			CopyFromWindZone();
		}

		private void CopyFromWindZone()
		{
			Settings = WindSettings.FromWindZone(_sourceWindZone);
		}

		private bool WindZoneHasChanged()
		{
			if (_cachedRotation != _sourceWindZone.transform.rotation)
			{
				return true;
			}
			if (_cachedWindMain != _sourceWindZone.windMain)
			{
				return true;
			}
			if (_cachedWindPulseFrequency != _sourceWindZone.windPulseFrequency)
			{
				return true;
			}
			if (_cachedWindTurbulence != _sourceWindZone.windTurbulence)
			{
				return true;
			}
			return false;
		}

		private void CacheWindZoneProperties()
		{
			_cachedRotation = _sourceWindZone.transform.rotation;
			_cachedWindMain = _sourceWindZone.windMain;
			_cachedWindPulseFrequency = _sourceWindZone.windPulseFrequency;
			_cachedWindTurbulence = _sourceWindZone.windTurbulence;
		}

		private void ValidateWindZone()
		{
			if (_sourceWindZone != null && _sourceWindZone.mode != WindZoneMode.Directional)
			{
				Debug.LogWarning(GetType().Name + " requires a directional wind zone.", this);
			}
		}

		private void UpdateDirection(bool useCache)
		{
			if (!(_sourceWindZone != null) && (!useCache || !(base.transform.rotation == _cachedRotation)))
			{
				_cachedRotation = base.transform.rotation;
				_windSettings.WindDirection = WindSettings.RotationToDirection(base.transform.rotation);
				_windSettings.Apply();
			}
		}
	}
}
