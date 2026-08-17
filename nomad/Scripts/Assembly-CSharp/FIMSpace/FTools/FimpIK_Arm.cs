using System;
using UnityEngine;

namespace FIMSpace.FTools
{
	[Serializable]
	public class FimpIK_Arm
	{
		[Serializable]
		public class IKBone
		{
			public float sqrMagn = 0.1f;

			public float BoneLength = 0.1f;

			public float MotionWeight = 1f;

			public Vector3 InitialLocalPosition;

			public Quaternion InitialLocalRotation;

			public Quaternion LastKeyLocalRotation;

			[SerializeField]
			private Quaternion targetToLocalSpace;

			[SerializeField]
			private Vector3 defaultLocalPoleNormal;

			public IKBone Child { get; private set; }

			public Transform transform { get; protected set; }

			public Vector3 right { get; private set; }

			public Vector3 up { get; private set; }

			public Vector3 forward { get; private set; }

			public Vector3 srcPosition { get; private set; }

			public Quaternion srcRotation { get; private set; }

			public Vector3 GetDefaultPoleNormal()
			{
				return defaultLocalPoleNormal;
			}

			public IKBone(Transform t)
			{
				if (!(t == null))
				{
					transform = t;
					InitialLocalPosition = transform.localPosition;
					InitialLocalRotation = transform.localRotation;
					LastKeyLocalRotation = t.localRotation;
				}
			}

			public virtual void SetChild(IKBone child)
			{
				if (!(child.transform == null))
				{
					Child = child;
					sqrMagn = (child.transform.position - transform.position).sqrMagnitude;
					BoneLength = (child.transform.position - transform.position).magnitude;
				}
			}

			public Vector3 Dir(Vector3 local)
			{
				return transform.TransformDirection(local);
			}

			public void Init(Transform root, Vector3 childPosition, Vector3 orientationNormal)
			{
				RefreshOrientations(childPosition, orientationNormal);
				sqrMagn = (childPosition - transform.position).sqrMagnitude;
				LastKeyLocalRotation = transform.localRotation;
				right = transform.InverseTransformDirection(root.right);
				up = transform.InverseTransformDirection(root.up);
				forward = transform.InverseTransformDirection(root.forward);
				CaptureSourceAnimation();
			}

			public void RefreshOrientations(Vector3 childPosition, Vector3 orientationNormal)
			{
				if (!(transform == null))
				{
					Vector3 vector = childPosition - transform.position;
					targetToLocalSpace = RotationToLocal(rotation: (!(vector == Vector3.zero)) ? Quaternion.LookRotation(vector, orientationNormal) : Quaternion.identity, parent: transform.rotation);
					defaultLocalPoleNormal = Quaternion.Inverse(transform.rotation) * orientationNormal;
				}
			}

			public void CaptureSourceAnimation()
			{
				srcPosition = transform.position;
				srcRotation = transform.rotation;
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
		}

		public enum FIK_HintMode
		{
			Default = 0,
			MiddleForward = 1,
			MiddleBack = 2,
			OnGoal = 3,
			EndForward = 4
		}

		private Vector3 lastIKBasePosition;

		private Quaternion lastIKBaseRotation;

		public Vector3 IKTargetPosition;

		public Quaternion IKTargetRotation;

		private float sd_ikBlend;

		private Vector3 sd_ikTargetPosition = Vector3.zero;

		[NonSerialized]
		public Vector3 HandMiddleOffset;

		private Vector3 shoulderForward;

		private UniRotateBone shoulderRotate;

		private Vector3 initHandRootSpaceFlatTowards;

		internal bool UseRotationMapping = true;

		[NonSerialized]
		public float MaxStretching = 1.2f;

		public IKBone ChestIKBone;

		private bool everyIsChild;

		[Range(0f, 1f)]
		public float ManualHintPositionWeight;

		[HideInInspector]
		public Vector3 IKManualHintPosition = Vector3.zero;

		private float sd_targetIKRotation;

		private float sd_positionWeight;

		[NonSerialized]
		public float _internalIKWeight = 1f;

