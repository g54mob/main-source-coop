using System;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public struct ObiWingedPoint
	{
		public enum TangentMode
		{
			Aligned = 0,
			Mirrored = 1,
			Free = 2
		}

		public TangentMode tangentMode;

		public Vector3 inTangent;

		public Vector3 position;

		public Vector3 outTangent;

		public Vector3 inTangentEndpoint => position + inTangent;

		public Vector3 outTangentEndpoint => position + outTangent;

		public ObiWingedPoint(Vector3 inTangent, Vector3 point, Vector3 outTangent)
		{
			tangentMode = TangentMode.Aligned;
			this.inTangent = inTangent;
			position = point;
			this.outTangent = outTangent;
		}

		public void SetInTangentEndpoint(Vector3 value)
		{
			Vector3 vector = value - position;
			switch (tangentMode)
			{
			case TangentMode.Mirrored:
				outTangent = -vector;
				break;
			case TangentMode.Aligned:
				outTangent = -vector.normalized * outTangent.magnitude;
				break;
			}
			inTangent = vector;
		}

		public void SetOutTangentEndpoint(Vector3 value)
		{
			Vector3 vector = value - position;
			switch (tangentMode)
			{
			case TangentMode.Mirrored:
				inTangent = -vector;
				break;
			case TangentMode.Aligned:
				inTangent = -vector.normalized * inTangent.magnitude;
				break;
			}
			outTangent = vector;
		}

		public void SetInTangent(Vector3 value)
		{
			Vector3 vector = value;
			switch (tangentMode)
			{
			case TangentMode.Mirrored:
				outTangent = -vector;
				break;
			case TangentMode.Aligned:
				outTangent = -vector.normalized * outTangent.magnitude;
				break;
			}
			inTangent = vector;
		}

		public void SetOutTangent(Vector3 value)
		{
			Vector3 vector = value;
			switch (tangentMode)
			{
			case TangentMode.Mirrored:
				inTangent = -vector;
				break;
			case TangentMode.Aligned:
				inTangent = -vector.normalized * inTangent.magnitude;
				break;
			}
			outTangent = vector;
		}

		public void Transform(Vector3 translation, Quaternion rotation, Vector3 scale)
		{
			position += translation;
			inTangent = rotation * Vector3.Scale(inTangent, scale);
			outTangent = rotation * Vector3.Scale(outTangent, scale);
		}
	}
}
