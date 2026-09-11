using System;
using System.Collections.Generic;
using System.Linq;
using SoftMasking.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace SoftMasking
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Soft Mask", 14)]
	[RequireComponent(typeof(RectTransform))]
	[HelpURL("https://github.com/olegknyazev/SoftMask/blob/bf89dda09b9ac46f7baa6f2ec53448eeb319c605/Packages/com.olegknyazev.softmask/Documentation%7E/Documentation.pdf")]
	public class SoftMask : UIBehaviour, ISoftMask, ICanvasRaycastFilter
	{
		[Serializable]
		public enum MaskSource
		{
			Graphic = 0,
			Sprite = 1,
			Texture = 2
		}

		[Serializable]
		public enum BorderMode
		{
			Simple = 0,
			Sliced = 1,
			Tiled = 2
		}

		[Serializable]
		[Flags]
		public enum Errors
		{
			NoError = 0,
			UnsupportedShaders = 1,
			NestedMasks = 2,
			TightPackedSprite = 4,
			AlphaSplitSprite = 8,
			UnsupportedImageType = 0x10,
			UnreadableTexture = 0x20,
			UnreadableRenderTexture = 0x40
		}

		private struct SourceParameters
		{
			public Image image;

			public Sprite sprite;

			public BorderMode spriteBorderMode;

			public float spritePixelsPerUnit;

			public Texture texture;

			public Rect textureUVRect;
		}

		private class MaterialReplacerImpl : IMaterialReplacer
		{
			public int order => 0;

			private static string DefaultUIETC1ShaderReplacement => "SoftMaskETC1PremultipliedAlpha";

			private static string DefaultUIShaderReplacement => "SoftMaskPremultipliedAlpha";

			public Material Replace(Material original)
			{
				if (original == null || original.HasDefaultUIShader())
				{
					return Replace(original, Resources.Load<Shader>(DefaultUIShaderReplacement));
				}
				if (original.HasDefaultETC1UIShader())
				{
					return Replace(original, Resources.Load<Shader>(DefaultUIETC1ShaderReplacement));
				}
				if (original.SupportsSoftMask())
				{
					return new Material(original);
				}
				return null;
			}

			private static Material Replace(Material original, Shader defaultReplacementShader)
			{
				Material material = (defaultReplacementShader ? new Material(defaultReplacementShader) : null);
				if ((bool)material && (bool)original)
				{
					material.CopyPropertiesFromMaterial(original);
				}
				return material;
			}
		}

		private static class Mathr
		{
			public static Vector4 ToVector(Rect r)
			{
				return new Vector4(r.xMin, r.yMin, r.xMax, r.yMax);
			}

			public static Vector4 Div(Vector4 v, Vector2 s)
			{
				return new Vector4(v.x / s.x, v.y / s.y, v.z / s.x, v.w / s.y);
			}

			public static Vector2 Div(Vector2 v, Vector2 s)
			{
				return new Vector2(v.x / s.x, v.y / s.y);
			}

			public static Vector4 Mul(Vector4 v, Vector2 s)
			{
				return new Vector4(v.x * s.x, v.y * s.y, v.z * s.x, v.w * s.y);
			}

			public static Vector2 Size(Vector4 r)
			{
				return new Vector2(r.z - r.x, r.w - r.y);
			}

			public static Vector4 ApplyBorder(Vector4 v, Vector4 b)
			{
				return new Vector4(v.x + b.x, v.y + b.y, v.z - b.z, v.w - b.w);
			}

			private static Vector2 Min(Vector4 r)
			{
				return new Vector2(r.x, r.y);
			}

			private static Vector2 Max(Vector4 r)
			{
				return new Vector2(r.z, r.w);
			}

			public static Vector2 Remap(Vector2 c, Vector4 from, Vector4 to)
			{
				Vector2 s = Max(from) - Min(from);
				Vector2 b = Max(to) - Min(to);
				return Vector2.Scale(Div(c - Min(from), s), b) + Min(to);
			}

			public static bool Inside(Vector2 v, Vector4 r)
			{
				if (v.x >= r.x && v.y >= r.y && v.x <= r.z)
				{
					return v.y <= r.w;
				}
				return false;
			}
		}

		private struct MaterialParameters
		{
			public enum SampleMaskResult
			{
				Success = 0,
				NonReadable = 1,
				NonTexture2D = 2
			}

			private static class Ids
			{
				public static readonly int SoftMask = Shader.PropertyToID("_SoftMask");

				public static readonly int SoftMask_Rect = Shader.PropertyToID("_SoftMask_Rect");

				public static readonly int SoftMask_UVRect = Shader.PropertyToID("_SoftMask_UVRect");

				public static readonly int SoftMask_ChannelWeights = Shader.PropertyToID("_SoftMask_ChannelWeights");

				public static readonly int SoftMask_WorldToMask = Shader.PropertyToID("_SoftMask_WorldToMask");

				public static readonly int SoftMask_BorderRect = Shader.PropertyToID("_SoftMask_BorderRect");

				public static readonly int SoftMask_UVBorderRect = Shader.PropertyToID("_SoftMask_UVBorderRect");

				public static readonly int SoftMask_TileRepeat = Shader.PropertyToID("_SoftMask_TileRepeat");

				public static readonly int SoftMask_InvertMask = Shader.PropertyToID("_SoftMask_InvertMask");

				public static readonly int SoftMask_InvertOutsides = Shader.PropertyToID("_SoftMask_InvertOutsides");
			}

			public Vector4 maskRect;

			public Vector4 maskBorder;

			public Vector4 maskRectUV;

			public Vector4 maskBorderUV;

			public Vector2 tileRepeat;

			public Color maskChannelWeights;

			public Matrix4x4 worldToMask;

			public Texture texture;

			public BorderMode borderMode;

			public bool invertMask;

			public bool invertOutsides;

			private Texture activeTexture
			{
				get
				{
					if (!texture)
					{
						return Texture2D.whiteTexture;
					}
					return texture;
				}
			}

			public SampleMaskResult SampleMask(Vector2 localPos, out float mask)
			{
				mask = 0f;
				Texture2D texture2D = texture as Texture2D;
				if (!texture2D)
				{
					return SampleMaskResult.NonTexture2D;
				}
				Vector2 vector = XY2UV(localPos);
				try
				{
					mask = MaskValue(texture2D.GetPixelBilinear(vector.x, vector.y));
					return SampleMaskResult.Success;
				}
				catch (UnityException)
				{
					return SampleMaskResult.NonReadable;
				}
			}

			public void Apply(Material mat)
			{
				mat.SetTexture(Ids.SoftMask, activeTexture);
				mat.SetVector(Ids.SoftMask_Rect, maskRect);
				mat.SetVector(Ids.SoftMask_UVRect, maskRectUV);
				mat.SetColor(Ids.SoftMask_ChannelWeights, maskChannelWeights);
				mat.SetMatrix(Ids.SoftMask_WorldToMask, worldToMask);
				mat.SetFloat(Ids.SoftMask_InvertMask, invertMask ? 1 : 0);
				mat.SetFloat(Ids.SoftMask_InvertOutsides, invertOutsides ? 1 : 0);
				mat.EnableKeyword("SOFTMASK_SIMPLE", borderMode == BorderMode.Simple);
				mat.EnableKeyword("SOFTMASK_SLICED", borderMode == BorderMode.Sliced);
				mat.EnableKeyword("SOFTMASK_TILED", borderMode == BorderMode.Tiled);
				if (borderMode != BorderMode.Simple)
				{
					mat.SetVector(Ids.SoftMask_BorderRect, maskBorder);
					mat.SetVector(Ids.SoftMask_UVBorderRect, maskBorderUV);
					if (borderMode == BorderMode.Tiled)
					{
						mat.SetVector(Ids.SoftMask_TileRepeat, tileRepeat);
					}
				}
			}

			private Vector2 XY2UV(Vector2 localPos)
			{
				return borderMode switch
				{
					BorderMode.Simple => MapSimple(localPos), 
					BorderMode.Sliced => MapBorder(localPos, repeat: false), 
					BorderMode.Tiled => MapBorder(localPos, repeat: true), 
					_ => MapSimple(localPos), 
				};
			}

			private Vector2 MapSimple(Vector2 localPos)
			{
				return Mathr.Remap(localPos, maskRect, maskRectUV);
			}

			private Vector2 MapBorder(Vector2 localPos, bool repeat)
			{
				return new Vector2(Inset(localPos.x, maskRect.x, maskBorder.x, maskBorder.z, maskRect.z, maskRectUV.x, maskBorderUV.x, maskBorderUV.z, maskRectUV.z, repeat ? tileRepeat.x : 1f), Inset(localPos.y, maskRect.y, maskBorder.y, maskBorder.w, maskRect.w, maskRectUV.y, maskBorderUV.y, maskBorderUV.w, maskRectUV.w, repeat ? tileRepeat.y : 1f));
			}

			private float Inset(float v, float x1, float x2, float u1, float u2, float repeat = 1f)
			{
				float num = x2 - x1;
				return Mathf.Lerp(u1, u2, (num != 0f) ? Frac((v - x1) / num * repeat) : 0f);
			}

			private float Inset(float v, float x1, float x2, float x3, float x4, float u1, float u2, float u3, float u4, float repeat = 1f)
			{
				if (v < x2)
				{
					return Inset(v, x1, x2, u1, u2);
				}
				if (v < x3)
				{
					return Inset(v, x2, x3, u2, u3, repeat);
				}
				return Inset(v, x3, x4, u3, u4);
			}

			private float Frac(float v)
			{
				return v - Mathf.Floor(v);
			}

			private float MaskValue(Color mask)
			{
				Color color = mask * maskChannelWeights;
				return color.a + color.r + color.g + color.b;
			}
		}

		private struct Diagnostics
		{
			private SoftMask _softMask;

			private Image image => _softMask.DeduceSourceParameters().image;

			private Sprite sprite => _softMask.DeduceSourceParameters().sprite;

			private Texture texture => _softMask.DeduceSourceParameters().texture;

			public Diagnostics(SoftMask softMask)
			{
				_softMask = softMask;
			}

			public Errors PollErrors()
			{
				SoftMask softMask = _softMask;
				Errors errors = Errors.NoError;
				softMask.GetComponentsInChildren(s_maskables);
				using (new ClearListAtExit<SoftMaskable>(s_maskables))
				{
					if (s_maskables.Any((SoftMaskable m) => m.mask == softMask && m.shaderIsNotSupported))
					{
						errors |= Errors.UnsupportedShaders;
					}
				}
				if (ThereAreNestedMasks())
				{
					errors |= Errors.NestedMasks;
				}
				errors |= CheckSprite(sprite);
				errors |= CheckImage();
				return errors | CheckTexture();
			}

			public static Errors CheckSprite(Sprite sprite)
			{
				Errors errors = Errors.NoError;
				if (!sprite)
				{
					return errors;
				}
				if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight)
				{
					errors |= Errors.TightPackedSprite;
				}
				if ((bool)sprite.associatedAlphaSplitTexture)
				{
					errors |= Errors.AlphaSplitSprite;
				}
				return errors;
			}

			private bool ThereAreNestedMasks()
			{
				SoftMask softMask = _softMask;
				bool flag = false;
				using (new ClearListAtExit<SoftMask>(s_masks))
				{
					softMask.GetComponentsInParent(includeInactive: false, s_masks);
					flag |= s_masks.Any((SoftMask x) => AreCompeting(softMask, x));
					softMask.GetComponentsInChildren(includeInactive: false, s_masks);
					return flag | s_masks.Any((SoftMask x) => AreCompeting(softMask, x));
				}
			}

			private Errors CheckImage()
			{
				Errors errors = Errors.NoError;
				if (!_softMask.isBasedOnGraphic)
				{
					return errors;
				}
				if ((bool)image && !IsImageTypeSupported(image.type))
				{
					errors |= Errors.UnsupportedImageType;
				}
				return errors;
			}

			private Errors CheckTexture()
			{
				Errors errors = Errors.NoError;
				if (_softMask.isUsingRaycastFiltering && (bool)texture)
				{
					Texture2D texture2D = texture as Texture2D;
					if (!texture2D)
					{
						errors |= Errors.UnreadableRenderTexture;
					}
					else if (!IsReadable(texture2D))
					{
						errors |= Errors.UnreadableTexture;
					}
				}
				return errors;
			}

			private static bool AreCompeting(SoftMask softMask, SoftMask other)
			{
				if (softMask.isMaskingEnabled && softMask != other && other.isMaskingEnabled && softMask.canvas.rootCanvas == other.canvas.rootCanvas)
				{
					return !SelectChild(softMask, other).canvas.overrideSorting;
				}
				return false;
			}

			private static T SelectChild<T>(T first, T second) where T : Component
			{
				if (!first.transform.IsChildOf(second.transform))
				{
					return second;
				}
				return first;
			}

			private static bool IsReadable(Texture2D texture)
			{
				try
				{
					texture.GetPixel(0, 0);
					return true;
				}
				catch (UnityException)
				{
					return false;
				}
			}
		}

		private struct WarningReporter
		{
			private readonly UnityEngine.Object _owner;

			private Texture _lastReadTexture;

			private Sprite _lastUsedSprite;

			private Sprite _lastUsedImageSprite;

			private Image.Type _lastUsedImageType;

			public WarningReporter(UnityEngine.Object owner)
			{
				_owner = owner;
				_lastReadTexture = null;
				_lastUsedSprite = null;
				_lastUsedImageSprite = null;
				_lastUsedImageType = Image.Type.Simple;
			}

			public void TextureRead(Texture texture, MaterialParameters.SampleMaskResult sampleResult)
			{
				if (!(_lastReadTexture == texture))
				{
					_lastReadTexture = texture;
					switch (sampleResult)
					{
					case MaterialParameters.SampleMaskResult.NonReadable:
						Debug.LogErrorFormat(_owner, "Raycast Threshold greater than 0 can't be used on Soft Mask with texture '{0}' because it's not readable. You can make the texture readable in the Texture Import Settings.", texture.name);
						break;
					case MaterialParameters.SampleMaskResult.NonTexture2D:
						Debug.LogErrorFormat(_owner, "Raycast Threshold greater than 0 can't be used on Soft Mask with texture '{0}' because it's not a Texture2D. Raycast Threshold may be used only with regular 2D textures.", texture.name);
						break;
					}
				}
			}

			public void SpriteUsed(Sprite sprite, Errors errors)
			{
				if (!(_lastUsedSprite == sprite))
				{
					_lastUsedSprite = sprite;
					if ((errors & Errors.TightPackedSprite) != Errors.NoError)
					{
						Debug.LogError("SoftMask doesn't support tight packed sprites", _owner);
					}
					if ((errors & Errors.AlphaSplitSprite) != Errors.NoError)
					{
						Debug.LogError("SoftMask doesn't support sprites with an alpha split texture", _owner);
					}
				}
			}

			public void ImageUsed(Image image)
			{
				if (!image)
				{
					_lastUsedImageSprite = null;
					_lastUsedImageType = Image.Type.Simple;
				}
				else if (!(_lastUsedImageSprite == image.sprite) || _lastUsedImageType != image.type)
				{
					_lastUsedImageSprite = image.sprite;
					_lastUsedImageType = image.type;
					if ((bool)image && !IsImageTypeSupported(image.type))
					{
						Debug.LogErrorFormat(_owner, "SoftMask doesn't support image type {0}. Image type Simple will be used.", image.type);
					}
				}
			}
		}

		[SerializeField]
		private MaskSource _source;

		[SerializeField]
		private RectTransform _separateMask;

		[SerializeField]
		private Sprite _sprite;

		[SerializeField]
		private BorderMode _spriteBorderMode;

		[SerializeField]
		private float _spritePixelsPerUnitMultiplier = 1f;

		[SerializeField]
		private Texture _texture;

		[SerializeField]
		private Rect _textureUVRect = DefaultUVRect;

		[SerializeField]
		private Color _channelWeights = MaskChannel.alpha;

		[SerializeField]
		private float _raycastThreshold;

		[SerializeField]
		private bool _invertMask;

		[SerializeField]
		private bool _invertOutsides;

		private readonly MaterialReplacements _materials;

		private MaterialParameters _parameters;

		private WarningReporter _warningReporter;

		private Rect _lastMaskRect;

		private bool _maskingWasEnabled;

		private bool _destroyed;

		private bool _dirty;

		private readonly Queue<Transform> _transformsToSpawnMaskablesIn = new Queue<Transform>();

		private RectTransform _maskTransform;

		private Graphic _graphic;

		private Canvas _canvas;

		private static readonly Rect DefaultUVRect = new Rect(0f, 0f, 1f, 1f);

		private const float DefaultPixelsPerUnit = 100f;

		private static readonly List<SoftMask> s_masks = new List<SoftMask>();

		private static readonly List<SoftMaskable> s_maskables = new List<SoftMaskable>();

		public MaskSource source
		{
			get
			{
				return _source;
			}
			set
			{
				if (_source != value)
				{
					Set(ref _source, value);
				}
			}
		}

		public RectTransform separateMask
		{
			get
			{
				return _separateMask;
			}
			set
			{
				if (_separateMask != value)
				{
					Set(ref _separateMask, value);
					_graphic = null;
					_maskTransform = null;
				}
			}
		}

		public Sprite sprite
		{
			get
			{
				return _sprite;
			}
			set
			{
				if (_sprite != value)
				{
					Set(ref _sprite, value);
				}
			}
		}

		public BorderMode spriteBorderMode
		{
			get
			{
				return _spriteBorderMode;
			}
			set
			{
				if (_spriteBorderMode != value)
				{
					Set(ref _spriteBorderMode, value);
				}
			}
		}

		public float spritePixelsPerUnitMultiplier
		{
			get
			{
				return _spritePixelsPerUnitMultiplier;
			}
			set
			{
				if (_spritePixelsPerUnitMultiplier != value)
				{
					Set(ref _spritePixelsPerUnitMultiplier, ClampPixelsPerUnitMultiplier(value));
				}
			}
		}

		public Texture2D texture
		{
			get
			{
				return _texture as Texture2D;
			}
			set
			{
				if (_texture != value)
				{
					Set(ref _texture, value);
				}
			}
		}

		public RenderTexture renderTexture
		{
			get
			{
				return _texture as RenderTexture;
			}
			set
			{
				if (_texture != value)
				{
					Set(ref _texture, value);
				}
			}
		}

		public Rect textureUVRect
		{
			get
			{
				return _textureUVRect;
			}
			set
			{
				if (_textureUVRect != value)
				{
					Set(ref _textureUVRect, value);
				}
			}
		}

		public Color channelWeights
		{
			get
			{
				return _channelWeights;
			}
			set
			{
				if (_channelWeights != value)
				{
					Set(ref _channelWeights, value);
				}
			}
		}

		public float raycastThreshold
		{
			get
			{
				return _raycastThreshold;
			}
			set
			{
				_raycastThreshold = value;
			}
		}

		public bool invertMask
		{
			get
			{
				return _invertMask;
			}
			set
			{
				if (_invertMask != value)
				{
					Set(ref _invertMask, value);
				}
			}
		}

		public bool invertOutsides
		{
			get
			{
				return _invertOutsides;
			}
			set
			{
				if (_invertOutsides != value)
				{
					Set(ref _invertOutsides, value);
				}
			}
		}

		public bool isUsingRaycastFiltering => _raycastThreshold > 0f;

		public bool isMaskingEnabled
		{
			get
			{
				if (base.isActiveAndEnabled)
				{
					return canvas;
				}
				return false;
			}
		}

		private RectTransform maskTransform
		{
			get
			{
				if (!_maskTransform)
				{
					return _maskTransform = (_separateMask ? _separateMask : GetComponent<RectTransform>());
				}
				return _maskTransform;
			}
		}

		private Canvas canvas
		{
			get
			{
				if (!_canvas)
				{
					return _canvas = NearestEnabledCanvas();
				}
				return _canvas;
			}
		}

		private bool isBasedOnGraphic => _source == MaskSource.Graphic;

		bool ISoftMask.isAlive
		{
			get
			{
				if ((bool)this)
				{
					return !_destroyed;
				}
				return false;
			}
		}

		public SoftMask()
		{
			MaterialReplacerChain replacer = new MaterialReplacerChain(MaterialReplacer.globalReplacers, new MaterialReplacerImpl());
			_materials = new MaterialReplacements(replacer, delegate(Material m)
			{
				_parameters.Apply(m);
			});
			_warningReporter = new WarningReporter(this);
		}

		public Errors PollErrors()
		{
			return new Diagnostics(this).PollErrors();
		}

		public bool IsRaycastLocationValid(Vector2 sp, Camera cam)
		{
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(maskTransform, sp, cam, out var localPoint))
			{
				return false;
			}
			if (!Mathr.Inside(localPoint, LocalMaskRect(Vector4.zero)))
			{
				return _invertOutsides;
			}
			if (!_parameters.texture)
			{
				return true;
			}
			if (!isUsingRaycastFiltering)
			{
				return true;
			}
			float mask;
			MaterialParameters.SampleMaskResult sampleMaskResult = _parameters.SampleMask(localPoint, out mask);
			_warningReporter.TextureRead(_parameters.texture, sampleMaskResult);
			if (sampleMaskResult != MaterialParameters.SampleMaskResult.Success)
			{
				return true;
			}
			if (_invertMask)
			{
				mask = 1f - mask;
			}
			return mask >= _raycastThreshold;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SubscribeOnWillRenderCanvases();
			MarkTransformForMaskablesSpawn(base.transform);
			FindGraphic();
			if (isMaskingEnabled)
			{
				UpdateMaskParameters();
			}
			NotifyChildrenThatMaskMightChanged();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			UnsubscribeFromWillRenderCanvases();
			if ((bool)_graphic)
			{
				_graphic.UnregisterDirtyVerticesCallback(OnGraphicDirty);
				_graphic.UnregisterDirtyMaterialCallback(OnGraphicDirty);
				_graphic = null;
			}
			NotifyChildrenThatMaskMightChanged();
			DestroyMaterials();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_destroyed = true;
			NotifyChildrenThatMaskMightChanged();
		}

		protected virtual void LateUpdate()
		{
			bool flag = isMaskingEnabled;
			if (flag)
			{
				if (_maskingWasEnabled != flag)
				{
					MarkTransformForMaskablesSpawn(base.transform);
				}
				SpawnMaskables();
				Graphic graphic = _graphic;
				FindGraphic();
				if (_lastMaskRect != maskTransform.rect || (object)_graphic != graphic)
				{
					_dirty = true;
				}
			}
			_maskingWasEnabled = flag;
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			_dirty = true;
		}

		protected override void OnDidApplyAnimationProperties()
		{
			base.OnDidApplyAnimationProperties();
			_dirty = true;
		}

		private static float ClampPixelsPerUnitMultiplier(float value)
		{
			return Mathf.Max(value, 0.01f);
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			_canvas = null;
			_dirty = true;
		}

		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			_canvas = null;
			_dirty = true;
			NotifyChildrenThatMaskMightChanged();
		}

		private void OnTransformChildrenChanged()
		{
			MarkTransformForMaskablesSpawn(base.transform);
		}

		private void MarkTransformForMaskablesSpawn(Transform transform)
		{
			if (!_transformsToSpawnMaskablesIn.Contains(transform))
			{
				_transformsToSpawnMaskablesIn.Enqueue(transform);
			}
		}

		private void SpawnMaskables()
		{
			while (_transformsToSpawnMaskablesIn.Count > 0)
			{
				Transform transform = _transformsToSpawnMaskablesIn.Dequeue();
				if ((bool)transform)
				{
					SpawnMaskablesInChildren(transform);
				}
			}
		}

		private void SubscribeOnWillRenderCanvases()
		{
			Touch(CanvasUpdateRegistry.instance);
			Canvas.willRenderCanvases += OnWillRenderCanvases;
		}

		private void UnsubscribeFromWillRenderCanvases()
		{
			Canvas.willRenderCanvases -= OnWillRenderCanvases;
		}

		private void OnWillRenderCanvases()
		{
			SpawnMaskables();
			if (isMaskingEnabled)
			{
				UpdateMaskParameters();
			}
		}

		private static T Touch<T>(T obj)
		{
			return obj;
		}

		Material ISoftMask.GetReplacement(Material original)
		{
			return _materials.Get(original);
		}

		void ISoftMask.ReleaseReplacement(Material replacement)
		{
			_materials.Release(replacement);
		}

		void ISoftMask.UpdateTransformChildren(Transform transform)
		{
			MarkTransformForMaskablesSpawn(transform);
		}

		private void OnGraphicDirty()
		{
			if (isBasedOnGraphic)
			{
				_dirty = true;
			}
		}

		private void FindGraphic()
		{
			if (!_graphic && isBasedOnGraphic)
			{
				_graphic = maskTransform.GetComponent<Graphic>();
				if ((bool)_graphic)
				{
					_graphic.RegisterDirtyVerticesCallback(OnGraphicDirty);
					_graphic.RegisterDirtyMaterialCallback(OnGraphicDirty);
				}
			}
		}

		private Canvas NearestEnabledCanvas()
		{
			Canvas[] componentsInParent = GetComponentsInParent<Canvas>(includeInactive: false);
			for (int i = 0; i < componentsInParent.Length; i++)
			{
				if (componentsInParent[i].isActiveAndEnabled)
				{
					return componentsInParent[i];
				}
			}
			return null;
		}

		private void UpdateMaskParameters()
		{
			if (_dirty || maskTransform.hasChanged)
			{
				CalculateMaskParameters();
				maskTransform.hasChanged = false;
				_lastMaskRect = maskTransform.rect;
				_dirty = false;
			}
			_materials.ApplyAll();
		}

		private void SpawnMaskablesInChildren(Transform root)
		{
			using (new ClearListAtExit<SoftMaskable>(s_maskables))
			{
				for (int i = 0; i < root.childCount; i++)
				{
					Transform child = root.GetChild(i);
					child.GetComponents(s_maskables);
					bool flag = false;
					for (int j = 0; j < s_maskables.Count; j++)
					{
						if (!s_maskables[j].isDestroyed)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						child.gameObject.AddComponent<SoftMaskable>();
					}
				}
			}
		}

		private void NotifyChildrenThatMaskMightChanged()
		{
			ForEachChildMaskable(delegate(SoftMaskable x)
			{
				x.MaskMightChanged();
			}, includeInactive: true);
		}

		private void ForEachChildMaskable(Action<SoftMaskable> action, bool includeInactive = false)
		{
			base.transform.GetComponentsInChildren(includeInactive, s_maskables);
			using (new ClearListAtExit<SoftMaskable>(s_maskables))
			{
				for (int i = 0; i < s_maskables.Count; i++)
				{
					SoftMaskable softMaskable = s_maskables[i];
					if ((bool)softMaskable && softMaskable.gameObject != base.gameObject)
					{
						action(softMaskable);
					}
				}
			}
		}

		private void DestroyMaterials()
		{
			_materials.DestroyAllAndClear();
		}

		private SourceParameters DeduceSourceParameters()
		{
			SourceParameters result = default(SourceParameters);
			switch (_source)
			{
			case MaskSource.Graphic:
				if (_graphic is Image { sprite: var sprite } image)
				{
					result.image = image;
					result.sprite = sprite;
					result.spriteBorderMode = ImageTypeToBorderMode(image.type);
					if ((bool)sprite)
					{
						result.spritePixelsPerUnit = sprite.pixelsPerUnit * image.pixelsPerUnitMultiplier;
						result.texture = sprite.texture;
					}
					else
					{
						result.spritePixelsPerUnit = 100f;
					}
				}
				else if (_graphic is RawImage rawImage)
				{
					result.texture = rawImage.texture;
					result.textureUVRect = rawImage.uvRect;
				}
				break;
			case MaskSource.Sprite:
				result.sprite = _sprite;
				result.spriteBorderMode = _spriteBorderMode;
				if ((bool)_sprite)
				{
					result.spritePixelsPerUnit = _sprite.pixelsPerUnit * _spritePixelsPerUnitMultiplier;
					result.texture = _sprite.texture;
				}
				else
				{
					result.spritePixelsPerUnit = 100f;
				}
				break;
			case MaskSource.Texture:
				result.texture = _texture;
				result.textureUVRect = _textureUVRect;
				break;
			}
			return result;
		}

		public static BorderMode ImageTypeToBorderMode(Image.Type type)
		{
			return type switch
			{
				Image.Type.Simple => BorderMode.Simple, 
				Image.Type.Sliced => BorderMode.Sliced, 
				Image.Type.Tiled => BorderMode.Tiled, 
				_ => BorderMode.Simple, 
			};
		}

		public static bool IsImageTypeSupported(Image.Type type)
		{
			if (type != Image.Type.Simple && type != Image.Type.Sliced)
			{
				return type == Image.Type.Tiled;
			}
			return true;
		}

		private void CalculateMaskParameters()
		{
			SourceParameters sourceParameters = DeduceSourceParameters();
			_warningReporter.ImageUsed(sourceParameters.image);
			Errors errors = Diagnostics.CheckSprite(sourceParameters.sprite);
			_warningReporter.SpriteUsed(sourceParameters.sprite, errors);
			if ((bool)sourceParameters.sprite)
			{
				if (errors == Errors.NoError)
				{
					CalculateSpriteBased(sourceParameters.sprite, sourceParameters.spriteBorderMode, sourceParameters.spritePixelsPerUnit);
				}
				else
				{
					CalculateSolidFill();
				}
			}
			else if ((bool)sourceParameters.texture)
			{
				CalculateTextureBased(sourceParameters.texture, sourceParameters.textureUVRect);
			}
			else
			{
				CalculateSolidFill();
			}
		}

		private void CalculateSpriteBased(Sprite sprite, BorderMode borderMode, float spritePixelsPerUnit)
		{
			FillCommonParameters();
			Vector4 innerUV = DataUtility.GetInnerUV(sprite);
			Vector4 outerUV = DataUtility.GetOuterUV(sprite);
			Vector4 padding = DataUtility.GetPadding(sprite);
			Vector4 vector = LocalMaskRect(Vector4.zero);
			_parameters.maskRectUV = outerUV;
			if (borderMode == BorderMode.Simple)
			{
				if (ShouldPreserveAspect())
				{
					vector = PreserveSpriteAspectRatio(vector, sprite.rect.size);
				}
				Vector4 v = Mathr.Div(padding, sprite.rect.size);
				_parameters.maskRect = Mathr.ApplyBorder(vector, Mathr.Mul(v, Mathr.Size(vector)));
			}
			else
			{
				float num = SpriteToCanvasScale(spritePixelsPerUnit);
				_parameters.maskRect = Mathr.ApplyBorder(vector, padding * num);
				Vector4 border = AdjustBorders(sprite.border * num, vector);
				_parameters.maskBorder = LocalMaskRect(border);
				_parameters.maskBorderUV = innerUV;
			}
			_parameters.texture = sprite.texture;
			_parameters.borderMode = borderMode;
			if (borderMode == BorderMode.Tiled)
			{
				_parameters.tileRepeat = MaskRepeat(sprite, spritePixelsPerUnit, _parameters.maskBorder);
			}
		}

		private static Vector4 AdjustBorders(Vector4 border, Vector4 rect)
		{
			Vector2 vector = Mathr.Size(rect);
			for (int i = 0; i <= 1; i++)
			{
				float num = border[i] + border[i + 2];
				if (vector[i] < num && num != 0f)
				{
					float num2 = vector[i] / num;
					border[i] *= num2;
					border[i + 2] *= num2;
				}
			}
			return border;
		}

		private bool ShouldPreserveAspect()
		{
			if (isBasedOnGraphic)
			{
				return (_graphic as Image).preserveAspect;
			}
			return false;
		}

		private Vector4 PreserveSpriteAspectRatio(Vector4 rect, Vector2 spriteSize)
		{
			float num = spriteSize.x / spriteSize.y;
			float num2 = (rect.z - rect.x) / (rect.w - rect.y);
			if (num > num2)
			{
				float num3 = num2 / num;
				return new Vector4(rect.x, rect.y * num3, rect.z, rect.w * num3);
			}
			float num4 = num / num2;
			return new Vector4(rect.x * num4, rect.y, rect.z * num4, rect.w);
		}

		private void CalculateTextureBased(Texture texture, Rect uvRect)
		{
			FillCommonParameters();
			_parameters.maskRect = LocalMaskRect(Vector4.zero);
			_parameters.maskRectUV = Mathr.ToVector(uvRect);
			_parameters.texture = texture;
			_parameters.borderMode = BorderMode.Simple;
		}

		private void CalculateSolidFill()
		{
			CalculateTextureBased(null, DefaultUVRect);
		}

		private void FillCommonParameters()
		{
			_parameters.worldToMask = WorldToMask();
			_parameters.maskChannelWeights = _channelWeights;
			_parameters.invertMask = _invertMask;
			_parameters.invertOutsides = _invertOutsides;
		}

		private float SpriteToCanvasScale(float spritePixelsPerUnit)
		{
			return (canvas ? canvas.referencePixelsPerUnit : 100f) / spritePixelsPerUnit;
		}

		private Matrix4x4 WorldToMask()
		{
			return maskTransform.worldToLocalMatrix * canvas.rootCanvas.transform.localToWorldMatrix;
		}

		private Vector4 LocalMaskRect(Vector4 border)
		{
			return Mathr.ApplyBorder(Mathr.ToVector(maskTransform.rect), border);
		}

		private Vector2 MaskRepeat(Sprite sprite, float spritePixelsPerUnit, Vector4 centralPart)
		{
			Vector4 r = Mathr.ApplyBorder(Mathr.ToVector(sprite.rect), sprite.border);
			return Mathr.Div(Mathr.Size(centralPart), Mathr.Size(r) * SpriteToCanvasScale(spritePixelsPerUnit));
		}

		private void Set<T>(ref T field, T value)
		{
			field = value;
			_dirty = true;
		}
	}
}
