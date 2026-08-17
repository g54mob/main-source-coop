using System;
using System.Collections.Generic;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Utils.Classes.Utility;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal static class EnumConverter
	{
		public static int ToUpdateLoopTypes(UpdateLoopSetting updateLoopSetting, List<UpdateLoopType> results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			results.Clear();
			EnumNameValueCache<UpdateLoopSetting> enumNameValueCache = EnumNameValueCache<UpdateLoopSetting>.Default;
			int count = enumNameValueCache.Count;
			for (int i = 0; i < count; i++)
			{
				UpdateLoopSetting valueAt = enumNameValueCache.GetValueAt(i);
				if (valueAt != UpdateLoopSetting.None && (updateLoopSetting & valueAt) != UpdateLoopSetting.None)
				{
					results.Add(EnumNameValueCache<UpdateLoopType>.Default.GetValue(enumNameValueCache.GetName((long)valueAt)));
				}
			}
			return results.Count;
		}

		public static AlternateAxisCalibrationType ToAlternateAxisCalibrationType(ThrottleCalibrationMode throttleCalibrationMode)
		{
			return throttleCalibrationMode switch
			{
				ThrottleCalibrationMode.ZeroToOne => AlternateAxisCalibrationType.Default, 
				ThrottleCalibrationMode.NegativeOneToOne => AlternateAxisCalibrationType.ThrottleZeroCenter, 
				_ => throw new NotImplementedException(), 
			};
		}
	}
}
