using System;
using UnityEngine;

namespace RootMotion.FinalIK
{
	public class UniversalPoser : Poser
	{
		[Serializable]
		public class Map
		{
			public Transform bone;

			[HideInInspector]
			public Transform target;

			private Vector3 defaultLocalPosition;

			private Quaternion defaultLocalRotation;

			private Vector3 lerpLocalPosition;

			private Quaternion lerpLocalRotation;

			public Map(Transform bone, Transform target)
			{
				this.bone = bone;
				this.target = target;
				StoreDefaultState();
			}

			public void StoreDefaultState()
			{
				defaultLocalPosition = bone.localPosition;
				defaultLocalRotation = bone.localRotation;
			}

			public void StoreCurrentPose()
			{
				lerpLocalPosition = bone.localPosition;
				lerpLocalRotation = bone.localRotation;
			}

			public void FixTransform()
			{
				bone.localPosition = defaultLocalPosition;
				bone.localRotation = defaultLocalRotation;
			}

			public void Update(float localRotationWeight, float localPositionWeight, Vector3 targetAxis1, Vector3 targetAxis2, Vector3 axis1, Vector3 axis2)
			{
				if (targetAxis1 == axis1 && targetAxis2 == axis2)
				{
					bone.localRotation = Quaternion.Lerp(bone.localRotation, target.localRotation, localRotationWeight);
					return;
				}
				Quaternion quaternion = Quaternion.Lerp(bone.localRotation, QuaTools.MatchRotation(target.localRotation, targetAxis1, targetAxis2, axis1, axis2), localRotationWeight);
				Quaternion quaternion2 = QuaTools.MatchRotation(Quaternion.identity, targetAxis1, targetAxis2, axis1, axis2);
				bone.localRotation = quaternion2 * quaternion;
			}

			public void BlendFromLastPose(float lerpTimer)
			{
				bone.localPosition = Vector3.Lerp(lerpLocalPosition, bone.localPosition, lerpTimer);
				bone.localRotation = Quaternion.Lerp(lerpLocalRotation, bone.localRotation, lerpTimer);
			}
		}

		[Tooltip("Choose 2 axes of a finger bone. For example 1 pointing towards the next finger and 2 pointing up. Select a finger bone in the InteractionTarget hierarchy and see which local axis points towards the next bone and which local axis points up and set targetAxis1 and targetAxis2 accordingly. Then select a finger in this poser's hierarchy and do the same for axis1 and axis2.")]
		public Vector3 targetAxis1 = Vector3.forward;

		[Tooltip("Choose 2 axes of a finger bone. For example 1 pointing towards the next finger and 2 pointing up. Select a finger bone in the InteractionTarget hierarchy and see which local axis points towards the next bone and which local axis points up and set targetAxis1 and targetAxis2 accordingly. Then select a finger in this poser's hierarchy and do the same for axis1 and axis2.")]
		public Vector3 targetAxis2 = Vector3.up;

		[Tooltip("Choose 2 axes of a finger bone. For example 1 pointing towards the next finger and 2 pointing up. Select a finger bone in the InteractionTarget hierarchy and see which local axis points towards the next bone and which local axis points up and set targetAxis1 and targetAxis2 accordingly. Then select a finger in this poser's hierarchy and do the same for axis1 and axis2.")]
		public Vector3 axis1 = Vector3.forward;

		[Tooltip("Choose 2 axes of a finger bone. For example 1 pointing towards the next finger and 2 pointing up. Select a finger bone in the InteractionTarget hierarchy and see which local axis points towards the next bone and which local axis points up and set targetAxis1 and targetAxis2 accordingly. Then select a finger in this poser's hierarchy and do the same for axis1 and axis2.")]
		public Vector3 axis2 = Vector3.up;

		[Tooltip("List of bones must match InteractionTarget's list of bones in both array size and hierarchy.")]
		public Map[] bones;

		public override void AutoMapping()
		{
		}

		public override void AutoMapping(Transform[] bones)
		{
			if (bones.Length != this.bones.Length)
			{
				Debug.LogError("Trying to use UniversalPoser with an InteractionTarget that has a different number of bones. Bones must match with UniversalPoser bones in both array size and hierarchy", base.transform);
				Debug.LogError("InterqactionTarget.bones: " + bones.Length);
				for (int i = 0; i < bones.Length; i++)
				{
					Debug.LogError("   " + i + ": " + bones[i].name);
				}
			}
			else
			{
				for (int j = 0; j < this.bones.Length; j++)
				{
					this.bones[j].target = bones[j];
				}
				StoreDefaultState();
			}
		}

		protected override void InitiatePoser()
		{
			StoreDefaultState();
		}

		protected override void UpdatePoser()
		{
			if (!(weight <= 0f) && (!(localPositionWeight <= 0f) || !(localRotationWeight <= 0f)) && !(poseRoot == null))
			{
				float num = localRotationWeight * weight;
				float num2 = localPositionWeight * weight;
				for (int i = 0; i < bones.Length; i++)
				{
					bones[i].Update(num, num2, targetAxis1, targetAxis2, axis1, axis2);
				}
			}
		}

		protected override void StoreCurrentPose()
		{
			for (int i = 0; i < bones.Length; i++)
			{
				bones[i].StoreCurrentPose();
			}
		}

		protected override void BlendFromLastPose(float lerpTimer)
		{
			for (int i = 0; i < bones.Length; i++)
			{
				bones[i].BlendFromLastPose(lerpTimer);
			}
		}

		protected override void FixPoserTransforms()
		{
			for (int i = 0; i < bones.Length; i++)
			{
				bones[i].FixTransform();
			}
		}

		private void StoreDefaultState()
		{
			for (int i = 0; i < bones.Length; i++)
			{
				bones[i].StoreDefaultState();
			}
		}

		private Transform GetTargetNamed(string tName, Transform[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name == tName)
				{
					return array[i];
				}
			}
			return null;
		}
	}
}
