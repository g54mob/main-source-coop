using System;
using UnityEngine;

namespace DunGen
{
	[Serializable]
	public class PathStraighteningSettings
	{
		public bool OverrideStraightenChance;

		[Range(0f, 1f)]
		public float StraightenChance;

		public bool OverrideCanStraightenMainPath;

		public bool CanStraightenMainPath = true;

		public bool OverrideCanStraightenBranchPaths;

		public bool CanStraightenBranchPaths;

		public static T GetFinalValue<T>(Func<PathStraighteningSettings, T> valueGetter, Func<PathStraighteningSettings, bool> overriddenGetter, params PathStraighteningSettings[] settingsHierarchy)
		{
			for (int i = 0; i < settingsHierarchy.Length; i++)
			{
				bool num = i == settingsHierarchy.Length - 1;
				PathStraighteningSettings arg = settingsHierarchy[i];
				if (num)
				{
					return valueGetter(arg);
				}
				if (overriddenGetter(arg))
				{
					return valueGetter(arg);
				}
			}
			return default(T);
		}

		public static PathStraighteningSettings GetFinalValues(params PathStraighteningSettings[] settingsHierarchy)
		{
			return new PathStraighteningSettings
			{
				StraightenChance = GetFinalValue((PathStraighteningSettings s) => s.StraightenChance, (PathStraighteningSettings s) => s.OverrideStraightenChance, settingsHierarchy),
				CanStraightenMainPath = GetFinalValue((PathStraighteningSettings s) => s.CanStraightenMainPath, (PathStraighteningSettings s) => s.OverrideCanStraightenMainPath, settingsHierarchy),
				CanStraightenBranchPaths = GetFinalValue((PathStraighteningSettings s) => s.CanStraightenBranchPaths, (PathStraighteningSettings s) => s.OverrideCanStraightenBranchPaths, settingsHierarchy)
			};
		}
	}
}
