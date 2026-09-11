using System;
using DefaultNamespace.Petter.TitleCard;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using pworld.Scripts.Extensions;

public class TitleCardSyncer : MonoBehaviour
{
	public float sendCoolDown = 2f;

	public TitleCardCanvas canvas;

	public ComputeShader csRtToUintPacker;

	public ComputeShader csRtToUintPackerPS;

	public ComputeShader csUintToRtUnPacker;

	public ComputeShader csUintToRtUnPackerPS;

	public void Awake()
	{
		canvas = GetComponent<TitleCardCanvas>();
	}

	public void ToRt(NativeArray<uint> data)
	{
		RenderTexture titleCardRt = canvas.titleCardRt;
		int width = titleCardRt.width;
		int height = titleCardRt.height;
		ComputeBuffer computeBuffer = new ComputeBuffer(data.Length, 4);
		computeBuffer.SetData(data);
		int num = csUintToRtUnPacker.FindKernel("CsUintToRtUnPacker");
		csUintToRtUnPacker.SetTexture(num, "destinationTexture", titleCardRt);
		csUintToRtUnPacker.SetBuffer(num, "data", computeBuffer);
		csUintToRtUnPacker.SetVector("resolution", new Vector2(width, height));
		csUintToRtUnPacker.PDispatch(num, "threadGroups", width, height);
		computeBuffer.Release();
	}

	public void ToByte(Action<NativeArray<uint>> callback)
	{
		RenderTexture titleCardRt = canvas.titleCardRt;
		int count = Mathf.CeilToInt((float)(titleCardRt.width * titleCardRt.height) / 32f);
		int width = titleCardRt.width;
		int height = titleCardRt.height;
		ComputeBuffer computeBuffer = new ComputeBuffer(count, 4);
		int num = csRtToUintPacker.FindKernel("CsRtToUintPacker");
		csRtToUintPacker.SetTexture(num, "sourceTexture", titleCardRt);
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
				callback?.Invoke(data.GetData<uint>());
			}
		});
		computeBuffer.Release();
	}
}
