using System;
using FIMSpace.AnimationTools;
using UnityEngine;

namespace FIMSpace.FTools
{
	[Serializable]
	public class FIK_IKProcessor : FIK_ProcessorBase
	{
		public enum FIK_HintMode
		{
			Default = 0,
			MiddleForward = 1,
			MiddleBack = 2,
			OnGoal = 3,
			EndForward = 4,
			Cross = 5
		}

		[Serializable]
		public class IKBone : FIK_IKBoneBase
		{
			[SerializeField]
			private Quaternion targetToLocalSpace;

			[SerializeField]
			private Vector3 defaultLocalPoleNormal;

			public Vector3 right;

			public Vector3 up;

			public Vector3 forward;

			public Vector3 srcPosition;

			public Quaternion srcRotation;

			private bool ensured;

			private Quaternion pre = Quaternion.identity;

			public IKBone(Transform t)
				: base(t)
			{
			}

			public void Init(Transform root, Vector3 childPosition, Vector3 orientationNormal, bool ensured)
			{
				RefreshOrientations(childPosition, orientationNormal);
				sqrMagn = (childPosition - base.transform.position).sqrMagnitude;
				LastKeyLocalRotation = base.transform.localRotation;
				right = base.transform.InverseTransformDirection(root.right);
				up = base.transform.InverseTransformDirection(root.up);
				forward = base.transform.InverseTransformDirection(root.forward);
				CaptureSourceAnimation();
			}

			public void RefreshOrientations(Vector3 childPosition, Vector3 orientationNormal)
			{
				Quaternion quaternion = Quaternion.LookRotation(childPosition - base.transform.position, orientationNormal);
				if (ensured)
				{
					quaternion = (pre = AnimationGenerateUtils.EnsureQuaternionContinuity(pre, quaternion));
				}
				targetToLocalSpace = RotationToLocal(base.transform.rotation, quaternion);
				defaultLocalPoleNormal = Quaternion.Inverse(base.transform.rotation) * orientationNormal;
			}

			public void CaptureSourceAnimation()
			{
				srcPosition = base.transform.position;
				srcRotation = base.transform.rotation;
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
				return base.transform.rotation * defaultLocalPoleNormal;
			}
		}

		[Space(4f)]
		[SerializeField]
		private IKBone[] IKBones;

		[Space(4f)]
		[Range(0f, 1f)]
		public float PositionWeight = 1f;

		[Range(0f, 1f)]
		public float RotationWeight = 1f;

		[HideInInspector]
		public bool UseEnsuredRotation;

		public FIK_HintMode AutoHintMode = FIK_HintMode.MiddleForward;

		[Range(0f, 1f)]
		public float ManualHintPositionWeight;

		public Vector3 IKManualHintPosition = Vector3.zero;

		private Transform rootTransform;

		private bool everyIsChild;

		private Vector3 targetElbowNormal = Vector3.right;

		private Quaternion lastEndBoneRotation;

		private Quaternion postIKAnimatorEndBoneRot;

		private Quaternion preS = Quaternion.identity;

		private Quaternion preM = Quaternion.identity;

		private Quaternion preE = Quaternion.identity;

		private float limbLengthRootScale;

		private float limbLength;

		private float limbMidLength;

		[NonSerialized]
		public bool AllowEditModeInit;

		public IKBone StartIKBone => IKBones[0];

		public IKBone MiddleIKBone => IKBones[1];

		public IKBone EndIKBone => IKBones[2];

		public int BonesCount => IKBones.Length;

		public float ScaleReference { get; protected set; }

		public IKBone GetBone(int index)
		{
			return IKBones[index];
		}

		public FIK_IKProcessor(Transform startBone, Transform midBone, Transform endBone)
		{
			SetBones(startBone, midBone, endBone);
			IKTargetPosition = endBone.position;
			IKTargetRotation = endBone.rotation;
		}

		public void SetBones(Transform startBone, Transform midBone, Transform endBone)
		{
			IKBones = new IKBone[3];
			IKBones[0] = new IKBone(startBone);
			IKBones[1] = new IKBone(midBone);
			IKBones[2] = new IKBone(endBone);
			base.Bones = new FIK_IKBoneBase[3]
			{
				IKBones[0],
				IKBones[1],
				IKBones[2]
			};
			IKBones[0].SetChild(IKBones[1]);
			IKBones[1].SetChild(IKBones[2]);
		}

		public void SetBones(Transform startBone, Transform endBone)
		{
			SetBones(startBone, endBone.parent, endBone);
		}

		public override void PreCalibrate()
		{
			base.PreCalibrate();
			RefreshScaleReference();
		}

