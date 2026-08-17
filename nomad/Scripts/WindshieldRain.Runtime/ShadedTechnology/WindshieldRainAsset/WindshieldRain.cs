using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShadedTechnology.WindshieldRainAsset
{
	[AddComponentMenu("Windshield Rain Asset/WindshieldRain")]
	public class WindshieldRain : MonoBehaviour
	{
		[HideInInspector]
		public const int MAX_DROPLETS_IN_CELL = 5;

		[HideInInspector]
		public const int NUM_THREADS_PER_GROUP = 16;

		[SerializeField]
		public WindshieldPlane m_WindshieldPlane;

		public Vector2Int m_Resolution = new Vector2Int(2048, 1024);

		public Vector2Int m_GridDimensions = new Vector2Int(256, 128);

		public int m_MaxDropletsInCell = 5;

		public ComputeShader m_ComputeShader;

		public bool m_EnableTurbulance = true;

		public Texture m_TurbulanceTexture;

		public Vector2 m_TurbulanceScale = new Vector2(1f, 1f);

		public float m_TurbulanceImpact = 0.7f;

		public float m_TurbulanceSpeedImpactMultiplier = 1f;

		[Range(0f, 1f)]
		public float m_MaxTurbulanceSpeedImpact = 0.1f;

		public Vector2 m_Movement;

		public float m_MaxDropletRadius = 0.2f;

		public float m_MinDropletRadius = 0.5f;

		public float m_MaxDropVelocity = 1f;

		[Range(0f, 10000f)]
		public float m_SpawnRate = 500f;

		[Range(0f, 100f)]
		public float m_SpawnAmount = 0.3f;

		[Range(0f, 1f)]
		public float m_Drag = 0.2f;

		public float m_SpeedMultiplier = 1f;

		public float m_StartDropsVelocity = 1f;

		[Range(0f, 10f)]
		public float m_RainStreakDecayRate = 1f;

		public bool m_EnableDropsInfluence = true;

		[Range(0f, 1f)]
		public float m_DropsInfluenceDistance = 1f;

		[Range(0f, 1f)]
		public float m_DropsInfluenceProportion = 0.5f;

		public float m_DropsInfluenceAddition = 0.2f;

		public bool m_WipersEnabled = true;

		public Wipers m_WipersScript;

		[Range(0f, 1f)]
		public float m_WipersThreshold = 0.9f;

		public float m_WipersSpeedMultiplier = 2f;

		public bool m_SetRainMaterialTexture = true;

		public Material m_RainMaterial;

		public RainPostProcessProfile m_RainPostProcessProfile;

		private int _kernelInitIndex;

		private int _kernelUpdateIndex;

		private int _kernelToTexture;

		private int _kernelInitTextures;

		private ComputeBuffer _rainGridBuffer;

		private ComputeBuffer _rainGridBuffer2;

		private ComputeBuffer _rainCountBuffer;

		private ComputeBuffer _rainCountBuffer2;

		private ComputeBuffer _wipersBuffer;

		private RenderTexture _renderTexture1;

		private RenderTexture _renderTexture2;

		[SerializeField]
		private float _fixedDeltaTime = 0.01f;

		private float _lastDeltaTime;

		private float _lastUpdateTime;

		private LocalKeyword _enableTurbulanceKeyword;

		private LocalKeyword _wipersEnabledKeyword;

		private bool _isInitialized;

		private bool bufferSwap;

		private bool textureSwap;

		public Vector2 Movement
		{
			get
			{
				return m_Movement;
			}
			set
			{
				m_Movement = value;
				if (m_ComputeShader != null)
				{
					m_ComputeShader.SetVector("_MovementVector", m_Movement);
				}
			}
		}

		private void OnDestroy()
		{
			ReleaseBuffers();
		}

		private void InitRenderTexture(ref RenderTexture renderTexture)
		{
			renderTexture = new RenderTexture(m_Resolution.x, m_Resolution.y, 0, RenderTextureFormat.ARGBFloat);
			renderTexture.enableRandomWrite = true;
			renderTexture.Create();
			renderTexture.filterMode = FilterMode.Point;
		}

		private void InitializeBuffer(ref ComputeBuffer buffer)
		{
			buffer = new ComputeBuffer(m_GridDimensions.x * m_GridDimensions.y * m_MaxDropletsInCell, Marshal.SizeOf(typeof(Droplet)));
		}

		private void InitializeCountBuffer(ref ComputeBuffer buffer)
		{
			buffer = new ComputeBuffer(m_GridDimensions.x * m_GridDimensions.y, Marshal.SizeOf(typeof(int)));
		}

		private void InitializeWipersBuffer(ref ComputeBuffer buffer)
		{
			if (!m_WipersEnabled || m_WipersScript == null || m_WipersScript.wipers.Length == 0)
			{
				m_ComputeShader.SetInt("_WipersCount", 0);
				return;
			}
			buffer = new ComputeBuffer(m_WipersScript.wipers.Length, Marshal.SizeOf(typeof(Wiper)));
			m_ComputeShader.SetInt("_WipersCount", m_WipersScript.wipers.Length);
		}

		private void InitKernelIndicies()
		{
			_kernelInitIndex = m_ComputeShader.FindKernel("CSInit");
			_kernelUpdateIndex = m_ComputeShader.FindKernel("CSUpdatePhysics");
			_kernelToTexture = m_ComputeShader.FindKernel("CSToTexture");
			_kernelInitTextures = m_ComputeShader.FindKernel("CSInitTextures");
		}

		private void DispatchInitTextures()
		{
			int threadGroupsX = Mathf.CeilToInt((float)m_Resolution.x / 16f);
			int threadGroupsY = Mathf.CeilToInt((float)m_Resolution.y / 16f);
			m_ComputeShader.SetTexture(_kernelInitTextures, "Result", _renderTexture1);
			m_ComputeShader.SetTexture(_kernelInitTextures, "PrevState", _renderTexture2);
			m_ComputeShader.Dispatch(_kernelInitTextures, threadGroupsX, threadGroupsY, 1);
		}

		private void DispatchInitBuffers()
		{
			m_ComputeShader.SetInt("_MaxDropletsInCell", m_MaxDropletsInCell);
			m_ComputeShader.SetBuffer(_kernelInitIndex, "_CountInBuffer", _rainCountBuffer);
			m_ComputeShader.SetBuffer(_kernelInitIndex, "_CountOutBuffer", _rainCountBuffer2);
			m_ComputeShader.SetBuffer(_kernelInitIndex, "InBuffer", _rainGridBuffer);
			m_ComputeShader.SetBuffer(_kernelInitIndex, "OutBuffer", _rainGridBuffer2);
			m_ComputeShader.SetInts("_Resolution", m_Resolution.x, m_Resolution.y);
			m_ComputeShader.SetFloats("_WindshieldPlaneSize", m_WindshieldPlane.width, m_WindshieldPlane.height);
			m_ComputeShader.SetInts("_CellsCount2d", m_GridDimensions.x, m_GridDimensions.y);
			_enableTurbulanceKeyword = new LocalKeyword(m_ComputeShader, "TURBULANCE_ENABLED");
			_wipersEnabledKeyword = new LocalKeyword(m_ComputeShader, "WIPERS_ENABLED");
			m_ComputeShader.SetKeyword(in _enableTurbulanceKeyword, m_EnableTurbulance && m_TurbulanceTexture != null);
			m_ComputeShader.SetKeyword(in _wipersEnabledKeyword, m_WipersEnabled && m_WipersScript != null);
			if (m_EnableTurbulance && m_TurbulanceTexture != null)
			{
				m_ComputeShader.SetTexture(_kernelUpdateIndex, "_TurbulanceTexture", m_TurbulanceTexture);
				m_ComputeShader.SetInts("_TurbulanceResolution", m_TurbulanceTexture.width, m_TurbulanceTexture.height);
			}
			int threadGroupsX = Mathf.CeilToInt((float)m_GridDimensions.x / 16f);
			int threadGroupsY = Mathf.CeilToInt((float)m_GridDimensions.y / 16f);
			m_ComputeShader.Dispatch(_kernelInitIndex, threadGroupsX, threadGroupsY, 1);
		}

		public void ResetRain()
		{
			if (_isInitialized)
			{
				ReleaseBuffers();
				InitializeBuffer(ref _rainGridBuffer);
				InitializeBuffer(ref _rainGridBuffer2);
				InitializeCountBuffer(ref _rainCountBuffer);
				InitializeCountBuffer(ref _rainCountBuffer2);
				InitializeWipersBuffer(ref _wipersBuffer);
				ReleaseRenderTextures();
				InitRenderTexture(ref _renderTexture1);
				InitRenderTexture(ref _renderTexture2);
				DispatchInitTextures();
				DispatchInitBuffers();
			}
		}

		private void Start()
		{
			if (m_EnableTurbulance && m_TurbulanceTexture == null)
			{
				Debug.LogError("Turbulance is enabled, but TurbulanceTexture is not set");
			}
			if (m_WipersEnabled && m_WipersScript == null)
			{
				Debug.LogError("Wipers are enabled, but WipersScript is not set");
			}
			m_ComputeShader = Object.Instantiate(m_ComputeShader);
			if (m_RainPostProcessProfile != null)
			{
				m_RainPostProcessProfile = Object.Instantiate(m_RainPostProcessProfile);
			}
			InitKernelIndicies();
			_isInitialized = true;
			ResetRain();
			if (m_RainPostProcessProfile != null)
			{
				m_RainPostProcessProfile.InitPostProcesses(m_Resolution);
			}
			UpdateShaderValues();
		}

		private ComputeBuffer GetBuffer(int id)
		{
			if ((!bufferSwap && id == 0) || (bufferSwap && id == 1))
			{
				return _rainGridBuffer;
			}
			return _rainGridBuffer2;
		}

		private ComputeBuffer GetCountBuffer(int id)
		{
			if ((!bufferSwap && id == 0) || (bufferSwap && id == 1))
			{
				return _rainCountBuffer;
			}
			return _rainCountBuffer2;
		}

		private RenderTexture GetPrevTexture()
		{
			if (!textureSwap)
			{
				return _renderTexture2;
			}
			return _renderTexture1;
		}

		public RenderTexture GetCurrTexture()
		{
			if (!textureSwap)
			{
				return _renderTexture1;
			}
			return _renderTexture2;
		}

		private void ReleaseBuffers()
		{
			if (_rainGridBuffer != null)
			{
				_rainGridBuffer.Release();
			}
			if (_rainGridBuffer2 != null)
			{
				_rainGridBuffer2.Release();
			}
			if (_rainCountBuffer != null)
			{
				_rainCountBuffer.Release();
			}
			if (_rainCountBuffer2 != null)
			{
				_rainCountBuffer2.Release();
			}
			if (_wipersBuffer != null)
			{
				_wipersBuffer.Release();
			}
		}

		private void ReleaseRenderTextures()
		{
			if (_renderTexture1 != null)
			{
				_renderTexture1.Release();
				Object.Destroy(_renderTexture1);
				_renderTexture1 = null;
			}
			if (_renderTexture2 != null)
			{
				_renderTexture2.Release();
				Object.Destroy(_renderTexture2);
				_renderTexture2 = null;
			}
		}

		public void UpdateShaderValues()
		{
			if (!(m_ComputeShader == null))
			{
				m_ComputeShader.SetFloat("_MaxVelocity", m_MaxDropVelocity);
				m_ComputeShader.SetVector("_MovementVector", m_Movement);
				if (m_WindshieldPlane == null)
				{
					m_WindshieldPlane = new WindshieldPlane(this);
				}
				float num = Mathf.Min(m_WindshieldPlane.width / (float)m_GridDimensions.x, m_WindshieldPlane.height / (float)m_GridDimensions.y);
				m_ComputeShader.SetFloat("_MaxPossibleDropletSize", num);
				m_ComputeShader.SetFloat("_MaxDropletSize", m_MaxDropletRadius * num);
				m_ComputeShader.SetFloat("_MinDropletSize", m_MinDropletRadius * num);
				m_ComputeShader.SetFloat("_SpawnRate", m_SpawnRate);
				m_ComputeShader.SetFloat("_SpawnThreshold", m_SpawnAmount);
				m_ComputeShader.SetFloat("_StartVelocity", m_StartDropsVelocity);
				m_ComputeShader.SetFloat("_Drag", m_Drag);
				m_ComputeShader.SetFloat("_SpeedMultiplier", m_SpeedMultiplier);
				if (m_EnableTurbulance)
				{
					m_ComputeShader.SetFloat("_MaxTurbulanceSpeedImpact", m_MaxTurbulanceSpeedImpact);
					m_ComputeShader.SetFloats("_TurbulanceScale", m_TurbulanceScale.x, m_TurbulanceScale.y);
					m_ComputeShader.SetFloat("_TurbulanceImpact", m_TurbulanceImpact);
					m_ComputeShader.SetFloat("_TurbulanceSpeedImpact", m_TurbulanceSpeedImpactMultiplier);
				}
				m_DropsInfluenceDistance = Mathf.Min(Mathf.Max(0f, m_DropsInfluenceDistance), 1f);
				m_ComputeShader.SetFloat("_DropsInfluenceDistance", m_EnableDropsInfluence ? Mathf.Max(0f, m_DropsInfluenceDistance * num) : 0f);
				m_ComputeShader.SetFloat("_DropsInfluenceProportion", m_DropsInfluenceProportion);
				m_ComputeShader.SetFloat("_DropsInfluenceAddition", m_DropsInfluenceAddition);
				m_ComputeShader.SetFloat("_RainStreakDecayRate", m_RainStreakDecayRate);
				m_ComputeShader.SetFloat("_WipersThreshold", m_WipersThreshold);
				m_ComputeShader.SetFloat("_WipersSpeed", m_WipersSpeedMultiplier);
			}
		}

		private void UpdateDropletsBuffers()
		{
			int threadGroupsX = Mathf.CeilToInt((float)m_GridDimensions.x / 16f);
			int threadGroupsY = Mathf.CeilToInt((float)m_GridDimensions.y / 16f);
			m_ComputeShader.SetFloat("_Time", Time.time);
			m_ComputeShader.SetFloat("_DeltaTime", _lastDeltaTime);
			m_ComputeShader.SetBuffer(_kernelUpdateIndex, "InBuffer", GetBuffer(0));
			m_ComputeShader.SetBuffer(_kernelUpdateIndex, "OutBuffer", GetBuffer(1));
			m_ComputeShader.SetBuffer(_kernelUpdateIndex, "_CountInBuffer", GetCountBuffer(0));
			m_ComputeShader.SetBuffer(_kernelUpdateIndex, "_CountOutBuffer", GetCountBuffer(1));
			if (m_WipersEnabled)
			{
				if (!m_WipersScript || _wipersBuffer == null || !m_WipersScript.IsInitialized())
				{
					return;
				}
				m_ComputeShader.SetTexture(_kernelUpdateIndex, "WipersTexture", m_WipersScript.getCurrTexture());
				if (m_WipersScript.GetWipersData() != null)
				{
					_wipersBuffer.SetData(m_WipersScript.GetWipersData());
				}
				m_ComputeShader.SetBuffer(_kernelUpdateIndex, "_Wipers", _wipersBuffer);
			}
			m_ComputeShader.Dispatch(_kernelUpdateIndex, threadGroupsX, threadGroupsY, 1);
		}

		private void UpdateComputeTexture()
		{
			int threadGroupsX = Mathf.CeilToInt((float)m_Resolution.x / 16f);
			int threadGroupsY = Mathf.CeilToInt((float)m_Resolution.y / 16f);
			if (m_WipersEnabled)
			{
				if (!m_WipersScript || _wipersBuffer == null || !m_WipersScript.IsInitialized())
				{
					return;
				}
				m_ComputeShader.SetBuffer(_kernelToTexture, "_Wipers", _wipersBuffer);
				m_ComputeShader.SetTexture(_kernelToTexture, "WipersTexture", m_WipersScript.getCurrTexture());
			}
			m_ComputeShader.SetBuffer(_kernelToTexture, "OutBuffer", GetBuffer(1));
			m_ComputeShader.SetTexture(_kernelToTexture, "Result", GetCurrTexture());
			m_ComputeShader.SetTexture(_kernelToTexture, "PrevState", GetPrevTexture());
			m_ComputeShader.Dispatch(_kernelToTexture, threadGroupsX, threadGroupsY, 1);
		}

		private void SwapTexturesAndBuffers()
		{
			bufferSwap = !bufferSwap;
			textureSwap = !textureSwap;
		}

		private void Update()
		{
			if (!(Time.time - _lastUpdateTime < _fixedDeltaTime))
			{
				_lastDeltaTime = Time.time - _lastUpdateTime;
				_lastUpdateTime = Time.time;
				if (m_WipersEnabled && m_WipersScript.IsInitialized())
				{
					m_WipersScript?.UpdateWipers(_lastDeltaTime);
				}
				UpdateDropletsBuffers();
				UpdateComputeTexture();
				Texture value = ((!(m_RainPostProcessProfile != null)) ? GetCurrTexture() : m_RainPostProcessProfile.UpdatePostProcesses(GetCurrTexture()));
				if (m_SetRainMaterialTexture && m_RainMaterial != null)
				{
					m_RainMaterial.SetTexture("_HeightMap", value);
				}
				SwapTexturesAndBuffers();
			}
		}
	}
}
