using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public interface IArcBuildService
	{
		List<Vector3> BuildArc(Vector3 from, Vector3 to);
	}
}
