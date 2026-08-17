using System;
using System.Collections;
using System.Collections.Generic;
using NWH.VehiclePhysics2.GroundDetection;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class SkidmarkManager : Effect
	{
		[Range(0f, 5f)]
		[Tooltip("Higher value will give darker skidmarks for the same slip. Check corresponding SurfacePreset (GroundDetection -> Presets)\r\nfor per-surface settings.")]
		public float globalSkidmarkIntensity = 0.6f;

		[Tooltip("Height above ground at which skidmarks will be drawn. If too low clipping between skidmark and ground surface will\r\noccur.")]
		public float groundOffset = 0.025f;

		[Tooltip("    When skidmark alpha value is below this value skidmark mesh will not be generated.")]
		public float lowerIntensityThreshold = 0.05f;

		[Tooltip("Number of triangles that will be drawn per one section, before mesh is saved and new one is generated.")]
		public int maxTrisPerSection = 300;

		[Tooltip("Total number of skidmark mesh triangles per wheel before the oldest skidmark section gets destroyed.")]
		public int maxTotalTris = 1440;

		[Range(0f, 1f)]
		[Tooltip("    Max skidmark texture alpha.")]
		public float maxSkidmarkAlpha = 0.6f;

		[Tooltip("    Distance from the last skidmark section needed to generate a new one.")]
		public float minDistance = 0.12f;

		[Tooltip("    Skidmarks get deleted when distance from the parent vehicle is higher than this.")]
		public float skidmarkDestroyDistance = 100f;

		[Tooltip("    Time after which the skidmark will get destroyed. Set to 0 to disable.")]
		public float skidmarkDestroyTime;

		[Tooltip("Game object that contains all the skidmark objects.")]
		public GameObject skidmarkContainer;

		[Tooltip("Material that will be used if no material is assigned to current surface or current surface is null.")]
		public Material fallbackMaterial;

		private int _prevWheelCount;

		private List<SkidmarkGenerator> _skidmarkGenerators = new List<SkidmarkGenerator>();

		private Coroutine _updateCoroutine;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				InitializeSkidmarks();
				_updateCoroutine = vehicleController.StartCoroutine(SkidmarkUpdateCoroutine());
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				if (_updateCoroutine != null)
				{
					vehicleController.StopCoroutine(_updateCoroutine);
				}
				return true;
			}
			return false;
		}

		private void InitializeSkidmarks()
		{
			if (vehicleController.groundDetection.groundDetectionPreset == null)
			{
				Debug.LogWarning("Trying to use SkidmarkManager without a GroundDetectionPreset assigned to the vehicle " + vehicleController.name);
				return;
			}
			skidmarkContainer = GameObject.Find("SkidContainer");
			if (skidmarkContainer == null)
			{
				skidmarkContainer = new GameObject("SkidContainer");
				skidmarkContainer.isStatic = true;
			}
			fallbackMaterial = vehicleController.groundDetection.groundDetectionPreset.fallbackSurfacePreset.skidmarkMaterial;
			List<Material> list = new List<Material>();
			int count = vehicleController.groundDetection.groundDetectionPreset.surfaceMaps.Count;
			for (int i = 0; i < count; i++)
			{
				SurfaceMap surfaceMap = vehicleController.groundDetection.groundDetectionPreset.surfaceMaps[i];
				list.Add(surfaceMap.surfacePreset.skidmarkMaterial);
			}
			_skidmarkGenerators = new List<SkidmarkGenerator>();
			for (int j = 0; j < vehicleController.powertrain.wheelCount; j++)
			{
				WheelComponent wheelComponent = vehicleController.powertrain.wheels[j];
				SkidmarkGenerator skidmarkGenerator = new SkidmarkGenerator();
				skidmarkGenerator.Initialize(this, wheelComponent);
				_skidmarkGenerators.Add(skidmarkGenerator);
			}
			float num = (float)maxTrisPerSection * minDistance * 0.75f;
			if (skidmarkDestroyDistance < num)
			{
				skidmarkDestroyDistance = num;
			}
			if (maxTrisPerSection * 2 > maxTotalTris)
			{
				maxTotalTris = maxTrisPerSection * 2 + 1;
				Debug.LogWarning("MaxTotalTris must be at least double the value of MaxTrisPerSection. Adjusting.");
			}
			_prevWheelCount = vehicleController.powertrain.wheelCount;
		}

		public IEnumerator SkidmarkUpdateCoroutine()
		{
			float dt = 0.05f;
			while (true)
			{
				yield return new WaitForSeconds(dt);
				if (!base.IsActive || !vehicleController.groundDetection.state.isEnabled)
				{
					continue;
				}
				if (_prevWheelCount != vehicleController.powertrain.wheelCount || _skidmarkGenerators.Count != vehicleController.powertrain.wheelCount)
				{
					InitializeSkidmarks();
				}
				_prevWheelCount = vehicleController.powertrain.wheelCount;
				int count = _skidmarkGenerators.Count;
				for (int i = 0; i < count; i++)
				{
					WheelComponent wheelComponent = vehicleController.powertrain.wheels[i];
					SurfacePreset surfacePreset = wheelComponent.surfacePreset;
					float targetIntensity = 0f;
					if (surfacePreset != null && surfacePreset.drawSkidmarks)
					{
						float num = Mathf.Max(0f, wheelComponent.wheelUAPI.NormalizedLateralSlip - vehicleController.lateralSlipThreshold);
						float num2 = Mathf.Max(0f, wheelComponent.wheelUAPI.NormalizedLongitudinalSlip - vehicleController.longitudinalSlipThreshold);
						float num3 = num + num2;
						float num4 = Mathf.Clamp(wheelComponent.wheelUAPI.Load * 3f / wheelComponent.wheelUAPI.MaxLoad, 0f, 1f);
						num3 *= wheelComponent.surfacePreset.slipFactor * num4;
						targetIntensity = Mathf.Clamp(wheelComponent.surfacePreset.skidmarkBaseIntensity + num3, 0f, 1f);
						targetIntensity *= globalSkidmarkIntensity;
						targetIntensity = Mathf.Clamp(targetIntensity, 0f, maxSkidmarkAlpha);
					}
					_skidmarkGenerators[i].Update(wheelComponent.surfaceMapIndex, targetIntensity, dt);
				}
			}
		}
	}
}
