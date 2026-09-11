using UnityEngine;
using UnityEngine.Rendering;

public class DynamicResolution : MonoBehaviour
{
	public static double DesiredFrameRate = 30.0;

	private static double DesiredFrameTime = 1000.0 / DesiredFrameRate;

	private const uint ScaleRaiseCounterLimit = 360u;

	private const uint ScaleRaiseCounterSmallIncrement = 3u;

	private const uint ScaleRaiseCounterBigIncrement = 10u;

	private const double HeadroomThreshold = 0.06;

	private const double DeltaThreshold = 0.035;

	private const float ScaleIncreaseBasis = 0.035f;

	private const float ScaleIncreaseSmallFactor = 0.25f;

	private const float ScaleIncreaseBigFactor = 1f;

	private const float ScaleHeadroomClampMin = 0.1f;

	private const float ScaleHeadroomClampMax = 0.5f;

	private const uint NumFrameTimings = 30u;

	private const float MinScaleFactor = 0.5f;

	private const float MaxScaleFactor = 1f;

	private uint FrameCount;

	private FrameTiming[] FrameTimings = new FrameTiming[30];

	private double GPUFrameTime;

	private double CPUFrameTime;

	private double GPUTimeDelta;

	private uint ScaleRaiseCounter;

	private static float CurrentScaleFactor = 1f;

	private static bool CanUpdate = false;

	private static bool SystemEnabled = true;

	private static bool PlatformSupported = true;

	private void Update()
	{
		if (!SystemEnabled || !CanUpdate)
		{
			return;
		}
		GetFrameStats();
		double num = DesiredFrameTime - GPUFrameTime;
		if (num < 0.0)
		{
			ScaleRaiseCounter = 0u;
			float num2 = (float)(num / DesiredFrameTime);
			CurrentScaleFactor = Mathf.Clamp01(CurrentScaleFactor + num2);
			SetNewScale();
			return;
		}
		if (GPUTimeDelta > num)
		{
			ScaleRaiseCounter = 0u;
			float num3 = (float)(GPUTimeDelta / DesiredFrameTime);
			CurrentScaleFactor = Mathf.Clamp01(CurrentScaleFactor - num3);
			SetNewScale();
			return;
		}
		if (GPUTimeDelta < 0.0)
		{
			ScaleRaiseCounter += 10u;
		}
		else
		{
			double num4 = DesiredFrameTime * 0.06;
			double num5 = DesiredFrameTime * 0.035;
			if (num > num4 && GPUTimeDelta < num5)
			{
				ScaleRaiseCounter += 3u;
			}
		}
		if (ScaleRaiseCounter >= 360)
		{
			ScaleRaiseCounter = 0u;
			float t = (Mathf.Clamp((float)(num / DesiredFrameTime), 0.1f, 0.5f) - 0.1f) / 0.4f;
			float num6 = 0.035f * Mathf.Lerp(0.25f, 1f, t);
			CurrentScaleFactor = Mathf.Clamp01(CurrentScaleFactor + num6);
			SetNewScale();
		}
	}

	private void SetNewScale()
	{
		float num = Mathf.Lerp(0.5f, 1f, CurrentScaleFactor);
		ScalableBufferManager.ResizeBuffers(num, num);
	}

	private static void ResetScale()
	{
		CurrentScaleFactor = 1f;
		ScalableBufferManager.ResizeBuffers(1f, 1f);
	}

	private void GetFrameStats()
	{
		if (FrameCount < 30)
		{
			FrameCount++;
			return;
		}
		FrameCount = 0u;
		FrameTimingManager.CaptureFrameTimings();
		FrameTimingManager.GetLatestTimings(30u, FrameTimings);
		if ((long)FrameTimings.Length >= 30L && FrameTimings[0].cpuTimeFrameComplete >= FrameTimings[0].cpuTimePresentCalled)
		{
			if (GPUFrameTime != 0.0)
			{
				GPUTimeDelta = FrameTimings[0].gpuFrameTime - GPUFrameTime;
			}
			double num = 0.0;
			double num2 = 0.0;
			FrameTiming[] frameTimings = FrameTimings;
			for (int i = 0; i < frameTimings.Length; i++)
			{
				FrameTiming frameTiming = frameTimings[i];
				num += frameTiming.gpuFrameTime;
				num2 += frameTiming.cpuFrameTime;
			}
			GPUFrameTime = num / (double)FrameTimings.Length;
			CPUFrameTime = num2 / (double)FrameTimings.Length;
		}
	}

	public static void Enable()
	{
		if (PlatformSupported)
		{
			SystemEnabled = true;
		}
	}

	public static void Disable()
	{
		if (PlatformSupported)
		{
			SystemEnabled = false;
			ResetScale();
		}
	}

	public static bool IsSupportedOnPlatform()
	{
		return PlatformSupported;
	}

	public static bool IsEnabled()
	{
		return SystemEnabled;
	}

	public static double GetTargetFramerate()
	{
		return DesiredFrameRate;
	}

	public static void SetTargetFramerate(double target)
	{
		DesiredFrameRate = target;
		DesiredFrameTime = 1000.0 / target;
		ResetScale();
	}

	private void Start()
	{
		if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Metal && (FrameTimingManager.GetCpuTimerFrequency() == 0L || FrameTimingManager.GetGpuTimerFrequency() == 0L))
		{
			PlatformSupported = false;
			SystemEnabled = false;
		}
		CanUpdate = true;
	}

	private void OnDestroy()
	{
		if (SystemEnabled)
		{
			ResetScale();
		}
	}
}