		[Range(0f, 1f)]
		public float IKWeight = 1f;

		[Tooltip("Blend value for goal position")]
		[Space(4f)]
		[Range(0f, 1f)]
		public float IKPositionWeight = 1f;

		[Tooltip("Blend value hand rotation")]
		[Range(0f, 1f)]
		public float HandRotationWeight = 1f;

		[Tooltip("Blend value for shoulder rotation")]
		[Range(0f, 1f)]
		public float ShoulderBlend = 1f;

		[Tooltip("Flex style algorithm for different limbs")]
		public FIK_HintMode AutoHintMode = FIK_HintMode.MiddleForward;

		[Tooltip("If left limb behaves wrong in comparison to right one")]
		public bool MirrorMaths;

		[FPD_Header("Bones References", 6f, 4f, 2)]
		public Transform ShoulderTransform;

		public Transform UpperarmTransform;

		public Transform LowerarmTransform;

		public Transform HandTransform;

		[SerializeField]
		[HideInInspector]
		private IKBone[] IKBones;

		[NonSerialized]
		public Vector3 ikCustomHintOffset = Vector3.zero;

		public float FullLength { get; protected set; }

		public bool Initialized { get; set; }

		public Quaternion HandIKBoneMapping { get; protected set; }

		public float ScaleReference { get; protected set; }

		public bool PreventShoulderThirdQuat { get; set; } = true;

		public float ShoulderSensitivity { get; set; } = 0.75f;

		public float PreventShoulderThirdQuatFactor { get; set; } = 0.01f;

		public float limbLength { get; private set; } = 0.1f;

		public Transform Root { get; protected set; }

		public IKBone ShoulderIKBone => IKBones[0];

		public IKBone UpperArmIKBone => IKBones[1];

		public IKBone ForeArmIKBone => IKBones[2];

		public IKBone HandIKBone => IKBones[3];

		public int BonesCount => IKBones.Length;

		public bool IsCorrect
		{
			get
			{
				if (!Initialized)
				{
					return false;
				}
				if (UpperarmTransform == null)
				{
					return false;
				}
				if (LowerarmTransform == null)
				{
					return false;
				}
				if (shoulderRotate == null)
				{
					return false;
				}
				if (shoulderRotate.transform == null)
				{
					return false;
				}
				return true;
			}
		}

		public Vector3 TargetElbowNormal { get; private set; }

		public Quaternion UpperarmRotationOffset { get; set; }

		public void PreCalibrate(float blend = 1f)
		{
			IKBone iKBone = IKBones[0];
			if (blend >= 1f)
			{
				while (iKBone != null)
				{
					iKBone.transform.localRotation = iKBone.InitialLocalRotation;
					iKBone = iKBone.Child;
				}
			}
			else
			{
				while (iKBone != null)
				{
					iKBone.transform.localRotation = Quaternion.LerpUnclamped(iKBone.transform.localRotation, iKBone.InitialLocalRotation, blend);
					iKBone = iKBone.Child;
				}
			}
			RefreshScaleReference();
		}

		public void User_SmoothIKBlend(float target, float duration, float delta, float maxSpeed = 1000f)
		{
			IKWeight = Mathf.SmoothDamp(IKWeight, target, ref sd_ikBlend, duration, maxSpeed, delta);
		}

		public void User_SmoothPositionTowards(Vector3 newIKPos, float duration, float delta, float maxSpeed = 1000f)
		{
			IKTargetPosition = Vector3.SmoothDamp(IKTargetPosition, newIKPos, ref sd_ikTargetPosition, duration, maxSpeed, delta);
		}

		public Vector3 GetHintDefaultPosition()
		{
			return ForeArmIKBone.transform.position + ForeArmIKBone.transform.rotation * UpperArmIKBone.GetDefaultPoleNormal();
		}

		public void SetCustomIKRotationMappingOffset(Quaternion mappingCorrection)
		{
			HandIKBoneMapping = mappingCorrection;
		}