		public void RefreshScaleReference()
		{
			ScaleReference = (base.StartBone.transform.position - MiddleIKBone.transform.position).magnitude;
		}

		public override void Init(Transform root)
		{
			if (!base.Initialized)
			{
				rootTransform = root;
				Vector3 vector = Vector3.Cross(MiddleIKBone.transform.position - base.StartBone.transform.position, base.EndBone.transform.position - MiddleIKBone.transform.position);
				if (vector != Vector3.zero)
				{
					targetElbowNormal = vector;
				}
				base.fullLength = 0f;
				StartIKBone.Init(root, MiddleIKBone.transform.position, targetElbowNormal, UseEnsuredRotation);
				MiddleIKBone.Init(root, base.EndBone.transform.position, targetElbowNormal, UseEnsuredRotation);
				EndIKBone.Init(root, base.EndBone.transform.position + (base.EndBone.transform.position - MiddleIKBone.transform.position), targetElbowNormal, UseEnsuredRotation);
				base.fullLength = base.Bones[0].BoneLength + base.Bones[1].BoneLength;
				RefreshOrientationNormal();
				limbLengthRootScale = root.lossyScale.x;
				limbLength = Vector3.Distance(StartIKBone.transform.position, MiddleIKBone.transform.position);
				limbLength += Vector3.Distance(EndIKBone.transform.position, MiddleIKBone.transform.position);
				if (base.EndBone.transform.parent != MiddleIKBone.transform)
				{
					everyIsChild = false;
					limbMidLength = Vector3.Distance(EndIKBone.transform.position, MiddleIKBone.transform.position);
				}
				else if (MiddleIKBone.transform.parent != base.StartBone.transform)
				{
					everyIsChild = false;
				}
				else
				{
					everyIsChild = true;
				}
				if (AllowEditModeInit)
				{
					base.Initialized = true;
				}
				else if (Application.isPlaying)
				{
					base.Initialized = true;
				}
			}
		}

		public void RefreshAnimatorCoords()
		{
			StartIKBone.CaptureSourceAnimation();
			MiddleIKBone.CaptureSourceAnimation();
			EndIKBone.CaptureSourceAnimation();
		}

		public override void Update()
		{
			if (!base.Initialized)
			{
				return;
			}
			RefreshAnimatorCoords();
			if (!everyIsChild)
			{
				MiddleIKBone.RefreshOrientations(base.EndBone.transform.position, targetElbowNormal);
			}
			float num = PositionWeight * IKWeight;
			base.StartBone.sqrMagn = (MiddleIKBone.transform.position - base.StartBone.transform.position).sqrMagnitude;
			MiddleIKBone.sqrMagn = (base.EndBone.transform.position - MiddleIKBone.transform.position).sqrMagnitude;
			targetElbowNormal = GetOrientationNormal();
			Vector3 vector = GetOrientationDirection(IKTargetPosition, targetElbowNormal);
			if (vector == Vector3.zero)
			{
				vector = MiddleIKBone.transform.position - base.StartBone.transform.position;
			}
			if (num > 0f)
			{
				Quaternion quaternion = StartIKBone.GetRotation(vector, targetElbowNormal) * base.StartBoneRotationOffset;
				if (num < 1f)
				{
					quaternion = Quaternion.LerpUnclamped(StartIKBone.srcRotation, quaternion, num);
				}
				base.StartBone.transform.rotation = quaternion;
				if (UseEnsuredRotation)
				{
					base.StartBone.transform.rotation = AnimationGenerateUtils.EnsureQuaternionContinuity(preS, base.StartBone.transform.rotation);
					preS = base.StartBone.transform.rotation;
				}
				Quaternion quaternion2 = MiddleIKBone.GetRotation(IKTargetPosition - MiddleIKBone.transform.position, MiddleIKBone.GetCurrentOrientationNormal());
				if (num < 1f)
				{
					quaternion2 = Quaternion.LerpUnclamped(MiddleIKBone.srcRotation, quaternion2, num);
				}
				MiddleIKBone.transform.rotation = quaternion2;
				if (UseEnsuredRotation)
				{
					MiddleIKBone.transform.rotation = AnimationGenerateUtils.EnsureQuaternionContinuity(preM, MiddleIKBone.transform.rotation);
					preM = MiddleIKBone.transform.rotation;
				}
			}
			postIKAnimatorEndBoneRot = base.EndBone.transform.rotation;
			float num2 = RotationWeight * IKWeight;
			if (num2 > 0f)
			{
				if (num2 < 1f)
				{
					base.EndBone.transform.rotation = Quaternion.LerpUnclamped(postIKAnimatorEndBoneRot, IKTargetRotation, num2);
				}
				else
				{
					base.EndBone.transform.rotation = IKTargetRotation;
				}
				if (UseEnsuredRotation)
				{
					base.EndBone.transform.rotation = AnimationGenerateUtils.EnsureQuaternionContinuity(preE, base.EndBone.transform.rotation);
					preE = base.EndBone.transform.rotation;
				}
			}
			lastEndBoneRotation = base.EndBone.transform.rotation;
		}

