using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.Player
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/FPS Controller Setting", fileName = "FirstPersonControllerSetting")]
	public class FirstPersonControllerSettings : ScriptableObject
	{
		[Tooltip("Normal walking speed in m/s")]
		[Range(0.5f, 10f)]
		public float walkSpeed = 3.75f;

		[Tooltip("Running speed in m/s")]
		[Range(1f, 20f)]
		public float sprintSpeed = 5.5f;

		[Tooltip("Walking speed while crouched in m/s")]
		[Range(0.1f, 5f)]
		public float crouchedWalkSpeed = 1f;

		[Tooltip("Sprint speed while crouched in m/s")]
		[Range(0.1f, 5f)]
		public float crouchedSprintSpeed = 1.2f;

		[Tooltip("Minimum speed to transition from Idle to Walk (as percentage of walk speed)")]
		[Range(0.1f, 1f)]
		public float idleToWalkThreshold = 0.605f;

		[Tooltip("Minimum speed to transition from Walk to Sprint (as percentage of sprint speed)")]
		[Range(0.5f, 0.9f)]
		public float walkToSprintThreshold = 0.5f;

		[Tooltip("Player mass for ECM2 physics")]
		[Range(20f, 200f)]
		public float playerMass = 80f;

		[Tooltip("Vertical force applied when jumping")]
		[Range(1f, 20f)]
		public float jumpForce = 9.37f;

		[Tooltip("Gravity acceleration (Earth default: 9.8)")]
		[Range(-25f, 0f)]
		public float gravity = -9.81f;

		[Tooltip("Capsule collider height when standing. Also used for head follow target origin.")]
		[Range(0.5f, 2.5f)]
		public float standingCapsuleHeight = 1.6f;

		[Tooltip("Capsule collider height when crouched.")]
		[Range(0.3f, 1.5f)]
		public float crouchedCapsuleHeight = 0.7f;

		[Tooltip("Capsule collider height applied while the player is seated (e.g. driving a vehicle).")]
		[Range(0.5f, 2f)]
		[FormerlySerializedAs("sittingCameraY")]
		public float sittingCapsuleHeight = 1.24f;

		[Tooltip("Maximum upward camera angle")]
		[Range(0f, 90f)]
		public float maxCameraPitch = 80f;

		[Tooltip("Maximum downward camera angle")]
		[Range(-90f, 0f)]
		public float minCameraPitch = -75f;

		[Tooltip("Maximum upward angle while sitting")]
		[Range(0f, 90f)]
		public float maxCameraPitchOnSitting = 80f;

		[Tooltip("Maximum downward angle while sitting")]
		[Range(-90f, 0f)]
		public float minCameraPitchOnSitting = -80f;

		[Tooltip("Maximum left rotation while sitting")]
		[Range(-180f, 0f)]
		public float minCameraYawOnSitting = -110f;

		[Tooltip("Maximum right rotation while sitting")]
		[Range(0f, 180f)]
		public float maxCameraYawOnSitting = 110f;

		[Tooltip("X = Horizontal, Y = Vertical sensitivity. Runtime-derived from the user 1-10 scale × Sensitivity Per Step.")]
		public Vector2 cameraRotateSensitivity = new Vector2(0.1f, 0.1f);

		[Tooltip("Multiplier per 1 unit of the 1-10 user sensitivity scale. Scale 4 × 0.025 = 0.1 (default feel).")]
		public float sensitivityScaleMultiplier = 0.025f;

		[Tooltip("How quickly camera rotation reaches target angle. 0 = instant.")]
		[Range(0f, 30f)]
		public float cameraRotationSmoothSpeed;

		[Tooltip("Camera Y offset from character root when standing")]
		[Range(0.5f, 2.5f)]
		public float standingEyeHeight = 1.6f;

		[Tooltip("Camera Y offset from character root when crouched")]
		[Range(0.3f, 1.5f)]
		public float crouchedEyeHeight = 0.7f;

		[Tooltip("Speed of stand/crouch camera height interpolation")]
		[Range(1f, 20f)]
		public float heightTransitionSpeed = 8f;

		[Range(-0.3f, 0.3f)]
		public float cameraOffsetX;

		[Tooltip("Forward offset from character center. Pushes camera to eye position.")]
		[Range(-0.3f, 0.5f)]
		public float cameraOffsetZ = 0.12f;

		[Tooltip("Horizontal offset while crouched. Lerps from cameraOffsetX -> crouchedOffsetX using heightTransitionSpeed.")]
		[Range(-0.3f, 0.3f)]
		public float crouchedOffsetX;

		[Tooltip("Additive vertical delta applied on top of crouchedEyeHeight while fully crouched. Fine-tunes crouched eye position without changing the height transition target.")]
		[Range(-0.3f, 0.3f)]
		public float crouchedOffsetY;

		[Tooltip("Forward offset while crouched. Lerps from cameraOffsetZ -> crouchedOffsetZ using heightTransitionSpeed.")]
		[Range(-0.3f, 0.5f)]
		public float crouchedOffsetZ;

		[Range(-0.3f, 0.3f)]
		public float sittingOffsetX;

		[Range(-0.3f, 0.3f)]
		public float sittingOffsetY = 0.152f;

		[Range(-0.3f, 0.5f)]
		public float sittingOffsetZ = 0.042f;

		[Tooltip("How much the camera position shifts when looking around while sitting. Simulates eye pivot distance from neck.")]
		[Range(0f, 0.2f)]
		public float eyeRotationRadius = 0.1f;

		[Tooltip("Fraction (0-1) of animation movement applied to camera. 0 = rigid, 0.05 = subtle organic feel")]
		[Range(0f, 0.6f)]
		public float animationInfluence = 0.05f;

		[Tooltip("SmoothDamp time for animation influence (higher = smoother/slower)")]
		[Range(0.01f, 0.5f)]
		public float animationDamping = 0.15f;

		[Tooltip("Maximum distance animation can shift the camera")]
		[Range(0f, 0.2f)]
		public float maxAnimationOffset = 0.03f;

		[Tooltip("How strongly the camera follows the head bone during Jump/Falling/Landing. Higher = camera tracks animated head more tightly (prevents seeing headless body from above).")]
		[Range(0f, 1f)]
		public float airborneAnimationInfluence = 0.85f;

		[Tooltip("Max meters the camera may offset from the static eye height during airborne states. Larger than ground locomotion because jump anims have dramatic squat-tuck movement.")]
		[Range(0f, 1.5f)]
		public float airborneMaxAnimationOffset = 0.5f;

		[Tooltip("SmoothDamp time for airborne head tracking. Higher = smoother but laggier. Tune up if jump feels nauseating.")]
		[Range(0.01f, 0.5f)]
		public float airborneAnimationDamping = 0.06f;

		[Tooltip("0 = ignore horizontal head sway during airborne (recommended for stable aim); 1 = full XZ tracking.")]
		[Range(0f, 1f)]
		public float airborneHorizontalInfluence;

		[Tooltip("Prevents camera from clipping inside character model or geometry")]
		public bool enableAntiClip;

		[Tooltip("SphereCast radius for camera clip detection")]
		[Range(0.05f, 0.2f)]
		public float antiClipRadius = 0.14f;

		[Tooltip("Layers to check for camera clipping")]
		public LayerMask antiClipLayerMask = -1;

		[Tooltip("Distance of the head look target in front of the player")]
		[Range(0.25f, 2f)]
		public float headFollowDistance = 2f;

		[Tooltip("How much camera pitch affects the look target height (0 = fully horizontal, 1 = follows camera fully)")]
		[Range(0f, 1f)]
		public float headFollowPitchInfluence = 1f;

		[Tooltip("Minimum render distance. Higher values hide close geometry like the neck opening")]
		[Range(0.01f, 0.3f)]
		public float nearClipPlane = 0.01f;

		[Tooltip("Normal field of view angle")]
		[Range(60f, 120f)]
		public float defaultFov = 90f;

		[Tooltip("Field of view when zooming/aiming")]
		[Range(20f, 90f)]
		public float zoomedFov = 60f;

		[Tooltip("Time to transition between FOV states")]
		[Range(0.1f, 2f)]
		public float zoomDuration = 0.49f;

		[Tooltip("When enabled, edits to this asset during play mode immediately apply to the live player.\nSkips the runtime clone in FirstPersonController.Init, so changes WILL persist to the asset on disk after exiting play mode.\nEditor-only - has no effect in builds.")]
		public bool debugLiveApply;
	}
}
