using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public abstract class MonoRestorationTool : MonoBehaviour
	{
		[Header("Tool Tip Configuration")]
		[SerializeField]
		protected Transform _toolTip;

		[SerializeField]
		protected CwHitBetween _hitBetween;

		[Header("Hit Detection (Fast Movement)")]
		[SerializeField]
		[Range(-1f, 1f)]
		protected float _hitInterval;

		[SerializeField]
		[Range(1f, 3f)]
		protected float _fastMovementRadiusMultiplier = 1.5f;

		[Header("Paint Configuration")]
		[SerializeField]
		protected CwPaintSphere[] _paintSpheres;

		[SerializeField]
		[Range(0.01f, 1f)]
		protected float _activeRadius = 0.1f;

		[SerializeField]
		protected ParticleSystem _toolEffect;

		[Header("Pressure Sensitivity")]
		[SerializeField]
		protected bool _enablePressureResponse = true;

		[SerializeField]
		[Range(0.1f, 1f)]
		protected float _minPressureOpacity = 0.3f;

		[SerializeField]
		[Range(0.5f, 5f)]
		protected float _maxMovementSpeed = 2f;

		protected RestorationToolCore _core;

		public bool IsToolActive => _core?.IsToolActive ?? false;

		protected abstract void ConfigurePaintSpheres();

		protected virtual float GetToolRadius()
		{
			return _activeRadius;
		}

		protected virtual float GetBaseOpacity()
		{
			return 0.7f;
		}

		protected virtual void Awake()
		{
			InitCore();
			_core.ValidateSetup(base.transform, GetType().Name);
			DisableTool();
		}

		protected virtual void Start()
		{
			ConfigurePaintSpheres();
			_core.Initialize(GetBaseOpacity());
		}

		protected virtual void Update()
		{
			if (_core.IsToolActive && _enablePressureResponse)
			{
				_core.UpdatePressureSensitivity();
			}
		}

		private void InitCore()
		{
			_core = new RestorationToolCore();
			SyncToCore();
		}

		private void SyncToCore()
		{
			_core.ToolTip = _toolTip;
			_core.HitBetween = _hitBetween;
			_core.PaintSpheres = _paintSpheres;
			_core.ToolEffect = _toolEffect;
			_core.HitInterval = _hitInterval;
			_core.FastMovementRadiusMultiplier = _fastMovementRadiusMultiplier;
			_core.ActiveRadius = _activeRadius;
			_core.EnablePressureResponse = _enablePressureResponse;
			_core.MinPressureOpacity = _minPressureOpacity;
			_core.MaxMovementSpeed = _maxMovementSpeed;
		}

		public virtual void EnableTool()
		{
			SyncToCore();
			_ = _core.EnableToolLocal().radius;
			_ = 0f;
		}

		public virtual void DisableTool()
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

		protected void OnHitSettingsChanged()
		{
			if (_core != null)
			{
				_core.HitInterval = _hitInterval;
				_core.OnHitSettingsChanged();
			}
		}

		protected void OnRadiusMultiplierChanged()
		{
			if (_core != null)
			{
				_core.FastMovementRadiusMultiplier = _fastMovementRadiusMultiplier;
				_core.OnRadiusMultiplierChanged();
			}
		}

		public void SetHitPresetUltraSmooth()
		{
			SyncToCore();
			_core.SetHitPresetUltraSmooth();
			_hitInterval = _core.HitInterval;
			_fastMovementRadiusMultiplier = _core.FastMovementRadiusMultiplier;
		}

		public void SetHitPresetBalanced()
		{
			SyncToCore();
			_core.SetHitPresetBalanced();
			_hitInterval = _core.HitInterval;
			_fastMovementRadiusMultiplier = _core.FastMovementRadiusMultiplier;
		}

		public void SetHitPresetPrecision()
		{
			SyncToCore();
			_core.SetHitPresetPrecision();
			_hitInterval = _core.HitInterval;
			_fastMovementRadiusMultiplier = _core.FastMovementRadiusMultiplier;
		}

		protected virtual void FullAutoSetup()
		{
			SyncToCore();
			_core.FullAutoSetup(base.transform, GetType().Name, SetupPaintSpheresOnToolTip);
			_toolTip = _core.ToolTip;
			_hitBetween = _core.HitBetween;
			_paintSpheres = _core.PaintSpheres;
		}

		protected virtual void SetupPaintSpheresOnToolTip()
		{
		}

		protected void FindComponents()
		{
			SyncToCore();
			_core.FindComponents(base.transform, GetType().Name);
			_toolTip = _core.ToolTip;
			_hitBetween = _core.HitBetween;
			_paintSpheres = _core.PaintSpheres;
		}
	}
}
