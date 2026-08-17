using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwCloneMirror")]
	[AddComponentMenu("CW/Paint Core/CW Clone Mirror")]
	public class CwCloneMirror : CwClone
	{
		[SerializeField]
		private bool flip;

		public bool Flip
		{
			get
			{
				return flip;
			}
			set
			{
				flip = value;
			}
		}

		public override void Transform(ref Matrix4x4 posMatrix, ref Matrix4x4 rotMatrix, ref Matrix4x4 rotMatrix2)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
			Matrix4x4 inverse = matrix4x.inverse;
			Matrix4x4 matrix4x2 = Matrix4x4.TRS(Vector3.zero, base.transform.rotation, Vector3.one);
			Matrix4x4 inverse2 = matrix4x2.inverse;
			Matrix4x4 matrix4x3 = Matrix4x4.Scale(new Vector3(1f, 1f, -1f));
			posMatrix = matrix4x * matrix4x3 * inverse * posMatrix;
			rotMatrix = matrix4x2 * matrix4x3 * inverse2 * rotMatrix;
			if (flip)
			{
				rotMatrix2.m00 *= -1f;
				rotMatrix2.m10 *= -1f;
				rotMatrix2.m20 *= -1f;
				rotMatrix2.m30 *= -1f;
			}
		}
	}
}
