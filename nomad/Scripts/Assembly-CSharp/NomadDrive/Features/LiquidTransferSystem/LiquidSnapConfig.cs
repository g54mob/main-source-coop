using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem
{
	[CreateAssetMenu(menuName = "NomadDrive/Configs/LiquidTransfer/Liquid Snap Config")]
	public class LiquidSnapConfig : ScriptableObject
	{
		public List<LiquidSnapEntry> entries = new List<LiquidSnapEntry>();

		[Range(0.05f, 1.5f)]
		public float entryDuration = 0.25f;

		[Range(0.05f, 1.5f)]
		public float exitDuration = 0.2f;

		public Ease entryEase = Ease.OutCubic;

		public Ease exitEase = Ease.InOutCubic;

		public bool TryGetPose(LiquidSnapTargetType targetType, out LiquidSnapPose pose)
		{
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].targetType == targetType)
				{
					pose = entries[i].pose;
					return true;
				}
			}
			pose = default(LiquidSnapPose);
			return false;
		}

		public void SetPose(LiquidSnapTargetType targetType, LiquidSnapPose pose)
		{
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].targetType == targetType)
				{
					LiquidSnapEntry value = entries[i];
					value.pose = pose;
					entries[i] = value;
					return;
				}
			}
			entries.Add(new LiquidSnapEntry
			{
				targetType = targetType,
				pose = pose
			});
		}
	}
}