		public virtual void SetRootReference(Transform mainParentTransform)
		{
			Root = mainParentTransform;
			Quaternion rotation = Root.transform.rotation;
			Root.transform.rotation = Quaternion.identity;
			Vector3 normalized = (HandIKBone.transform.position - ForeArmIKBone.transform.position).normalized;
			Vector3 lhs = HandIKBone.transform.InverseTransformDirection(normalized);
			Vector3 forward = mainParentTransform.forward;
			Vector3 fromDirection = Vector3.Cross(lhs, forward);
			Vector3 normalized2 = (ShoulderIKBone.transform.position - ShoulderIKBone.transform.parent.position).normalized;
			shoulderForward = ShoulderIKBone.transform.InverseTransformDirection(normalized2);
			HandIKBoneMapping = Quaternion.FromToRotation(forward, Vector3.right);
			HandIKBoneMapping *= Quaternion.FromToRotation(fromDirection, Vector3.up);
			shoulderRotate = new UniRotateBone(ShoulderTransform, mainParentTransform);
			Root.transform.rotation = rotation;
			initHandRootSpaceFlatTowards = Root.InverseTransformPoint(HandTransform.position);
			initHandRootSpaceFlatTowards.y = 0f;
			initHandRootSpaceFlatTowards.Normalize();
		}

		public void SetCustomIKRotation(Quaternion rotation, float blend = 1f, bool fromDefault = false)
		{
			if (blend == 1f)
			{
				if (UseRotationMapping)
				{
					IKTargetRotation = rotation * HandIKBoneMapping;
				}
				else
				{
					IKTargetRotation = rotation;
				}
			}
			else if (UseRotationMapping)
			{
				if (fromDefault)
				{
					IKTargetRotation = Quaternion.LerpUnclamped(IKTargetRotation, rotation * HandIKBoneMapping, blend);
				}
				else
				{
					IKTargetRotation = Quaternion.LerpUnclamped(rotation, rotation * HandIKBoneMapping, blend);
				}
			}
			else if (fromDefault)
			{
				IKTargetRotation = Quaternion.LerpUnclamped(IKTargetRotation, rotation, blend);
			}
			else
			{
				IKTargetRotation = Quaternion.LerpUnclamped(rotation, rotation, blend);
			}
		}

		public void CaptureKeyframeAnimation()
		{
			shoulderRotate.CaptureKeyframeAnimation();
			for (IKBone iKBone = IKBones[0]; iKBone != null; iKBone = iKBone.Child)
			{
				iKBone.CaptureSourceAnimation();
			}
		}

		public void RefreshLength()
		{
			ScaleReference = (UpperArmIKBone.transform.position - ForeArmIKBone.transform.position).magnitude;
		}

		public void RefreshScaleReference()
		{
			ScaleReference = (UpperArmIKBone.transform.position - ForeArmIKBone.transform.position).magnitude;
		}

		public float GetStretchValue(Vector3 targetPos)
		{
			return (UpperArmIKBone.transform.position - targetPos).magnitude / limbLength;
		}

		public float GetStretchValueSrc(Vector3 targetPos)
		{
			return (UpperArmIKBone.srcPosition - targetPos).magnitude / limbLength;
		}

		protected virtual void CalculateLimbLength()
		{
			limbLength = Mathf.Epsilon;
			limbLength += (UpperArmIKBone.transform.position - ForeArmIKBone.transform.position).magnitude;
			limbLength += (ForeArmIKBone.transform.position - HandIKBone.transform.position).magnitude;
		}

