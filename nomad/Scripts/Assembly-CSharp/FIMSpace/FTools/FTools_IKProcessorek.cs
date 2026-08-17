using System;
using UnityEngine;

namespace FIMSpace.FTools
{
	[Serializable]
	public class FTools_IKProcessorek
	{
		public enum FIK_ElbowMode
		{
			None = 0,
			Animation = 1,
			Target = 2,
			Parent = 3
		}

		[Serializable]
		public class FTools_IKProcessorBone
		{
			public Transform transform;

			public float BoneLength;

			public Vector3 Axis;

			public float MotionWeight = 1f;

			[SerializeField]
			private Quaternion targetToLocalSpace;

			[SerializeField]
			private Vector3 defaultLocalPoleNormal;

			public Quaternion initWorldRotation;

			[Range(0f, 180f)]
			public float angleLimit = 45f;

			[Range(0f, 180f)]
			public float twistAngleLimit = 180f;

			public Vector2 hingeLimits = Vector2.zero;

			public Quaternion initLocalRotation;

			public Quaternion previousHingeRotation;

			public float previousHingeAngle;

			public void Init(Vector3 childPosition, Vector3 orientationNormal)
			{
				Quaternion rotation = Quaternion.LookRotation(childPosition - transform.position, orientationNormal);
				targetToLocalSpace = RotationToLocal(transform.rotation, rotation);
				defaultLocalPoleNormal = Quaternion.Inverse(transform.rotation) * orientationNormal;
				BoneLength = (childPosition - transform.position).sqrMagnitude;
				initLocalRotation = transform.localRotation;
				initWorldRotation = transform.rotation;
			}

			public static Quaternion RotationToLocal(Quaternion parent, Quaternion rotation)
			{
				return Quaternion.Inverse(Quaternion.Inverse(parent) * rotation);
			}

			public Quaternion GetRotation(Vector3 direction, Vector3 orientationNormal)
			{
				return Quaternion.LookRotation(direction, orientationNormal) * targetToLocalSpace;
			}

			public Vector3 GetCurrentOrientationNormal()
			{
				return transform.rotation * defaultLocalPoleNormal;
			}

			public void AngleLimiting()
			{
				Quaternion quaternion = Quaternion.Inverse(initLocalRotation) * transform.localRotation;
				Quaternion quaternion2 = quaternion;
				if (hingeLimits.sqrMagnitude == 0f)
				{
					if (angleLimit < 180f)
					{
						quaternion2 = LimitPY(quaternion2);
					}
					if (twistAngleLimit < 180f)
					{
						quaternion2 = LimitRoll(quaternion2);
					}
				}
				else
				{
					quaternion2 = LimitHinge(quaternion2);
				}
				if (!object.Equals(quaternion2, quaternion))
				{
					transform.localRotation = initLocalRotation * quaternion2;
				}
			}

			private Quaternion LimitPY(Quaternion rotation)
			{
				if (object.Equals(rotation, Quaternion.identity))
				{
					return rotation;
				}
				Vector3 vector = rotation * Axis;
				Quaternion to = Quaternion.FromToRotation(Axis, vector);
				Quaternion quaternion = Quaternion.RotateTowards(Quaternion.identity, to, angleLimit);
				return Quaternion.FromToRotation(vector, quaternion * Axis) * rotation;
			}

			private Quaternion LimitRoll(Quaternion currentRotation)
			{
				Vector3 vector = new Vector3(Axis.y, Axis.z, Axis.x);
				Vector3 normal = currentRotation * Axis;
				Vector3 tangent = vector;
				Vector3.OrthoNormalize(ref normal, ref tangent);
				Vector3 tangent2 = currentRotation * vector;
				Vector3.OrthoNormalize(ref normal, ref tangent2);
				Quaternion quaternion = Quaternion.FromToRotation(tangent2, tangent) * currentRotation;
				if (twistAngleLimit <= 0f)
				{
					return quaternion;
				}
				return Quaternion.RotateTowards(quaternion, currentRotation, twistAngleLimit);
			}

			private Quaternion LimitHinge(Quaternion rotation)
			{
				Quaternion quaternion = Quaternion.FromToRotation(rotation * Axis, Axis) * rotation * Quaternion.Inverse(previousHingeRotation);
				float num = Quaternion.Angle(Quaternion.identity, quaternion);
				Vector3 vector = new Vector3(Axis.z, Axis.x, Axis.y);
				Vector3 rhs = Vector3.Cross(vector, Axis);
				if (Vector3.Dot(quaternion * vector, rhs) > 0f)
				{
					num = 0f - num;
				}
				previousHingeAngle = Mathf.Clamp(previousHingeAngle + num, hingeLimits.x, hingeLimits.y);
				previousHingeRotation = Quaternion.AngleAxis(previousHingeAngle, Axis);
				return previousHingeRotation;
			}
		}

