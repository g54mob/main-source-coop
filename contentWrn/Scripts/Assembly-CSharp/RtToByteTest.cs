using DefaultNamespace.Petter.TitleCard;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using pworld.Scripts.Extensions;

public class RtToByteTest : MonoBehaviour
{
	[FormerlySerializedAs("csRtToByte")]
	public ComputeShader csRtToUintPacker;

	[FormerlySerializedAs("csUintToRtUnpacker")]
	[FormerlySerializedAs("csByteToRt")]
	public ComputeShader csUintToRtUnPacker;

	public RenderTexture renderTexture;

	public uint[] resultData;

	public int debugModValue = 4;

	private int BufferCount => Mathf.CeilToInt((float)(renderTexture.width * renderTexture.height) / 32f);

	private void ToByte()
	{
		renderTexture = Object.FindObjectOfType<TitleCardCanvas>().titleCardRt;
		if (renderTexture == null)
		{
			renderTexture = Object.FindObjectOfType<TitleCardCanvas>().CreateTitleCardRT();
		}
		int width = renderTexture.width;
		int height = renderTexture.height;
		ComputeBuffer computeBuffer = new ComputeBuffer(BufferCount, 4);
		int num = csRtToUintPacker.FindKernel("CsRtToUintPacker");
		csRtToUintPacker.SetTexture(num, "sourceTexture", renderTexture);
		csRtToUintPacker.SetBuffer(num, "resultBuffer", computeBuffer);
		csRtToUintPacker.SetVector("resolution", new Vector2(width, height));
		csRtToUintPacker.PDispatch(num, "threadGroups", computeBuffer.count);
		AsyncGPUReadback.Request(computeBuffer, delegate(AsyncGPUReadbackRequest data)
		{
			if (data.hasError)
			{
				Debug.LogError("GPU readback error detected.");
			}
			else
			{
				resultData = data.GetData<uint>().ToArray();
			}
		});
		computeBuffer.Release();
	}

	private void ToRt()
	{
		renderTexture = Object.FindObjectOfType<TitleCardCanvas>().titleCardRt;
		if (renderTexture == null)
		{
			renderTexture = Object.FindObjectOfType<TitleCardCanvas>().CreateTitleCardRT();
		}
		int width = renderTexture.width;
		int height = renderTexture.height;
		ComputeBuffer computeBuffer = new ComputeBuffer(resultData.Length, 4);
		computeBuffer.SetData(resultData);
		int num = csUintToRtUnPacker.FindKernel("CsUintToRtUnPacker");
		csUintToRtUnPacker.SetTexture(num, "destinationTexture", renderTexture);
		csUintToRtUnPacker.SetBuffer(num, "data", computeBuffer);
		csUintToRtUnPacker.SetVector("resolution", new Vector2(width, height));
		csUintToRtUnPacker.PDispatch(num, "threadGroups", width, height);
		computeBuffer.Release();
	}

	public void BitShift()
	{
		bool[] array = new bool[32];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = i % 4 == 0;
		}
		uint num = 0u;
		for (int j = 0; j < array.Length; j++)
		{
			uint num2 = (array[j] ? ((uint)(1 << j)) : 0u);
			num = (num & (uint)(~(1 << j))) | num2;
		}
		PrintBits2(num);
	}

	public void PrintBits(uint a)
	{
		string text = "";
		for (int i = 0; i < 32; i++)
		{
			text = (a & 1) + " " + text;
			a >>= 1;
		}
		Debug.Log(text);
	}

	public void PrintBits2(uint existingUint)
	{
		string text = "";
		for (int i = 0; i < 32; i++)
		{
			text = (((existingUint & (uint)(1 << i)) >> i == 1) ? 1 : 0) + " " + text;
		}
		Debug.Log(text);
	}
}
