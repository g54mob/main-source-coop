using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class ButtSlapBlushController : MonoBehaviour
	{
		private const int MAX_BLUSH_SPOTS = 16;

		private const float DEAD_SPOT_START_TIME = -10000f;

		private static readonly int _blushSpotsId = Shader.PropertyToID("_BlushSpots");

		private static readonly int _blushIntensitiesId = Shader.PropertyToID("_BlushIntensities");

		private static readonly int _blushRadiusScalesId = Shader.PropertyToID("_BlushRadiusScales");

		[SerializeField]
		private Renderer _renderer;

		[SerializeField]
		private List<SimplePointGrabable> _simplePointGrabables = new List<SimplePointGrabable>();

		[SerializeField]
		private float _slapIntensity = 0.65f;

		[Header("Radius Growth By Stack")]
		[Tooltip("Must match Blush Fade Duration on the blush material — used to estimate how much live stack already sits at the new slap position.")]
		[SerializeField]
		private float _blushFadeDuration = 4f;

		[Tooltip("World distance within which existing spots count towards the stack at the new slap position.")]
		[SerializeField]
		private float _stackOverlapRadius = 0.35f;

		[Tooltip("How much each stacked unit grows the new spot radius: scale = 1 + stack * growth. 0 = disabled.")]
		[SerializeField]
		private float _radiusGrowthPerStack = 0.35f;

		[Tooltip("Upper bound for the per-spot radius scale.")]
		[Range(1f, 4f)]
		[SerializeField]
		private float _maxRadiusScale = 2.5f;

		private LineArmsModel _lineArmsModel;

		private MaterialPropertyBlock _materialPropertyBlock;

		private readonly Vector4[] _blushSpots = new Vector4[16];

		private readonly float[] _blushIntensities = new float[16];

		private readonly float[] _blushRadiusScales = new float[16];

		private readonly Dictionary<SimplePointGrabable, Action<int>> _grabHandlers = new Dictionary<SimplePointGrabable, Action<int>>();

		private int _nextSpotIndex;

		[Inject]
		private void InjectDependencies(LineArmsModel lineArmsModel)
		{
			_lineArmsModel = lineArmsModel;
		}

		private void Awake()
		{
			_materialPropertyBlock = new MaterialPropertyBlock();
			for (int i = 0; i < 16; i++)
			{
				_blushSpots[i] = new Vector4(0f, 0f, 0f, -10000f);
				_blushRadiusScales[i] = 1f;
			}
			Apply();
		}

		private void OnEnable()
		{
			foreach (SimplePointGrabable simplePointGrabable in _simplePointGrabables)
			{
				if (!(simplePointGrabable == null) && !_grabHandlers.ContainsKey(simplePointGrabable))
				{
					SimplePointGrabable captured = simplePointGrabable;
					Action<int> value = delegate(int index)
					{
						HandleGrab(captured, index);
					};
					_grabHandlers[captured] = value;
					captured.LocalOnGrab += value;
				}
			}
		}

		private void OnDisable()
		{
			foreach (KeyValuePair<SimplePointGrabable, Action<int>> grabHandler in _grabHandlers)
			{
				if (grabHandler.Key != null)
				{
					grabHandler.Key.LocalOnGrab -= grabHandler.Value;
				}
			}
			_grabHandlers.Clear();
		}

		public void AddBlush(Vector3 worldPosition, float intensity)
		{
			_blushSpots[_nextSpotIndex] = new Vector4(worldPosition.x, worldPosition.y, worldPosition.z, Time.time);
			_blushIntensities[_nextSpotIndex] = intensity;
			_blushRadiusScales[_nextSpotIndex] = GetRadiusScaleForStack(worldPosition, _nextSpotIndex);
			_nextSpotIndex = (_nextSpotIndex + 1) % 16;
			Apply();
		}

		private float GetRadiusScaleForStack(Vector3 worldPosition, int newSpotIndex)
		{
			if (_radiusGrowthPerStack <= 0f)
			{
				return 1f;
			}
			float time = Time.time;
			float num = 0f;
			for (int i = 0; i < 16; i++)
			{
				if (i != newSpotIndex)
				{
					float num2 = time - _blushSpots[i].w;
					if (!(num2 < 0f) && !(num2 >= _blushFadeDuration) && !(Vector3.Distance(new Vector3(_blushSpots[i].x, _blushSpots[i].y, _blushSpots[i].z), worldPosition) > _stackOverlapRadius))
					{
						float num3 = 1f - num2 / Mathf.Max(_blushFadeDuration, 0.0001f);
						num += _blushIntensities[i] * num3 * num3;
					}
				}
			}
			return Mathf.Min(1f + num * _radiusGrowthPerStack, _maxRadiusScale);
		}

		private void HandleGrab(SimplePointGrabable simplePointGrabable, int playerId)
		{
			AddBlush(ResolveSlapWorldPosition(simplePointGrabable, playerId), _slapIntensity);
		}

		private Vector3 ResolveSlapWorldPosition(SimplePointGrabable simplePointGrabable, int playerId)
		{
			Transform nearestGrabHandle = GetNearestGrabHandle(simplePointGrabable, playerId);
			if (!(nearestGrabHandle != null))
			{
				return simplePointGrabable.transform.position;
			}
			return nearestGrabHandle.position;
		}

		private Transform GetNearestGrabHandle(SimplePointGrabable simplePointGrabable, int playerId)
		{
			if (simplePointGrabable == null || _lineArmsModel == null)
			{
				return null;
			}
			LineArmControllerBase grabbingLineArm = GetGrabbingLineArm(simplePointGrabable, playerId);
			if (!(grabbingLineArm != null))
			{
				return null;
			}
			return simplePointGrabable.GetNearestHandle(grabbingLineArm.transform.position);
		}

		private LineArmControllerBase GetGrabbingLineArm(SimplePointGrabable simplePointGrabable, int playerId)
		{
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(playerId);
			if (allLineArmsForPlayer == null)
			{
				return null;
			}
			LineArmControllerBase lineArmControllerBase = null;
			foreach (LineArmControllerBase value in allLineArmsForPlayer.Values)
			{
				if (!(value == null) && value.enabled)
				{
					if ((object)lineArmControllerBase == null)
					{
						lineArmControllerBase = value;
					}
					if (IsGrabbing(value, simplePointGrabable))
					{
						return value;
					}
				}
			}
			return lineArmControllerBase;
		}

		private static bool IsGrabbing(LineArmControllerBase lineArm, SimplePointGrabable simplePointGrabable)
		{
			foreach (IPointGrabable currentGrabbable in lineArm.CurrentGrabbables)
			{
				if (currentGrabbable == simplePointGrabable)
				{
					return true;
				}
			}
			return false;
		}

		private void Apply()
		{
			_renderer.GetPropertyBlock(_materialPropertyBlock);
			_materialPropertyBlock.SetVectorArray(_blushSpotsId, _blushSpots);
			_materialPropertyBlock.SetFloatArray(_blushIntensitiesId, _blushIntensities);
			_materialPropertyBlock.SetFloatArray(_blushRadiusScalesId, _blushRadiusScales);
			_renderer.SetPropertyBlock(_materialPropertyBlock);
		}
	}
}
