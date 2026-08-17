using System.Runtime.InteropServices;
using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	[AddComponentMenu("Windshield Rain Asset/Wipers")]
	public class Wipers : MonoBehaviour
	{
		[Space]
		[Header("Shader Settings")]
		public int delay = 2;

		public WindshieldRain rainScript;

		[HideInInspector]
		public ComputeShader computeShader;

		[Space]
		[Range(0f, 10f)]
		public float wipingDisappearingRate = 1f;

		[Range(0f, 10f)]
		public float smudgeDisappearingRate = 1f;

		[Range(0f, 1f)]
		public float smudgesNoiseStrength = 0.5f;

		public float smudgesNoiseScale = 1f;

		[Space]
		public WipersMaterialTexture[] texturesToSet = new WipersMaterialTexture[0];

		[Space]
		public WiperObject[] wipers;

		private Wiper[] wipersData;

		private int kernelIndex;

		private RenderTexture renderTexture1;

		private RenderTexture renderTexture2;

		private ComputeBuffer wipersBuffer;

		private bool textureSwap;

		private bool _isInitialized;

		private Vector2Int Resolution => rainScript.m_Resolution;

		private RenderTexture getPrevTexture()
		{
			if (!textureSwap)
			{
				return renderTexture2;
			}
			return renderTexture1;
		}

		public RenderTexture getCurrTexture()
		{
			if (!textureSwap)
			{
				return renderTexture1;
			}
			return renderTexture2;
		}

		private void InitRenderTexture(ref RenderTexture renderTexture)
		{
			renderTexture = new RenderTexture(Resolution.x, Resolution.y, 0, RenderTextureFormat.ARGBFloat);
			renderTexture.enableRandomWrite = true;
			renderTexture.Create();
			renderTexture.filterMode = FilterMode.Point;
		}

		private void InitializeWipersBuffer()
		{
			if (wipers.Length == 0)
			{
				Debug.LogWarning("No Wipers Objects assigned in the Wipers script!");
				return;
			}
			wipersBuffer = new ComputeBuffer(wipers.Length, Marshal.SizeOf(typeof(Wiper)));
			wipersData = new Wiper[wipers.Length];
			UpdateWipersBuffer();
		}

		public void UpdateWipersBuffer()
		{
			if (wipersData != null)
			{
				for (int i = 0; i < wipersData.Length; i++)
				{
					wipersData[i] = GetWiperData(wipers[i]);
				}
				wipersBuffer.SetData(wipersData);
			}
		}

		private void ReleaseBuffers()
		{
			if (wipersBuffer != null)
			{
				wipersBuffer.Release();
			}
		}

		private void OnDestroy()
		{
			ReleaseBuffers();
		}

		private void InitKernelIndicies()
		{
			kernelIndex = computeShader.FindKernel("CSMain");
		}

		public bool IsInitialized()
		{
			return _isInitialized;
		}

		private void Start()
		{
			computeShader = Object.Instantiate(computeShader);
			InitKernelIndicies();
			InitRenderTexture(ref renderTexture1);
			InitRenderTexture(ref renderTexture2);
			ReleaseBuffers();
			InitializeWipersBuffer();
			InitWipers();
			computeShader.SetInt("_WipersCount", wipers.Length);
			computeShader.SetInts("_Resolution", Resolution.x, Resolution.y);
			computeShader.SetFloats("_WindshieldPlaneSize", rainScript.m_WindshieldPlane.width, rainScript.m_WindshieldPlane.height);
			_isInitialized = true;
		}

		public void UpdateWipers(float deltaTime)
		{
			for (int i = 0; i < wipers.Length; i++)
			{
				wipers[i].UpdateWiper(rainScript.m_WindshieldPlane);
			}
			UpdateWipersBuffer();
			if (wipersData != null && wipersBuffer != null)
			{
				computeShader.SetFloat("_DeltaTime", deltaTime);
				computeShader.SetFloat("_WipingDisappearingRate", wipingDisappearingRate);
				computeShader.SetFloat("_SmudgeDisappearingRate", smudgeDisappearingRate);
				computeShader.SetFloat("_SmudgesNoiseStrength", smudgesNoiseStrength);
				computeShader.SetFloat("_SmudgesNoiseScale", smudgesNoiseScale);
				int threadGroupsX = Mathf.CeilToInt((float)Resolution.x / 16f);
				int threadGroupsY = Mathf.CeilToInt((float)Resolution.y / 16f);
				computeShader.SetBuffer(kernelIndex, "_Wipers", wipersBuffer);
				computeShader.SetTexture(kernelIndex, "Result", getCurrTexture());
				computeShader.SetTexture(kernelIndex, "Prev", getPrevTexture());
				computeShader.SetTexture(kernelIndex, "DropsTexture", rainScript.GetCurrTexture());
				computeShader.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
				WipersMaterialTexture[] array = texturesToSet;
				foreach (MaterialTexture materialTexture in array)
				{
					materialTexture.material.SetTexture(materialTexture.textureName, getCurrTexture());
				}
				textureSwap = !textureSwap;
			}
		}

		private void InitWipers()
		{
			for (int i = 0; i < wipers.Length; i++)
			{
				Vector2 vector = rainScript.m_WindshieldPlane.WorldPosToWindshieldPos(wipers[i].originPos.position);
				_ = (rainScript.m_WindshieldPlane.WorldPosToWindshieldPos(wipers[i].endPos.position) - vector).magnitude;
				wipers[i].InitWiper(delay, rainScript.m_WindshieldPlane);
			}
		}

		public Wiper GetWiperData(WiperObject wiperObject)
		{
			Wiper result = default(Wiper);
			result.origin = rainScript.m_WindshieldPlane.WorldPosToWindshieldPos(wiperObject.originPos.position);
			Vector2 vector = rainScript.m_WindshieldPlane.WorldPosToWindshieldPos(wiperObject.startPos.position);
			Vector2 vector2 = rainScript.m_WindshieldPlane.WorldPosToWindshieldPos(wiperObject.endPos.position);
			result.pos = new Vector4(vector.x, vector.y, vector2.x, vector2.y);
			Vector2 lastStartPos = wiperObject.lastStartPos;
			Vector2 lastEndPos = wiperObject.lastEndPos;
			result.prevPos = new Vector4(lastStartPos.x, lastStartPos.y, lastEndPos.x, lastEndPos.y);
			return result;
		}

		public Wiper[] GetWipersData()
		{
			return wipersData;
		}
	}
}
