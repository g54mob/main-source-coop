using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Features.Tools;
using UnityEngine;

namespace NomadDrive.Features.PolaroidCamera
{
	public class PolaroidCamera : DirectHeldItem
	{
		[Header("Capture Settings")]
		[SerializeField]
		private Camera captureCamera;

		[SerializeField]
		private int captureResolution = 512;

		[SerializeField]
		[Range(1f, 100f)]
		private int jpegQuality = 92;

		[Header("Print Spawning")]
		[SerializeField]
		private GameObject printPrefab;

		[SerializeField]
		[Tooltip("Auto-filled from printPrefab's asset GUID (OnValidate) so the save system can respawn spawned prints on load. The print prefab MUST be Addressable.")]
		private string printPrefabAddressableGuid;

		[SerializeField]
		private Transform printSpawnPoint;

		[SerializeField]
		private float printEjectForce = 1f;

		[SerializeField]
		private float spawnForwardOffset = 0.3f;

		[Tooltip("How long the spawned print streams its transform (physics fall) before its NetworkTransform goes dormant.")]
		[SerializeField]
		private float printSettleSeconds = 4f;

		[Header("Cooldown")]
		[SerializeField]
		private float captureCooldown = 2f;

		private RenderTexture _renderTexture;

		private float _lastCaptureTime;

		public override string UseActionPromptId => "Camera_TakePhoto";

		protected override void Awake()
		{
			base.Awake();
			if (captureCamera == null)
			{
				captureCamera = GetComponentInChildren<Camera>();
			}
			if (captureCamera == null)
			{
				EvilLogger.LogError("[PolaroidCamera] No capture camera found!", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\PolaroidCamera\\PolaroidCamera.cs", 46);
				return;
			}
			_renderTexture = new RenderTexture(captureResolution, captureResolution, 24, RenderTextureFormat.ARGB32);
			captureCamera.targetTexture = _renderTexture;
			captureCamera.enabled = false;
		}

		protected void OnDestroy()
		{
			if (_renderTexture != null)
			{
				_renderTexture.Release();
				Object.Destroy(_renderTexture);
			}
		}

		public override void OnUseButtonDown()
		{
			base.OnUseButtonDown();
			if (base.isOwned && base.IsEquipped && !(Time.time - _lastCaptureTime < captureCooldown))
			{
				TakePhoto();
			}
		}

		private void TakePhoto()
		{
			if (captureCamera == null || _renderTexture == null)
			{
				EvilLogger.LogError("[PolaroidCamera] Camera or RenderTexture not initialized!", "TakePhoto", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\PolaroidCamera\\PolaroidCamera.cs", 84);
				return;
			}
			_lastCaptureTime = Time.time;
			if (!_renderTexture.IsCreated())
			{
				_renderTexture.Create();
			}
			Transform transform = playerService?.CameraTransform;
			bool num = transform != null;
			Vector3 position = default(Vector3);
			Quaternion rotation = default(Quaternion);
			if (num)
			{
				position = captureCamera.transform.position;
				rotation = captureCamera.transform.rotation;
				captureCamera.transform.SetPositionAndRotation(transform.position, transform.rotation);
			}
			captureCamera.enabled = true;
			captureCamera.Render();
			captureCamera.enabled = false;
			if (num)
			{
				captureCamera.transform.SetPositionAndRotation(position, rotation);
			}
			Texture2D texture2D = _renderTexture.ToTexture2D(TextureFormat.RGB24);
			byte[] imageData = texture2D.EncodeToJPG(jpegQuality);
			Object.Destroy(texture2D);
			Transform obj = ((printSpawnPoint != null) ? printSpawnPoint : base.transform);
			Vector3 forward = obj.forward;
			Vector3 spawnPosition = obj.position + forward * spawnForwardOffset;
			CmdSpawnPrint(imageData, spawnPosition, forward);
		}

		[Command]
		private void CmdSpawnPrint(byte[] imageData, Vector3 spawnPosition, Vector3 ejectDirection)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBytesAndSize(imageData);
			writer.WriteVector3(spawnPosition);
			writer.WriteVector3(ejectDirection);
			SendCommandInternal("System.Void NomadDrive.Features.PolaroidCamera.PolaroidCamera::CmdSpawnPrint(System.Byte[],UnityEngine.Vector3,UnityEngine.Vector3)", 351690460, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayCaptureEffects(Vector3 position)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			SendRPCInternal("System.Void NomadDrive.Features.PolaroidCamera.PolaroidCamera::RpcPlayCaptureEffects(UnityEngine.Vector3)", 1285047091, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSpawnPrint__Byte_005B_005D__Vector3__Vector3(byte[] imageData, Vector3 spawnPosition, Vector3 ejectDirection)
		{
			if (printPrefab == null)
			{
				EvilLogger.LogError("[PolaroidCamera] Print prefab not assigned!", "CmdSpawnPrint", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\PolaroidCamera\\PolaroidCamera.cs", 135);
				return;
			}
			GameObject obj = Object.Instantiate(printPrefab, spawnPosition, Quaternion.LookRotation(ejectDirection));
			PolaroidPrint component = obj.GetComponent<PolaroidPrint>();
			if (component != null)
			{
				component.SetImageData(imageData);
			}
			NetworkServer.Spawn(obj);
			PersistentObject.ServerEnsure(obj, printPrefabAddressableGuid);
			if (obj.TryGetComponent<Rigidbody>(out var component2))
			{
				component2.isKinematic = false;
				component2.useGravity = true;
				component2.AddForce(ejectDirection * printEjectForce, ForceMode.Impulse);
			}
			if (component != null)
			{
				component.ServerOpenTransformStreamingWindow(printSettleSeconds);
			}
			RpcPlayCaptureEffects(spawnPosition);
		}

		protected static void InvokeUserCode_CmdSpawnPrint__Byte_005B_005D__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSpawnPrint called on client.");
			}
			else
			{
				((PolaroidCamera)obj).UserCode_CmdSpawnPrint__Byte_005B_005D__Vector3__Vector3(reader.ReadBytesAndSize(), reader.ReadVector3(), reader.ReadVector3());
			}
		}

		protected void UserCode_RpcPlayCaptureEffects__Vector3(Vector3 position)
		{
		}

		protected static void InvokeUserCode_RpcPlayCaptureEffects__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayCaptureEffects called on server.");
			}
			else
			{
				((PolaroidCamera)obj).UserCode_RpcPlayCaptureEffects__Vector3(reader.ReadVector3());
			}
		}

		static PolaroidCamera()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PolaroidCamera), "System.Void NomadDrive.Features.PolaroidCamera.PolaroidCamera::CmdSpawnPrint(System.Byte[],UnityEngine.Vector3,UnityEngine.Vector3)", InvokeUserCode_CmdSpawnPrint__Byte_005B_005D__Vector3__Vector3, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PolaroidCamera), "System.Void NomadDrive.Features.PolaroidCamera.PolaroidCamera::RpcPlayCaptureEffects(UnityEngine.Vector3)", InvokeUserCode_RpcPlayCaptureEffects__Vector3);
		}
	}
}
