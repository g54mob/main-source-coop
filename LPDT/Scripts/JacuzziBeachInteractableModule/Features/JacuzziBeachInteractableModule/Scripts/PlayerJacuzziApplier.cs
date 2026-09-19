using System;
using System.Collections.Generic;
using Features.JacuzziBeachInteractableModule.Scripts.Rendering;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.StrechArmsModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	public class PlayerJacuzziApplier : MonoBehaviour
	{
		private const float BLEND_EPSILON = 0.001f;

		[SerializeField]
		private StaticColorChangerByPlayerRef _staticColorChangerByPlayerRef;

		[SerializeField]
		private ParticleSystem _steamParticleSystem;

		[SerializeField]
		private NetworkObject _networkObject;

		private JacuzziBeachInteractableConfiguration _configuration;

		private ArmVisualsControllerModel _armVisualsControllerModel;

		private ArmStartsModel _armStartsModel;

		private MultiplayerModel _multiplayerModel;

		private JacuzziHazeBlendAggregator _hazeBlendAggregator;

		private float _appliedBlend = -1f;

		[Inject]
		public void InjectDependencies(JacuzziBeachInteractableConfiguration configuration, ArmVisualsControllerModel armVisualsControllerModel, ArmStartsModel armStartsModel, MultiplayerModel multiplayerModel, JacuzziHazeBlendAggregator hazeBlendAggregator)
		{
			_configuration = configuration;
			_armVisualsControllerModel = armVisualsControllerModel;
			_armStartsModel = armStartsModel;
			_multiplayerModel = multiplayerModel;
			_hazeBlendAggregator = hazeBlendAggregator;
		}

		private void Start()
		{
			_armVisualsControllerModel.OnPlayerArmVisualsControllerRegistered += OnArmVisualsControllerRegistered;
			ArmStartsModel armStartsModel = _armStartsModel;
			armStartsModel.OnArmEndAdded = (Action<PlayerRef, IArmEndEntity, Arm>)Delegate.Combine(armStartsModel.OnArmEndAdded, new Action<PlayerRef, IArmEndEntity, Arm>(OnArmEndAdded));
			TickJacuzziPresence(0f);
		}

		private void OnDestroy()
		{
			_armVisualsControllerModel.OnPlayerArmVisualsControllerRegistered -= OnArmVisualsControllerRegistered;
			ArmStartsModel armStartsModel = _armStartsModel;
			armStartsModel.OnArmEndAdded = (Action<PlayerRef, IArmEndEntity, Arm>)Delegate.Remove(armStartsModel.OnArmEndAdded, new Action<PlayerRef, IArmEndEntity, Arm>(OnArmEndAdded));
		}

		public void TickJacuzziPresence(float blend01)
		{
			float num = Mathf.Clamp01(blend01);
			if (!(Mathf.Abs(num - _appliedBlend) < 0.001f))
			{
				_appliedBlend = num;
				ApplyColor(num);
				ApplySteam(num);
				ApplyHaze(num);
			}
		}

		public void ResetJacuzziPresence()
		{
			TickJacuzziPresence(0f);
		}

		private void ApplySteam(float blend)
		{
			ParticleSystem.EmissionModule emission = _steamParticleSystem.emission;
			emission.rateOverTimeMultiplier = Mathf.Lerp(0f, _configuration.MaxSteamEmissionRate, blend);
			if (blend <= 0f)
			{
				_steamParticleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
			}
			else if (!_steamParticleSystem.isPlaying)
			{
				_steamParticleSystem.Play(withChildren: true);
			}
		}

		private void ApplyColor(float blend)
		{
			ApplyColorTo(_staticColorChangerByPlayerRef, blend);
			ApplyArmsColor(blend);
			ApplyArmEndColor(Arm.Left, blend);
			ApplyArmEndColor(Arm.Right, blend);
		}

		private void ApplyArmsColor(float blend)
		{
			foreach (KeyValuePair<Arm, ArmVisualsController> armVisualsController in _armVisualsControllerModel.GetArmVisualsControllers(_networkObject.InputAuthority.PlayerId))
			{
				ApplyColorTo(armVisualsController.Value.ColorChanger, blend);
			}
		}

		private void ApplyArmEndColor(Arm arm, float blend)
		{
			if (_armStartsModel.IsContainsEnd(_networkObject.InputAuthority, arm))
			{
				ApplyColorTo(_armStartsModel.GetEnd(_networkObject.InputAuthority, arm).ColorChanger, blend);
			}
		}

		private void ApplyColorTo(StaticColorChangerByPlayerRef colorChanger, float blend)
		{
			if (blend <= 0f)
			{
				colorChanger.ClearFlushBlend();
			}
			else
			{
				colorChanger.SetFlushBlend(_configuration.FlushColor, blend);
			}
		}

		private void OnArmVisualsControllerRegistered(int playerId, Arm arm)
		{
			if (playerId == _networkObject.InputAuthority.PlayerId && !(_appliedBlend <= 0f))
			{
				ApplyColorTo(_armVisualsControllerModel.GetArmVisualsController(playerId, arm).ColorChanger, _appliedBlend);
			}
		}

		private void OnArmEndAdded(PlayerRef playerRef, IArmEndEntity armEndEntity, Arm arm)
		{
			if (!(playerRef != _networkObject.InputAuthority) && !(_appliedBlend <= 0f))
			{
				ApplyColorTo(armEndEntity.ColorChanger, _appliedBlend);
			}
		}

		private void ApplyHaze(float blend)
		{
			if (!(_networkObject.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer))
			{
				_hazeBlendAggregator.SetJacuzziBlend(blend);
			}
		}
	}
}
