using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class TipService : ITipService
	{
		private class ActiveTip
		{
			public ITipEntity Tip { get; }

			public int FollowId { get; }

			public ActiveTip(ITipEntity tip, int followId)
			{
				Tip = tip;
				FollowId = followId;
			}
		}

		private readonly ITipPoolService _tipPoolService;

		private readonly TipConfiguration _configuration;

		private readonly TipFollowerDataHolder _tipFollowerDataHolder;

		private readonly Dictionary<int, ActiveTip> _activeTips = new Dictionary<int, ActiveTip>();

		private int _nextHandleId = 1;

		public TipService(ITipPoolService tipPoolService, TipConfiguration configuration, TipFollowerDataHolder tipFollowerDataHolder)
		{
			_tipPoolService = tipPoolService;
			_configuration = configuration;
			_tipFollowerDataHolder = tipFollowerDataHolder;
		}

		public TipHandle CreateTip(TipType tipType, Transform target, TipFollowFlags followFlags, float upOffset = 0f, bool project = false)
		{
			ITipEntity tipEntity = _tipPoolService.Pop(tipType);
			if (tipEntity == null)
			{
				return TipHandle.Invalid;
			}
			tipEntity.Transform.SetParent(null);
			tipEntity.Transform.position = target.position + Vector3.up * upOffset;
			tipEntity.Transform.rotation = (((followFlags & TipFollowFlags.Rotation) != TipFollowFlags.None) ? target.rotation : Quaternion.identity);
			if (project)
			{
				ProjectOntoSurface(tipEntity.Transform, target.position, upOffset);
			}
			int followId = 0;
			TipFollower tipFollower = _tipFollowerDataHolder.TipFollower;
			if (tipFollower != null && followFlags != TipFollowFlags.None)
			{
				followId = tipFollower.StartFollow(tipEntity, target, followFlags, upOffset);
			}
			int num = _nextHandleId++;
			_activeTips[num] = new ActiveTip(tipEntity, followId);
			tipEntity.IsInitialized = true;
			return new TipHandle(num);
		}

		public void KillTip(TipHandle handle)
		{
			if (handle.IsValid && _activeTips.TryGetValue(handle.Id, out var value))
			{
				_activeTips.Remove(handle.Id);
				if (value.FollowId != 0 && _tipFollowerDataHolder.TipFollower != null)
				{
					_tipFollowerDataHolder.TipFollower.StopFollow(value.FollowId);
				}
				if (value.Tip is Object obj && obj != null)
				{
					value.Tip.IsInitialized = false;
				}
				_tipPoolService.Return(value.Tip);
			}
		}

		private void ProjectOntoSurface(Transform tipTransform, Vector3 targetPosition, float upOffset)
		{
			if (Physics.Raycast(targetPosition + Vector3.up * _configuration.ProjectionUpOffset, Vector3.down, out var hitInfo, _configuration.ProjectionDistance, _configuration.ProjectionLayerMask))
			{
				tipTransform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + upOffset, hitInfo.point.z);
			}
		}
	}
}
