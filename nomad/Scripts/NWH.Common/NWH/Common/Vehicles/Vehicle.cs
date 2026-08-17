using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.Common.Vehicles
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Rigidbody))]
	public abstract class Vehicle : MonoBehaviour
	{
		public const float INPUT_DEADZONE = 0.02f;

		public const float SPEED_DEADZONE = 0.2f;

		public const float SMALL_NUMBER = 1E-05f;

		public const float KINDA_SMALL_NUMBER = 0.01f;

		public static List<Vehicle> ActiveVehicles = new List<Vehicle>();

		public static UnityEvent<Vehicle, Vehicle> onActiveVehicleChanged = new UnityEvent<Vehicle, Vehicle>();

		[NonSerialized]
		[Tooltip("    Cached value of vehicle rigidbody.")]
		public Rigidbody vehicleRigidbody;

		[NonSerialized]
		[Tooltip("    Cached value of vehicle transform.")]
		public Transform vehicleTransform;

		public bool isPlayerControllable = true;

		private bool _cameraInsideVehicle;

		public UnityEvent onCameraEnterVehicle = new UnityEvent();

		public UnityEvent onCameraExitVehicle = new UnityEvent();

		[NonSerialized]
		[Tooltip("    Called when vehicle is put to sleep.")]
		public UnityEvent onDisable = new UnityEvent();

		[NonSerialized]
		[Tooltip("    Called when vehicle is woken up.")]
		public UnityEvent onEnable = new UnityEvent();

		[Tooltip("    Determines if vehicle is running locally is synchronized over active multiplayer framework.")]
		private bool _multiplayerIsRemote;

		public UnityEvent<bool> onMultiplayerStatusChanged = new UnityEvent<bool>();

		private Vector3 _prevLocalVelocity;

		public static Vehicle ActiveVehicle
		{
			get
			{
				int count = ActiveVehicles.Count;
				if (count == 0)
				{
					return null;
				}
				return ActiveVehicles[count - 1];
			}
		}

		public bool CameraInsideVehicle
		{
			get
			{
				return _cameraInsideVehicle;
			}
			set
			{
				if (_cameraInsideVehicle && !value)
				{
					onCameraExitVehicle.Invoke();
				}
				else if (!CameraInsideVehicle && value)
				{
					onCameraEnterVehicle.Invoke();
				}
				_cameraInsideVehicle = value;
			}
		}

		public bool MultiplayerIsRemote
		{
			get
			{
				return _multiplayerIsRemote;
			}
			set
			{
				if (_multiplayerIsRemote && !value)
				{
					onMultiplayerStatusChanged.Invoke(arg0: false);
				}
				else if (!_multiplayerIsRemote && value)
				{
					onMultiplayerStatusChanged.Invoke(arg0: true);
				}
				_multiplayerIsRemote = value;
			}
		}

		public Vector3 LocalAcceleration { get; private set; }

		public float LocalForwardAcceleration { get; private set; }

		public float LocalForwardVelocity { get; private set; }

		public Vector3 LocalVelocity { get; private set; }

		public float Speed
		{
			get
			{
				if (!(LocalForwardVelocity < 0f))
				{
					return LocalForwardVelocity;
				}
				return 0f - LocalForwardVelocity;
			}
		}

		public float SpeedSigned => LocalForwardVelocity;

		public Vector3 Velocity { get; protected set; }

		public float VelocityMagnitude { get; protected set; }

		public Vector3 AngularVelocity { get; protected set; }

		public float AngularVelocityMagnitude { get; protected set; }

		public virtual void Awake()
		{
			vehicleTransform = base.transform;
			vehicleRigidbody = GetComponent<Rigidbody>();
			vehicleRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		}

		public virtual void FixedUpdate()
		{
			_prevLocalVelocity = LocalVelocity;
			Velocity = vehicleRigidbody.linearVelocity;
			LocalVelocity = base.transform.InverseTransformDirection(Velocity);
			LocalAcceleration = (LocalVelocity - _prevLocalVelocity) / Time.fixedDeltaTime;
			LocalForwardVelocity = LocalVelocity.z;
			LocalForwardAcceleration = LocalAcceleration.z;
			VelocityMagnitude = Velocity.magnitude;
			AngularVelocity = vehicleRigidbody.angularVelocity;
			AngularVelocityMagnitude = AngularVelocity.magnitude;
		}

		public virtual void OnEnable()
		{
			onEnable.Invoke();
			if (isPlayerControllable && !ActiveVehicles.Contains(this))
			{
				Vehicle activeVehicle = ActiveVehicle;
				ActiveVehicles.Add(this);
				Vehicle activeVehicle2 = ActiveVehicle;
				onActiveVehicleChanged.Invoke(activeVehicle, activeVehicle2);
			}
		}

		public virtual void OnDisable()
		{
			onDisable.Invoke();
			if (isPlayerControllable)
			{
				Vehicle activeVehicle = ActiveVehicle;
				ActiveVehicles.RemoveAll((Vehicle vehicle) => vehicle == this);
				Vehicle activeVehicle2 = ActiveVehicle;
				onActiveVehicleChanged.Invoke(activeVehicle, activeVehicle2);
			}
		}
	}
}
