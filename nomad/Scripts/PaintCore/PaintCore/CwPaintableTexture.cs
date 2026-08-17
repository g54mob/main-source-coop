using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PaintCore
{
	public abstract class CwPaintableTexture : MonoBehaviour
	{
		public enum UndoRedoType
		{
			None = 0,
			FullTextureCopy = 1,
			LocalCommandCopy = 2
		}

		public enum SaveLoadType
		{
			Manual = 0,
			Automatic = 1,
			SemiManual = 2
		}

		public enum MipType
		{
			Auto = 0,
			On = 1,
			Off = 2
		}

		public enum FilterType
		{
			Auto = -1,
			Point = 0,
			Bilinear = 1,
			Trilinear = 2
		}

		public enum AnisoType
		{
			Auto = -1,
			Off = 0,
			One = 1,
			Four = 4,
			Eight = 8,
			Sixteen = 16
		}

		public enum WrapType
		{
			Auto = -1,
			Repeat = 0,
			Clamp = 1,
			Mirror = 2,
			MirrorOnce = 3
		}

		public enum ExistingType
		{
			Ignore = 0,
			Use = 1,
			UseAndKeep = 2
		}

		public enum ConversionType
		{
			None = 0,
			Normal = 1,
			Premultiply = 2
		}

		[Serializable]
		public class PaintableTextureEvent : UnityEvent<CwPaintableTexture>
		{
		}

		[SerializeField]
		private CwSlot slot = new CwSlot(0, "_MainTex");

		[FormerlySerializedAs("channel")]
		[SerializeField]
		private CwCoord coord;

		[SerializeField]
		private CwGroup group;

		[FormerlySerializedAs("state")]
		[SerializeField]
		private UndoRedoType undoRedo;

		[SerializeField]
		private int stateLimit = 10;

		[SerializeField]
		private SaveLoadType saveLoad;

		[SerializeField]
		private string saveName;

		[SerializeField]
		private int width = 512;

		[SerializeField]
		private int height = 512;

		[SerializeField]
		private Texture texture;

		[SerializeField]
		private Color color = Color.white;

		[SerializeField]
		private RenderTextureFormat format;

		[SerializeField]
		private MipType mipMaps;

		[SerializeField]
		private FilterType filter = FilterType.Auto;

		[SerializeField]
		private AnisoType aniso = AnisoType.Auto;

		[SerializeField]
		private WrapType wrapU = WrapType.Auto;

		[SerializeField]
		private WrapType wrapV = WrapType.Auto;

		[SerializeField]
		private ExistingType existing = ExistingType.UseAndKeep;

		[SerializeField]
		private ConversionType conversion;

		[SerializeField]
		private Texture localMaskTexture;

		[SerializeField]
		private CwChannel localMaskChannel;

		[SerializeField]
		private string shaderKeyword;

		[SerializeField]
		protected CwHash hash;

		[SerializeField]
		private bool isDummy;

		[SerializeField]
		private bool autoStoreState;

		[SerializeField]
		private string output;

		public static Action<CwPaintableTexture> OnInstanceAdded;

		public static Action<CwPaintableTexture> OnInstanceRemoved;

		[SerializeField]
		private bool activated;

		[SerializeField]
		private RenderTexture current;

		[SerializeField]
		private RenderTexture preview;

		[NonSerialized]
		private List<CwPaintableState> paintableStates = new List<CwPaintableState>();

		[NonSerialized]
		private int stateIndex;

		[NonSerialized]
		private CwModel model;

		[NonSerialized]
		private bool paintableSet;

		[NonSerialized]
		private Texture oldTexture;

		[NonSerialized]
		private List<CwCommand> paintCommands = new List<CwCommand>();

		[NonSerialized]
		private List<CwCommand> previewCommands = new List<CwCommand>();

		[NonSerialized]
		private List<CwCommand> localCommands = new List<CwCommand>();

		private static LinkedList<CwPaintableTexture> instances = new LinkedList<CwPaintableTexture>();

		private LinkedListNode<CwPaintableTexture> instancesNode;

		private static int _Buffer = Shader.PropertyToID("_Buffer");

		private static int _BufferSize = Shader.PropertyToID("_BufferSize");

		public CwSlot Slot
		{
			get
			{
				return slot;
			}
			set
			{
				slot = value;
			}
		}

		public CwCoord Coord
		{
			get
			{
				return coord;
			}
			set
			{
				coord = value;
			}
		}

		public CwGroup Group
		{
			get
			{
				return group;
			}
			set
			{
				group = value;
			}
		}

		public UndoRedoType UndoRedo
		{
			get
			{
				return undoRedo;
			}
			set
			{
				undoRedo = value;
			}
		}

		public int StateLimit
		{
			get
			{
				return stateLimit;
			}
			set
			{
				stateLimit = value;
			}
		}

		public SaveLoadType SaveLoad
		{
			get
			{
				return saveLoad;
			}
			set
			{
				saveLoad = value;
			}
		}

		public string SaveName
		{
			get
			{
				return saveName;
			}
			set
			{
				saveName = value;
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}

		public Texture Texture
		{
			get
			{
				return texture;
			}
			set
			{
				texture = value;
			}
		}

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}

		public RenderTextureFormat Format
		{
			get
			{
				return format;
			}
			set
			{
				format = value;
			}
		}

		public MipType MipMaps
		{
			get
			{
				return mipMaps;
			}
			set
			{
				mipMaps = value;
			}
		}

		public FilterType Filter
		{
			get
			{
				return filter;
			}
			set
			{
				filter = value;
			}
		}

		public AnisoType Aniso
		{
			get
			{
				return aniso;
			}
			set
			{
				aniso = value;
			}
		}

		public WrapType WrapU
		{
			get
			{
				return wrapU;
			}
			set
			{
				wrapU = value;
			}
		}

		public WrapType WrapV
		{
			get
			{
				return wrapV;
			}
			set
			{
				wrapV = value;
			}
		}

		public ExistingType Existing
		{
			get
			{
				return existing;
			}
			set
			{
				existing = value;
			}
		}

		public ConversionType Conversion
		{
			get
			{
				return conversion;
			}
			set
			{
				conversion = value;
			}
		}

		public Texture LocalMaskTexture
		{
			get
			{
				return localMaskTexture;
			}
			set
			{
				localMaskTexture = value;
			}
		}

		public CwChannel LocalMaskChannel
		{
			get
			{
				return localMaskChannel;
			}
			set
			{
				localMaskChannel = value;
			}
		}

		public string ShaderKeyword
		{
			get
			{
				return shaderKeyword;
			}
			set
			{
				shaderKeyword = value;
			}
		}

		public CwHash Hash
		{
			get
			{
				return hash;
			}
			set
			{
				hash = value;
				CwSerialization.TryRegister(this, hash);
			}
		}

		public bool IsDummy
		{
			get
			{
				return isDummy;
			}
			set
			{
				isDummy = value;
			}
		}

		public bool AutoCreateState
		{
			get
			{
				return autoStoreState;
			}
			set
			{
				autoStoreState = value;
			}
		}

		public string Output
		{
			get
			{
				return output;
			}
			set
			{
				output = value;
			}
		}

		public static LinkedList<CwPaintableTexture> Instances => instances;

		public bool Activated => activated;

		public bool CanUndo
		{
			get
			{
				if (undoRedo != UndoRedoType.None)
				{
					return stateIndex > 0;
				}
				return false;
			}
		}

		public bool CanRedo
		{
			get
			{
				if (undoRedo != UndoRedoType.None)
				{
					return stateIndex < paintableStates.Count - 1;
				}
				return false;
			}
		}

		public List<CwPaintableState> States => paintableStates;

		public int StateIndex => stateIndex;

		public CwModel Model
		{
			get
			{
				if (model == null)
				{
					model = GetComponent<CwModel>();
				}
				return model;
			}
		}

		public RenderTexture Current
		{
			get
			{
				return current;
			}
			set
			{
				current = value;
				if (!isDummy)
				{
					ApplyTexture(current);
				}
			}
		}

		public RenderTexture Preview => preview;

		public bool CommandsPending => paintCommands.Count + previewCommands.Count > 0;

		public event Action<CwCommand> OnAddCommand;

		public static event Action<CwPaintableTexture, CwCommand> OnAddCommandGlobal;

		public event Action<bool> OnModified;

		[ContextMenu("Clear States")]
		public void ClearStates()
		{
			if (paintableStates != null)
			{
				for (int num = paintableStates.Count - 1; num >= 0; num--)
				{
					paintableStates[num].Pool();
				}
				paintableStates.Clear();
				stateIndex = 0;
			}
		}

		[ContextMenu("Store State")]
		public void StoreState()
		{
			if (activated)
			{
				if (stateIndex != paintableStates.Count - 1)
				{
					TrimFuture();
					AddState();
				}
				if (undoRedo == UndoRedoType.FullTextureCopy)
				{
					TrimPast();
				}
				stateIndex = paintableStates.Count;
			}
		}

		[ContextMenu("Undo")]
		public void Undo()
		{
			if (CanUndo)
			{
				if (stateIndex == paintableStates.Count)
				{
					AddState();
				}
				ClearCommands();
				stateIndex--;
				switch (undoRedo)
				{
				case UndoRedoType.FullTextureCopy:
				{
					CwPaintableState cwPaintableState = paintableStates[stateIndex];
					Replace(cwPaintableState.Texture, Color.white);
					break;
				}
				case UndoRedoType.LocalCommandCopy:
					RebuildFromCommands();
					break;
				}
				NotifyOnModified(preview: false);
			}
		}

		[ContextMenu("Redo")]
		public void Redo()
		{
			if (CanRedo)
			{
				ClearCommands();
				stateIndex++;
				switch (undoRedo)
				{
				case UndoRedoType.FullTextureCopy:
				{
					CwPaintableState cwPaintableState = paintableStates[stateIndex];
					Replace(cwPaintableState.Texture, Color.white);
					break;
				}
				case UndoRedoType.LocalCommandCopy:
					RebuildFromCommands();
					break;
				}
				NotifyOnModified(preview: false);
			}
		}

		public void SetColor(string html)
		{
			ColorUtility.TryParseHtmlString(html, out color);
		}

		public Vector2 GetCoord(ref CwHit hit)
		{
			return coord switch
			{
				CwCoord.First => hit.First, 
				CwCoord.Second => hit.Second, 
				_ => default(Vector2), 
			};
		}

		public Vector2 GetCoord(ref RaycastHit hit)
		{
			return coord switch
			{
				CwCoord.First => hit.textureCoord, 
				CwCoord.Second => hit.textureCoord2, 
				_ => default(Vector2), 
			};
		}

		private bool StatesContainTextureOrCommands()
		{
			if (stateIndex >= 0 && stateIndex < paintableStates.Count)
			{
				for (int i = 0; i <= stateIndex; i++)
				{
					CwPaintableState cwPaintableState = paintableStates[i];
					if (cwPaintableState.Texture != null || cwPaintableState.Commands.Count > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool LastStateWithTextureOr0(ref int startIndex)
		{
			for (int num = paintableStates.Count - 1; num >= 0; num--)
			{
				if (paintableStates[num].Texture != null)
				{
					startIndex = num;
					return false;
				}
			}
			startIndex = 0;
			return true;
		}

		public void RebuildFromCommands()
		{
			if (StatesContainTextureOrCommands())
			{
				int startIndex = 0;
				if (LastStateWithTextureOr0(ref startIndex))
				{
					Clear(texture, color, updateMips: false);
				}
				Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
				Matrix4x4 rotMatrix = Matrix4x4.Rotate(localToWorldMatrix.rotation);
				Matrix4x4 identity = Matrix4x4.identity;
				for (int i = startIndex; i <= stateIndex; i++)
				{
					CwPaintableState cwPaintableState = paintableStates[i];
					if (cwPaintableState.Texture != null)
					{
						Clear(cwPaintableState.Texture, Color.white, updateMips: false);
						continue;
					}
					int count = cwPaintableState.Commands.Count;
					if (count > 0)
					{
						cwPaintableState.Commands.Sort(CwCommand.Compare);
						for (int j = 0; j < count; j++)
						{
							CwCommand cwCommand = cwPaintableState.Commands[j].SpawnCopy();
							cwCommand.Transform(localToWorldMatrix, rotMatrix, identity);
							paintCommands.Add(cwCommand);
						}
					}
				}
				ExecuteCommands(sendNotifications: false, doSort: false);
			}
			else
			{
				Clear(texture, color);
			}
			NotifyOnModified(preview: false);
		}

		private void AddState()
		{
			CwPaintableState cwPaintableState = CwPaintableState.Pop();
			bool flag = false;
			if (paintableStates.Count == 0 && saveLoad == SaveLoadType.Automatic)
			{
				flag = true;
			}
			switch (undoRedo)
			{
			case UndoRedoType.FullTextureCopy:
				cwPaintableState.Write(current);
				break;
			case UndoRedoType.LocalCommandCopy:
				if (flag)
				{
					cwPaintableState.Write(current, localCommands);
				}
				else
				{
					cwPaintableState.Write(localCommands);
				}
				localCommands.Clear();
				break;
			}
			paintableStates.Add(cwPaintableState);
		}

		private void TrimFuture()
		{
			for (int num = paintableStates.Count - 1; num >= stateIndex; num--)
			{
				CwPaintableState cwPaintableState = paintableStates[num];
				if (num == stateIndex)
				{
					localCommands.AddRange(cwPaintableState.Commands);
					cwPaintableState.Commands.Clear();
				}
				cwPaintableState.Pool();
				paintableStates.RemoveAt(num);
			}
		}

		private void TrimPast()
		{
			for (int num = paintableStates.Count - stateLimit - 1; num >= 0; num--)
			{
				paintableStates[num].Pool();
				paintableStates.RemoveAt(num);
			}
		}

		public void NotifyOnModified(bool preview)
		{
			if (this.OnModified != null)
			{
				this.OnModified(preview);
			}
		}

		public Texture2D GetReadableCopy(bool convertBack = false)
		{
			Texture2D texture2D = null;
			if (activated)
			{
				texture2D = CwCommon.GetReadableCopy(current);
			}
			else
			{
				RenderTexture renderTexture = CwCommon.GetRenderTexture(new RenderTextureDescriptor(width, height, format, 0));
				Texture existingTexture = texture;
				if (existingTexture == null && existing != ExistingType.Ignore && model != null)
				{
					existingTexture = model.GetExistingTexture(slot);
				}
				CwCommandReplace.Blit(renderTexture, existingTexture, color);
				texture2D = CwCommon.GetReadableCopy(renderTexture);
				CwCommon.ReleaseRenderTexture(renderTexture);
			}
			if (convertBack && conversion == ConversionType.Normal)
			{
				for (int i = 0; i < texture2D.height; i++)
				{
					for (int j = 0; j < texture2D.width; j++)
					{
						Color pixel = texture2D.GetPixel(j, i);
						texture2D.SetPixel(j, i, new Color(pixel.r, pixel.g, pixel.b, 1f));
					}
				}
				texture2D.Apply();
			}
			return texture2D;
		}

		public byte[] GetPngData(bool convertBack = false)
		{
			Texture2D readableCopy = GetReadableCopy(convertBack);
			if (readableCopy != null)
			{
				byte[] result = readableCopy.EncodeToPNG();
				CwHelper.Destroy(readableCopy);
				return result;
			}
			return null;
		}

		public byte[] GetTgaData(bool convertBack = false)
		{
			Texture2D readableCopy = GetReadableCopy(convertBack);
			if (readableCopy != null)
			{
				byte[] result = readableCopy.EncodeToTGA();
				CwHelper.Destroy(readableCopy);
				return result;
			}
			return null;
		}

		[ContextMenu("Clear")]
		public void Clear()
		{
			Clear(texture, color);
		}

		public void Clear(Texture texture, Color tint, bool updateMips = true)
		{
			if (activated)
			{
				if (conversion == ConversionType.Normal)
				{
					CwBlit.Normal(current, texture);
				}
				else if (conversion == ConversionType.Premultiply)
				{
					CwBlit.Premultiply(current, texture, tint);
				}
				else
				{
					CwCommandReplace.Blit(current, texture, tint);
				}
				if (updateMips && current.useMipMap)
				{
					current.GenerateMips();
				}
			}
		}

		[ContextMenu("Replace")]
		public void Replace()
		{
			Replace(texture, color);
		}

		public void Replace(Texture texture, Color tint)
		{
			if (texture != null)
			{
				Resize(texture.width, texture.height, copyContents: false);
			}
			else
			{
				Resize(width, height, copyContents: false);
			}
			Clear(texture, tint);
		}

		public bool Resize(int width, int height, bool copyContents = true)
		{
			if (activated && (current.width != width || current.height != height))
			{
				RenderTextureDescriptor descriptor = current.descriptor;
				descriptor.width = width;
				descriptor.height = height;
				RenderTexture renderTexture = CwCommon.GetRenderTexture(descriptor, current);
				if (copyContents)
				{
					CwCommandReplace.Blit(renderTexture, current, Color.white);
					if (renderTexture.useMipMap)
					{
						renderTexture.GenerateMips();
					}
				}
				CwCommon.ReleaseRenderTexture(current);
				current = renderTexture;
				return true;
			}
			return false;
		}

		[ContextMenu("Save")]
		public void Save()
		{
			Save(saveName);
		}

		public void Save(string saveName)
		{
			if (activated && !string.IsNullOrEmpty(saveName))
			{
				CwCommon.SaveBytes(saveName, GetPngData());
			}
		}

		[ContextMenu("Load")]
		public void Load()
		{
			Load(saveName);
		}

		public void Load(string saveName, bool replace = true)
		{
			if (activated)
			{
				LoadFromData(CwCommon.LoadBytes(saveName));
			}
		}

		public void LoadFromData(byte[] data, bool allowResize = true)
		{
			if (data != null && data.Length != 0)
			{
				Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipChain: false, linear: false);
				texture2D.LoadImage(data);
				if (allowResize)
				{
					Replace(texture2D, Color.white);
				}
				else
				{
					Clear(texture2D, Color.white);
				}
				CwHelper.Destroy(texture2D);
			}
		}

		public void HidePreview()
		{
			if (activated && current != null && !isDummy)
			{
				ApplyTexture(current);
			}
		}

		public static void HideAllPreviews()
		{
			foreach (CwPaintableTexture instance in instances)
			{
				instance.HidePreview();
			}
		}

		[ContextMenu("Clear Save")]
		public void ClearSave()
		{
			CwCommon.ClearSave(saveName);
		}

		public static void ClearSave(string saveName)
		{
			CwCommon.ClearSave(saveName);
		}

		[ContextMenu("Copy Size")]
		public void CopySize()
		{
			Texture texture = Slot.FindTexture(base.gameObject);
			if (texture != null)
			{
				width = texture.width;
				height = texture.height;
			}
		}

		[ContextMenu("Copy Texture")]
		public void CopyTexture()
		{
			Texture = Slot.FindTexture(base.gameObject);
		}

		[ContextMenu("Activate")]
		public void Activate()
		{
			if (activated)
			{
				return;
			}
			model = GetComponentInParent<CwModel>();
			if (!(model != null))
			{
				return;
			}
			oldTexture = model.GetExistingTexture(slot);
			int num = width;
			int num2 = height;
			Texture texture = this.texture;
			model.ScaleSize(ref num, ref num2);
			if (texture == null && existing != ExistingType.Ignore)
			{
				texture = oldTexture;
				if (existing == ExistingType.UseAndKeep)
				{
					this.texture = oldTexture;
				}
			}
			string.IsNullOrEmpty(shaderKeyword);
			RenderTextureDescriptor desc = new RenderTextureDescriptor(width, height, format, 0);
			desc.autoGenerateMips = false;
			if (mipMaps == MipType.Auto)
			{
				if (texture != null)
				{
					desc.useMipMap = CwCommon.HasMipMaps(texture);
				}
			}
			else
			{
				desc.useMipMap = mipMaps == MipType.On;
			}
			current = CwCommon.GetRenderTexture(desc);
			if (filter == FilterType.Auto)
			{
				if (texture != null)
				{
					current.filterMode = texture.filterMode;
				}
			}
			else
			{
				current.filterMode = (FilterMode)filter;
			}
			if (aniso == AnisoType.Auto)
			{
				if (texture != null)
				{
					current.anisoLevel = texture.anisoLevel;
				}
			}
			else
			{
				current.anisoLevel = (int)aniso;
			}
			if (wrapU == WrapType.Auto)
			{
				if (texture != null)
				{
					current.wrapModeU = texture.wrapModeU;
				}
			}
			else
			{
				current.wrapModeU = (TextureWrapMode)wrapU;
			}
			if (wrapV == WrapType.Auto)
			{
				if (texture != null)
				{
					current.wrapModeV = texture.wrapModeV;
				}
			}
			else
			{
				current.wrapModeV = (TextureWrapMode)wrapV;
			}
			activated = true;
			Clear(texture, color);
			if (!isDummy)
			{
				ApplyTexture(current);
			}
			if (saveLoad == SaveLoadType.Automatic && !string.IsNullOrEmpty(saveName))
			{
				Load();
			}
			NotifyOnModified(preview: false);
			if (autoStoreState && CwStateManager.AllStatesStored)
			{
				StoreState();
			}
		}

		[ContextMenu("Deactivate")]
		public void Deactivate()
		{
			if (activated)
			{
				if (saveLoad == SaveLoadType.Automatic)
				{
					Save();
				}
				activated = false;
				if (!isDummy)
				{
					ApplyTexture(oldTexture);
				}
				current = CwCommon.ReleaseRenderTexture(current);
				preview = CwCommon.ReleaseRenderTexture(preview);
				ClearCommands();
				ClearStates();
			}
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
			if (OnInstanceAdded != null)
			{
				OnInstanceAdded(this);
			}
			CwSerialization.TryRegister(this, hash);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode);
			instancesNode = null;
			if (OnInstanceRemoved != null)
			{
				OnInstanceRemoved(this);
			}
		}

		protected virtual void OnApplicationPause(bool paused)
		{
			if (paused && activated && saveLoad == SaveLoadType.Automatic && !string.IsNullOrEmpty(saveName))
			{
				Save();
			}
		}

		protected virtual void OnDestroy()
		{
			if (activated)
			{
				if (saveLoad == SaveLoadType.Automatic && !string.IsNullOrEmpty(saveName))
				{
					Save();
				}
				CwCommon.ReleaseRenderTexture(current);
				CwCommon.ReleaseRenderTexture(preview);
				ClearStates();
			}
			CwSerialization.TryRegister(this, default(CwHash));
		}

		public void AddCommand(CwCommand command)
		{
			if (command.Preview)
			{
				command.Index = previewCommands.Count;
				previewCommands.Add(command);
			}
			else
			{
				command.Index = paintCommands.Count;
				paintCommands.Add(command);
				if (undoRedo == UndoRedoType.LocalCommandCopy && !command.Preview)
				{
					CwCommand cwCommand = command.SpawnCopyLocal(base.transform);
					cwCommand.Index = localCommands.Count;
					localCommands.Add(cwCommand);
				}
			}
			if (this.OnAddCommand != null)
			{
				this.OnAddCommand(command);
			}
			if (CwPaintableTexture.OnAddCommandGlobal != null)
			{
				CwPaintableTexture.OnAddCommandGlobal(this, command);
			}
		}

		public void ClearCommand(CwCommand command)
		{
			if (previewCommands.Remove(command))
			{
				command.Pool();
			}
			else if (paintCommands.Remove(command))
			{
				command.Pool();
			}
			else if (localCommands.Remove(command))
			{
				command.Pool();
			}
		}

		public void ClearCommands()
		{
			for (int num = previewCommands.Count - 1; num >= 0; num--)
			{
				previewCommands[num].Pool();
			}
			previewCommands.Clear();
			for (int num2 = paintCommands.Count - 1; num2 >= 0; num2--)
			{
				paintCommands[num2].Pool();
			}
			paintCommands.Clear();
			for (int num3 = localCommands.Count - 1; num3 >= 0; num3--)
			{
				localCommands[num3].Pool();
			}
			localCommands.Clear();
		}

		public void ExecuteCommands(bool sendNotifications, bool doSort)
		{
			if (!activated)
			{
				return;
			}
			bool flag = true;
			if (CommandsPending)
			{
				RenderTexture active = RenderTexture.active;
				if (paintCommands.Count > 0)
				{
					if (doSort)
					{
						paintCommands.Sort(CwCommand.Compare);
					}
					ExecuteCommands(paintCommands, sendNotifications, current, ref preview);
				}
				RenderTexture swap = preview;
				preview = null;
				if (previewCommands.Count > 0)
				{
					preview = swap;
					swap = null;
					if (preview == null)
					{
						preview = CwCommon.GetRenderTexture(current);
					}
					flag = false;
					preview.DiscardContents();
					Graphics.Blit(current, preview);
					if (doSort)
					{
						previewCommands.Sort(CwCommand.Compare);
					}
					ExecuteCommands(previewCommands, sendNotifications, preview, ref swap);
				}
				CwCommon.ReleaseRenderTexture(swap);
				RenderTexture.active = active;
			}
			if (flag)
			{
				preview = CwCommon.ReleaseRenderTexture(preview);
			}
			if (!isDummy)
			{
				ApplyTexture((preview != null) ? preview : current);
			}
		}

		protected virtual void ApplyTexture(Texture texture)
		{
			if (model != null)
			{
				model.ApplyTexture(slot, texture);
			}
		}

		protected virtual void PostExecuteCommands(RenderTexture main)
		{
		}

		private void ExecuteCommands(List<CwCommand> commands, bool sendNotifications, RenderTexture main, ref RenderTexture swap)
		{
			RenderTexture.active = main;
			foreach (CwCommand command in commands)
			{
				if (command.Model.TryGetInstance(out var cwModel) && command.Material.TryGetInstance(out var material))
				{
					Mesh mesh = null;
					Matrix4x4 matrix = Matrix4x4.identity;
					int num = 0;
					CwCoord cwCoord = CwCoord.First;
					if (command.RequireMesh)
					{
						cwModel.GetPrepared(ref mesh, ref matrix, coord);
						num = command.Submesh;
						cwCoord = coord;
					}
					else
					{
						mesh = CwCommon.GetQuadMesh();
					}
					if (swap == null)
					{
						swap = CwCommon.GetRenderTexture(main);
					}
					CwBlit.Blit(swap, mesh, num, main, cwCoord);
					material.SetTexture(_Buffer, swap);
					material.SetVector(_BufferSize, new Vector2(swap.width, swap.height));
					command.Apply(material);
					RenderTexture.active = main;
					CwCommon.Draw(material, command.Pass, mesh, matrix, num, cwCoord);
				}
				command.Pool();
			}
			commands.Clear();
			PostExecuteCommands(main);
			if (main.useMipMap)
			{
				main.GenerateMips();
			}
			if (sendNotifications)
			{
				NotifyOnModified(commands == previewCommands);
			}
		}
	}
}
