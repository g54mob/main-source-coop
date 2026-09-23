using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mimicraft.Analytics;
using Mimicraft.Customization;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Tutorial;
using Mimicraft.UI;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerVoxelBody : NetworkBehaviour, ILimbBody
	{
		private struct PlacedShape
		{
			public PieceShapeData Shape;

			public Vector3 Position;

			public Quaternion Rotation;
		}

		public enum PlacementVerdict
		{
			Clear = 0,
			Pending = 1,
			Sanitized = 2,
			SendBack = 3
		}

		[SerializeField]
		private GameObject capsuleBody;

		[Tooltip("İnsansı karakter modelinin kökü (CharacterModel). Voxel gövde gösterilirken gizlenir. Boş bırakılırsa PlayerCameraRig'den çözülür.")]
		[SerializeField]
		private GameObject characterModel;

		[SerializeField]
		private GameObject voxelBodyRoot;

		[SerializeField]
		private VoxelModel voxelModel;

		[SerializeField]
		private VoxelEditorController voxelEditorController;

		public readonly NetworkVariable<VoxelModelPayload> ModelData = new NetworkVariable<VoxelModelPayload>();

		private readonly List<VoxelModel> mirrorPieces = new List<VoxelModel>();

		private const double MinSubmitIntervalSeconds = 0.25;

		private const double SendQuietSeconds = 0.35;

		private const double SendMaxWaitSeconds = 1.0;

		private const double ServerMinSubmitIntervalSeconds = 0.1;

		private GameModeController roundManager;

		private CharacterController ownController;

		private bool lastSetupWindow;

		private bool lastShouldHaveModel;

		private bool bodyDirty;

		private double nextSendAllowedTime;

		private double lastChangeTime;

		private double firstDirtyTime;

		private const float ClearanceCheckSeconds = 1f;

		private readonly HashSet<VoxelModel> clearanceWithheld = new HashSet<VoxelModel>();

		private readonly HashSet<VoxelModel> clearanceScratch = new HashSet<VoxelModel>();

		private readonly List<VoxelModel> clearancePieces = new List<VoxelModel>();

		private float nextClearanceCheck;

		private const float ServerClearanceGraceSeconds = 2f;

		private readonly List<PlacedShape> acceptedShapes = new List<PlacedShape>();

		private readonly List<PlacedShape> pendingShapes = new List<PlacedShape>();

		private readonly List<int> badPieces = new List<int>();

		private float acceptedVoxelSize;

		private float serverClearanceBadFor;

		private byte[] acceptedPayload;

		private double nextAcceptedSubmitServerTime;

		private bool locallyConcealed;

		private bool companionMode;

		private float appliedRunLift;

		private Quaternion appliedRunLean = Quaternion.identity;

		private Vector3 appliedRunLeanPivot;

		private bool capturedDefaultCapsule;

		private float defaultRadius;

		private float defaultHeight;

		private Vector3 defaultCenter;

		private float defaultStepOffset;

		private float defaultSkinWidth;

		private const float CapsuleFrameTolerance = 1f;

		private Quaternion lastCapsuleFrame = Quaternion.identity;

		private bool hasCapsuleFrame;

		private const float MaxAlignDistance = 2f;

		public static bool AlignDistanceUnlimited;

		private const float PushOutBudget = 0.5f;

		private const float PushOutStep = 0.125f;

		private const float ContactSlack = 0.01f;

		private static readonly Vector3[] PushOutHorizontals = new Vector3[4]
		{
			Vector3.forward,
			Vector3.back,
			Vector3.right,
			Vector3.left
		};

		private PlayerMovement ownMovement;

		private int cachedBoundsVersion = -1;

		private Bounds cachedRestBounds;

		private bool hasCachedBounds;

		private bool bodyHiddenFromOwner;

		private bool ownerSpectating;

		public static int SendCount { get; private set; }

		public static double LastEncodeMs { get; private set; }

		public static int LastPayloadBytes { get; private set; }

		public static double EncodeMsTotal { get; private set; }

		public static int ValidateCount { get; private set; }

		public static double LastValidateMs { get; private set; }

		public static double ValidateMsTotal { get; private set; }

		public bool HasPendingBody => bodyDirty;

		public double PendingBodyAgeSeconds
		{
			get
			{
				if (!bodyDirty)
				{
					return 0.0;
				}
				return Time.timeAsDouble - firstDirtyTime;
			}
		}

		public VoxelEditorController EditorController => voxelEditorController;

		public bool IsLocallyConcealed => locallyConcealed;

		public bool HasVoxelBody
		{
			get
			{
				if (voxelBodyRoot != null)
				{
					return voxelBodyRoot.activeSelf;
				}
				return false;
			}
		}

		public static PlayerVoxelBody Local => LocalBody();

		public float CurrentRunLift => appliedRunLift;

		public Quaternion CurrentRunLean => appliedRunLean;

		public Vector3 CurrentRunLeanPivot => appliedRunLeanPivot;

		private Vector3 CapsuleCenterWorld => base.transform.position + base.transform.TransformVector(ownController.center);

		private bool IsClinging
		{
			get
			{
				if (ownMovement == null)
				{
					ownMovement = GetComponent<PlayerMovement>();
				}
				if (ownMovement != null)
				{
					return ownMovement.IsWallClinging;
				}
				return false;
			}
		}

		public bool IsBuried
		{
			get
			{
				if (ownController == null)
				{
					ownController = GetComponent<CharacterController>();
				}
				Collider blocker;
				if (ownController != null)
				{
					return IsCapsuleBuriedAt(CapsuleCenterWorld, out blocker);
				}
				return false;
			}
		}

		public float BodySizeRatio
		{
			get
			{
				if (!HasVoxelBody || voxelModel == null || !TryMeasureBodyInPlayerSpace(out var bounds))
				{
					return 1f;
				}
				float num = Mathf.Max(voxelModel.InitialWorldSize, 0.0001f);
				Vector3 size = bounds.size;
				return Mathf.Clamp(Mathf.Pow(Mathf.Max(size.x, 0.0001f) * Mathf.Max(size.y, 0.0001f) * Mathf.Max(size.z, 0.0001f), 1f / 3f) / num, 0.25f, 8f);
			}
		}

		public int BodyVersion { get; private set; }

		public static void ResetSendStats()
		{
			SendCount = 0;
			LastEncodeMs = 0.0;
			LastPayloadBytes = 0;
			EncodeMsTotal = 0.0;
			ValidateCount = 0;
			LastValidateMs = 0.0;
			ValidateMsTotal = 0.0;
		}

		public void SetReferences(GameObject capsuleBody, GameObject voxelBodyRoot, VoxelModel voxelModel, VoxelEditorController voxelEditorController)
		{
			this.capsuleBody = capsuleBody;
			this.voxelBodyRoot = voxelBodyRoot;
			this.voxelModel = voxelModel;
			this.voxelEditorController = voxelEditorController;
		}

		public override void OnNetworkSpawn()
		{
			ClampBodyStepOffsets();
			voxelBodyRoot.SetActive(value: true);
			voxelEditorController.enabled = false;
			NetworkVariable<VoxelModelPayload> modelData = ModelData;
			modelData.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Combine(modelData.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnModelDataChanged));
			ApplyPayload(ModelData.Value);
			if (base.IsOwner)
			{
				UndoManager.EditStepApplied += OnEditStepApplied;
				PlayerCameraRig component = GetComponent<PlayerCameraRig>();
				if (component != null)
				{
					voxelEditorController.SetCamera(component.TpsCameraTransform.GetComponent<Camera>());
				}
			}
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<VoxelModelPayload> modelData = ModelData;
			modelData.OnValueChanged = (NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate)Delegate.Remove(modelData.OnValueChanged, new NetworkVariable<VoxelModelPayload>.OnValueChangedDelegate(OnModelDataChanged));
			if (base.IsOwner)
			{
				UndoManager.EditStepApplied -= OnEditStepApplied;
			}
		}

		private void Update()
		{
			if (!base.IsOwner)
			{
				return;
			}
			KeepCapsuleFittedToPose();
			CheckClearancePeriodically();
			TrySendBody();
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
			}
			if (roundManager == null)
			{
				return;
			}
			bool isModelSetupWindow = roundManager.IsModelSetupWindow;
			bool localPlayerShouldHaveModel = roundManager.LocalPlayerShouldHaveModel;
			bool num = isModelSetupWindow && (!lastSetupWindow || lastShouldHaveModel != localPlayerShouldHaveModel);
			lastSetupWindow = isModelSetupWindow;
			lastShouldHaveModel = localPlayerShouldHaveModel;
			if (num)
			{
				if (localPlayerShouldHaveModel)
				{
					ResetToDefaultBody();
				}
				else
				{
					ClearBody();
				}
			}
		}

		public static void RequestLocalReset()
		{
			LocalBody()?.RequestResetToCube();
		}

		public static bool CanLocalReset(bool report)
		{
			PlayerVoxelBody playerVoxelBody = LocalBody();
			GameModeController current = GameModeController.Current;
			bool flag = playerVoxelBody != null && current != null && current.CanLocalPlayerEditModel && playerVoxelBody.HasVoxelBody;
			if (flag || !report)
			{
				return flag;
			}
			if (ToastView.Instance != null)
			{
				ToastView.Instance.Show((playerVoxelBody == null) ? Loc.Get("Reset.NoModel") : "Şu anda model değiştirilemez.");
			}
			if (playerVoxelBody != null)
			{
				AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.editDeniedClip);
			}
			return false;
		}

		private static PlayerVoxelBody LocalBody()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			NetworkObject networkObject = ((singleton != null && singleton.IsClient) ? singleton.LocalClient.PlayerObject : null);
			if (!(networkObject != null))
			{
				return null;
			}
			return networkObject.GetComponent<PlayerVoxelBody>();
		}

		public void RequestResetToCube()
		{
			RequestPrimitive(VoxelPrimitive.StartingModel);
		}

		public void RequestPrimitive(VoxelPrimitive shape)
		{
			if (!base.IsOwner)
			{
				return;
			}
			GameModeController current = GameModeController.Current;
			if (current == null || !current.CanLocalPlayerEditModel || !HasVoxelBody)
			{
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show("Şu anda model değiştirilemez.");
				}
				AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.editDeniedClip);
			}
			else
			{
				ResetToDefaultBody(shape);
			}
		}

		public static void RequestLocalPrimitive(VoxelPrimitive shape)
		{
			LocalBody()?.RequestPrimitive(shape);
		}

		private void ResetToDefaultBody(VoxelPrimitive shape = VoxelPrimitive.StartingModel)
		{
			UndoManager.Clear();
			DestroySplitPieces();
			voxelBodyRoot.SetActive(value: true);
			SetCharacterModelVisible(visible: false);
			voxelModel.ResetToDefaultCube();
			ResetBodyPose();
			VoxelGrid voxelGrid = VoxelPrimitives.Build(shape, voxelModel.InitialSize, voxelModel.InitialColor);
			if (voxelGrid != null)
			{
				voxelModel.Grid.Clear();
				foreach (KeyValuePair<Vector3Int, VoxelData> voxel in voxelGrid.Voxels)
				{
					voxelModel.Grid.Set(voxel.Key, voxel.Value);
				}
				voxelModel.RebuildMesh();
				SyncControllerToBody();
				PushBody();
			}
			else
			{
				ApplyStartingMimic();
				SyncControllerToBody();
				PushBody();
			}
		}

		private void ApplyStartingMimic()
		{
			if (TutorialLaunch.IsTutorialSession)
			{
				return;
			}
			string rejection;
			TemplateModel templateModel = StartingMimic.LoadPlayable(out rejection);
			if (templateModel == null)
			{
				if (!string.IsNullOrEmpty(rejection) && ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get(rejection));
				}
				if (!string.IsNullOrEmpty(rejection))
				{
					Telemetry.Send("body_rejected", ("reason", rejection), ("origin", "starting_mimic"));
				}
				return;
			}
			voxelModel.Grid.Clear();
			TemplateStorage.ApplyPiece(templateModel.Pieces[0], voxelModel.Grid);
			voxelModel.RebuildMesh();
			Camera cam = null;
			for (int i = 1; i < templateModel.Pieces.Count; i++)
			{
				TemplatePiece piece = templateModel.Pieces[i];
				GameObject gameObject = VoxelSplitFactory.CreateSplitPiece(voxelModel, Array.Empty<KeyValuePair<Vector3Int, VoxelData>>(), cam);
				if (!(gameObject == null) && gameObject.TryGetComponent<VoxelModel>(out var component))
				{
					TemplateStorage.ApplyPiece(piece, component.Grid);
					component.transform.localPosition = voxelBodyRoot.transform.localPosition + piece.LocalPosition;
					component.transform.localRotation = piece.LocalRotation;
					component.RebuildMesh();
					DetachFromOwnController(component);
				}
			}
			UndoManager.Clear();
		}

		private void ResetBodyPose()
		{
			Vector3 vector = voxelModel.GetInitialBoundsCenterLocal() * voxelModel.VoxelSize;
			voxelBodyRoot.transform.localPosition = new Vector3(0f - vector.x, 0f, 0f - vector.z);
			voxelBodyRoot.transform.localRotation = Quaternion.identity;
			appliedRunLift = 0f;
			appliedRunLean = Quaternion.identity;
			appliedRunLeanPivot = Vector3.zero;
		}

		public void SetRunLift(float lift)
		{
			if (voxelBodyRoot == null || Mathf.Approximately(lift, appliedRunLift))
			{
				return;
			}
			float num = lift - appliedRunLift;
			appliedRunLift = lift;
			Vector3 vector = appliedRunLean * (Vector3.up * num);
			foreach (VoxelModel item in OwnedPieces())
			{
				item.transform.localPosition += vector;
			}
			SyncControllerToBody();
		}

		public void SetRunLean(Quaternion lean, Vector3 pivotLocal)
		{
			if (voxelBodyRoot == null)
			{
				return;
			}
			bool num = Quaternion.Angle(lean, appliedRunLean) < 0.01f;
			bool flag = (pivotLocal - appliedRunLeanPivot).sqrMagnitude < 1E-08f;
			if (num && (flag || Quaternion.Angle(lean, Quaternion.identity) < 0.01f))
			{
				return;
			}
			Quaternion quaternion = Quaternion.Inverse(appliedRunLean);
			foreach (VoxelModel item in OwnedPieces())
			{
				Transform transform = item.transform;
				Vector3 vector = appliedRunLeanPivot + quaternion * (transform.localPosition - appliedRunLeanPivot);
				Quaternion quaternion2 = quaternion * transform.localRotation;
				transform.localPosition = pivotLocal + lean * (vector - pivotLocal);
				transform.localRotation = lean * quaternion2;
			}
			appliedRunLean = lean;
			appliedRunLeanPivot = pivotLocal;
		}

		public void GetRestPose(VoxelModel piece, out Vector3 localPosition, out Quaternion localRotation)
		{
			Quaternion quaternion = Quaternion.Inverse(appliedRunLean);
			localPosition = appliedRunLeanPivot + quaternion * (piece.transform.localPosition - appliedRunLeanPivot) - Vector3.up * appliedRunLift;
			localRotation = quaternion * piece.transform.localRotation;
		}

		private void ClearBody()
		{
			UndoManager.Clear();
			DestroySplitPieces();
			bodyDirty = false;
			SubmitBodyServerRpc(new VoxelModelPayload
			{
				Data = Array.Empty<byte>()
			});
			ResetBodyPose();
			SyncControllerToBody();
		}

		private void DestroySplitPieces()
		{
			foreach (VoxelModel item in Pieces(includeInactive: true))
			{
				if (!(item == voxelModel))
				{
					item.gameObject.SetActive(value: false);
					UnityEngine.Object.Destroy(item.gameObject);
				}
			}
			mirrorPieces.Clear();
		}

		private void OnEditStepApplied(IUndoableCommand command, UndoManager.EditStepKind kind)
		{
			MarkBodyChanged();
			PushBody();
		}

		private IEnumerable<VoxelModel> OwnedPieces()
		{
			return Pieces(includeInactive: false);
		}

		public IEnumerable<VoxelModel> BodyPieces()
		{
			return OwnedPieces();
		}

		private IEnumerable<VoxelModel> Pieces(bool includeInactive)
		{
			Transform bodyParent = ((this.voxelModel != null) ? this.voxelModel.transform.parent : null);
			VoxelModel[] componentsInChildren = GetComponentsInChildren<VoxelModel>(includeInactive);
			foreach (VoxelModel voxelModel in componentsInChildren)
			{
				if (bodyParent == null || voxelModel.transform.parent == bodyParent)
				{
					yield return voxelModel;
				}
			}
		}

		public void SetStrandedPiecesHidden(bool hidden)
		{
			if (base.IsOwner)
			{
				StrandedPieceVisibility.Apply(Pieces(includeInactive: true), hidden);
			}
		}

		public void ApplyUnlitSetting()
		{
			if (!base.IsOwner)
			{
				return;
			}
			foreach (VoxelModel item in OwnedPieces())
			{
				item.SetUnlit(VoxelEditorSettings.UnlitMode);
			}
		}

		private void ClampBodyStepOffsets()
		{
			if (voxelBodyRoot == null)
			{
				return;
			}
			CharacterController[] componentsInChildren = voxelBodyRoot.GetComponentsInChildren<CharacterController>(includeInactive: true);
			foreach (CharacterController characterController in componentsInChildren)
			{
				float num = Mathf.Max(characterController.transform.lossyScale.y, 0.0001f);
				float num2 = (characterController.height * num + characterController.radius * num * 2f) * 0.95f;
				if (characterController.stepOffset > num2)
				{
					characterController.stepOffset = num2;
				}
			}
		}

		private void SyncControllerToBody()
		{
			if (ownController == null)
			{
				ownController = GetComponent<CharacterController>();
			}
			if (ownController == null)
			{
				return;
			}
			if (!capturedDefaultCapsule)
			{
				capturedDefaultCapsule = true;
				defaultRadius = ownController.radius;
				defaultHeight = ownController.height;
				defaultCenter = ownController.center;
				defaultStepOffset = ownController.stepOffset;
				defaultSkinWidth = ownController.skinWidth;
			}
			if (!HasVoxelBody || !TryGetLiveBoundsLocal(out var bounds))
			{
				ApplyCapsule(defaultRadius, defaultHeight, defaultCenter, defaultStepOffset, defaultSkinWidth);
				hasCapsuleFrame = false;
				return;
			}
			bounds.center -= Vector3.up * appliedRunLift;
			if (appliedRunLift > 0f)
			{
				bounds.Encapsulate(new Vector3(bounds.center.x, bounds.max.y + appliedRunLift, bounds.center.z));
			}
			Quaternion quaternion = BodyCapsule.UprightFrame(base.transform);
			bounds = BodyCapsule.Reframe(bounds, Quaternion.Inverse(quaternion) * base.transform.rotation);
			BodyCapsule.Shape shape = BodyCapsule.Compute(bounds, base.transform.lossyScale.y);
			Vector3 center = Quaternion.Inverse(base.transform.rotation) * (quaternion * shape.Center);
			ApplyCapsule(shape.Radius, shape.Height, center, Mathf.Min(defaultStepOffset, shape.WorldHeight), shape.SkinWidth);
			lastCapsuleFrame = quaternion;
			hasCapsuleFrame = true;
		}

		private void KeepCapsuleFittedToPose()
		{
			if (HasVoxelBody)
			{
				Quaternion a = BodyCapsule.UprightFrame(base.transform);
				if (!hasCapsuleFrame || !(Quaternion.Angle(a, lastCapsuleFrame) < 1f))
				{
					SyncControllerToBody();
				}
			}
		}

		private void ApplyCapsule(float radius, float height, Vector3 center, float stepOffset, float skinWidth)
		{
			if (!Mathf.Approximately(ownController.radius, radius) || !Mathf.Approximately(ownController.height, height) || !Mathf.Approximately(ownController.stepOffset, stepOffset) || !Mathf.Approximately(ownController.skinWidth, skinWidth) || !(ownController.center == center))
			{
				bool num = ownController.enabled;
				if (num)
				{
					ownController.enabled = false;
				}
				ownController.radius = radius;
				ownController.height = height;
				ownController.center = center;
				ownController.stepOffset = stepOffset;
				ownController.skinWidth = skinWidth;
				if (num)
				{
					ownController.enabled = true;
				}
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetAlignOverride()
		{
			AlignDistanceUnlimited = false;
		}

		private bool IsCapsuleBuriedAt(Vector3 capsuleCenterWorld, out Collider blocker)
		{
			blocker = null;
			if (ownController == null)
			{
				ownController = GetComponent<CharacterController>();
			}
			if (ownController == null)
			{
				return false;
			}
			float num = Mathf.Max(base.transform.lossyScale.y, 0.0001f);
			float margin = ownController.skinWidth * num + 0.01f;
			return SolidSpaceProbe.IsCapsuleBuriedInSolid(capsuleCenterWorld, base.transform.up, ownController.radius * num, ownController.height * num, margin, base.transform, out blocker);
		}

		private bool TryFindPushOut(Vector3 capsuleCenterWorld, Collider blocker, out Vector3 shift)
		{
			shift = Vector3.zero;
			Vector3 direction = Vector3.zero;
			if (blocker != null)
			{
				Vector3 vector = capsuleCenterWorld - blocker.ClosestPoint(capsuleCenterWorld);
				if (vector.sqrMagnitude > 1E-06f)
				{
					direction = vector.normalized;
				}
			}
			for (float num = 0.125f; num <= 0.501f; num += 0.125f)
			{
				if (TryFreeShift(capsuleCenterWorld, Vector3.up, num, out shift) || TryFreeShift(capsuleCenterWorld, direction, num, out shift))
				{
					return true;
				}
				Vector3[] pushOutHorizontals = PushOutHorizontals;
				foreach (Vector3 direction2 in pushOutHorizontals)
				{
					if (TryFreeShift(capsuleCenterWorld, direction2, num, out shift))
					{
						return true;
					}
				}
			}
			shift = Vector3.zero;
			return false;
		}

		private bool TryFreeShift(Vector3 capsuleCenterWorld, Vector3 direction, float distance, out Vector3 shift)
		{
			shift = Vector3.zero;
			if (direction.sqrMagnitude < 0.5f)
			{
				return false;
			}
			shift = direction * distance;
			Collider blocker;
			return !IsCapsuleBuriedAt(capsuleCenterWorld + shift, out blocker);
		}

		private Vector3 BodyAnchorLocal(Bounds bounds)
		{
			if (!IsClinging)
			{
				return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
			}
			return new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);
		}

		public void CenterBodyOnCharacter()
		{
			if (!base.IsOwner || !HasVoxelBody || !TryMeasureBodyInPlayerSpace(out var bounds))
			{
				return;
			}
			Vector3 vector = BodyAnchorLocal(bounds);
			if (vector.sqrMagnitude < 1E-08f)
			{
				return;
			}
			foreach (VoxelModel item in OwnedPieces())
			{
				item.transform.localPosition -= vector;
			}
			MarkBodyChanged();
			SyncControllerToBody();
			PushBody();
		}

		public bool AlignCharacterToBody()
		{
			if (!base.IsOwner)
			{
				return false;
			}
			SyncControllerToBody();
			if (!HasVoxelBody || !TryMeasureBodyInPlayerSpace(out var bounds))
			{
				return false;
			}
			if (ownController == null)
			{
				ownController = GetComponent<CharacterController>();
			}
			Vector3 vector = BodyAnchorLocal(bounds);
			Vector3 vector2 = base.transform.TransformVector(vector);
			bool flag = vector.sqrMagnitude > 1E-08f;
			if (flag && !AlignDistanceUnlimited && vector2.magnitude > 2f)
			{
				return RefuseAlignment("Model çok uzakta - modelin sana geri getirildi.");
			}
			Vector3 shift = Vector3.zero;
			if (IsCapsuleBuriedAt(CapsuleCenterWorld, out var blocker) && !TryFindPushOut(CapsuleCenterWorld, blocker, out shift))
			{
				return RefuseAlignment("Modelin duvarın içinde - modelin sana geri getirildi.");
			}
			if (!flag && shift.sqrMagnitude < 1E-08f)
			{
				return false;
			}
			bool flag2 = ownController != null && ownController.enabled;
			if (flag2)
			{
				ownController.enabled = false;
			}
			base.transform.position += vector2 + shift;
			foreach (VoxelModel item in OwnedPieces())
			{
				item.transform.localPosition -= vector;
			}
			MarkBodyChanged();
			if (flag2)
			{
				ownController.enabled = true;
			}
			SyncControllerToBody();
			if (flag)
			{
				PushBody();
			}
			return true;
		}

		private bool RefuseAlignment(string reason)
		{
			CenterBodyOnCharacter();
			bool result = TryUnbury();
			if (ToastView.Instance != null)
			{
				ToastView.Instance.Show(reason);
			}
			AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.editDeniedClip);
			return result;
		}

		public bool TryUnbury()
		{
			if (!base.IsOwner)
			{
				return false;
			}
			if (ownController == null)
			{
				ownController = GetComponent<CharacterController>();
			}
			if (ownController == null)
			{
				return false;
			}
			if (!IsCapsuleBuriedAt(CapsuleCenterWorld, out var blocker))
			{
				return false;
			}
			if (!TryFindPushOut(CapsuleCenterWorld, blocker, out var shift))
			{
				return false;
			}
			bool num = ownController.enabled;
			if (num)
			{
				ownController.enabled = false;
			}
			base.transform.position += shift;
			if (num)
			{
				ownController.enabled = true;
			}
			return true;
		}

		private void MarkBodyChanged()
		{
			BodyVersion++;
		}

		public bool TryGetBodyTopLocalY(out float topY)
		{
			topY = 0f;
			if (!HasVoxelBody || !TryMeasureBodyInPlayerSpace(out var bounds))
			{
				return false;
			}
			topY = bounds.max.y;
			return true;
		}

		public bool TryGetBodyBoundsLocal(out Bounds bounds)
		{
			if (!HasVoxelBody || !TryMeasureBodyInPlayerSpace(out bounds))
			{
				return Fail(out bounds);
			}
			return true;
		}

		private static bool Fail(out Bounds bounds)
		{
			bounds = default(Bounds);
			return false;
		}

		public bool TryGetCameraPivotLocal(out Vector3 pivotLocal)
		{
			pivotLocal = default(Vector3);
			if (!TryGetLiveBoundsLocal(out var bounds))
			{
				return false;
			}
			pivotLocal = bounds.center;
			return true;
		}

		public bool TryGetLiveBoundsLocal(out Bounds bounds)
		{
			bounds = default(Bounds);
			if (!HasVoxelBody)
			{
				return false;
			}
			if (cachedBoundsVersion != BodyVersion)
			{
				cachedBoundsVersion = BodyVersion;
				hasCachedBounds = TryMeasureBodyInPlayerSpace(out var bounds2);
				if (hasCachedBounds)
				{
					bounds2.center -= Vector3.up * appliedRunLift;
					cachedRestBounds = bounds2;
				}
			}
			if (!hasCachedBounds)
			{
				return false;
			}
			bounds = cachedRestBounds;
			bounds.center += Vector3.up * appliedRunLift;
			return true;
		}

		public bool IsInsideBody(Vector3 worldPoint, float margin)
		{
			if (!TryGetLiveBoundsLocal(out var bounds))
			{
				return false;
			}
			bounds.Expand(margin * 2f);
			return bounds.Contains(base.transform.InverseTransformPoint(worldPoint));
		}

		public void SetBodyHiddenFromOwner(bool hidden)
		{
			if (!base.IsOwner || bodyHiddenFromOwner == hidden)
			{
				return;
			}
			bodyHiddenFromOwner = hidden;
			if (TryGetLiveBoundsLocal(out var bounds))
			{
				UnityEngine.Debug.Log("[PlayerVoxelBody] Kendi govden " + (hidden ? "GIZLENDI" : "geri geldi") + " - " + $"govde kutusu merkez={bounds.center} boyut={bounds.size} (oyuncu uzayinda).", this);
			}
			ShadowCastingMode shadowCastingMode = ((!hidden) ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly);
			foreach (VoxelModel item in Pieces(includeInactive: true))
			{
				Renderer component = item.GetComponent<Renderer>();
				if (component != null)
				{
					component.shadowCastingMode = shadowCastingMode;
				}
			}
		}

		public bool TryGetBodyCenterWorld(out Vector3 center)
		{
			center = base.transform.position;
			if (!HasVoxelBody || !TryMeasureBodyInPlayerSpace(out var bounds))
			{
				return false;
			}
			center = base.transform.TransformPoint(bounds.center);
			return true;
		}

		private bool TryMeasureBodyInPlayerSpace(out Bounds bounds)
		{
			bounds = default(Bounds);
			bool flag = false;
			Quaternion quaternion = Quaternion.Inverse(appliedRunLean);
			Vector3 vector = appliedRunLeanPivot;
			VoxelModel[] componentsInChildren = base.transform.GetComponentsInChildren<VoxelModel>(includeInactive: false);
			foreach (VoxelModel voxelModel in componentsInChildren)
			{
				if (voxelModel.Grid == null || !GridBounds.TryCompute(voxelModel.Grid, out var min, out var max))
				{
					continue;
				}
				for (int j = 0; j < 8; j++)
				{
					Vector3 position = new Vector3(((j & 1) == 0) ? min.x : (max.x + 1), ((j & 2) == 0) ? min.y : (max.y + 1), ((j & 4) == 0) ? min.z : (max.z + 1));
					Vector3 vector2 = base.transform.InverseTransformPoint(voxelModel.transform.TransformPoint(position));
					vector2 = vector + quaternion * (vector2 - vector);
					if (!flag)
					{
						bounds = new Bounds(vector2, Vector3.zero);
						flag = true;
					}
					else
					{
						bounds.Encapsulate(vector2);
					}
				}
			}
			return flag;
		}

		private void PushBody()
		{
			double timeAsDouble = Time.timeAsDouble;
			if (!bodyDirty)
			{
				firstDirtyTime = timeAsDouble;
			}
			bodyDirty = true;
			lastChangeTime = timeAsDouble;
		}

		private void TrySendBody()
		{
			if (!bodyDirty)
			{
				return;
			}
			double timeAsDouble = Time.timeAsDouble;
			if (!(timeAsDouble < nextSendAllowedTime))
			{
				bool num = timeAsDouble - lastChangeTime >= 0.35;
				bool flag = timeAsDouble - firstDirtyTime >= 1.0;
				if (num || flag)
				{
					bodyDirty = false;
					nextSendAllowedTime = timeAsDouble + 0.25;
					SendBodyNow();
				}
			}
		}

		private void SendBodyNow()
		{
			RefreshClearanceWithheld();
			VoxelBodyData voxelBodyData = new VoxelBodyData
			{
				voxelSize = voxelModel.VoxelSize
			};
			foreach (VoxelModel item in OwnedPieces())
			{
				DetachFromOwnController(item);
				GetRestPose(item, out var localPosition, out var localRotation);
				voxelBodyData.pieces.Add(new VoxelPieceData
				{
					grid = (clearanceWithheld.Contains(item) ? new VoxelGrid() : item.Grid),
					localPosition = localPosition,
					localRotation = localRotation
				});
			}
			BlankStrandedPieces(voxelBodyData);
			Stopwatch stopwatch = Stopwatch.StartNew();
			byte[] array = ((voxelBodyData.pieces.Count > 0) ? VoxelBodyCodec.Encode(voxelBodyData) : Array.Empty<byte>());
			stopwatch.Stop();
			SendCount++;
			LastEncodeMs = stopwatch.Elapsed.TotalMilliseconds;
			EncodeMsTotal += LastEncodeMs;
			LastPayloadBytes = array.Length;
			SubmitBodyServerRpc(new VoxelModelPayload
			{
				Data = array
			});
		}

		private void BlankStrandedPieces(VoxelBodyData body)
		{
			if (body.pieces.Count < 2)
			{
				return;
			}
			float num = Mathf.Max(body.voxelSize, 0.0001f);
			List<VoxelBodyPiece> list = new List<VoxelBodyPiece>(body.pieces.Count);
			foreach (VoxelPieceData piece in body.pieces)
			{
				list.Add(VoxelBodyPiece.FromGrid(piece.grid, Matrix4x4.TRS(piece.localPosition / num, piece.localRotation, Vector3.one)));
			}
			VoxelBodyReport voxelBodyReport = VoxelBodyRules.Evaluate(list);
			if (!voxelBodyReport.HasInvalidPieces)
			{
				return;
			}
			if (voxelBodyReport.StrandedPieces != null)
			{
				foreach (int strandedPiece in voxelBodyReport.StrandedPieces)
				{
					body.pieces[strandedPiece].grid = new VoxelGrid();
				}
			}
			if (voxelBodyReport.OverlappingPieces != null)
			{
				foreach (int overlappingPiece in voxelBodyReport.OverlappingPieces)
				{
					body.pieces[overlappingPiece].grid = new VoxelGrid();
				}
			}
			if (voxelBodyReport.SmallPieces == null)
			{
				return;
			}
			foreach (int smallPiece in voxelBodyReport.SmallPieces)
			{
				body.pieces[smallPiece].grid = new VoxelGrid();
			}
		}

		private void CheckClearancePeriodically()
		{
			if (!(Time.unscaledTime < nextClearanceCheck))
			{
				nextClearanceCheck = Time.unscaledTime + 1f;
				if (RefreshClearanceWithheld())
				{
					PushBody();
				}
			}
		}

		private bool RefreshClearanceWithheld()
		{
			clearancePieces.Clear();
			if (HasVoxelBody)
			{
				clearancePieces.AddRange(OwnedPieces());
			}
			bool result = clearanceWithheld.RemoveWhere(IsNoLongerOwned) > 0;
			if (!BodyClearance.Applies || clearancePieces.Count == 0)
			{
				if (clearanceWithheld.Count > 0)
				{
					clearanceWithheld.Clear();
					result = true;
				}
				return result;
			}
			clearanceScratch.Clear();
			foreach (VoxelModel clearancePiece in clearancePieces)
			{
				ClearanceResult clearanceResult = MeasureAtRest(clearancePiece);
				float num = (clearanceWithheld.Contains(clearancePiece) ? 0.2f : 0.35f);
				if (clearanceResult.Burial > num || clearanceResult.OutsideCorners > 0)
				{
					clearanceScratch.Add(clearancePiece);
				}
			}
			if (clearanceScratch.Count >= clearancePieces.Count)
			{
				clearanceScratch.Clear();
			}
			if (clearanceScratch.SetEquals(clearanceWithheld))
			{
				return result;
			}
			clearanceWithheld.Clear();
			clearanceWithheld.UnionWith(clearanceScratch);
			return true;
		}

		private bool IsNoLongerOwned(VoxelModel piece)
		{
			if (!(piece == null))
			{
				return !clearancePieces.Contains(piece);
			}
			return true;
		}

		private ClearanceResult MeasureAtRest(VoxelModel piece)
		{
			GetRestPose(piece, out var localPosition, out var localRotation);
			Matrix4x4 localToWorld = base.transform.localToWorldMatrix * Matrix4x4.TRS(localPosition, localRotation, piece.transform.localScale);
			return BodyClearance.Measure(PieceShape.Of(piece.Grid), localToWorld, base.transform);
		}

		public static HashSet<VoxelModel> LocallyWithheld()
		{
			PlayerVoxelBody playerVoxelBody = LocalBody();
			if (!(playerVoxelBody != null) || !playerVoxelBody.IsOwner)
			{
				return null;
			}
			return playerVoxelBody.clearanceWithheld;
		}

		public string ClearanceReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Clearance rules: " + (BodyClearance.Applies ? "on" : "off (customizer or mode)") + ", map baked: " + (MapBounds.AnyBaked ? "yes" : "no"));
			int num = 0;
			foreach (VoxelModel item in OwnedPieces())
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				PieceShapeData pieceShapeData = PieceShape.Of(item.Grid);
				ClearanceResult clearanceResult = MeasureAtRest(item);
				stopwatch.Stop();
				string text = ((clearanceResult.Blocker != null) ? clearanceResult.Blocker.name : "-");
				stringBuilder.AppendLine($"  piece {num}: {pieceShapeData.Boxes.Count} boxes, buried {clearanceResult.Burial:P0}, " + $"corners outside {clearanceResult.OutsideCorners}, centres outside {clearanceResult.OutsideCentres}, " + "inside " + text + ", withheld " + (clearanceWithheld.Contains(item) ? "YES" : "no") + ", " + $"{stopwatch.Elapsed.TotalMilliseconds:F2} ms");
				num++;
			}
			stringBuilder.Append($"Limits: withheld above {0.35f:P0} buried (back under " + $"{0.2f:P0}) or any corner outside; the server refuses above " + $"{0.6f:P0} or any box centre outside.");
			if (base.IsServer)
			{
				stringBuilder.Append($"\nServer has {acceptedShapes.Count} accepted piece shapes for this body.");
			}
			return stringBuilder.ToString();
		}

		private bool ServerAcceptsPlacement(byte[] data, out string rejection)
		{
			rejection = null;
			pendingShapes.Clear();
			GameModeController current = GameModeController.Current;
			if (current != null && !current.EnforcesBodyClearance)
			{
				acceptedShapes.Clear();
				return true;
			}
			if (!VoxelBodyCodec.TryDecodeSummary(data, out var summary, 200000, 64, findIslands: false, findShapes: true))
			{
				rejection = "Reject.ModelUnreadable";
				return false;
			}
			bool flag = current != null && current.AllowsOutsidePlayArea(base.OwnerClientId);
			foreach (VoxelPieceSummary piece in summary.Pieces)
			{
				PlacedShape placedShape = new PlacedShape
				{
					Shape = (piece.HasVoxels ? piece.Shape : null),
					Position = piece.LocalPosition,
					Rotation = piece.LocalRotation
				};
				pendingShapes.Add(placedShape);
				if (placedShape.Shape != null)
				{
					ClearanceResult clearanceResult = BodyClearance.Measure(placedShape.Shape, Placed(placedShape, summary.VoxelSize), base.transform);
					if (!flag && clearanceResult.OutsideCentres > 0)
					{
						rejection = "Reject.ModelOutsideMap";
						return false;
					}
					if (clearanceResult.Burial > 0.6f)
					{
						UnityEngine.Debug.LogWarning($"[PlayerVoxelBody] Gövde parçası geometrinin içinde (owner {base.OwnerClientId}): " + string.Format("%{0:F0}, {1}", clearanceResult.Burial * 100f, (clearanceResult.Blocker != null) ? clearanceResult.Blocker.name : "?"));
						rejection = "Reject.ModelInsideGeometry";
						return false;
					}
				}
			}
			acceptedShapes.Clear();
			acceptedShapes.AddRange(pendingShapes);
			acceptedPayload = data;
			acceptedVoxelSize = summary.VoxelSize;
			serverClearanceBadFor = 0f;
			return true;
		}

		private Matrix4x4 Placed(PlacedShape placed, float voxelSize)
		{
			return base.transform.localToWorldMatrix * Matrix4x4.TRS(placed.Position, placed.Rotation, Vector3.one * voxelSize);
		}

		public PlacementVerdict ServerCheckPlacement(float elapsedSeconds, bool allowOutside)
		{
			if (!base.IsServer || base.IsOwner || companionMode || acceptedShapes.Count == 0)
			{
				return PlacementVerdict.Clear;
			}
			if (!SamePayload(ModelData.Value.Data, acceptedPayload))
			{
				acceptedShapes.Clear();
				acceptedPayload = null;
				return PlacementVerdict.Clear;
			}
			GameModeController current = GameModeController.Current;
			if (current != null && !current.EnforcesBodyClearance)
			{
				return PlacementVerdict.Clear;
			}
			badPieces.Clear();
			int num = 0;
			for (int i = 0; i < acceptedShapes.Count; i++)
			{
				PlacedShape placed = acceptedShapes[i];
				if (placed.Shape != null)
				{
					num++;
					ClearanceResult clearanceResult = BodyClearance.Measure(placed.Shape, Placed(placed, acceptedVoxelSize), base.transform);
					if (clearanceResult.Burial > 0.6f || (!allowOutside && clearanceResult.OutsideCentres > 0))
					{
						badPieces.Add(i);
					}
				}
			}
			if (badPieces.Count == 0)
			{
				serverClearanceBadFor = 0f;
				return PlacementVerdict.Clear;
			}
			serverClearanceBadFor += elapsedSeconds;
			if (serverClearanceBadFor < 2f)
			{
				return PlacementVerdict.Pending;
			}
			serverClearanceBadFor = 0f;
			if (badPieces.Count >= num || !BlankAcceptedPieces(badPieces))
			{
				return PlacementVerdict.SendBack;
			}
			return PlacementVerdict.Sanitized;
		}

		private bool BlankAcceptedPieces(List<int> indices)
		{
			if (!VoxelBodyCodec.TryDecode(ModelData.Value.Data, out var body) || body.pieces.Count != acceptedShapes.Count)
			{
				return false;
			}
			foreach (int index in indices)
			{
				body.pieces[index].grid = new VoxelGrid();
				PlacedShape value = acceptedShapes[index];
				value.Shape = null;
				acceptedShapes[index] = value;
			}
			UnityEngine.Debug.LogWarning($"[PlayerVoxelBody] {indices.Count} parça geometrinin içinde ya da harita dışında " + $"kaldı (owner {base.OwnerClientId}) - kabul edilen gövdeden çıkarıldı.");
			acceptedPayload = VoxelBodyCodec.Encode(body);
			ModelData.Value = new VoxelModelPayload
			{
				Data = acceptedPayload
			};
			return true;
		}

		private static bool SamePayload(byte[] a, byte[] b)
		{
			if (a != b)
			{
				if (a != null && b != null)
				{
					return MemoryExtensions.SequenceEqual<byte>(a, b);
				}
				return false;
			}
			return true;
		}

		[ServerRpc]
		private void SubmitBodyServerRpc(VoxelModelPayload payload)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				UnityEngine.Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				if (base.OwnerClientId != networkManager.LocalClientId)
				{
					if (networkManager.LogLevel <= LogLevel.Normal)
					{
						UnityEngine.Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
					}
					return;
				}
				ServerRpcParams serverRpcParams = default(ServerRpcParams);
				FastBufferWriter bufferWriter = __beginSendServerRpc(974676354u, serverRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in payload, default(FastBufferWriter.ForNetworkSerializable));
				__endSendServerRpc(ref bufferWriter, 974676354u, serverRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsServer && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			bool flag = payload.Data == null || payload.Data.Length == 0;
			if (!flag)
			{
				GameModeController current = GameModeController.Current;
				if (current == null)
				{
					return;
				}
				if (!current.ServerAllowsBodyEdit(base.OwnerClientId))
				{
					Reject("Reject.ModelLocked");
					return;
				}
				if (base.NetworkManager.ServerTime.Time < nextAcceptedSubmitServerTime)
				{
					Reject("Reject.ModelTooOften");
					return;
				}
				nextAcceptedSubmitServerTime = base.NetworkManager.ServerTime.Time + 0.1;
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			string rejection;
			bool flag2 = VoxelBodyValidator.Accepts(expectedVoxelSize: (voxelModel != null) ? voxelModel.VoxelSize : 0f, data: payload.Data, rejection: out rejection);
			if (flag2 && !flag)
			{
				flag2 = ServerAcceptsPlacement(payload.Data, out rejection);
			}
			else if (flag)
			{
				acceptedShapes.Clear();
			}
			stopwatch.Stop();
			ValidateCount++;
			LastValidateMs = stopwatch.Elapsed.TotalMilliseconds;
			ValidateMsTotal += LastValidateMs;
			if (!flag2)
			{
				Reject(rejection);
			}
			else
			{
				ModelData.Value = payload;
			}
		}

		private void Reject(string reasonKey)
		{
			UnityEngine.Debug.LogWarning($"[PlayerVoxelBody] Gövde reddedildi (owner {base.OwnerClientId}): {reasonKey}");
			RejectBodyClientRpc(reasonKey, new ClientRpcParams
			{
				Send = new ClientRpcSendParams
				{
					TargetClientIds = new ulong[1] { base.OwnerClientId }
				}
			});
		}

		[ClientRpc]
		private void RejectBodyClientRpc(string reasonKey, ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				UnityEngine.Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(3556744699u, clientRpcParams, RpcDelivery.Reliable);
				bool value = reasonKey != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(reasonKey);
				}
				__endSendClientRpc(ref bufferWriter, 3556744699u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get(reasonKey));
				}
				AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.editDeniedClip);
				Telemetry.Send("body_rejected", ("reason", reasonKey), ("origin", "round"));
			}
		}

		public void SetLocallyConcealed(bool concealed)
		{
			if (locallyConcealed != concealed)
			{
				locallyConcealed = concealed;
				ApplyPayload(ModelData.Value);
			}
		}

		public void SetOwnerSpectating(bool spectating)
		{
			if (base.IsOwner && ownerSpectating != spectating)
			{
				ownerSpectating = spectating;
				ApplyPayload(ModelData.Value);
			}
		}

		public void SetCompanionMode(bool active)
		{
			if (companionMode != active)
			{
				companionMode = active;
				ApplyPayload(ModelData.Value);
				SyncControllerToBody();
			}
		}

		private void OnModelDataChanged(VoxelModelPayload previous, VoxelModelPayload current)
		{
			ApplyPayload(current);
		}

		private void SetCharacterModelVisible(bool visible)
		{
			if (characterModel == null)
			{
				PlayerCameraRig component = GetComponent<PlayerCameraRig>();
				characterModel = ((component != null) ? component.CharacterModel : null);
				if (characterModel == null)
				{
					return;
				}
			}
			if (characterModel.activeSelf != visible)
			{
				characterModel.SetActive(visible);
			}
		}

		public void DevApplyPayload(byte[] data)
		{
			ApplyPayload(new VoxelModelPayload
			{
				Data = (data ?? Array.Empty<byte>())
			});
		}

		public void DevApplyBody(VoxelBodyData body)
		{
			if (body != null && body.pieces.Count != 0 && !(capsuleBody == null) && !(voxelBodyRoot == null) && !(voxelModel == null))
			{
				MarkBodyChanged();
				capsuleBody.SetActive(value: false);
				voxelBodyRoot.SetActive(value: true);
				SetCharacterModelVisible(visible: false);
				voxelModel.SetUnlit(unlit: false);
				ApplyDecoded(body);
			}
		}

		private void ApplyPayload(VoxelModelPayload payload)
		{
			MarkBodyChanged();
			if (locallyConcealed || companionMode || ownerSpectating)
			{
				capsuleBody.SetActive(value: false);
				voxelBodyRoot.SetActive(value: false);
				SetCharacterModelVisible(visible: false);
				ClearMirrorPieces();
				return;
			}
			bool flag = payload.Data != null && payload.Data.Length != 0;
			capsuleBody.SetActive(!flag);
			voxelBodyRoot.SetActive(flag);
			SetCharacterModelVisible(!flag);
			if (!flag)
			{
				ClearMirrorPieces();
				return;
			}
			if (base.IsOwner)
			{
				foreach (VoxelModel item in OwnedPieces())
				{
					DetachFromOwnController(item);
				}
				return;
			}
			voxelModel.SetUnlit(unlit: false);
			if (!VoxelBodyCodec.TryDecode(payload.Data, out var body) || body.pieces.Count == 0)
			{
				UnityEngine.Debug.LogWarning($"[PlayerVoxelBody] Gövde çözülemedi (owner {base.OwnerClientId}, " + $"{payload.Data.Length} bayt) - uzaktaki kopya varsayılan küple kalıyor.");
			}
			else
			{
				ApplyDecoded(body);
			}
		}

		private void ApplyDecoded(VoxelBodyData body)
		{
			ApplyPiece(voxelModel, body.pieces[0]);
			MatchMirrorCount(body.pieces.Count - 1);
			for (int i = 0; i < mirrorPieces.Count; i++)
			{
				ApplyPiece(mirrorPieces[i], body.pieces[i + 1]);
			}
			SyncControllerToBody();
		}

		private void ApplyPiece(VoxelModel target, VoxelPieceData piece)
		{
			target.Grid.Clear();
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in piece.grid.Voxels)
			{
				target.Grid.Set(voxel.Key, voxel.Value);
			}
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in piece.grid.FaceColors)
			{
				target.Grid.SetFaceColor(faceColor.Key.Item1, faceColor.Key.Item2, faceColor.Value);
			}
			target.RebuildMesh();
			target.transform.localPosition = piece.localPosition + Vector3.up * appliedRunLift;
			target.transform.localRotation = piece.localRotation;
			target.SetUnlit(unlit: false);
			DetachFromOwnController(target);
		}

		private void DetachFromOwnController(VoxelModel piece)
		{
			if (ownController == null)
			{
				ownController = GetComponent<CharacterController>();
			}
			Collider component = piece.GetComponent<Collider>();
			if (ownController != null && component != null && ownController.enabled && component.enabled && ownController.gameObject.activeInHierarchy && component.gameObject.activeInHierarchy)
			{
				Physics.IgnoreCollision(ownController, component, ignore: true);
			}
		}

		private void MatchMirrorCount(int wanted)
		{
			for (int num = mirrorPieces.Count - 1; num >= wanted; num--)
			{
				if (mirrorPieces[num] != null)
				{
					UnityEngine.Object.Destroy(mirrorPieces[num].gameObject);
				}
				mirrorPieces.RemoveAt(num);
			}
			while (mirrorPieces.Count < wanted)
			{
				mirrorPieces.Add(CreateMirrorPiece());
			}
		}

		private VoxelModel CreateMirrorPiece()
		{
			GameObject obj = new GameObject("VoxelBody (Piece)");
			obj.transform.SetParent(base.transform, worldPositionStays: false);
			obj.layer = voxelModel.gameObject.layer;
			VoxelModel obj2 = obj.AddComponent<VoxelModel>();
			obj2.ConfigureVoxelSize(voxelModel.VoxelSize);
			obj2.ConfigureMaterials(voxelModel.UnlitMaterial, voxelModel.LitMaterial);
			obj2.SetUnlit(unlit: false);
			return obj2;
		}

		private void ClearMirrorPieces()
		{
			foreach (VoxelModel mirrorPiece in mirrorPieces)
			{
				if (mirrorPiece != null)
				{
					UnityEngine.Object.Destroy(mirrorPiece.gameObject);
				}
			}
			mirrorPieces.Clear();
		}

		protected override void __initializeVariables()
		{
			if (ModelData == null)
			{
				throw new Exception("PlayerVoxelBody.ModelData cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			ModelData.Initialize(this);
			__nameNetworkVariable(ModelData, "ModelData");
			NetworkVariableFields.Add(ModelData);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(974676354u, __rpc_handler_974676354, "SubmitBodyServerRpc", RpcInvokePermission.Owner);
			__registerRpc(3556744699u, __rpc_handler_3556744699, "RejectBodyClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_974676354(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				return;
			}
			if (rpcParams.Server.Receive.SenderClientId != target.OwnerClientId)
			{
				if (networkManager.LogLevel <= LogLevel.Normal)
				{
					UnityEngine.Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
				}
			}
			else
			{
				reader.ReadValueSafe(out VoxelModelPayload value, default(FastBufferWriter.ForNetworkSerializable));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerVoxelBody)target).SubmitBodyServerRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3556744699(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				string s = null;
				if (value)
				{
					reader.ReadValueSafe(out s, false);
				}
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerVoxelBody)target).RejectBodyClientRpc(s, client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerVoxelBody";
		}
	}
}