		public float GetLimbLength()
		{
			if (rootTransform.lossyScale.x == 0f)
			{
				return 0f;
			}
			float num = rootTransform.lossyScale.x / limbLengthRootScale;
			if (!everyIsChild)
			{
				float num2 = limbMidLength * num - Vector3.Distance(EndIKBone.srcPosition, MiddleIKBone.srcPosition);
				return limbLength * num - num2;
			}
			return limbLength * num;
		}

		public Vector3 GetHintDefaultPosition()
		{
			return MiddleIKBone.srcPosition + MiddleIKBone.srcRotation * StartIKBone.GetCurrentOrientationNormal();
		}

		public float GetStretchValue(Vector3 targetPos)
		{
			return GetStretchValue((StartIKBone.srcPosition - targetPos).magnitude);
		}

		public float GetStretchValue(float distance)
		{
			return distance / GetLimbLength();
		}

		private Vector3 GetOrientationNormal()
		{
			if (ManualHintPositionWeight > 0f)
			{
				if (ManualHintPositionWeight >= 1f)
				{
					return CalculateElbowNormalToPosition(IKManualHintPosition);
				}
				return Vector3.LerpUnclamped(GetAutomaticElbowNormal().normalized, CalculateElbowNormalToPosition(IKManualHintPosition), ManualHintPositionWeight);
			}
			return GetAutomaticElbowNormal();
		}

		public Vector3 CalculateElbowNormalToPosition(Vector3 targetElbowPos)
		{
			return Vector3.Cross(targetElbowPos - base.StartBone.transform.position, base.EndBone.transform.position - base.StartBone.transform.position);
		}

		public void RefreshOrientationNormal()
		{
			Vector3 vector = Vector3.Cross(MiddleIKBone.transform.position - base.StartBone.transform.position, base.EndBone.transform.position - MiddleIKBone.transform.position);
			if (vector != Vector3.zero)
			{
				targetElbowNormal = vector;
			}
		}

		private Vector3 GetOrientationDirection(Vector3 ikPosition, Vector3 orientationNormal)
		{
			Vector3 vector = ikPosition - base.StartBone.transform.position;
			if (vector == Vector3.zero)
			{
				return Vector3.zero;
			}
			float sqrMagnitude = vector.sqrMagnitude;
			float num = Mathf.Sqrt(sqrMagnitude);
			float num2 = (sqrMagnitude + base.StartBone.sqrMagn - MiddleIKBone.sqrMagn) / 2f / num;
			float y = Mathf.Sqrt(Mathf.Clamp(base.StartBone.sqrMagn - num2 * num2, 0f, 1f / 0f));
			Vector3 upwards = Vector3.Cross(vector / num, orientationNormal);
			return Quaternion.LookRotation(vector, upwards) * new Vector3(0f, y, num2);
		}

		private Vector3 GetAutomaticElbowNormal()
		{
			Vector3 currentOrientationNormal = StartIKBone.GetCurrentOrientationNormal();
			switch (AutoHintMode)
			{
			case FIK_HintMode.MiddleForward:
				return Vector3.LerpUnclamped(currentOrientationNormal.normalized, MiddleIKBone.srcRotation * MiddleIKBone.right, 0.5f);
			case FIK_HintMode.MiddleBack:
				return MiddleIKBone.srcRotation * -MiddleIKBone.right;
			case FIK_HintMode.EndForward:
			{
				Vector3 vector = Vector3.Cross(MiddleIKBone.srcPosition + EndIKBone.srcRotation * EndIKBone.forward - StartIKBone.srcPosition, IKTargetPosition - StartIKBone.srcPosition);
				if (vector == Vector3.zero)
				{
					return currentOrientationNormal;
				}
				return vector;
			}
			case FIK_HintMode.OnGoal:
				return lastEndBoneRotation * EndIKBone.right;
			case FIK_HintMode.Cross:
				return Vector3.Cross(MiddleIKBone.srcPosition - StartIKBone.srcPosition, EndIKBone.srcPosition - MiddleIKBone.srcPosition);
			default:
				return currentOrientationNormal;
			}
		}

		public void OnDrawGizmos()
		{
			_ = base.Initialized;
		}
	}
}
