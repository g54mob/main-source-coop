using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Input
{
	[Serializable]
	public struct VehicleInputStates
	{
		[NonSerialized]
		[Range(-1f, 1f)]
		public float steering;

		[HideInInspector]
		public float steeringRaw;

		[NonSerialized]
		[Range(0f, 1f)]
		public float throttle;

		[HideInInspector]
		public float throttleRaw;

		[HideInInspector]
		public float inputSwappedThrottle;

		[HideInInspector]
		public float inputSwappedThrottleRaw;

		[NonSerialized]
		[Range(0f, 1f)]
		public float brakes;

		[HideInInspector]
		public float brakesRaw;

		[HideInInspector]
		public float inputSwappedBrakes;

		[HideInInspector]
		public float inputSwappedBrakesRaw;

		[NonSerialized]
		[Range(0f, 1f)]
		public float clutch;

		[HideInInspector]
		public float clutchRaw;

		[NonSerialized]
		[Range(0f, 1f)]
		public float handbrake;

		[HideInInspector]
		public float handbrakeRaw;

		[NonSerialized]
		public bool engineStartStop;

		[NonSerialized]
		public bool extraLights;

		[NonSerialized]
		public bool highBeamLights;

		[NonSerialized]
		public bool hazardLights;

		[NonSerialized]
		public bool horn;

		[NonSerialized]
		public bool leftBlinker;

		[NonSerialized]
		public bool lowBeamLights;

		[NonSerialized]
		public bool rightBlinker;

		[NonSerialized]
		public bool shiftDown;

		[NonSerialized]
		public int shiftInto;

		[NonSerialized]
		public bool shiftUp;

		[NonSerialized]
		public bool trailerAttachDetach;

		[NonSerialized]
		public bool cruiseControl;

		[NonSerialized]
		public bool boost;

		[NonSerialized]
		public bool flipOver;

		public void Reset()
		{
			steering = 0f;
			steeringRaw = 0f;
			throttle = 0f;
			throttleRaw = 0f;
			inputSwappedThrottle = 0f;
			inputSwappedThrottleRaw = 0f;
			clutch = 0f;
			clutchRaw = 0f;
			brakes = 0f;
			brakesRaw = 0f;
			inputSwappedBrakes = 0f;
			inputSwappedBrakesRaw = 0f;
			handbrake = 0f;
			handbrakeRaw = 0f;
			shiftInto = -999;
			shiftUp = false;
			shiftDown = false;
			leftBlinker = false;
			rightBlinker = false;
			lowBeamLights = false;
			highBeamLights = false;
			hazardLights = false;
			extraLights = false;
			trailerAttachDetach = false;
			horn = false;
			engineStartStop = false;
			cruiseControl = false;
			boost = false;
			flipOver = false;
		}
	}
}
