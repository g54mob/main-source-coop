using System;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using pworld.Scripts.PPhys.Bursted.Jobs;

[Unity.Jobs.DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__17127445829547203645
{
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForTransformExtensions.EarlyJobInit<SpringJobPosition>();
			IJobParallelForTransformExtensions.EarlyJobInit<SpringJobRotation>();
			IJobParallelForTransformExtensions.EarlyJobInit<SpringJobScale>();
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
