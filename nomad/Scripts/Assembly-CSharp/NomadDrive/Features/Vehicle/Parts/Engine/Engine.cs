using Ami.BroAudio;
using EvilCore.Particles;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Vehicle.UI;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle.Parts.Engine
{
	[RequireComponent(typeof(ConditionComponent))]
	[RequireComponent(typeof(EngineHeatComponent))]
	public class Engine : AttachableObject
	{
		[SerializeField]
		public EngineConfig engineConfig;

		[SerializeField]
		private ParticleKey engineOverheatParticleKey = new ParticleKey("EngineSteam");

		[Header("Audio")]
		[SerializeField]
		private SoundID repairSound;

		[Inject]
		private EngineHeatInfoPanel _heatPanel;

		private EngineHeatComponent _heatComponent;

		public string EngineOverheatParticleKey => engineOverheatParticleKey;

		public EngineHeatComponent HeatComponent => _heatComponent;

		protected override void Awake()
		{
			base.Awake();
			_heatComponent = GetComponent<EngineHeatComponent>();
		}

		protected override void OnRepair()
		{
			base.OnRepair();
			if (repairSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(repairSound, base.transform.position);
			}
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			if (!(_heatComponent == null))
			{
				_heatPanel?.SetHeat(_heatComponent.HeatRatio);
				_heatPanel?.Show(interactable: false, blockRaycast: false);
				_heatPanel?.SetWorldTarget(base.gameObject);
				_heatComponent.OnHeatLevelChanged.RemoveListener(OnHeatLevelChangedWhileHovered);
				_heatComponent.OnHeatLevelChanged.AddListener(OnHeatLevelChangedWhileHovered);
			}
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			if (_heatComponent != null)
			{
				_heatComponent.OnHeatLevelChanged.RemoveListener(OnHeatLevelChangedWhileHovered);
			}
			_heatPanel?.Clear();
			_heatPanel?.Hide();
			_heatPanel?.ClearWorldTarget();
		}

		private void OnHeatLevelChangedWhileHovered(float oldValue, float newValue)
		{
			if (_heatComponent != null)
			{
				_heatPanel?.SetHeat(_heatComponent.HeatRatio);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
