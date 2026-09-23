using Mimicraft.Settings;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(VoxelModel))]
	public class PropMovementController : MonoBehaviour
	{
		private VoxelModel model;

		private CharacterController controller;

		private MeshCollider modelMeshCollider;

		private Transform cameraTransform;

		private readonly WallClimbMover mover = new WallClimbMover();

		[Tooltip("Which layers this prop may climb in Movement mode. Everything by default. Does NOT affect standing on the ground - an unclimbable surface is still solid.")]
		[SerializeField]
		private LayerMask climbableLayers = -1;

		private void Awake()
		{
			mover.ClimbableLayers = climbableLayers;
			model = GetComponent<VoxelModel>();
			modelMeshCollider = GetComponent<MeshCollider>();
			controller = GetComponent<CharacterController>();
			if (controller == null)
			{
				Vector3 localScale = base.transform.localScale;
				base.transform.localScale = Vector3.one;
				controller = base.gameObject.AddComponent<CharacterController>();
				base.transform.localScale = localScale;
				controller.stepOffset = 0f;
				ApplyDefaultSize();
			}
			controller.enabled = false;
			base.enabled = false;
		}

		private void ApplyDefaultSize()
		{
			if (model.Grid != null)
			{
				SyncCapsuleToModel();
			}
		}

		private void SyncCapsuleToModel()
		{
			if (BodyCapsule.TryMeasure(base.transform, out var boundsLocal))
			{
				BodyCapsule.Shape shape = BodyCapsule.Compute(boundsLocal, base.transform.lossyScale.y);
				controller.radius = shape.Radius;
				controller.height = shape.Height;
				controller.center = shape.Center;
				controller.skinWidth = shape.SkinWidth;
				controller.stepOffset = Mathf.Min(shape.WorldRadius, shape.WorldHeight) * 0.3f;
				controller.slopeLimit = 89f;
			}
		}

		public void Activate(Transform cameraTransform)
		{
			this.cameraTransform = cameraTransform;
			SyncCapsuleToModel();
			CaptureFacingOffset();
			if (modelMeshCollider != null)
			{
				modelMeshCollider.enabled = false;
			}
			controller.enabled = true;
			base.enabled = true;
		}

		public void Deactivate()
		{
			base.enabled = false;
			controller.enabled = false;
			if (modelMeshCollider != null)
			{
				modelMeshCollider.enabled = true;
			}
		}

		private float ContactRadius()
		{
			if (model == null || model.Grid == null || !GridBounds.TryCompute(model.Grid, out var min, out var max))
			{
				return 0f;
			}
			float num = (float)(max.x - min.x + 1) * model.VoxelSize;
			float num2 = (float)(max.y - min.y + 1) * model.VoxelSize;
			return (num + num2) * 0.25f;
		}

		private void CaptureFacingOffset()
		{
			Vector3 vector = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
			if (vector.sqrMagnitude < 1E-06f)
			{
				vector = Vector3.ProjectOnPlane(base.transform.up, Vector3.up);
			}
			if (vector.sqrMagnitude < 1E-06f)
			{
				mover.FacingOffset = Quaternion.identity;
				return;
			}
			Quaternion rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
			mover.FacingOffset = Quaternion.Inverse(rotation) * base.transform.rotation;
		}

		private void Update()
		{
			Vector2 input2D = GameInput.Move.ReadValue<Vector2>();
			bool running = GameInput.Run.IsPressed();
			bool jumpPressed = GameInput.Jump.WasPressedThisFrame();
			Vector3 rawCamForward = ((cameraTransform != null) ? cameraTransform.forward : base.transform.forward);
			Vector3 rawCamRight = ((cameraTransform != null) ? cameraTransform.right : base.transform.right);
			bool climbHeld = GameInput.Climb.IsPressed();
			mover.Tick(controller, rawCamForward, rawCamRight, input2D, jumpPressed, running, climbHeld, Time.deltaTime);
			if (mover.GrabbedThisTick)
			{
				ImpactEffects.SpawnClingEffect(mover.SurfacePoint, mover.SurfaceNormal, attached: true, ContactRadius());
			}
			else if (mover.ReleasedThisTick)
			{
				ImpactEffects.SpawnClingEffect(mover.SurfacePoint, mover.SurfaceNormal, attached: false, ContactRadius());
			}
		}
	}
}
