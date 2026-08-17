using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.GraphicsQuality;
using NomadDrive.Features.Player;
using NomadDrive.Managers.GameTime;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.OutdoorLighting
{
	public class OutdoorLightingManager : MonoBehaviour, IOutdoorLightingManager
	{
		private struct EvalEntry
		{
			public OutdoorLightController Controller;

			public int Index;

			public bool ScheduledOn;

			public float DistanceSqr;

			public bool Fog;
		}

		[SerializeField]
		private OutdoorLightingLodConfig config;

		private ITimeManager _timeManager;

		private IPlayerService _playerService;

		private IGraphicsQualityManager _qualityManager;

		private readonly List<OutdoorLightController> _controllers = new List<OutdoorLightController>();

		private readonly List<EvalEntry> _eval = new List<EvalEntry>();

		private readonly List<int> _fogCandidates = new List<int>();

		private readonly List<OutdoorLightController> _flicker = new List<OutdoorLightController>();

		private OutdoorLightingLodConfig.LevelSettings _settings;

		private readonly float[] _enterSqr = new float[3];

		private readonly float[] _exitSqr = new float[3];

		private float _flickerSqr;

		private float _interval = 0.33f;

		private int _maxFog;

		private int _currentHour;

		private float _evalTimer;

		private bool _initialized;

		[Inject]
		private void Construct(ITimeManager timeManager, IPlayerService playerService, IGraphicsQualityManager qualityManager)
		{
			_timeManager = timeManager;
			_playerService = playerService;
			_qualityManager = qualityManager;
			if (config == null)
			{
				EvilLogger.LogError("[OutdoorLightingManager] LOD config is not assigned. Outdoor light LOD disabled.", "Construct", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\OutdoorLighting\\Scripts\\OutdoorLightingManager.cs", 60);
				return;
			}
			_currentHour = _timeManager.CurrentHour;
			_settings = config.GetSettings(_qualityManager.CurrentLevel);
			RecomputeThresholds();
			_timeManager.OnMinutePassed.AddListener(OnMinutePassed);
			_qualityManager.OnQualityChanged += OnQualityChanged;
			_initialized = true;
		}

		private void OnDestroy()
		{
			if (_timeManager != null)
			{
				_timeManager.OnMinutePassed.RemoveListener(OnMinutePassed);
			}
			if (_qualityManager != null)
			{
				_qualityManager.OnQualityChanged -= OnQualityChanged;
			}
		}

		public void Register(OutdoorLightController controller)
		{
			if (!(controller == null) && !_controllers.Contains(controller))
			{
				_controllers.Add(controller);
			}
		}

		public void Unregister(OutdoorLightController controller)
		{
			_controllers.Remove(controller);
			_flicker.Remove(controller);
		}

		private void OnMinutePassed()
		{
			_currentHour = _timeManager.CurrentHour;
		}

		private void OnQualityChanged(GraphicsQualityLevel level)
		{
			_settings = config.GetSettings(level);
			RecomputeThresholds();
		}

		private void RecomputeThresholds()
		{
			float hysteresis = Mathf.Max(0f, _settings.hysteresis);
			float nearDistance = _settings.nearDistance;
			float midDistance = _settings.midDistance;
			float emissiveDistance = _settings.emissiveDistance;
			SetBoundary(0, nearDistance, hysteresis);
			SetBoundary(1, midDistance, hysteresis);
			SetBoundary(2, emissiveDistance, hysteresis);
			_flickerSqr = _settings.flickerDistance * _settings.flickerDistance;
			_interval = Mathf.Max(0.05f, _settings.evaluateInterval);
			_maxFog = Mathf.Max(0, _settings.maxVolumetricLights);
		}

		private void SetBoundary(int boundary, float distance, float hysteresis)
		{
			float num = Mathf.Max(0f, distance - hysteresis);
			float num2 = distance + hysteresis;
			_enterSqr[boundary] = num * num;
			_exitSqr[boundary] = num2 * num2;
		}

		private void Update()
		{
			if (!_initialized)
			{
				return;
			}
			_evalTimer += Time.deltaTime;
			if (_evalTimer >= _interval)
			{
				_evalTimer = 0f;
				EvaluateAll();
			}
			if (_flicker.Count == 0)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < _flicker.Count; i++)
			{
				OutdoorLightController outdoorLightController = _flicker[i];
				if (outdoorLightController != null)
				{
					outdoorLightController.TickFlicker(deltaTime);
				}
			}
		}

		private void EvaluateAll()
		{
			if (!_playerService.IsPlayerSpawned)
			{
				return;
			}
			Transform cameraTransform = _playerService.CameraTransform;
			if (cameraTransform == null)
			{
				return;
			}
			Vector3 position = cameraTransform.position;
			int currentHour = _currentHour;
			_eval.Clear();
			_flicker.Clear();
			for (int num = _controllers.Count - 1; num >= 0; num--)
			{
				OutdoorLightController outdoorLightController = _controllers[num];
				if (outdoorLightController == null)
				{
					_controllers.RemoveAt(num);
				}
				else
				{
					float sqrMagnitude = (outdoorLightController.Position - position).sqrMagnitude;
					int index = (outdoorLightController.LodBandIndex = ComputeBandIndex(sqrMagnitude, outdoorLightController.LodBandIndex));
					_eval.Add(new EvalEntry
					{
						Controller = outdoorLightController,
						Index = index,
						ScheduledOn = outdoorLightController.IsScheduledOn(currentHour),
						DistanceSqr = sqrMagnitude,
						Fog = false
					});
				}
			}
			AssignFogBudget();
			for (int i = 0; i < _eval.Count; i++)
			{
				EvalEntry e = _eval[i];
				OutdoorLightLodLevel outdoorLightLodLevel = ResolveLevel(e);
				if (outdoorLightLodLevel != e.Controller.CurrentLevel)
				{
					e.Controller.ApplyState(outdoorLightLodLevel);
				}
				if (e.ScheduledOn && e.Controller.IsBroken && e.Index <= 1 && e.DistanceSqr <= _flickerSqr)
				{
					_flicker.Add(e.Controller);
				}
			}
		}

		private static OutdoorLightLodLevel ResolveLevel(EvalEntry e)
		{
			if (!e.ScheduledOn)
			{
				return OutdoorLightLodLevel.Off;
			}
			return e.Index switch
			{
				0 => e.Fog ? OutdoorLightLodLevel.LightWithFog : OutdoorLightLodLevel.LightOnly, 
				1 => OutdoorLightLodLevel.LightOnly, 
				2 => OutdoorLightLodLevel.EmissiveOnly, 
				_ => OutdoorLightLodLevel.Off, 
			};
		}

		private void AssignFogBudget()
		{
			if (_maxFog <= 0)
			{
				return;
			}
			_fogCandidates.Clear();
			for (int i = 0; i < _eval.Count; i++)
			{
				EvalEntry evalEntry = _eval[i];
				if (evalEntry.ScheduledOn && evalEntry.Index == 0)
				{
					_fogCandidates.Add(i);
				}
			}
			if (_fogCandidates.Count == 0)
			{
				return;
			}
			if (_fogCandidates.Count > _maxFog)
			{
				_fogCandidates.Sort((int a, int b) => _eval[a].DistanceSqr.CompareTo(_eval[b].DistanceSqr));
			}
			int num = Mathf.Min(_maxFog, _fogCandidates.Count);
			for (int num2 = 0; num2 < num; num2++)
			{
				int index = _fogCandidates[num2];
				EvalEntry value = _eval[index];
				value.Fog = true;
				_eval[index] = value;
			}
		}

		private int ComputeBandIndex(float distanceSqr, int previous)
		{
			int i;
			for (i = Mathf.Clamp(previous, 0, 3); i < 3 && distanceSqr > _exitSqr[i]; i++)
			{
			}
			while (i > 0 && distanceSqr < _enterSqr[i - 1])
			{
				i--;
			}
			return i;
		}
	}
}
