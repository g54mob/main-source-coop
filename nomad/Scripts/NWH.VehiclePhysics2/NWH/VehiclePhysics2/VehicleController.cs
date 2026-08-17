using System;
using System.Collections;
using System.Collections.Generic;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Effects;
using NWH.VehiclePhysics2.GroundDetection;
using NWH.VehiclePhysics2.Input;
using NWH.VehiclePhysics2.Modules;
using NWH.VehiclePhysics2.Powertrain;
using NWH.VehiclePhysics2.Sound;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2
{
	[RequireComponent(typeof(Rigidbody))]
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(90)]
	public class VehicleController : Vehicle
	{
		public struct MultiplayerState
		{
			public int lightState;

			public float engineAngularVelocity;

			public float steering;

			public float throttle;

			public float clutch;

			public float handbrake;

			public int shiftInto;

			public bool shiftUp;

			public bool shiftDown;

			public bool trailerAttachDetach;

			public bool horn;

			public bool engineStartStop;

			public bool cruiseControl;

			public bool boost;

			public bool flipOver;
		}

		[Tooltip("    Called when a collision happens.")]
		public UnityEvent<Collision> onCollision = new UnityEvent<Collision>();

		[NonSerialized]
		[Tooltip("    Distance between camera and vehicle used for determining LOD.")]
		public float vehicleToCamDistance;

		[NonSerialized]
		[Tooltip("    Currently active LOD.")]
		public LOD activeLOD;

		[NonSerialized]
		[Tooltip("    Currently active LOD index.")]
		public int activeLODIndex = -2;

		[FormerlySerializedAs("LODCamera")]
		[Tooltip("Camera from which the LOD distance will be measured.\r\nTo use Camera.main instead, leave empty/null.")]
		public Camera lodCamera;

		[NonSerialized]
		[Tooltip("Called when active LOD is changed.")]
		public UnityEvent onLODChanged = new UnityEvent();

		[NonSerialized]
		protected MultiplayerState _multiplayerState;

		public const string DEFAULT_RESOURCES_PATH = "NWH Vehicle Physics 2/Defaults/";

		public Brakes brakes = new Brakes();

		public EffectManager effectsManager = new EffectManager();

		public NWH.VehiclePhysics2.GroundDetection.GroundDetection groundDetection = new NWH.VehiclePhysics2.GroundDetection.GroundDetection();

		public VehicleInputHandler input = new VehicleInputHandler();

		public ModuleManager moduleManager = new ModuleManager();

		public NWH.VehiclePhysics2.Powertrain.Powertrain powertrain = new NWH.VehiclePhysics2.Powertrain.Powertrain();

		public SoundManager soundManager = new SoundManager();

		public Steering steering = new Steering();

		[Tooltip("State settings for the current vehicle.\r\nState settings determine which components are enabled or disabled, as well as which LOD they belong to.")]
		public StateSettings stateSettings;

		[Tooltip("Used as a threshold value for lateral slip. When absolute lateral slip of a wheel is\r\nlower than this value wheel is considered to have no lateral slip (wheel skid). Used mostly for effects and sound.")]
		public float lateralSlipThreshold = 0.15f;

		[Tooltip("Used as a threshold value for longitudinal slip. When absolute longitudinal slip of a wheel is\r\nlower than this value wheel is considered to have no longitudinal slip (wheel spin). Used mostly for effects and sound.")]
		public float longitudinalSlipThreshold = 0.3f;

		[Tooltip("    Position of the engine relative to the vehicle. Turn on gizmos to see the marker.")]
		public Vector3 enginePosition = new Vector3(0f, 0.4f, 1.5f);

		[Tooltip("    Position of the exhaust relative to the vehicle. Turn on gizmos to see the marker.")]
		public Vector3 exhaustPosition = new Vector3(0f, 0.1f, -2f);

		[Tooltip("    Position of the transmission relative to the vehicle. Turn on gizmos to see the marker.")]
		public Vector3 transmissionPosition = new Vector3(0f, 0.2f, 0.2f);

		[Tooltip("    Valid only for 4-wheeled vehicles with 2 axles (i.e. cars).\r\n    For other vehicles this value will be 0.")]
		public float wheelbase = -1f;

		[NonSerialized]
		[Tooltip("    Cached Time.fixedDeltaTime.")]
		public float fixedDeltaTime = 0.02f;

		[NonSerialized]
		[Tooltip("    Cached Time.deltaTime;")]
		public float deltaTime = 0.02f;

		public float realtimeSinceStartup;

		public bool runAutomaticValidation = true;

		[NonSerialized]
		[Tooltip("    Called after vehicle has finished initializing.")]
		public UnityEvent onVehicleInitialized = new UnityEvent();

		[NonSerialized]
		private List<VehicleComponent> _components;

		private int _componentCount;

		private Transform _cameraTransform;

		protected bool _isInitialized;

		public Vector3 WorldEnginePosition => base.transform.TransformPoint(enginePosition);

		public Vector3 WorldExhaustPosition => base.transform.TransformPoint(exhaustPosition);

		public Vector3 WorldTransmissionPosition => base.transform.TransformPoint(transmissionPosition);

		public bool IsInitialized => _isInitialized;

		public List<VehicleComponent> Components
		{
			get
			{
				List<VehicleComponent> list = _components;
				if (list == null)
				{
					List<VehicleComponent> obj = new List<VehicleComponent> { input, soundManager, moduleManager, steering, powertrain, effectsManager, brakes, groundDetection };
					List<VehicleComponent> list2 = obj;
					_components = obj;
					list = list2;
				}
				return list;
			}
		}

		private void OnCollisionEnter(Collision other)
		{
			onCollision.Invoke(other);
		}

		private IEnumerator LODCheckCoroutine()
		{
			while (LODCheck())
			{
				if (activeLODIndex == -2)
				{
					Debug.LogWarning("LOD is -2 meaning that the vehicle " + base.name + " initialization failed!");
					break;
				}
				UpdateComponentLODs();
				yield return new WaitForSeconds(0.2f);
			}
			yield return null;
		}

		protected virtual void UpdateComponentLODs()
		{
			for (int i = 0; i < Components.Count; i++)
			{
				Components[i].UpdateLOD();
			}
		}

		private bool LODCheck()
		{
			if (stateSettings == null)
			{
				Debug.LogError("StateSettings are null. Exiting LOD check!");
				return false;
			}
			int num = activeLODIndex;
			int count = stateSettings.LODs.Count;
			if (count == 0)
			{
				Debug.LogError("Lod count is 0!");
				return false;
			}
			Camera main = lodCamera;
			if (main == null)
			{
				main = Camera.main;
			}
			if (main == null)
			{
				Debug.LogWarning("LOD camera is null. Make sure that there is a camera with tag 'MainCamera' in the scene and/or that the vehicle cameras have this tag.");
				return false;
			}
			_cameraTransform = main.transform;
			vehicleToCamDistance = Vector3.Distance(vehicleTransform.position, _cameraTransform.position);
			for (int i = 0; i < count; i++)
			{
				if (stateSettings.LODs[i].distance > vehicleToCamDistance)
				{
					activeLODIndex = i;
					activeLOD = stateSettings.LODs[i];
					break;
				}
			}
			if (activeLODIndex != num)
			{
				onLODChanged.Invoke();
			}
			return true;
		}

		public MultiplayerState GetMultiplayerState()
		{
			_multiplayerState.lightState = effectsManager.lightsManager.GetIntState();
			_multiplayerState.engineAngularVelocity = powertrain.engine.outputAngularVelocity;
			_multiplayerState.steering = input.states.steering;
			_multiplayerState.throttle = input.states.throttle;
			_multiplayerState.clutch = input.states.clutch;
			_multiplayerState.handbrake = input.states.handbrake;
			_multiplayerState.shiftInto = input.states.shiftInto;
			_multiplayerState.shiftUp = input.states.shiftUp;
			_multiplayerState.shiftDown = input.states.shiftDown;
			_multiplayerState.trailerAttachDetach = input.states.trailerAttachDetach;
			_multiplayerState.horn = input.states.horn;
			_multiplayerState.engineStartStop = input.states.engineStartStop;
			_multiplayerState.cruiseControl = input.states.cruiseControl;
			_multiplayerState.boost = input.states.boost;
			_multiplayerState.flipOver = input.states.flipOver;
			return _multiplayerState;
		}

		public bool SetMultiplayerState(MultiplayerState inboundState)
		{
			effectsManager.lightsManager.SetStateFromInt(inboundState.lightState);
			powertrain.engine.outputAngularVelocity = inboundState.engineAngularVelocity;
			_multiplayerState = inboundState;
			input.autoSetInput = false;
			input.states.steering = inboundState.steering;
			input.states.throttle = inboundState.throttle;
			input.states.clutch = inboundState.clutch;
			input.states.handbrake = inboundState.handbrake;
			input.states.shiftInto = inboundState.shiftInto;
			input.states.shiftUp = inboundState.shiftUp;
			input.states.shiftDown = inboundState.shiftDown;
			input.states.trailerAttachDetach = inboundState.trailerAttachDetach;
			input.states.horn = inboundState.horn;
			input.states.engineStartStop = inboundState.engineStartStop;
			input.states.cruiseControl = inboundState.cruiseControl;
			input.states.boost = inboundState.boost;
			input.states.flipOver = inboundState.flipOver;
			return true;
		}

		private void Start()
		{
			_componentCount = Components.Count;
			for (int i = 0; i < _componentCount; i++)
			{
				_components[i].VC_SetVehicleController(this);
			}
			foreach (VehicleComponent component in _components)
			{
				component.VC_LoadStateFromStateSettings();
			}
			StartCoroutine(LODCheckCoroutine());
			_isInitialized = true;
			onVehicleInitialized.Invoke();
			for (int j = 0; j < _componentCount; j++)
			{
				_components[j].VC_Enable(calledByParent: true);
			}
			onEnable.Invoke();
		}

		public virtual void Update()
		{
			deltaTime = Time.deltaTime;
			realtimeSinceStartup = Time.realtimeSinceStartup;
			for (int i = 0; i < _componentCount; i++)
			{
				VehicleComponent vehicleComponent = Components[i];
				if (vehicleComponent.IsActive)
				{
					vehicleComponent.VC_Update();
				}
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			fixedDeltaTime = Time.fixedDeltaTime;
			for (int i = 0; i < _componentCount; i++)
			{
				VehicleComponent vehicleComponent = _components[i];
				if (vehicleComponent.IsActive)
				{
					vehicleComponent.VC_FixedUpdate();
				}
			}
		}

		public override void OnEnable()
		{
			base.OnEnable();
			if (_isInitialized)
			{
				StartCoroutine(LODCheckCoroutine());
				for (int i = 0; i < _componentCount; i++)
				{
					_components[i].VC_Enable(calledByParent: true);
				}
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			for (int i = 0; i < _componentCount; i++)
			{
				_components[i].VC_Disable(calledByParent: true);
			}
			StopAllCoroutines();
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
		}

		public virtual void Reset()
		{
			SetDefaults();
		}

		private void OnValidate()
		{
			if (runAutomaticValidation && !Application.isPlaying)
			{
				Validate();
			}
		}

		public virtual void SetDefaults()
		{
			foreach (VehicleComponent component in Components)
			{
				component.VC_SetVehicleController(this);
				component.VC_SetDefaults();
			}
		}

		public bool IsFullyGrounded()
		{
			int wheelCount = powertrain.wheelCount;
			for (int i = 0; i < wheelCount; i++)
			{
				if (!powertrain.wheels[i].wheelUAPI.IsGrounded)
				{
					return false;
				}
			}
			return true;
		}

		public bool IsGrounded()
		{
			int wheelCount = powertrain.wheelCount;
			for (int i = 0; i < wheelCount; i++)
			{
				if (powertrain.wheels[i].wheelUAPI.IsGrounded)
				{
					return true;
				}
			}
			return false;
		}

		public void Validate()
		{
			if (Application.isPlaying)
			{
				return;
			}
			if (base.transform.localScale != Vector3.one)
			{
				VC_LogWarning("VehicleController Transform scale is other than [1,1,1]. It is recommended to avoid  scaling the vehicle parent object and use Scale Factor from Unity model import settings instead.");
			}
			foreach (VehicleComponent component in Components)
			{
				if (!component.state.initialized)
				{
					component.VC_SetVehicleController(this);
					component.VC_LoadStateFromStateSettings();
				}
				component.VC_Validate(this);
			}
		}

		public void VC_LogWarning(string message)
		{
			Debug.LogWarning(base.name + " > " + message + "\r\n This message will show up for prefabs, too, so make sure to apply the changes to the prefab after fixing the issue, or disable the validation through Settings tab of VehicleController.");
		}

		private void OnDrawGizmosSelected()
		{
		}
	}
}
