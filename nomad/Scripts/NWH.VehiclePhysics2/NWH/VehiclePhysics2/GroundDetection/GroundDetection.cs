using System;
using System.Collections;
using System.Collections.Generic;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;

namespace NWH.VehiclePhysics2.GroundDetection
{
	[Serializable]
	public class GroundDetection : VehicleComponent
	{
		public GroundDetectionPreset groundDetectionPreset;

		public float groundDetectionInterval = 0.1f;

		private Terrain _activeTerrain;

		private Transform _hitTransform;

		private float[] _mix;

		private float[,,] _splatmapData;

		private TerrainData _terrainData;

		private Vector3 _terrainPos;

		private List<int> _dominanceWeighs;

		private SurfacePreset _dominantSurfacePreset;

		private Coroutine _groundDetectionCoroutine;

		public SurfacePreset DominantSurfacePreset => _dominantSurfacePreset;

		protected override void VC_Initialize()
		{
			if (groundDetectionPreset == null)
			{
				Debug.LogWarning("Ground detection preset is null. Will not use GroundDetection.");
				return;
			}
			groundDetectionInterval = UnityEngine.Random.Range(groundDetectionInterval * 0.8f, groundDetectionInterval * 1.2f);
			_dominanceWeighs = new List<int>();
			base.VC_Initialize();
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				_groundDetectionCoroutine = vehicleController.StartCoroutine(GroundDetectionCoroutine());
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				if (_groundDetectionCoroutine != null)
				{
					vehicleController.StopCoroutine(_groundDetectionCoroutine);
				}
				return true;
			}
			return false;
		}