		public Vector3 IKTargetPosition;

		public Quaternion IKTargetRotation;

		public Vector3 IKElbowTargetPosition = Vector3.zero;

		public FTools_IKProcessorBone[] IKBones;

		public bool Initialized;

		[Range(0f, 1f)]
		public float IKWeight = 1f;

		public Vector3 targetElbowNormal = Vector3.right;

		public bool LHand;

		public FIK_ElbowMode ElbowMode = FIK_ElbowMode.Target;

		public Quaternion frameEndBoneRotation;

		private float fullLength;

		[Range(1f, 12f)]
		public int CCD_ReactionQuality = 4;

		[Range(0f, 1f)]
		public float CCD_Smoothing;

		[Range(0f, 181f)]
		public float CCD_LimitAngle = 60f;

		public bool AutoWeight = true;

		public Vector3 LastLocalDirection;

		public Vector3 LocalDirection;

		private Quaternion initWorldRootRotation;

		private bool maintained;

		[Range(0f, 1f)]
		public float weight = 1f;

		private Quaternion startParentWorldRotation;

		public bool CCDIK { get; private set; }

		public FTools_IKProcessorBone StartBone => IKBones[0];

		public FTools_IKProcessorBone ElbowBone => IKBones[1];

		public FTools_IKProcessorBone EndBone
		{
			get
			{
				if (!CCDIK)
				{
					return IKBones[2];
				}
				return IKBones[IKBones.Length - 1];
			}
		}

		public void SetLimb(Transform startBone, Transform elbowBone, Transform endBone)
		{
			CCDIK = false;
			IKBones = new FTools_IKProcessorBone[3];
			IKBones[0] = new FTools_IKProcessorBone
			{
				transform = startBone
			};
			IKBones[1] = new FTools_IKProcessorBone
			{
				transform = elbowBone
			};
			IKBones[2] = new FTools_IKProcessorBone
			{
				transform = endBone
			};
			IKTargetPosition = endBone.position;
			IKTargetRotation = endBone.rotation;
		}

		public void SetCCD(Transform[] bonesChain)
		{
			CCDIK = true;
			IKBones = new FTools_IKProcessorBone[bonesChain.Length];
			for (int i = 0; i < bonesChain.Length; i++)
			{
				IKBones[i] = new FTools_IKProcessorBone
				{
					transform = bonesChain[i]
				};
			}
			IKTargetPosition = EndBone.transform.position;
			IKTargetRotation = EndBone.transform.rotation;
		}

		public void Initialize(Transform root)
		{
			if (Initialized)
			{
				return;
			}
			initWorldRootRotation = root.rotation;
			Vector3 vector = Vector3.Cross(ElbowBone.transform.position - StartBone.transform.position, EndBone.transform.position - ElbowBone.transform.position);
			if (vector != Vector3.zero)
			{
				targetElbowNormal = vector;
			}
			if (StartBone.transform.parent != null)
			{
				startParentWorldRotation = Quaternion.Inverse(initWorldRootRotation) * StartBone.transform.parent.rotation;
			}
			fullLength = 0f;
			if (!CCDIK)
			{
				StartBone.Init(ElbowBone.transform.position, targetElbowNormal);
				ElbowBone.Init(EndBone.transform.position, targetElbowNormal);
				EndBone.Init(EndBone.transform.position + (EndBone.transform.position - ElbowBone.transform.position), targetElbowNormal);
				fullLength = IKBones[0].BoneLength + IKBones[1].BoneLength;
				RefreshOrientationNormal();
			}
			else
			{
				float num = 1f / ((float)IKBones.Length * 1.3f);
				for (int i = 0; i < IKBones.Length; i++)
				{
					FTools_IKProcessorBone fTools_IKProcessorBone = IKBones[i];
					if (i < IKBones.Length - 1)
					{
						fTools_IKProcessorBone.Init(IKBones[i + 1].transform.position, targetElbowNormal);
						fullLength += fTools_IKProcessorBone.BoneLength;
						fTools_IKProcessorBone.Axis = Quaternion.Inverse(fTools_IKProcessorBone.transform.rotation) * (IKBones[i + 1].transform.position - fTools_IKProcessorBone.transform.position);
					}
					else
					{
						fTools_IKProcessorBone.Axis = Quaternion.Inverse(fTools_IKProcessorBone.transform.rotation) * (IKBones[IKBones.Length - 1].transform.position - IKBones[0].transform.position);
					}
					if (AutoWeight)
					{
						fTools_IKProcessorBone.MotionWeight = 1f - num * (float)i;
					}
				}
			}
			if (CCD_LimitAngle < 180f)
			{
				for (int j = 0; j < IKBones.Length; j++)
				{
					IKBones[j].angleLimit = CCD_LimitAngle;
					IKBones[j].twistAngleLimit = Mathf.Min(80f, CCD_LimitAngle);
				}
			}
			Initialized = true;
		}

