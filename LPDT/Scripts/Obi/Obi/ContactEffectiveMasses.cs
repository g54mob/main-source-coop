using Unity.Mathematics;

namespace Obi
{
	public struct ContactEffectiveMasses
	{
		public float normalInvMassA;

		public float tangentInvMassA;

		public float bitangentInvMassA;

		public float normalInvMassB;

		public float tangentInvMassB;

		public float bitangentInvMassB;

		public float TotalNormalInvMass => normalInvMassA + normalInvMassB;

		public float TotalTangentInvMass => tangentInvMassA + tangentInvMassB;

		public float TotalBitangentInvMass => bitangentInvMassA + bitangentInvMassB;

		public void ClearContactMassesA()
		{
			normalInvMassA = (tangentInvMassA = (bitangentInvMassA = 0f));
		}

		public void ClearContactMassesB()
		{
			normalInvMassB = (tangentInvMassB = (bitangentInvMassB = 0f));
		}

		public void CalculateContactMassesA(float invMass, float4 inverseInertiaTensor, float4 position, quaternion orientation, float4 contactPoint, float4 normal, float4 tangent, float4 bitangent, bool rollingContacts)
		{
			normalInvMassA = (tangentInvMassA = (bitangentInvMassA = invMass));
			if (rollingContacts)
			{
				float4 point = contactPoint - position;
				float4x4 inverseInertiaTensor2 = BurstMath.TransformInertiaTensor(inverseInertiaTensor, orientation);
				normalInvMassA += BurstMath.RotationalInvMass(inverseInertiaTensor2, point, normal);
				tangentInvMassA += BurstMath.RotationalInvMass(inverseInertiaTensor2, point, tangent);
				bitangentInvMassA += BurstMath.RotationalInvMass(inverseInertiaTensor2, point, bitangent);
			}
		}

		public void CalculateContactMassesB(float invMass, float4 inverseInertiaTensor, float4 position, quaternion orientation, float4 contactPoint, float4 normal, float4 tangent, float4 bitangent, bool rollingContacts)
		{
			normalInvMassB = (tangentInvMassB = (bitangentInvMassB = invMass));
			if (rollingContacts)
			{
				float4 point = contactPoint - position;
				float4x4 inverseInertiaTensor2 = BurstMath.TransformInertiaTensor(inverseInertiaTensor, orientation);
				normalInvMassB += BurstMath.RotationalInvMass(inverseInertiaTensor2, point, normal);
				tangentInvMassB += BurstMath.RotationalInvMass(inverseInertiaTensor2, point, tangent);
				bitangentInvMassB += BurstMath.RotationalInvMass(inverseInertiaTensor2, point, bitangent);
			}
		}

		public void CalculateContactMassesB(in BurstRigidbody rigidbody, in BurstAffineTransform solver2World, float4 pointB, float4 normal, float4 tangent, float4 bitangent)
		{
			float4 point = solver2World.TransformPoint(pointB) - rigidbody.com;
			normalInvMassB = (tangentInvMassB = (bitangentInvMassB = rigidbody.inverseMass));
			normalInvMassB += BurstMath.RotationalInvMass(rigidbody.inverseInertiaTensor, point, normal);
			tangentInvMassB += BurstMath.RotationalInvMass(rigidbody.inverseInertiaTensor, point, tangent);
			bitangentInvMassB += BurstMath.RotationalInvMass(rigidbody.inverseInertiaTensor, point, bitangent);
		}
	}
}
