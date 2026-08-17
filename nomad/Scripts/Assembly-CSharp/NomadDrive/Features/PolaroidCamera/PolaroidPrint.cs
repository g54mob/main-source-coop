using System;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.SaveSystem;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace NomadDrive.Features.PolaroidCamera
{
	public class PolaroidPrint : HeldItem, INetworkSaveable
	{
		[Header("Decal Settings")]
		[SerializeField]
		private DecalProjector decalProjector;

		[SerializeField]
		private Material decalMaterialTemplate;

		[Tooltip("Self-illumination so the photo stays visible in shadows. 0 = no emission, higher = brighter in dark areas.")]
		[SerializeField]
		[Range(0f, 5f)]
		private float emissiveIntensity = 1.5f;

		[Tooltip("How much scene exposure affects the emissive output. 0 = constant brightness, 1 = fully exposure-aware (may dim in bright scenes).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float emissiveExposureWeight = 0.5f;

		private readonly SyncList<byte> _imageDataSync = new SyncList<byte>();

		private Texture2D _photoTexture;

		private Material _decalMaterialInstance;

		private bool _textureApplied;

		public string ContributorKey => "polaroid";

		protected override void Awake()
		{
			base.Awake();
			if (decalProjector == null)
			{
				decalProjector = GetComponentInChildren<DecalProjector>();
			}
			if (decalProjector == null)
			{
				EvilLogger.LogError("[PolaroidPrint] DecalProjector not found!", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\PolaroidCamera\\PolaroidPrint.cs", 40);
				return;
			}
			if (decalMaterialTemplate == null)
			{
				EvilLogger.LogError("[PolaroidPrint] Decal material template not assigned!", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\PolaroidCamera\\PolaroidPrint.cs", 46);
				return;
			}
			_decalMaterialInstance = new Material(decalMaterialTemplate);
			decalProjector.material = _decalMaterialInstance;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SyncList<byte> imageDataSync = _imageDataSync;
			imageDataSync.Callback = (Action<SyncList<byte>.Operation, int, byte, byte>)Delegate.Combine(imageDataSync.Callback, new Action<SyncList<byte>.Operation, int, byte, byte>(OnImageDataSyncChanged));
			if (_imageDataSync.Count > 0 && !_textureApplied)
			{
				ApplyImageDataFromSyncList();
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			SyncList<byte> imageDataSync = _imageDataSync;
			imageDataSync.Callback = (Action<SyncList<byte>.Operation, int, byte, byte>)Delegate.Remove(imageDataSync.Callback, new Action<SyncList<byte>.Operation, int, byte, byte>(OnImageDataSyncChanged));
		}

		protected void OnDestroy()
		{
			CleanupAll();
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (_imageDataSync.Count > 0 && !_textureApplied)
			{
				ApplyImageDataFromSyncList();
			}
		}

		private void OnImageDataSyncChanged(SyncList<byte>.Operation op, int index, byte oldItem, byte newItem)
		{
			if (IsLateJoinCompleted)
			{
				_ = _textureApplied;
			}
		}

		[Server]
		public void SetImageData(byte[] data)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.PolaroidCamera.PolaroidPrint::SetImageData(System.Byte[])' called when server was not active");
			}
			else if (data != null && data.Length != 0)
			{
				_imageDataSync.Clear();
				_imageDataSync.AddRange(data);
				ApplyImageData(data);
				RpcApplyImageData();
			}
		}

		[ClientRpc]
		private void RpcApplyImageData()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.PolaroidCamera.PolaroidPrint::RpcApplyImageData()", -130848681, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void ApplyImageDataFromSyncList()
		{
			if (_imageDataSync.Count != 0)
			{
				byte[] array = new byte[_imageDataSync.Count];
				for (int i = 0; i < _imageDataSync.Count; i++)
				{
					array[i] = _imageDataSync[i];
				}
				ApplyImageData(array);
			}
		}

		private void ApplyImageData(byte[] data)
		{
			if (_textureApplied || _decalMaterialInstance == null)
			{
				return;
			}
			CleanupPhotoTexture();
			_photoTexture = new Texture2D(2, 2, TextureFormat.RGB24, mipChain: true);
			if (_photoTexture.LoadImage(data))
			{
				_photoTexture.filterMode = FilterMode.Trilinear;
				_photoTexture.anisoLevel = 8;
				_photoTexture.Apply(updateMipmaps: true);
				_decalMaterialInstance.EnableKeyword("_BASECOLORMAP");
				_decalMaterialInstance.EnableKeyword("_BASE_COLOR_MAP");
				_decalMaterialInstance.EnableKeyword("_EMISSIVE_COLOR_MAP");
				_decalMaterialInstance.EnableKeyword("_MATERIAL_AFFECTS_EMISSION");
				_decalMaterialInstance.SetTexture("_BaseColorMap", _photoTexture);
				_decalMaterialInstance.SetColor("_BaseColor", Color.white);
				_decalMaterialInstance.SetTexture("_EmissiveColorMap", _photoTexture);
				if (_decalMaterialInstance.HasProperty("_DecalBlend"))
				{
					_decalMaterialInstance.SetFloat("_DecalBlend", 1f);
				}
				if (_decalMaterialInstance.HasProperty("_AlbedoMode"))
				{
					_decalMaterialInstance.SetFloat("_AlbedoMode", 1f);
				}
				if (_decalMaterialInstance.HasProperty("_NormalBlendSrc"))
				{
					_decalMaterialInstance.SetFloat("_NormalBlendSrc", 0f);
				}
				if (_decalMaterialInstance.HasProperty("_EmissiveColor"))
				{
					_decalMaterialInstance.SetColor("_EmissiveColor", Color.white * emissiveIntensity);
				}
				if (_decalMaterialInstance.HasProperty("_EmissiveColorLDR"))
				{
					_decalMaterialInstance.SetColor("_EmissiveColorLDR", Color.white);
				}
				if (_decalMaterialInstance.HasProperty("_EmissiveIntensity"))
				{
					_decalMaterialInstance.SetFloat("_EmissiveIntensity", emissiveIntensity);
				}
				if (_decalMaterialInstance.HasProperty("_EmissiveExposureWeight"))
				{
					_decalMaterialInstance.SetFloat("_EmissiveExposureWeight", emissiveExposureWeight);
				}
				_textureApplied = true;
			}
			else
			{
				EvilLogger.LogError("[PolaroidPrint] Failed to load image data!", "ApplyImageData", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\PolaroidCamera\\PolaroidPrint.cs", 188);
			}
		}

		private void CleanupPhotoTexture()
		{
			if (_photoTexture != null)
			{
				UnityEngine.Object.Destroy(_photoTexture);
				_photoTexture = null;
			}
			_textureApplied = false;
		}

		private void CleanupAll()
		{
			CleanupPhotoTexture();
			if (_decalMaterialInstance != null)
			{
				UnityEngine.Object.Destroy(_decalMaterialInstance);
				_decalMaterialInstance = null;
			}
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			int count = _imageDataSync.Count;
			string text = ResolveOwnerGuid();
			bool flag = count > 0 && !string.IsNullOrEmpty(text);
			writer.Write((byte)1);
			writer.Write(flag);
			if (flag)
			{
				byte[] array = new byte[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = _imageDataSync[i];
				}
				EvilSave.SaveRaw(SideCarKey(text), array);
			}
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			bool flag = reader.ReadBool();
			if (!NetworkServer.active || !flag)
			{
				return;
			}
			string text = ResolveOwnerGuid();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string key = SideCarKey(text);
			if (EvilSave.HasKey(key))
			{
				byte[] array = EvilSave.LoadRaw(key);
				if (array != null && array.Length != 0)
				{
					SetImageData(array);
				}
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		private string ResolveOwnerGuid()
		{
			PersistentId componentInParent = GetComponentInParent<PersistentId>();
			if (!(componentInParent != null))
			{
				return null;
			}
			return componentInParent.Guid;
		}

		private static string SideCarKey(string guid)
		{
			return "polaroid." + guid;
		}

		public PolaroidPrint()
		{
			InitSyncObject(_imageDataSync);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_RpcApplyImageData()
		{
			if (!base.isServer)
			{
				ApplyImageDataFromSyncList();
			}
		}

		protected static void InvokeUserCode_RpcApplyImageData(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcApplyImageData called on server.");
			}
			else
			{
				((PolaroidPrint)obj).UserCode_RpcApplyImageData();
			}
		}

		static PolaroidPrint()
		{
			RemoteProcedureCalls.RegisterRpc(typeof(PolaroidPrint), "System.Void NomadDrive.Features.PolaroidCamera.PolaroidPrint::RpcApplyImageData()", InvokeUserCode_RpcApplyImageData);
		}
	}
}