		public void Update()
		{
			if (CCDIK)
			{
				UpdateCCDIK();
			}
			else
			{
				UpdateLimbIK();
			}
		}

		public void UpdateLimbIK()
		{
			if (Initialized)
			{
				frameEndBoneRotation = EndBone.transform.rotation;
				StartBone.BoneLength = (ElbowBone.transform.position - StartBone.transform.position).sqrMagnitude;
				ElbowBone.BoneLength = (EndBone.transform.position - ElbowBone.transform.position).sqrMagnitude;
				targetElbowNormal = GetOrientationNormal();
				Vector3 vector = GetOrientationDirection(IKTargetPosition, targetElbowNormal);
				if (vector == Vector3.zero)
				{
					vector = ElbowBone.transform.position - StartBone.transform.position;
				}
				StartBone.transform.rotation = StartBone.GetRotation(vector, targetElbowNormal);
				ElbowBone.transform.rotation = ElbowBone.GetRotation(IKTargetPosition - ElbowBone.transform.position, ElbowBone.GetCurrentOrientationNormal());
			}
		}

		public float GetStretchValue(Vector3 targetPos)
		{
			if (!CCDIK)
			{
				float epsilon = Mathf.Epsilon;
				epsilon += (StartBone.transform.position - ElbowBone.transform.position).magnitude;
				epsilon += (ElbowBone.transform.position - EndBone.transform.position).magnitude;
				return (StartBone.transform.position - targetPos).magnitude / epsilon;
			}
			float num = Mathf.Epsilon;
			for (int i = 0; i < IKBones.Length - 1; i++)
			{
				num += (IKBones[i].transform.position - IKBones[i + 1].transform.position).magnitude;
			}
			return (StartBone.transform.position - targetPos).magnitude / num;
		}

		private Vector3 GetOrientationNormal()
		{
			if (IKElbowTargetPosition.sqrMagnitude != 0f)
			{
				return CalculateElbowNormalToPosition(IKElbowTargetPosition);
			}
			return GetAutomaticElbowNormal();
		}

		public Vector3 CalculateElbowNormalToPosition(Vector3 targetElbowPos)
		{
			return Vector3.Cross(targetElbowPos - StartBone.transform.position, EndBone.transform.position - StartBone.transform.position);
		}

		public void RefreshOrientationNormal()
		{
			Vector3 vector = Vector3.Cross(ElbowBone.transform.position - StartBone.transform.position, EndBone.transform.position - ElbowBone.transform.position);
			if (vector != Vector3.zero)
			{
				targetElbowNormal = vector;
			}
		}

		private Vector3 GetOrientationDirection(Vector3 ikPosition, Vector3 orientationNormal)
		{
			Vector3 vector = ikPosition - StartBone.transform.position;
			if (vector == Vector3.zero)
			{
				return Vector3.zero;
			}
			float sqrMagnitude = vector.sqrMagnitude;
			float num = (sqrMagnitude + StartBone.BoneLength - ElbowBone.BoneLength) / 2f / Mathf.Sqrt(sqrMagnitude);
			float num2 = Mathf.Sqrt(StartBone.BoneLength - num * num);
			if (float.IsNaN(num2))
			{
				num2 = 0f;
			}
			Vector3 upwards = Vector3.Cross(vector, orientationNormal);
			return Quaternion.LookRotation(vector, upwards) * new Vector3(0f, num2, num);
		}

