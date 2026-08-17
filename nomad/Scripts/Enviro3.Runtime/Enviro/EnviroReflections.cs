using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Enviro
{
	[Serializable]
	public class EnviroReflections
	{
		public enum GlobalReflectionResolution
		{
			R16 = 0,
			R32 = 1,
			R64 = 2,
			R128 = 3,
			R256 = 4,
			R512 = 5,
			R1024 = 6,
			R2048 = 7
		}

		public bool globalReflections = true;

		[Tooltip("Set if enviro reflection probe should use custom rendering setup. For example to include post effectsin birp.")]
		public bool customRendering = true;

		[Tooltip("Set to use custom timeslicing when rendered in custom mode.")]
		public bool customRenderingTimeSlicing = true;

		[Tooltip("Set if enviro reflection probe should update faces individual on different frames.")]
		public ReflectionProbeTimeSlicingMode globalReflectionTimeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;

		[Tooltip("Enable/disable enviro reflection probe updates based on gametime changes..")]
		public bool globalReflectionsUpdateOnGameTime = true;

		[Tooltip("Enable/disable enviro reflection probe updates based on transform position changes..")]
		public bool globalReflectionsUpdateOnPosition = true;

		[Tooltip("Reflection probe intensity.")]
		[Range(0f, 2f)]
		public float globalReflectionsIntensity = 1f;

		[Tooltip("Reflection probe update rate based on game time.")]
		public float globalReflectionsTimeTreshold = 0.025f;

		[Tooltip("Reflection probe update rate based on camera position.")]
		public float globalReflectionsPositionTreshold = 0.5f;

		[Tooltip("Reflection probe scale. Increase that one to increase the area where reflection probe will influence your scene.")]
		[Range(10f, 10000f)]
		public float globalReflectionsScale = 10000f;

		[Tooltip("Reflection probe resolution.")]
		public GlobalReflectionResolution globalReflectionResolution = GlobalReflectionResolution.R256;

		[Tooltip("Reflection probe rendered Layers.")]
		public LayerMask globalReflectionLayers;

		[Tooltip("Enable this option to update the default reflection with global reflection probes cubemap. This can be needed for material that might not support direct reflection probes. (Instanced Indirect Rendering)")]
		public bool updateDefaultEnvironmentReflections = true;

		[Tooltip("Reflection cubemap used for default scene sky reflections in < Unity 2022.1 versions.")]
		public Cubemap defaultSkyReflectionTex;
	}
}
