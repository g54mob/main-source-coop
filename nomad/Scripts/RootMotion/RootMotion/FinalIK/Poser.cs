using UnityEngine;

namespace RootMotion.FinalIK
{
	public abstract class Poser : SolverManager
	{
		public Transform poseRoot;

		[Range(0f, 1f)]
		public float weight = 1f;

		[Range(0f, 1f)]
		public float localRotationWeight = 1f;

		[Range(0f, 1f)]
		public float localPositionWeight;

		private bool initiated;

		protected Transform nextPoseRoot;

		protected bool storeCurrentPoseFlag;

		private bool isLerping;

		private float lerpTimer;

		private Transform[] autoMappingBones = new Transform[0];

		private float blendSpeed;

		public abstract void AutoMapping();

		public virtual void AutoMapping(Transform[] bones)
		{
		}

		public void UpdateManual(float deltaTime)
		{
			UpdatePoser();
			UpdateLerping(deltaTime);
			if (storeCurrentPoseFlag)
			{
				OnStoreCurrentPoseFlag();
			}
		}

		public void SetPoseRoot(Transform newPoseRoot, Transform[] autoMappingBones, float blendSpeed)
		{
			this.blendSpeed = blendSpeed;
			if (newPoseRoot != null && poseRoot != null && weight > 0f)
			{
				nextPoseRoot = newPoseRoot;
				this.autoMappingBones = autoMappingBones;
				storeCurrentPoseFlag = true;
			}
			else
			{
				poseRoot = newPoseRoot;
				AutoMapping(autoMappingBones);
				storeCurrentPoseFlag = false;
			}
		}

		protected abstract void InitiatePoser();

		protected abstract void UpdatePoser();

		protected abstract void StoreCurrentPose();

		protected abstract void BlendFromLastPose(float lerpTimer);

		protected abstract void FixPoserTransforms();

		private void UpdateLerping(float deltaTime)
		{
			if (isLerping)
			{
				lerpTimer = Mathf.MoveTowards(lerpTimer, 1f, deltaTime * blendSpeed);
				BlendFromLastPose(lerpTimer);
				if (lerpTimer >= 1f)
				{
					isLerping = false;
					lerpTimer = 0f;
				}
			}
		}

		protected override void UpdateSolver()
		{
			if (!initiated)
			{
				InitiateSolver();
			}
			if (initiated)
			{
				UpdatePoser();
				UpdateLerping(Time.deltaTime);
				if (storeCurrentPoseFlag)
				{
					OnStoreCurrentPoseFlag();
				}
			}
		}

		private void OnStoreCurrentPoseFlag()
		{
			StoreCurrentPose();
			isLerping = true;
			lerpTimer = 0f;
			poseRoot = nextPoseRoot;
			AutoMapping(autoMappingBones);
			nextPoseRoot = null;
			storeCurrentPoseFlag = false;
		}

		protected override void InitiateSolver()
		{
			if (!initiated)
			{
				InitiatePoser();
				initiated = true;
			}
		}

		protected override void FixTransforms()
		{
			if (initiated)
			{
				FixPoserTransforms();
			}
		}
	}
}
