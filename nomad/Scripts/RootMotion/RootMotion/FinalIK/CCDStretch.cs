using UnityEngine;

namespace RootMotion.FinalIK
{
	public class CCDStretch : MonoBehaviour
	{
		public CCDIK ik;

		[Range(0f, 0.999f)]
		public float maxSquash;

		public float maxStretch = 2f;

		private Vector3[] defaultLocalPositions = new Vector3[0];

		private void Start()
		{
			defaultLocalPositions = new Vector3[ik.solver.bones.Length - 1];
			for (int i = 1; i < ik.solver.bones.Length; i++)
			{
				defaultLocalPositions[i - 1] = ik.solver.bones[i].transform.localPosition;
			}
		}

		private void LateUpdate()
		{
			for (int i = 1; i < ik.solver.bones.Length; i++)
			{
				ik.solver.bones[i].transform.localPosition = defaultLocalPositions[i - 1];
			}
			float num = Vector3.Magnitude(((ik.solver.target != null) ? ik.solver.target.position : ik.solver.IKPosition) - ik.solver.bones[0].transform.position);
			float num2 = 0f;
			for (int j = 1; j < ik.solver.bones.Length; j++)
			{
				num2 += Vector3.Magnitude(ik.solver.bones[j].transform.position - ik.solver.bones[j - 1].transform.position);
			}
			maxStretch = Mathf.Max(maxStretch, 1f);
			float num3 = Mathf.Clamp(num / num2, 1f - maxSquash, maxStretch);
			for (int k = 1; k < ik.solver.bones.Length; k++)
			{
				ik.solver.bones[k].transform.localPosition *= num3;
			}
		}
	}
}