		private void ComputeShoulder(Vector3 finalIKPos)
		{
			if (Initialized && !(ShoulderBlend <= 0f))
			{
				Vector3 direction = finalIKPos - shoulderRotate.transform.position;
				Quaternion rotation2;
				if ((bool)Root)
				{
					Quaternion rotation = shoulderRotate.transform.rotation;
					Vector3 vector = -Quaternion.FromToRotation(Root.InverseTransformDirection(direction).normalized, initHandRootSpaceFlatTowards).eulerAngles;
					shoulderRotate.RotateXBy(vector.x);
					shoulderRotate.RotateYBy(vector.y);
					shoulderRotate.RotateZBy(vector.z);
					rotation2 = shoulderRotate.transform.rotation;
					shoulderRotate.transform.rotation = rotation;
				}
				else
				{
					rotation2 = ShoulderIKBone.GetRotation(direction.normalized, ShoulderIKBone.srcRotation * shoulderRotate.upReference);
				}
				float num = IKWeight * ShoulderBlend;
				float stretchValue = GetStretchValue(finalIKPos);
				stretchValue *= 0.85f;
				if (stretchValue > 1f)
				{
					stretchValue = 1f;
				}
				num *= Mathf.InverseLerp(0.6f, 1f, stretchValue) * 0.9f;
				ShoulderIKBone.transform.rotation = Quaternion.Slerp(shoulderRotate.transform.rotation, rotation2, num);
			}
		}

		public IKBone GetBone(int index)
		{
			return IKBones[index];
		}

		public void Init(Transform root)
		{
			if (Initialized || IKBones == null)
			{
				return;
			}
			if (IKBones.Length == 0)
			{
				SetBones(ShoulderTransform, UpperarmTransform, LowerarmTransform, HandTransform);
			}
			UpperarmRotationOffset = Quaternion.identity;
			TargetElbowNormal = Vector3.right;
			Vector3 vector = Vector3.Cross(ForeArmIKBone.transform.position - UpperArmIKBone.transform.position, HandIKBone.transform.position - ForeArmIKBone.transform.position);
			if (vector != Vector3.zero)
			{
				TargetElbowNormal = vector;
			}
			FullLength = 0f;
			ShoulderIKBone.Init(root, UpperArmIKBone.transform.position, TargetElbowNormal);
			UpperArmIKBone.Init(root, ForeArmIKBone.transform.position, TargetElbowNormal);
			ForeArmIKBone.Init(root, HandIKBone.transform.position, TargetElbowNormal);
			HandIKBone.Init(root, HandIKBone.transform.position + (HandIKBone.transform.position - ForeArmIKBone.transform.position), TargetElbowNormal);
			FullLength = IKBones[1].BoneLength + IKBones[2].BoneLength;
			RefreshDefaultFlexNormal();
			if (HandIKBone.transform.parent != ForeArmIKBone.transform)
			{
				everyIsChild = false;
			}
			else if (ForeArmIKBone.transform.parent != UpperArmIKBone.transform)
			{
				everyIsChild = false;
			}
			else
			{
				everyIsChild = true;
			}
			ChestIKBone = new IKBone(ShoulderIKBone.transform.parent);
			ChestIKBone.Init(root, ShoulderIKBone.transform.position, TargetElbowNormal);
			SetRootReference(root);
			HandMiddleOffset = Vector3.zero;
			if (HandIKBone.transform.childCount > 0)
			{
				HandMiddleOffset = HandIKBone.transform.GetChild(0).position;
				for (int i = 1; i < HandIKBone.transform.childCount; i++)
				{
					HandMiddleOffset = Vector3.Lerp(HandMiddleOffset, HandIKBone.transform.GetChild(i).position, 0.5f);
				}
				HandMiddleOffset = Vector3.Lerp(HandMiddleOffset, HandIKBone.transform.position, 0.4f);
				HandMiddleOffset = HandIKBone.transform.InverseTransformPoint(HandMiddleOffset);
			}
			Initialized = true;
		}

		public void SetBones(Transform shoulder, Transform upperArm, Transform forearm, Transform hand)
		{
			if (!(upperArm == null) && !(forearm == null) && !(hand == null))
			{
				ShoulderTransform = shoulder;
				UpperarmTransform = upperArm;
				LowerarmTransform = forearm;
				HandTransform = hand;
				int num = 0;
				if (shoulder == null)
				{
					IKBones = new IKBone[3];
				}
				else
				{
					IKBones = new IKBone[4];
					IKBones[0] = new IKBone(shoulder);
					num = 1;
				}
				IKBones[num] = new IKBone(upperArm);
				IKBones[num + 1] = new IKBone(forearm);
				IKBones[num + 2] = new IKBone(hand);
				IKBones[0].SetChild(IKBones[1]);
				IKBones[1].SetChild(IKBones[2]);
				if (shoulder != null)
				{
					IKBones[2].SetChild(IKBones[3]);
				}
				IKTargetPosition = hand.position;
				IKTargetRotation = hand.rotation;
			}
		}

