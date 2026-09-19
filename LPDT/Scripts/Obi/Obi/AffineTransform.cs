using UnityEngine;

namespace Obi
{
	public struct AffineTransform
	{
		public Vector4 translation;

		public Vector4 scale;

		public Quaternion rotation;

		public AffineTransform(Vector4 translation, Quaternion rotation, Vector4 scale)
		{
			translation[3] = 0f;
			scale[3] = 1f;
			this.translation = translation;
			this.rotation = rotation;
			this.scale = scale;
		}

		public void FromTransform3D(Transform source, ObiRigidbody rb)
		{
			if (rb != null && rb.unityRigidbody != null)
			{
				translation = source.position - rb.unityRigidbody.transform.position + rb.position;
				rotation = source.rotation * Quaternion.Inverse(rb.unityRigidbody.transform.rotation) * rb.rotation;
			}
			else
			{
				translation = source.position;
				rotation = source.rotation;
			}
			scale = source.lossyScale;
		}

		public void FromTransform2D(Transform source, ObiRigidbody2D rb)
		{
			if (rb != null && rb.unityRigidbody != null)
			{
				translation = source.position - rb.unityRigidbody.transform.position + (Vector3)rb.position;
				rotation = source.rotation * Quaternion.Inverse(rb.unityRigidbody.transform.rotation) * Quaternion.AngleAxis(rb.rotation, Vector3.forward);
			}
			else
			{
				translation = source.position;
				rotation = source.rotation;
			}
			scale = source.lossyScale;
			translation[2] = 0f;
		}

		public AffineTransform Inverse()
		{
			Quaternion quaternion = Quaternion.Inverse(rotation);
			Vector3 vector = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
			return new AffineTransform(quaternion * Vector3.Scale(translation, -vector), quaternion, vector);
		}
	}
}
