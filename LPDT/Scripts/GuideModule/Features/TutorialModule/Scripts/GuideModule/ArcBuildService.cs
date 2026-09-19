using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class ArcBuildService : IArcBuildService
	{
		private readonly GuideLineConfiguration _guideLineConfiguration;

		public ArcBuildService(GuideLineConfiguration guideLineConfiguration)
		{
			_guideLineConfiguration = guideLineConfiguration;
		}

		public List<Vector3> BuildArc(Vector3 from, Vector3 to)
		{
			int num = Mathf.Max(2, _guideLineConfiguration.ArcSampleCount);
			List<Vector3> list = new List<Vector3>(num);
			float value = Vector3.Distance(from, to);
			float t = Mathf.InverseLerp(_guideLineConfiguration.ArcMaxHeightDistance, _guideLineConfiguration.ArcMinHeightDistance, value);
			float num2 = Mathf.Lerp(_guideLineConfiguration.ArcMaxHeight, _guideLineConfiguration.ArcMinHeight, t);
			Vector3 control = (from + to) * 0.5f + Vector3.up * num2;
			for (int i = 0; i < num; i++)
			{
				float t2 = (float)i / (float)(num - 1);
				list.Add(EvaluateQuadraticBezier(from, control, to, t2));
			}
			return list;
		}

		private static Vector3 EvaluateQuadraticBezier(Vector3 start, Vector3 control, Vector3 end, float t)
		{
			float num = 1f - t;
			return num * num * start + 2f * num * t * control + t * t * end;
		}
	}
}