		public void SetBones()
		{
			SetBones(ShoulderTransform, UpperarmTransform, LowerarmTransform, HandTransform);
		}

		protected virtual void Refresh()
		{
			RefreshAnimatorCoords();
			if (!everyIsChild)
			{
				UpperArmIKBone.RefreshOrientations(ForeArmIKBone.transform.position, TargetElbowNormal);
				ForeArmIKBone.RefreshOrientations(HandIKBone.transform.position, TargetElbowNormal);
			}
		}

		protected virtual void HandBoneRotation()
		{
			float num = HandRotationWeight * IKWeight * _internalIKWeight;
			if (num > 0f)
			{
				if (num < 1f)
				{
					HandIKBone.transform.rotation = Quaternion.LerpUnclamped(HandIKBone.transform.rotation, IKTargetRotation, num);
				}
				else
				{
					HandIKBone.transform.rotation = IKTargetRotation;
				}
			}
		}

		public void RefreshAnimatorCoords()
		{
			if (ShoulderIKBone != null)
			{
				ShoulderIKBone.CaptureSourceAnimation();
			}
			UpperArmIKBone.CaptureSourceAnimation();
			ForeArmIKBone.CaptureSourceAnimation();
			HandIKBone.CaptureSourceAnimation();
		}

		private Vector3 GetDefaultFlexNormal()
		{
			if (ManualHintPositionWeight > 0f)
			{
				if (ManualHintPositionWeight >= 1f)
				{
					return CalculateElbowNormalToPosition(IKManualHintPosition);
				}
				return Vector3.LerpUnclamped(GetAutomaticFlexNormal().normalized, CalculateElbowNormalToPosition(IKManualHintPosition), ManualHintPositionWeight);
			}
			return GetAutomaticFlexNormal();
		}

		public Vector3 CalculateElbowNormalToPosition(Vector3 targetElbowPos)
		{
			return Vector3.Cross(targetElbowPos - UpperArmIKBone.transform.position, HandIKBone.transform.position - UpperArmIKBone.transform.position);
		}

		public void RefreshDefaultFlexNormal()
		{
			Vector3 vector = Vector3.Cross(ForeArmIKBone.transform.position - UpperArmIKBone.transform.position, HandIKBone.transform.position - ForeArmIKBone.transform.position);
			if (vector != Vector3.zero)
			{
				TargetElbowNormal = vector;
			}
		}

		private Vector3 GetOrientationDirection(Vector3 ikPosition, Vector3 orientationNormal)
		{
			Vector3 vector = ikPosition - UpperArmIKBone.transform.position;
			if (vector == Vector3.zero)
			{
				return Vector3.zero;
			}
			float sqrMagnitude = vector.sqrMagnitude;
			float num = Mathf.Sqrt(sqrMagnitude);
			float num2 = (sqrMagnitude + UpperArmIKBone.sqrMagn - ForeArmIKBone.sqrMagn) / 2f / num;
			float y = Mathf.Sqrt(Mathf.Clamp(UpperArmIKBone.sqrMagn - num2 * num2, 0f, 1f / 0f));
			Vector3 upwards = Vector3.Cross(vector / num, orientationNormal);
			return Quaternion.LookRotation(vector, upwards) * new Vector3(0f, y, num2);
		}

		public void IKHandRotationWeightFadeTo(float to, float duration, float delta)
		{
			HandRotationWeight = Mathf.SmoothDamp(HandRotationWeight, to, ref sd_targetIKRotation, duration, 1f / 0f, delta);
		}

		public void IKHandPositionWeightFadeTo(float to, float duration, float delta)
		{
			IKPositionWeight = Mathf.SmoothDamp(IKPositionWeight, to, ref sd_positionWeight, duration, 1f / 0f, delta);
		}