		private IEnumerator GroundDetectionCoroutine()
		{
			while (true)
			{
				if (groundDetectionPreset == null)
				{
					yield return null;
				}
				int count = groundDetectionPreset.surfaceMaps.Count;
				if (_dominanceWeighs.Count != count)
				{
					_dominanceWeighs.Clear();
					for (int i = 0; i < count; i++)
					{
						_dominanceWeighs.Add(0);
					}
				}
				else
				{
					for (int j = 0; j < count; j++)
					{
						_dominanceWeighs[j] = 0;
					}
				}
				for (int k = 0; k < vehicleController.powertrain.wheelCount; k++)
				{
					WheelComponent wheelComponent = vehicleController.powertrain.wheels[k];
					vehicleController.groundDetection.GetCurrentSurfaceMap(wheelComponent.wheelUAPI, ref wheelComponent.surfaceMapIndex, ref wheelComponent.surfacePreset);
					if (wheelComponent.surfaceMapIndex >= 0)
					{
						_dominanceWeighs[wheelComponent.surfaceMapIndex]++;
						if (wheelComponent.surfacePreset.frictionPreset != null)
						{
							wheelComponent.wheelUAPI.FrictionPreset = wheelComponent.surfacePreset.frictionPreset;
							wheelComponent.ApplyRollingResistanceMultiplier(wheelComponent.surfacePreset.rollingResistanceMaxMultiplier);
						}
					}
					else
					{
						wheelComponent.wheelUAPI.FrictionPreset = groundDetectionPreset.fallbackSurfacePreset.frictionPreset;
						wheelComponent.ApplyRollingResistanceMultiplier(groundDetectionPreset.fallbackSurfacePreset.rollingResistanceMaxMultiplier);
					}
				}
				int num = -2147483648;
				for (int l = 0; l < _dominanceWeighs.Count; l++)
				{
					int val = _dominanceWeighs[l];
					num = Math.Max(num, val);
				}
				if (num == 0)
				{
					_dominantSurfacePreset = null;
				}
				else
				{
					int num2 = _dominanceWeighs.IndexOf(num);
					if (num2 >= 0)
					{
						_dominantSurfacePreset = groundDetectionPreset.surfaceMaps[num2].surfacePreset;
					}
					else
					{
						_dominantSurfacePreset = null;
					}
				}
				yield return new WaitForSeconds(groundDetectionInterval);
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			if (groundDetectionPreset == null)
			{
				groundDetectionPreset = Resources.Load("NWH Vehicle Physics 2/Defaults/DefaultGroundDetectionPreset") as GroundDetectionPreset;
			}
		}

		public override void VC_Validate(VehicleController vc)
		{
			base.VC_Validate(vc);
			_ = groundDetectionPreset != null;
		}

		public void GetCurrentSurfaceMap(WheelUAPI wheelController, ref int surfaceIndex, ref SurfacePreset outSurfacePreset)
		{
			outSurfacePreset = groundDetectionPreset?.fallbackSurfacePreset;
			surfaceIndex = -1;
			outSurfacePreset = null;
			if (!state.isEnabled)
			{
				return;
			}
			if (groundDetectionPreset == null)
			{
				Debug.LogError("GroundDetectionPreset is required but is null. Go to VehicleController > FX > Grnd. Det. and assign a GroundDetectionPreset.");
			}
			else
			{
				if (wheelController.HitCollider == null)
				{
					return;
				}
				_hitTransform = wheelController.HitCollider.transform;
				if (wheelController.IsGrounded && _hitTransform != null)
				{
					int count = groundDetectionPreset.surfaceMaps.Count;
					for (int i = 0; i < count; i++)
					{
						SurfaceMap surfaceMap = groundDetectionPreset.surfaceMaps[i];
						int count2 = surfaceMap.tags.Count;
						for (int j = 0; j < count2; j++)
						{
							if (_hitTransform.tag == surfaceMap.tags[j])
							{
								outSurfacePreset = surfaceMap.surfacePreset;
								surfaceIndex = i;
								return;
							}
						}
					}
					_activeTerrain = _hitTransform.GetComponent<Terrain>();
					if ((bool)_activeTerrain)
					{
						int dominantTerrainTexture = GetDominantTerrainTexture(wheelController.HitPoint, _activeTerrain);
						if (dominantTerrainTexture != -1)
						{
							int count3 = groundDetectionPreset.surfaceMaps.Count;
							for (int k = 0; k < count3; k++)
							{
								SurfaceMap surfaceMap2 = groundDetectionPreset.surfaceMaps[k];
								int count4 = surfaceMap2.terrainTextureIndices.Count;
								for (int l = 0; l < count4; l++)
								{
									if (surfaceMap2.terrainTextureIndices[l] == dominantTerrainTexture)
									{
										outSurfacePreset = surfaceMap2.surfacePreset;
										surfaceIndex = k;
										return;
									}
								}
							}
						}
					}
				}
				if (groundDetectionPreset.fallbackSurfacePreset != null)
				{
					outSurfacePreset = groundDetectionPreset.fallbackSurfacePreset;
					surfaceIndex = -1;
				}
				else
				{
					Debug.LogError("Fallback surface map of ground detection preset " + groundDetectionPreset.name + " not assigned.");
					outSurfacePreset = null;
					surfaceIndex = -1;
				}
			}
		}

		public int GetDominantTerrainTexture(Vector3 worldPos, Terrain terrain)
		{
			GetTerrainTextureComposition(worldPos, terrain, ref _mix);
			if (_mix != null)
			{
				float num = 0f;
				int result = 0;
				for (int i = 0; i < _mix.Length; i++)
				{
					if (_mix[i] > num)
					{
						result = i;
						num = _mix[i];
					}
				}
				return result;
			}
			return -1;
		}

		public void GetTerrainTextureComposition(Vector3 worldPos, Terrain terrain, ref float[] cellMix)
		{
			_terrainData = terrain.terrainData;
			_terrainPos = terrain.transform.position;
			int alphamapWidth = _terrainData.alphamapWidth;
			int alphamapHeight = _terrainData.alphamapHeight;
			int value = (int)((worldPos.x - _terrainPos.x) / _terrainData.size.x * (float)alphamapWidth);
			int value2 = (int)((worldPos.z - _terrainPos.z) / _terrainData.size.z * (float)alphamapHeight);
			value = Mathf.Clamp(value, 0, alphamapWidth - 1);
			value2 = Mathf.Clamp(value2, 0, alphamapHeight - 1);
			_splatmapData = _terrainData.GetAlphamaps(value, value2, 1, 1);
			cellMix = new float[_splatmapData.GetUpperBound(2) + 1];
			for (int i = 0; i < cellMix.Length; i++)
			{
				cellMix[i] = _splatmapData[0, 0, i];
			}
		}

		public override void VC_DrawGizmos()
		{
			base.VC_DrawGizmos();
		}
	}
}
