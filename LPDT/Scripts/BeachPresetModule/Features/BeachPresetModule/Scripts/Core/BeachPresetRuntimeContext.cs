using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core
{
	public class BeachPresetRuntimeContext
	{
		public Transform RuntimeAnchor { get; }

		public BeachPreset Preset { get; }

		public Pose BeachPose { get; }

		public LevelType LevelType { get; }

		public BeachPresetRuntimeContext(BeachPreset preset, Transform runtimeAnchor, LevelType levelType)
		{
			Preset = preset;
			RuntimeAnchor = runtimeAnchor;
			BeachPose = new Pose(runtimeAnchor.position, runtimeAnchor.rotation);
			LevelType = levelType;
		}

		public Vector3 ToWorldPosition(Vector3 localPosition, bool relativeToEntryPoint)
		{
			Vector3 localPosition2 = (relativeToEntryPoint ? (Preset.Layout.EntryPointOffset + localPosition) : localPosition);
			return Preset.Layout.LocalToWorld(BeachPose, localPosition2);
		}

		public Quaternion ToWorldRotation(Vector3 localEulerAngles)
		{
			return Preset.Layout.LocalToWorldRotation(BeachPose, localEulerAngles);
		}
	}
}
