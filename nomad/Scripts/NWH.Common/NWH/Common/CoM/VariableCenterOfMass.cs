using System;
using NWH.Common.Utility;
using UnityEngine;

namespace NWH.Common.CoM
{
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-1000)]
	[RequireComponent(typeof(Rigidbody))]
	public class VariableCenterOfMass : MonoBehaviour
	{
		public bool useDefaultMass = true;

		public bool useMassAffectors;

		[Tooltip("Base mass of the object, without IMassAffectors.")]
		public float baseMass = 1400f;

		[Tooltip("Total mass of the object with masses of IMassAffectors counted in.")]
		public float combinedMass = 1400f;

		[Tooltip("Object dimensions in [m]. X - width, Y - height, Z - length.\r\nIt is important to set the correct dimensions or otherwise inertia might be calculated incorrectly.")]
		public Vector3 dimensions = new Vector3(1.8f, 1.6f, 4.6f);

		[Tooltip("When enabled the Unity-calculated center of mass will be used.")]
		public bool useDefaultCenterOfMass = true;

		[Tooltip("Center of mass of the rigidbody. Needs to be readjusted when new colliders are added.")]
		public Vector3 centerOfMass = Vector3.zero;

		public Vector3 combinedCenterOfMass = Vector3.zero;

		[Tooltip("When true inertia settings will be ignored and default Rigidbody inertia tensor will be used.")]
		public bool useDefaultInertia = true;

		[Tooltip("    Vector by which the inertia tensor of the rigidbody will be scaled on Start().\r\n    Due to the unform density of the rigidbodies, versus the very non-uniform density of a vehicle, inertia can feel\r\n    off.\r\n    Use this to adjust inertia tensor values.")]
		public Vector3 inertiaTensor = new Vector3(1000f, 1000f, 1000f);

		public Vector3 combinedInertiaTensor;

		[NonSerialized]
		public IMassAffector[] affectors;

		private Rigidbody _rigidbody;

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			_rigidbody = GetComponent<Rigidbody>();
			if (useDefaultMass)
			{
				baseMass = _rigidbody.mass;
			}
			if (useDefaultInertia)
			{
				inertiaTensor = _rigidbody.inertiaTensor;
			}
			if (useDefaultCenterOfMass)
			{
				centerOfMass = _rigidbody.centerOfMass;
			}
			affectors = GetMassAffectors();
			UpdateAllProperties();
		}

		private void OnValidate()
		{
			_rigidbody = GetComponent<Rigidbody>();
			affectors = GetMassAffectors();
		}

		private void FixedUpdate()
		{
			UpdateAllProperties();
		}

		public void UpdateAllProperties()
		{
			if (!useDefaultMass)
			{
				UpdateMass();
			}
			if (!useDefaultCenterOfMass)
			{
				UpdateCoM();
			}
			if (!useDefaultInertia)
			{
				UpdateInertia();
			}
		}

		public void UpdateMass()
		{
			if (useMassAffectors)
			{
				combinedMass = CalculateMass();
			}
			else
			{
				combinedMass = baseMass;
			}
			_rigidbody.mass = combinedMass;
		}

		public void UpdateCoM()
		{
			if (useMassAffectors)
			{
				combinedCenterOfMass = centerOfMass + CalculateRelativeCenterOfMassOffset();
			}
			else
			{
				combinedCenterOfMass = centerOfMass;
			}
			_rigidbody.centerOfMass = combinedCenterOfMass;
		}

		public void UpdateInertia(bool applyUnchanged = false)
		{
			if (useMassAffectors)
			{
				combinedInertiaTensor = inertiaTensor + CalculateInertiaTensorOffset(dimensions);
			}
			else
			{
				combinedInertiaTensor = inertiaTensor;
			}
			if (combinedInertiaTensor.x > 0f && combinedInertiaTensor.y > 0f && combinedInertiaTensor.z > 0f)
			{
				_rigidbody.inertiaTensor = combinedInertiaTensor;
				_rigidbody.inertiaTensorRotation = Quaternion.identity;
			}
		}

		public IMassAffector[] GetMassAffectors()
		{
			return GetComponentsInChildren<IMassAffector>(includeInactive: true);
		}

		public float CalculateMass()
		{
			float num = baseMass;
			IMassAffector[] array = affectors;
			foreach (IMassAffector massAffector in array)
			{
				if (massAffector.GetTransform().gameObject.activeInHierarchy)
				{
					num += massAffector.GetMass();
				}
			}
			return num;
		}

		public Vector3 CalculateRelativeCenterOfMassOffset()
		{
			Vector3 zero = Vector3.zero;
			if (useMassAffectors)
			{
				float num = CalculateMass();
				for (int i = 0; i < affectors.Length; i++)
				{
					zero += base.transform.InverseTransformPoint(affectors[i].GetWorldCenterOfMass()) * (affectors[i].GetMass() / num);
				}
			}
			return zero;
		}

		public Vector3 CalculateInertiaTensorOffset(Vector3 dimensions)
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < affectors.Length; i++)
			{
				IMassAffector massAffector = affectors[i];
				if (massAffector.GetTransform().gameObject.activeInHierarchy)
				{
					float mass = massAffector.GetMass();
					Vector3 vector = base.transform.InverseTransformPoint(massAffector.GetTransform().position);
					float num = Vector3.ProjectOnPlane(vector, Vector3.right).magnitude * mass;
					float num2 = Vector3.ProjectOnPlane(vector, Vector3.up).magnitude * mass;
					float num3 = Vector3.ProjectOnPlane(vector, Vector3.forward).magnitude * mass;
					zero.x += num * num;
					zero.y += num2 * num2;
					zero.z += num3 * num3;
				}
			}
			return zero;
		}

		public static Vector3 CalculateInertia(Vector3 dimensions, float mass)
		{
			float num = 1f / 12f * mass;
			float x = num * (dimensions.y * dimensions.y + dimensions.z * dimensions.z);
			float y = num * (dimensions.x * dimensions.x + dimensions.z * dimensions.z);
			float z = num * (dimensions.y * dimensions.y + dimensions.x * dimensions.x);
			return new Vector3(x, y, z);
		}

		private void OnDrawGizmos()
		{
		}

		private void Reset()
		{
			_rigidbody = GetComponent<Rigidbody>();
			Bounds bounds = base.gameObject.FindBoundsIncludeChildren();
			dimensions = new Vector3(bounds.extents.x * 2f, bounds.extents.y * 2f, bounds.extents.z * 2f);
			Debug.Log($"Detected dimensions of {base.name} as {dimensions} [m]. If incorrect, adjust manually.");
			if (dimensions.x < 1E-05f)
			{
				dimensions.x = 1E-05f;
			}
			if (dimensions.y < 1E-05f)
			{
				dimensions.y = 1E-05f;
			}
			if (dimensions.z < 1E-05f)
			{
				dimensions.z = 1E-05f;
			}
			centerOfMass = _rigidbody.centerOfMass;
			baseMass = _rigidbody.mass;
			combinedMass = baseMass;
			inertiaTensor = _rigidbody.inertiaTensor;
		}

		public Vector3 GetWorldCenterOfMass()
		{
			return base.transform.TransformPoint(combinedCenterOfMass);
		}
	}
}
