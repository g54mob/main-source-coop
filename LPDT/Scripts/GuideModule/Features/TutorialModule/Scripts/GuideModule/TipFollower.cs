using System.Collections.Generic;
using Features.CameraModelModule;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class TipFollower : MonoBehaviour
	{
		private class FollowEntry
		{
			public ITipEntity Tip { get; }

			public Transform Target { get; }

			public TipFollowFlags FollowFlags { get; }

			public float UpOffset { get; }

			public FollowEntry(ITipEntity tip, Transform target, TipFollowFlags followFlags, float upOffset)
			{
				Tip = tip;
				Target = target;
				FollowFlags = followFlags;
				UpOffset = upOffset;
			}
		}

		private readonly Dictionary<int, FollowEntry> _followEntries = new Dictionary<int, FollowEntry>();

		private int _nextFollowId = 1;

		private CameraModel _cameraModel;

		[Inject]
		public void InjectDependencies(CameraModel cameraModel)
		{
			_cameraModel = cameraModel;
		}

		public int StartFollow(ITipEntity tip, Transform target, TipFollowFlags followFlags, float upOffset)
		{
			int num = _nextFollowId++;
			_followEntries[num] = new FollowEntry(tip, target, followFlags, upOffset);
			return num;
		}

		public void StopFollow(int followId)
		{
			_followEntries.Remove(followId);
		}

		private void Update()
		{
			foreach (KeyValuePair<int, FollowEntry> followEntry in _followEntries)
			{
				FollowEntry value = followEntry.Value;
				if (!(value.Target == null) && !(value.Tip.Transform == null))
				{
					if ((value.FollowFlags & TipFollowFlags.Position) != TipFollowFlags.None)
					{
						value.Tip.Transform.position = value.Target.position + Vector3.up * value.UpOffset;
					}
					bool num = (value.FollowFlags & TipFollowFlags.FaceCameraEntire) != 0;
					bool flag = (value.FollowFlags & TipFollowFlags.Rotation) != 0;
					if (num || flag)
					{
						Transform transform = _cameraModel.CameraObject.transform;
						Vector3 eulerAngles = Quaternion.LookRotation(transform.forward, transform.up).eulerAngles;
						Vector3 eulerAngles2 = value.Target.rotation.eulerAngles;
						Vector3 eulerAngles3 = value.Tip.Transform.rotation.eulerAngles;
						float x = ResolveAxis(value.FollowFlags, TipFollowFlags.FaceCameraX, flag, eulerAngles.x, eulerAngles2.x, eulerAngles3.x);
						float y = ResolveAxis(value.FollowFlags, TipFollowFlags.FaceCameraY, flag, eulerAngles.y, eulerAngles2.y, eulerAngles3.y);
						float z = ResolveAxis(value.FollowFlags, TipFollowFlags.FaceCameraZ, flag, eulerAngles.z, eulerAngles2.z, eulerAngles3.z);
						value.Tip.Transform.rotation = Quaternion.Euler(x, y, z);
					}
				}
			}
		}

		private static float ResolveAxis(TipFollowFlags flags, TipFollowFlags faceAxisFlag, bool followsRotation, float faceAngle, float targetAngle, float currentAngle)
		{
			if ((flags & faceAxisFlag) != TipFollowFlags.None)
			{
				return faceAngle;
			}
			if (followsRotation)
			{
				return targetAngle;
			}
			return currentAngle;
		}
	}
}
