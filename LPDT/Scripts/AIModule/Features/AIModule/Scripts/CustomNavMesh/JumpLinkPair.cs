using UnityEngine;

namespace Features.AIModule.Scripts.CustomNavMesh
{
	public readonly struct JumpLinkPair
	{
		public readonly Vector3 Start;

		public readonly Vector3 End;

		public readonly float Score;

		public JumpLinkPair(Vector3 start, Vector3 end, float score)
		{
			Start = start;
			End = end;
			Score = score;
		}
	}
}
