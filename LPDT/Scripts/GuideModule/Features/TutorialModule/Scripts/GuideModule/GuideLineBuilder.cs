using System;
using System.Collections.Generic;
using Features.NavigationModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class GuideLineBuilder : MonoBehaviour
	{
		private class TrackedPath
		{
			public Transform StartTransform { get; }

			public Transform EndTransform { get; }

			public IGuideLineEntity GuideLineEntity { get; }

			public float UpdateTimer { get; set; }

			public GuideLineBuildType BuildType { get; }

			public GuideLineUpdateFrequencyType UpdateFrequencyType { get; }

			public Func<bool> KillCondition { get; }

			public TrackedPath(Transform startTransform, Transform endTransform, IGuideLineEntity guideLineEntity, GuideLineBuildType buildType, GuideLineUpdateFrequencyType updateFrequencyType, Func<bool> killCondition)
			{
				StartTransform = startTransform;
				EndTransform = endTransform;
				GuideLineEntity = guideLineEntity;
				BuildType = buildType;
				UpdateFrequencyType = updateFrequencyType;
				KillCondition = killCondition;
			}
		}

		private IGuideLinePoolService _guideLinePoolService;

		private INavigationService _navigationService;

		private GuideLineConfiguration _guideLineConfiguration;

		private IArcBuildService _arcBuildService;

		private readonly Dictionary<int, TrackedPath> _trackedPaths = new Dictionary<int, TrackedPath>();

		private readonly List<int> _staleHandles = new List<int>();

		private int _nextHandleId = 1;

		[Inject]
		public void InjectDependencies(IGuideLinePoolService guideLinePoolService, INavigationService navigationService, GuideLineConfiguration guideLineConfiguration, IArcBuildService arcBuildService)
		{
			_guideLinePoolService = guideLinePoolService;
			_navigationService = navigationService;
			_guideLineConfiguration = guideLineConfiguration;
			_arcBuildService = arcBuildService;
		}

		public GuideLineHandle StartTrackingPath(Transform startTransform, Transform endTransform, GuideLineBuildType buildType, GuideLineUpdateFrequencyType updateFrequencyType, Func<bool> killCondition = null)
		{
			IGuideLineEntity guideLineEntity = _guideLinePoolService.Pop(buildType);
			if (guideLineEntity == null)
			{
				return GuideLineHandle.Invalid;
			}
			int num = _nextHandleId++;
			_trackedPaths[num] = new TrackedPath(startTransform, endTransform, guideLineEntity, buildType, updateFrequencyType, killCondition);
			return new GuideLineHandle(num);
		}

		public void StopTrackingPath(GuideLineHandle handle)
		{
			if (_trackedPaths.TryGetValue(handle.Id, out var value))
			{
				_guideLinePoolService.Return(value.GuideLineEntity);
				_trackedPaths.Remove(handle.Id);
			}
		}

		private void Update()
		{
			foreach (KeyValuePair<int, TrackedPath> trackedPath in _trackedPaths)
			{
				TrackedPath value = trackedPath.Value;
				if (value.StartTransform == null || value.EndTransform == null || (value.KillCondition != null && value.KillCondition()))
				{
					_staleHandles.Add(trackedPath.Key);
					continue;
				}
				value.UpdateTimer += Time.deltaTime;
				if (value.UpdateFrequencyType == GuideLineUpdateFrequencyType.FixedFrequency && value.UpdateTimer < _guideLineConfiguration.GuideLineUpdateFrequency)
				{
					continue;
				}
				value.UpdateTimer = 0f;
				switch (value.BuildType)
				{
				case GuideLineBuildType.PathFinding:
				{
					if (!_navigationService.TryGetPathPolyline(value.StartTransform.position + Vector3.up * _guideLineConfiguration.PathPointsGroundOffset, value.EndTransform.position + Vector3.up * _guideLineConfiguration.PathPointsGroundOffset, _guideLineConfiguration.GuideLineSampleRadius, _guideLineConfiguration.GuideLinePointSpacing, _guideLineConfiguration.GuideLineMaxPointsPerSegment, _guideLineConfiguration.GuideLineGroundProbeDistance, out var polyline))
					{
						break;
					}
					value.GuideLineEntity.SetPoints(polyline);
					foreach (Vector3 item in polyline)
					{
						Debug.DrawLine(item, item + Vector3.up * 3f, Color.red, 1f);
					}
					break;
				}
				case GuideLineBuildType.StraightArc:
					value.GuideLineEntity.SetPoints(_arcBuildService.BuildArc(value.StartTransform.position, value.EndTransform.position));
					break;
				default:
					throw new ArgumentException();
				}
			}
			foreach (int staleHandle in _staleHandles)
			{
				if (_trackedPaths.TryGetValue(staleHandle, out var value2))
				{
					_guideLinePoolService.Return(value2.GuideLineEntity);
					_trackedPaths.Remove(staleHandle);
				}
			}
			_staleHandles.Clear();
		}
	}
}
