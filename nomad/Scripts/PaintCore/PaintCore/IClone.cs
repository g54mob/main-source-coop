using UnityEngine;

namespace PaintCore
{
	public interface IClone
	{
		void Transform(ref Matrix4x4 posMatrix, ref Matrix4x4 rotMatrix, ref Matrix4x4 rotMatrix2);
	}
}
