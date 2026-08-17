using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class MonoPaintSprayTool : MonoBehaviour
	{
		[Header("Tool Tip Configuration")]
		[SerializeField]
		private Transform _toolTip;

		[Header("Particle System")]
		[SerializeField]
		private ParticleSystem _particleSystem;

		[SerializeField]
		private CwHitParticles _hitParticles;

		[Header("Paint Spheres")]
		[SerializeField]
		private CwPaintSphere _colorSphere;

		[SerializeField]
		private CwPaintSphere _maskSphere;

		[Header("Paint Spray Settings")]
		[SerializeField]
		private Color _paintColor = Color.red;

		[SerializeField]
		[Range(0.01f, 0.5f)]
		private float _sphereRadius = 0.08f;

		[SerializeField]
		[Range(0.001f, 20f)]
		private float _hardness = 1.5f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _colorOpacity = 0.15f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _maskOpacity = 0.1f;

		[Header("Spray Pattern")]
		[SerializeField]
		[Range(10f, 500f)]
		private float _emissionRate = 150f;

		[SerializeField]
		[Range(1f, 20f)]
		private float _particleSpeed = 6f;

		[SerializeField]
		[Range(0f, 45f)]
		private float _sprayAngle = 12f;

		[SerializeField]
		[Range(0.1f, 2f)]
		private float _particleLifetime = 0.4f;

		[Header("Target Groups")]
		[SerializeField]
		private int _paintColorGroupIndex = 200;

		[SerializeField]
		private int _paintMaskGroupIndex = 204;

		private PaintSprayCore _core;

		public bool IsToolActive => _core?.IsToolActive ?? false;

		public Color PaintColor
		{
			get
			{
				return _paintColor;
			}
			set
			{
				_paintColor = value;
				SyncToCore();
				_core.UpdatePaintColor();
			}
		}

		private void Awake()
		{
			_core = new PaintSprayCore();
			SyncToCore();
			_core.ValidateSetup(base.transform);
			DisableTool();
		}

		private void Start()
		{
			SyncToCore();
			_core.ConfigureSpheres();
		}

		private void SyncToCore()
		{
			_core.ToolTip = _toolTip;
			_core.ParticleSystem = _particleSystem;
			_core.HitParticles = _hitParticles;
			_core.ColorSphere = _colorSphere;
			_core.MaskSphere = _maskSphere;
			_core.PaintColor = _paintColor;
			_core.SphereRadius = _sphereRadius;
			_core.Hardness = _hardness;
			_core.ColorOpacity = _colorOpacity;
			_core.MaskOpacity = _maskOpacity;
			_core.EmissionRate = _emissionRate;
			_core.ParticleSpeed = _particleSpeed;
			_core.SprayAngle = _sprayAngle;
			_core.ParticleLifetime = _particleLifetime;
			_core.PaintColorGroupIndex = _paintColorGroupIndex;
			_core.PaintMaskGroupIndex = _paintMaskGroupIndex;
		}

		public void EnableTool()
		{
			SyncToCore();
			_core.EnableToolLocal();
			_ = _core.IsToolActive;
		}

		public void DisableTool()
		{
			SyncToCore();
			_core.DisableToolLocal();
		}

		public void ToggleTool()
		{
			if (_core.IsToolActive)
			{
				DisableTool();
			}
			else
			{
				EnableTool();
			}
		}

		private void OnSpraySettingsChanged()
		{
			if (_core != null && Application.isPlaying)
			{
				SyncToCore();
				_core.ConfigureSpheres();
				_core.UpdatePaintColor();
			}
		}

		private void OnParticleSettingsChanged()
		{
			if (_core != null && Application.isPlaying)
			{
				SyncToCore();
				_core.ConfigureParticleSystem();
			}
		}

		public void SetColorRed()
		{
			PaintColor = new Color(0.8f, 0.1f, 0.1f);
		}

		public void SetColorBlue()
		{
			PaintColor = new Color(0.1f, 0.2f, 0.8f);
		}

		public void SetColorGreen()
		{
			PaintColor = new Color(0.1f, 0.6f, 0.1f);
		}

		public void SetColorWhite()
		{
			PaintColor = Color.white;
		}

		public void SetColorBlack()
		{
			PaintColor = new Color(0.05f, 0.05f, 0.05f);
		}

		public void SetColorYellow()
		{
			PaintColor = new Color(0.9f, 0.8f, 0.1f);
		}

		public void SetPresetUltraSmooth()
		{
			_sphereRadius = 0.12f;
			_hardness = 1f;
			_colorOpacity = 0.1f;
			_maskOpacity = 0.08f;
			_emissionRate = 200f;
			_particleSpeed = 5f;
			_sprayAngle = 10f;
			SyncAndUpdate();
		}

		public void SetPresetBalanced()
		{
			_sphereRadius = 0.08f;
			_hardness = 1.5f;
			_colorOpacity = 0.15f;
			_maskOpacity = 0.1f;
			_emissionRate = 150f;
			_particleSpeed = 6f;
			_sprayAngle = 12f;
			SyncAndUpdate();
		}

		public void SetPresetPerformance()
		{
			_sphereRadius = 0.05f;
			_hardness = 3f;
			_colorOpacity = 0.25f;
			_maskOpacity = 0.15f;
			_emissionRate = 80f;
			_particleSpeed = 8f;
			_sprayAngle = 15f;
			SyncAndUpdate();
		}

		public void SetPresetAirbrush()
		{
			_sphereRadius = 0.15f;
			_hardness = 0.5f;
			_colorOpacity = 0.05f;
			_maskOpacity = 0.03f;
			_emissionRate = 300f;
			_particleSpeed = 4f;
			_sprayAngle = 8f;
			SyncAndUpdate();
		}

		private void SyncAndUpdate()
		{
			SyncToCore();
			_core.ConfigureParticleSystem();
			_core.ConfigureSpheres();
		}

		private void FullAutoSetup()
		{
			SyncToCore();
			_core.FullAutoSetup(base.transform);
			_toolTip = _core.ToolTip;
			_particleSystem = _core.ParticleSystem;
			_hitParticles = _core.HitParticles;
			_colorSphere = _core.ColorSphere;
			_maskSphere = _core.MaskSphere;
		}

		private void UpdateParticleSettings()
		{
			SyncToCore();
			_core.ConfigureParticleSystem();
			_core.ConfigureSpheres();
		}

		private void FindComponents()
		{
			SyncToCore();
			_core.FindComponents(base.transform);
			_toolTip = _core.ToolTip;
			_particleSystem = _core.ParticleSystem;
			_hitParticles = _core.HitParticles;
			_colorSphere = _core.ColorSphere;
			_maskSphere = _core.MaskSphere;
		}
	}
}
