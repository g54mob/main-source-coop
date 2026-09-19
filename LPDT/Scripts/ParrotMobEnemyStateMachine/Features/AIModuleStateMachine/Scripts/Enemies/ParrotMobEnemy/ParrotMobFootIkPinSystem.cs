using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[DefaultExecutionOrder(100)]
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobFootIkPinSystem : MonoSystem
	{
		[SerializeField]
		private Transform _bodyTransform;

		[SerializeField]
		private GameObject[] _sideAObjects;

		[SerializeField]
		private GameObject[] _sideBObjects;

		private Pose[] _sideAPinnedPoses;

		private Pose[] _sideBPinnedPoses;

		private bool _hasPinnedPose;

		private bool? _usingSideB;

		private float _spawnForwardYaw;

		private bool _hasSpawnForward;

		public override bool IsEnabled => true;

		public override void Enable()
		{
		}

		public override void Disable()
		{
		}

		public override void Clear()
		{
		}

		public override void Spawned()
		{
			base.Spawned();
			CaptureSpawnForward();
			CachePinnedPoses();
			_usingSideB = null;
			ApplySide(IsFacingBack());
			PinFootObjects();
		}

		private void LateUpdate()
		{
			if (base.Initialized && _hasPinnedPose && _hasSpawnForward)
			{
				bool flag = IsFacingBack();
				if (!_usingSideB.HasValue || _usingSideB.Value != flag)
				{
					ApplySide(flag);
				}
				PinFootObjects();
			}
		}

		private void CaptureSpawnForward()
		{
			Vector3 forward = _bodyTransform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude < 0.0001f)
			{
				forward = Vector3.forward;
			}
			else
			{
				forward.Normalize();
			}
			_spawnForwardYaw = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
			_hasSpawnForward = true;
		}

		private void CachePinnedPoses()
		{
			_sideAPinnedPoses = CapturePoses(_sideAObjects);
			_sideBPinnedPoses = CapturePoses(_sideBObjects);
			_hasPinnedPose = _sideAPinnedPoses != null && _sideBPinnedPoses != null && _sideAPinnedPoses.Length != 0 && _sideBPinnedPoses.Length != 0;
		}

		private void PinFootObjects()
		{
			ApplyPinnedPoses(_sideAObjects, _sideAPinnedPoses);
			ApplyPinnedPoses(_sideBObjects, _sideBPinnedPoses);
		}

		private void ApplySide(bool useSideB)
		{
			SetConstraintWeights(_sideAObjects, useSideB ? 0f : 1f);
			SetConstraintWeights(_sideBObjects, useSideB ? 1f : 0f);
			_usingSideB = useSideB;
		}

		private bool IsFacingBack()
		{
			float bodyWorldYaw = GetBodyWorldYaw();
			return Mathf.Abs(Mathf.DeltaAngle(_spawnForwardYaw, bodyWorldYaw)) > 90f;
		}

		private float GetBodyWorldYaw()
		{
			Vector3 forward = _bodyTransform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude < 0.0001f)
			{
				return _spawnForwardYaw;
			}
			forward.Normalize();
			return Mathf.Atan2(forward.x, forward.z) * 57.29578f;
		}

		private static Pose[] CapturePoses(GameObject[] objects)
		{
			if (objects == null || objects.Length == 0)
			{
				return null;
			}
			Pose[] array = new Pose[objects.Length];
			for (int i = 0; i < objects.Length; i++)
			{
				GameObject gameObject = objects[i];
				array[i] = ((gameObject != null) ? new Pose(gameObject.transform.position, gameObject.transform.rotation) : default(Pose));
			}
			return array;
		}

		private static void ApplyPinnedPoses(GameObject[] objects, Pose[] poses)
		{
			if (objects == null || poses == null)
			{
				return;
			}
			int num = Mathf.Min(objects.Length, poses.Length);
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = objects[i];
				if (!(gameObject == null))
				{
					gameObject.transform.SetPositionAndRotation(poses[i].position, poses[i].rotation);
				}
			}
		}

		private static void SetConstraintWeights(GameObject[] objects, float weight)
		{
			if (objects == null)
			{
				return;
			}
			foreach (GameObject gameObject in objects)
			{
				if (!(gameObject == null) && gameObject.TryGetComponent<ChainIKConstraint>(out var component) && !Mathf.Approximately(component.weight, weight))
				{
					component.weight = weight;
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
