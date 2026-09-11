using System;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Zorro.Core;
using Zorro.Core.SmallShadows;

[Unity.Jobs.DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__3239455958611053226
{
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForTransformExtensions.EarlyJobInit<DistanceDisablerJob>();
			IJobParallelForTransformExtensions.EarlyJobInit<SmallShadowCheckJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		CreateJobReflectionData();
	}
}