		public Vector3 GetMiddleHandPosition(Vector3 tgt)
		{
			return tgt - Matrix4x4.TRS(IKTargetPosition, IKTargetRotation, HandIKBone.transform.lossyScale).MultiplyVector(HandMiddleOffset);
		}

		public Vector3 GetLimitedIKPosToMax(Vector3 targetIKPos, float lengthFactor = 1f)
		{
			Vector3 vector = targetIKPos - UpperArmIKBone.transform.position;
			return UpperArmIKBone.transform.position + vector.normalized * lengthFactor * limbLength;
		}

		public void Update()
		{
			if (!Initialized)
			{
				return;
			}
			CalculateLimbLength();
			Refresh();
			ComputeShoulder(IKTargetPosition);
			Vector3 vector = IKTargetPosition;
			if (MaxStretching < 1.2f)
			{
				CalculateLimbLength();
				if (GetStretchValue(vector) > MaxStretching)
				{
					float num = MaxStretching * limbLength;
					vector = UpperArmIKBone.transform.position + (vector - UpperArmIKBone.transform.position).normalized * num;
				}
			}
			float num2 = IKPositionWeight * IKWeight * _internalIKWeight;
			UpperArmIKBone.sqrMagn = (ForeArmIKBone.transform.position - UpperArmIKBone.transform.position).sqrMagnitude;
			ForeArmIKBone.sqrMagn = (HandIKBone.transform.position - ForeArmIKBone.transform.position).sqrMagnitude;
			TargetElbowNormal = GetDefaultFlexNormal();
			Vector3 vector2 = GetOrientationDirection(vector, TargetElbowNormal);
			if (vector2 == Vector3.zero)
			{
				vector2 = ForeArmIKBone.transform.position - UpperArmIKBone.transform.position;
			}
			if (num2 > 0f)
			{
				Quaternion quaternion = UpperArmIKBone.GetRotation(vector2, TargetElbowNormal) * UpperarmRotationOffset;
				if (num2 < 1f)
				{
					quaternion = Quaternion.LerpUnclamped(UpperArmIKBone.transform.rotation, quaternion, num2);
				}
				UpperArmIKBone.transform.rotation = quaternion;
				Quaternion quaternion2 = ForeArmIKBone.GetRotation(vector - ForeArmIKBone.transform.position, ForeArmIKBone.GetCurrentOrientationNormal());
				if (num2 < 1f)
				{
					quaternion2 = Quaternion.LerpUnclamped(ForeArmIKBone.transform.rotation, quaternion2, num2);
				}
				ForeArmIKBone.transform.rotation = quaternion2;
			}
			HandBoneRotation();
		}

		private Vector3 GetAutomaticFlexNormal()
		{
			Vector3 vector = UpperArmIKBone.GetCurrentOrientationNormal();
			if (ikCustomHintOffset != Vector3.zero)
			{
				vector = (vector + ikCustomHintOffset).normalized;
			}
			switch (AutoHintMode)
			{
			case FIK_HintMode.MiddleForward:
				return Vector3.LerpUnclamped(vector.normalized, ForeArmIKBone.srcRotation * ForeArmIKBone.forward, 0.5f);
			case FIK_HintMode.MiddleBack:
				return ForeArmIKBone.srcRotation * -ForeArmIKBone.right + ikCustomHintOffset;
			case FIK_HintMode.EndForward:
			{
				Vector3 vector2 = Vector3.Cross(ForeArmIKBone.srcPosition + HandIKBone.srcRotation * HandIKBone.forward * (MirrorMaths ? (-1f) : 1f) - UpperArmIKBone.srcPosition, IKTargetPosition - UpperArmIKBone.srcPosition);
				if (vector2 == Vector3.zero)
				{
					return vector;
				}
				return vector2;
			}
			case FIK_HintMode.OnGoal:
				return Vector3.LerpUnclamped(vector, IKTargetRotation * HandIKBone.right, 0.5f);
			default:
				return vector;
			}
		}

		public void OnDrawGizmos()
		{
			_ = Initialized;
		}
	}
}