		private Vector3 GetAutomaticElbowNormal()
		{
			Vector3 currentOrientationNormal = StartBone.GetCurrentOrientationNormal();
			switch (ElbowMode)
			{
			case FIK_ElbowMode.Animation:
				if (!maintained)
				{
					targetElbowNormal = StartBone.GetCurrentOrientationNormal();
				}
				maintained = false;
				return Vector3.Lerp(currentOrientationNormal, targetElbowNormal, weight);
			case FIK_ElbowMode.Parent:
			{
				Quaternion quaternion = StartBone.transform.parent.rotation * Quaternion.Inverse(startParentWorldRotation);
				return Quaternion.Slerp(Quaternion.identity, quaternion * Quaternion.Inverse(initWorldRootRotation), weight) * currentOrientationNormal;
			}
			case FIK_ElbowMode.Target:
			{
				Quaternion b = IKTargetRotation * Quaternion.Inverse(EndBone.initLocalRotation);
				return Quaternion.Slerp(Quaternion.identity, b, weight) * currentOrientationNormal;
			}
			default:
				return currentOrientationNormal;
			}
		}

		public void UpdateCCDIK()
		{
			if (!Initialized)
			{
				return;
			}
			if (CCD_ReactionQuality < 0)
			{
				CCD_ReactionQuality = 1;
			}
			Vector3 vector = Vector3.zero;
			if (CCD_ReactionQuality > 1)
			{
				vector = GetGoalPivotOffset();
			}
			for (int i = 0; i < CCD_ReactionQuality && (i < 1 || vector.sqrMagnitude != 0f || !(CCD_Smoothing > 0f) || !(GetVelocityDifference() < CCD_Smoothing * CCD_Smoothing)); i++)
			{
				LastLocalDirection = RefreshLocalDirection();
				Vector3 vector2 = IKTargetPosition + vector;
				for (int num = IKBones.Length - 2; num > -1; num--)
				{
					float num2 = IKBones[num].MotionWeight * IKWeight;
					if (num2 > 0f)
					{
						Vector3 fromDirection = IKBones[IKBones.Length - 1].transform.position - IKBones[num].transform.position;
						Vector3 toDirection = vector2 - IKBones[num].transform.position;
						Quaternion quaternion = Quaternion.FromToRotation(fromDirection, toDirection) * IKBones[num].transform.rotation;
						if (num2 < 1f)
						{
							IKBones[num].transform.rotation = Quaternion.Lerp(IKBones[num].transform.rotation, quaternion, num2);
						}
						else
						{
							IKBones[num].transform.rotation = quaternion;
						}
					}
					IKBones[num].AngleLimiting();
				}
			}
			LastLocalDirection = RefreshLocalDirection();
		}

		protected Vector3 GetGoalPivotOffset()
		{
			if (!GoalPivotOffsetDetected())
			{
				return Vector3.zero;
			}
			Vector3 normalized = (IKTargetPosition - IKBones[0].transform.position).normalized;
			Vector3 rhs = new Vector3(normalized.y, normalized.z, normalized.x);
			if (CCD_LimitAngle > 0f && (IKBones[IKBones.Length - 2].angleLimit < 180f || IKBones[IKBones.Length - 2].twistAngleLimit < 180f))
			{
				rhs = IKBones[IKBones.Length - 2].transform.rotation * IKBones[IKBones.Length - 2].Axis;
			}
			return Vector3.Cross(normalized, rhs) * IKBones[IKBones.Length - 2].BoneLength * 0.5f;
		}

		private bool GoalPivotOffsetDetected()
		{
			if (!Initialized)
			{
				return false;
			}
			Vector3 vector = IKBones[IKBones.Length - 1].transform.position - IKBones[0].transform.position;
			Vector3 vector2 = IKTargetPosition - IKBones[0].transform.position;
			float magnitude = vector.magnitude;
			float magnitude2 = vector2.magnitude;
			if (magnitude2 == 0f)
			{
				return false;
			}
			if (magnitude == 0f)
			{
				return false;
			}
			if (magnitude < magnitude2)
			{
				return false;
			}
			if (magnitude < fullLength - IKBones[IKBones.Length - 2].BoneLength * 0.1f)
			{
				return false;
			}
			if (magnitude2 > magnitude)
			{
				return false;
			}
			if (Vector3.Dot(vector / magnitude, vector2 / magnitude2) < 0.999f)
			{
				return false;
			}
			return true;
		}

		private Vector3 RefreshLocalDirection()
		{
			LocalDirection = IKBones[0].transform.InverseTransformDirection(IKBones[IKBones.Length - 1].transform.position - IKBones[0].transform.position);
			return LocalDirection;
		}

		private float GetVelocityDifference()
		{
			return Vector3.SqrMagnitude(LocalDirection - LastLocalDirection);
		}
	}
}
