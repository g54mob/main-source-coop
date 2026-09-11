using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone Collider")]
public class DynamicBoneCollider : DynamicBoneColliderBase
{
	[Tooltip("The radius of the sphere or capsule.")]
	public float m_Radius = 0.5f;

	[Tooltip("The height of the capsule.")]
	public float m_Height;

	private void OnValidate()
	{
		m_Radius = Mathf.Max(m_Radius, 0f);
		m_Height = Mathf.Max(m_Height, 0f);
	}

	public override bool Collide(ref Vector3 particlePosition, float particleRadius)
	{
		float num = m_Radius * Mathf.Abs(base.transform.lossyScale.x);
		float num2 = m_Height * 0.5f - m_Radius;
		if (num2 <= 0f)
		{
			if (m_Bound == Bound.Outside)
			{
				return OutsideSphere(ref particlePosition, particleRadius, base.transform.TransformPoint(m_Center), num);
			}
			return InsideSphere(ref particlePosition, particleRadius, base.transform.TransformPoint(m_Center), num);
		}
		Vector3 center = m_Center;
		Vector3 center2 = m_Center;
		switch (m_Direction)
		{
		case Direction.X:
			center.x -= num2;
			center2.x += num2;
			break;
		case Direction.Y:
			center.y -= num2;
			center2.y += num2;
			break;
		case Direction.Z:
			center.z -= num2;
			center2.z += num2;
			break;
		}
		if (m_Bound == Bound.Outside)
		{
			return OutsideCapsule(ref particlePosition, particleRadius, base.transform.TransformPoint(center), base.transform.TransformPoint(center2), num);
		}
		return InsideCapsule(ref particlePosition, particleRadius, base.transform.TransformPoint(center), base.transform.TransformPoint(center2), num);
	}

	private static bool OutsideSphere(ref Vector3 particlePosition, float particleRadius, Vector3 sphereCenter, float sphereRadius)
	{
		float num = sphereRadius + particleRadius;
		float num2 = num * num;
		Vector3 vector = particlePosition - sphereCenter;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude > 0f && sqrMagnitude < num2)
		{
			float num3 = Mathf.Sqrt(sqrMagnitude);
			particlePosition = sphereCenter + vector * (num / num3);
			return true;
		}
		return false;
	}

	private static bool InsideSphere(ref Vector3 particlePosition, float particleRadius, Vector3 sphereCenter, float sphereRadius)
	{
		float num = sphereRadius - particleRadius;
		float num2 = num * num;
		Vector3 vector = particlePosition - sphereCenter;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude > num2)
		{
			float num3 = Mathf.Sqrt(sqrMagnitude);
			particlePosition = sphereCenter + vector * (num / num3);
			return true;
		}
		return false;
	}

	private static bool OutsideCapsule(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius)
	{
		float num = capsuleRadius + particleRadius;
		Vector3 vector = capsuleP1 - capsuleP0;
		Vector3 vector2 = particlePosition - capsuleP0;
		float num2 = Vector3.Dot(vector2, vector);
		if (num2 <= 0f)
		{
			float magnitude = vector2.magnitude;
			if (magnitude > 0f && magnitude < num)
			{
				particlePosition = capsuleP0 + vector2 * (num / magnitude);
				return true;
			}
		}
		else
		{
			float magnitude2 = vector.magnitude;
			if (num2 >= magnitude2)
			{
				vector2 = particlePosition - capsuleP1;
				float magnitude3 = vector2.magnitude;
				if (magnitude3 > 0f && magnitude3 < num)
				{
					particlePosition = capsuleP1 + vector2 * (num / magnitude3);
					return true;
				}
			}
			else if (magnitude2 > 0f)
			{
				num2 /= magnitude2;
				vector2 -= vector * num2;
				float magnitude4 = vector2.magnitude;
				if (magnitude4 > 0f && magnitude4 < num)
				{
					particlePosition += vector2 * ((num - magnitude4) / magnitude4);
					return true;
				}
			}
		}
		return false;
	}

	private static bool InsideCapsule(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius)
	{
		float num = capsuleRadius - particleRadius;
		Vector3 vector = capsuleP1 - capsuleP0;
		Vector3 vector2 = particlePosition - capsuleP0;
		float num2 = Vector3.Dot(vector2, vector);
		if (num2 <= 0f)
		{
			float magnitude = vector2.magnitude;
			if (magnitude > num)
			{
				particlePosition = capsuleP0 + vector2 * (num / magnitude);
				return true;
			}
		}
		else
		{
			float sqrMagnitude = vector.sqrMagnitude;
			if (num2 >= sqrMagnitude)
			{
				vector2 = particlePosition - capsuleP1;
				float magnitude2 = vector2.magnitude;
				if (magnitude2 > num)
				{
					particlePosition = capsuleP1 + vector2 * (num / magnitude2);
					return true;
				}
			}
			else if (sqrMagnitude > 0f)
			{
				num2 /= sqrMagnitude;
				vector2 -= vector * num2;
				float magnitude3 = vector2.magnitude;
				if (magnitude3 > num)
				{
					particlePosition += vector2 * ((num - magnitude3) / magnitude3);
					return true;
				}
			}
		}
		return false;
	}

	private void OnDrawGizmosSelected()
	{
		if (!base.enabled)
		{
			return;
		}
		if (m_Bound == Bound.Outside)
		{
			Gizmos.color = Color.yellow;
		}
		else
		{
			Gizmos.color = Color.magenta;
		}
		float radius = m_Radius * Mathf.Abs(base.transform.lossyScale.x);
		float num = m_Height * 0.5f - m_Radius;
		if (num <= 0f)
		{
			Gizmos.DrawWireSphere(base.transform.TransformPoint(m_Center), radius);
			return;
		}
		Vector3 center = m_Center;
		Vector3 center2 = m_Center;
		switch (m_Direction)
		{
		case Direction.X:
			center.x -= num;
			center2.x += num;
			break;
		case Direction.Y:
			center.y -= num;
			center2.y += num;
			break;
		case Direction.Z:
			center.z -= num;
			center2.z += num;
			break;
		}
		Gizmos.DrawWireSphere(base.transform.TransformPoint(center), radius);
		Gizmos.DrawWireSphere(base.transform.TransformPoint(center2), radius);
	}
}
