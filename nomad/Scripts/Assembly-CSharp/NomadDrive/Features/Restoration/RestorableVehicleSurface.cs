using System;
using EvilCore.DynamicCasting;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using PaintCore;
using PaintIn3D;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Restoration
{
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public class RestorableVehicleSurface : MonoBehaviour
	{
		[Header("Material Target")]
		[Tooltip("Which material index on the MeshRenderer to target (0 = first material)")]
		[SerializeField]
		private int _materialIndex;

		[Header("Texture Resolution")]
		[Tooltip("Resolution for paint color texture")]
		[SerializeField]
		private int _paintColorResolution = 1024;

		[Tooltip("Resolution for mask textures (dirt/rust/paint/polish)")]
		[SerializeField]
		private int _maskTextureResolution = 512;

		[Header("Initial State (Per-Prefab Configuration)")]
		[Tooltip("Initial paint color")]
		[SerializeField]
		private Color _initialPaintColor = Color.white;

		[Tooltip("Initial dirt coverage (1 = fully dirty, 0 = clean)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _initialDirtCoverage = 1f;

		[Tooltip("Initial rust coverage (1 = fully rusted, 0 = no rust)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _initialRustCoverage = 0.8f;

		[Tooltip("Initial paint coverage (1 = fully painted, 0 = bare metal)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _initialPaintCoverage;

		[Tooltip("Initial polish coverage (1 = fully polished, 0 = unpolished)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _initialPolishCoverage;

		[Tooltip("Initial metal coverage (1 = fully metallic, 0 = white base)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _initialMetalCoverage = 1f;

		[Header("Group Configuration")]
		[Tooltip("Group index for paint color (spray paint) - Use 'Vehicle Base (RGB)' group")]
		[SerializeField]
		private int _paintColorGroupIndex = 200;

		[Tooltip("Group index for dirt mask (cleaning tool) - Use 'Vehicle Dirt Mask (R)' group")]
		[SerializeField]
		private int _dirtGroupIndex = 201;

		[Tooltip("Group index for rust mask (grinder) - Use 'Vehicle Rust Mask (R)' group")]
		[SerializeField]
		private int _rustGroupIndex = 202;

		[Tooltip("Group index for polish mask (polish tool) - Use 'Vehicle Polish Mask (R)' group")]
		[SerializeField]
		private int _polishGroupIndex = 203;

		[Tooltip("Group index for paint mask (spray paint) - Use 'Vehicle Paint Mask (R)' group")]
		[SerializeField]
		private int _paintMaskGroupIndex = 204;

		[Tooltip("Group index for metal mask (grinder) - Use 'Vehicle Metal Mask (R)' group")]
		[SerializeField]
		private int _metalMaskGroupIndex = 205;

		private string _runtimeSaveKey;

		private RestorationNetworkSync _networkSync;

		private byte _surfaceIndex;

		[Header("Components (Auto-populated)")]
		[SerializeField]
		private CwPaintableMesh _paintableMesh;

		[SerializeField]
		private CwMaterialCloner _materialCloner;

		[SerializeField]
		private CwPaintableMeshTexture _paintColorTexture;

		[SerializeField]
		private CwPaintableMeshTexture _dirtMaskTexture;

		[SerializeField]
		private CwPaintableMeshTexture _rustMaskTexture;

		[SerializeField]
		private CwPaintableMeshTexture _polishMaskTexture;

		[SerializeField]
		private CwPaintableMeshTexture _paintMaskTexture;

		[SerializeField]
		private CwPaintableMeshTexture _metalMaskTexture;

		private MeshRenderer _renderer;

		private MeshFilter _meshFilter;

		[Inject]
		private ICastingManager _castingManager;

		private bool _meshTargetRegistered;

		[Header("Rust Re-emergence")]
		[Tooltip("Enable rust bleeding through poor quality paint over time")]
		[SerializeField]
		private bool _enableRustReemergence = true;

		[Tooltip("Time in seconds for rust to fully re-emerge (0 = instant for testing)")]
		[SerializeField]
		[Range(0f, 600f)]
		private float _reemergenceTimeSeconds = 120f;

		[Tooltip("Current paint age (0-1, updated at runtime)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _currentPaintAge;

		[Tooltip("Time multiplier for faster testing (1 = realtime, 10 = 10x faster)")]
		[SerializeField]
		[Range(1f, 100f)]
		private float _ageTimeMultiplier = 1f;

		private float _paintAppliedTime = -1f;

		private Material _runtimeMaterial;

		private static readonly int PaintAgeProperty = Shader.PropertyToID("_PaintAge");

		private float _lastSentPaintAge = -1f;

		private const float AgeUpdateThreshold = 0.01f;

		private MaterialPropertyBlock _previewPropertyBlock;

		private static readonly int DirtMaskProperty = Shader.PropertyToID("_DirtMask");

		private static readonly int RustMaskProperty = Shader.PropertyToID("_RustMask");

		private static readonly int PaintMaskProperty = Shader.PropertyToID("_PaintMask");

		private static readonly int PolishMaskProperty = Shader.PropertyToID("_PolishMask");

		private static readonly int PaintColorProperty = Shader.PropertyToID("_PaintColor");

		private static readonly int MetalMaskProperty = Shader.PropertyToID("_MetalMask");

		private static readonly int DirtMaskIntensityProperty = Shader.PropertyToID("_DirtMaskIntensity");

		private static readonly int RustMaskIntensityProperty = Shader.PropertyToID("_RustMaskIntensity");

		private static readonly int PaintMaskIntensityProperty = Shader.PropertyToID("_PaintMaskIntensity");

		private static readonly int PolishMaskIntensityProperty = Shader.PropertyToID("_PolishMaskIntensity");

		private static readonly int MetalMaskIntensityProperty = Shader.PropertyToID("_MetalMaskIntensity");

		private Texture2D _previewDirtTexture;

		private Texture2D _previewRustTexture;

		private Texture2D _previewPaintTexture;

		private Texture2D _previewPolishTexture;

		private Texture2D _previewPaintColorTexture;

		private Texture2D _previewMetalTexture;

		private const string PaintColorKeySuffix = "_paintColor";

		private const string DirtMaskKeySuffix = "_dirtMask";

		private const string RustMaskKeySuffix = "_rustMask";

		private const string PaintMaskKeySuffix = "_paintMask";

		private const string PolishMaskKeySuffix = "_polishMask";

		private const string MetalMaskKeySuffix = "_metalMask";

		private const string PaintAgeKeySuffix = "_paintAge";

		public byte SurfaceIndex => _surfaceIndex;

		public CwPaintableMeshTexture PaintColorTexture => _paintColorTexture;

		public CwPaintableMeshTexture DirtMaskTexture => _dirtMaskTexture;

		public CwPaintableMeshTexture RustMaskTexture => _rustMaskTexture;

		public CwPaintableMeshTexture PolishMaskTexture => _polishMaskTexture;

		public CwPaintableMeshTexture PaintMaskTexture => _paintMaskTexture;

		public CwPaintableMeshTexture MetalMaskTexture => _metalMaskTexture;

		public int PaintColorGroupIndex => _paintColorGroupIndex;

		public int DirtGroupIndex => _dirtGroupIndex;

		public int RustGroupIndex => _rustGroupIndex;

		public int PolishGroupIndex => _polishGroupIndex;

		public int PaintMaskGroupIndex => _paintMaskGroupIndex;

		public int MetalMaskGroupIndex => _metalMaskGroupIndex;

		public string SaveKey
		{
			get
			{
				if (string.IsNullOrEmpty(_runtimeSaveKey))
				{
					_runtimeSaveKey = $"Restoration_{Guid.NewGuid():N}";
				}
				return _runtimeSaveKey;
			}
		}

		public bool IsRustRemerging
		{
			get
			{
				if (_enableRustReemergence)
				{
					return _currentPaintAge > 0f;
				}
				return false;
			}
		}

		public void RegisterWithNetworkSync(RestorationNetworkSync sync, byte index)
		{
			_networkSync = sync;
			_surfaceIndex = index;
		}

		private void Awake()
		{
			_renderer = GetComponent<MeshRenderer>();
			_meshFilter = GetComponent<MeshFilter>();
		}

		private void Start()
		{
			if (!_meshTargetRegistered && _castingManager != null && _meshFilter != null)
			{
				_castingManager.RegisterMeshTarget(_meshFilter);
				_meshTargetRegistered = true;
			}
			if (_materialCloner != null && _renderer != null)
			{
				_runtimeMaterial = _renderer.materials[_materialIndex];
			}
		}

		private void Update()
		{
			if (_enableRustReemergence && !(_paintAppliedTime < 0f) && !(_runtimeMaterial == null))
			{
				float num = (Time.time - _paintAppliedTime) * _ageTimeMultiplier;
				_currentPaintAge = Mathf.Clamp01(num / _reemergenceTimeSeconds);
				if (Mathf.Abs(_currentPaintAge - _lastSentPaintAge) > 0.01f)
				{
					_runtimeMaterial.SetFloat(PaintAgeProperty, _currentPaintAge);
					_lastSentPaintAge = _currentPaintAge;
				}
			}
		}

		public void OnPaintApplied()
		{
			_paintAppliedTime = Time.time;
			_currentPaintAge = 0f;
			_lastSentPaintAge = 0f;
			if (_runtimeMaterial != null)
			{
				_runtimeMaterial.SetFloat(PaintAgeProperty, 0f);
			}
			_networkSync?.NotifyPaintApplied(_surfaceIndex);
		}

		public void OnRustRemoved()
		{
			_paintAppliedTime = -1f;
			_currentPaintAge = 0f;
			_lastSentPaintAge = -1f;
			if (_runtimeMaterial != null)
			{
				_runtimeMaterial.SetFloat(PaintAgeProperty, 0f);
			}
		}

		public void SetPaintAge(float age)
		{
			_currentPaintAge = Mathf.Clamp01(age);
			_lastSentPaintAge = _currentPaintAge;
			if (_runtimeMaterial != null)
			{
				_runtimeMaterial.SetFloat(PaintAgeProperty, _currentPaintAge);
			}
			if (age > 0f)
			{
				_paintAppliedTime = Time.time - age * _reemergenceTimeSeconds / _ageTimeMultiplier;
			}
			else
			{
				_paintAppliedTime = -1f;
			}
		}

		public float GetPaintAge()
		{
			return _currentPaintAge;
		}

		private void SimulatePaintApplied()
		{
			if (Application.isPlaying)
			{
				OnPaintApplied();
			}
		}

		private void ResetPaintAge()
		{
			SetPaintAge(0f);
		}

		private void MaxPaintAge()
		{
			if (Application.isPlaying)
			{
				SetPaintAge(1f);
			}
		}

		public void SetupComponents()
		{
			_renderer = GetComponent<MeshRenderer>();
			Material[] sharedMaterials = _renderer.sharedMaterials;
			if (_materialIndex < 0 || _materialIndex >= sharedMaterials.Length)
			{
				EvilLogger.LogError($"[RestorableVehicleSurface] Material index {_materialIndex} out of range (0-{sharedMaterials.Length - 1}) on {base.gameObject.name}", "SetupComponents", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Networked\\RestorableVehicleSurface.cs", 575);
				return;
			}
			Material material = sharedMaterials[_materialIndex];
			if (!(material == null))
			{
				material.shader.name.Contains("RestorationHDRP");
			}
			_paintableMesh = GetComponent<CwPaintableMesh>();
			if (_paintableMesh == null)
			{
				_paintableMesh = base.gameObject.AddComponent<CwPaintableMesh>();
			}
			_paintableMesh.Activation = CwPaintableMesh.ActivationType.Start;
			_paintableMesh.MaterialApplication = CwPaintableMesh.MaterialApplicationType.ClonerAndTextures;
			_materialCloner = GetComponent<CwMaterialCloner>();
			if (_materialCloner == null)
			{
				_materialCloner = base.gameObject.AddComponent<CwMaterialCloner>();
			}
			_materialCloner.Index = _materialIndex;
			Material material2 = sharedMaterials[_materialIndex];
			SetupPaintColorTexture(material2.GetTexture("_PaintColor"));
			SetupDirtMaskTexture(material2.GetTexture("_DirtMask"));
			SetupRustMaskTexture(material2.GetTexture("_RustMask"));
			SetupPolishMaskTexture(material2.GetTexture("_PolishMask"));
			SetupPaintMaskTexture(material2.GetTexture("_PaintMask"));
			SetupMetalMaskTexture(material2.GetTexture("_MetalMask"));
		}

		private void SetupPaintColorTexture(Texture existingTexture)
		{
			_paintColorTexture = FindOrCreateTexture("_PaintColor");
			_paintColorTexture.Slot = new CwSlot(_materialIndex, "_PaintColor");
			_paintColorTexture.Group = new CwGroup(_paintColorGroupIndex);
			_paintColorTexture.Width = _paintColorResolution;
			_paintColorTexture.Height = _paintColorResolution;
			_paintColorTexture.Color = _initialPaintColor;
			_paintColorTexture.Texture = existingTexture;
			_paintColorTexture.Format = RenderTextureFormat.ARGB32;
		}

		private void SetupDirtMaskTexture(Texture existingTexture)
		{
			_dirtMaskTexture = FindOrCreateTexture("_DirtMask");
			_dirtMaskTexture.Slot = new CwSlot(_materialIndex, "_DirtMask");
			_dirtMaskTexture.Group = new CwGroup(_dirtGroupIndex);
			_dirtMaskTexture.Width = _maskTextureResolution;
			_dirtMaskTexture.Height = _maskTextureResolution;
			_dirtMaskTexture.Color = new Color(_initialDirtCoverage, 0f, 0f, 1f);
			_dirtMaskTexture.Texture = existingTexture;
			_dirtMaskTexture.Format = RenderTextureFormat.R8;
		}

		private void SetupRustMaskTexture(Texture existingTexture)
		{
			_rustMaskTexture = FindOrCreateTexture("_RustMask");
			_rustMaskTexture.Slot = new CwSlot(_materialIndex, "_RustMask");
			_rustMaskTexture.Group = new CwGroup(_rustGroupIndex);
			_rustMaskTexture.Width = _maskTextureResolution;
			_rustMaskTexture.Height = _maskTextureResolution;
			_rustMaskTexture.Color = new Color(_initialRustCoverage, 0f, 0f, 1f);
			_rustMaskTexture.Texture = existingTexture;
			_rustMaskTexture.Format = RenderTextureFormat.R8;
		}

		private void SetupPolishMaskTexture(Texture existingTexture)
		{
			_polishMaskTexture = FindOrCreateTexture("_PolishMask");
			_polishMaskTexture.Slot = new CwSlot(_materialIndex, "_PolishMask");
			_polishMaskTexture.Group = new CwGroup(_polishGroupIndex);
			_polishMaskTexture.Width = _maskTextureResolution;
			_polishMaskTexture.Height = _maskTextureResolution;
			_polishMaskTexture.Color = new Color(_initialPolishCoverage, 0f, 0f, 1f);
			_polishMaskTexture.Texture = existingTexture;
			_polishMaskTexture.Format = RenderTextureFormat.R8;
		}

		private void SetupMetalMaskTexture(Texture existingTexture)
		{
			_metalMaskTexture = FindOrCreateTexture("_MetalMask");
			_metalMaskTexture.Slot = new CwSlot(_materialIndex, "_MetalMask");
			_metalMaskTexture.Group = new CwGroup(_metalMaskGroupIndex);
			_metalMaskTexture.Width = _maskTextureResolution;
			_metalMaskTexture.Height = _maskTextureResolution;
			_metalMaskTexture.Color = new Color(_initialMetalCoverage, 0f, 0f, 1f);
			_metalMaskTexture.Texture = existingTexture;
			_metalMaskTexture.Format = RenderTextureFormat.R8;
		}

		private void SetupPaintMaskTexture(Texture existingTexture)
		{
			_paintMaskTexture = FindOrCreateTexture("_PaintMask");
			_paintMaskTexture.Slot = new CwSlot(_materialIndex, "_PaintMask");
			_paintMaskTexture.Group = new CwGroup(_paintMaskGroupIndex);
			_paintMaskTexture.Width = _maskTextureResolution;
			_paintMaskTexture.Height = _maskTextureResolution;
			_paintMaskTexture.Color = new Color(_initialPaintCoverage, 0f, 0f, 1f);
			_paintMaskTexture.Texture = existingTexture;
			_paintMaskTexture.Format = RenderTextureFormat.R8;
		}

		private CwPaintableMeshTexture FindOrCreateTexture(string slotName)
		{
			CwPaintableMeshTexture[] components = GetComponents<CwPaintableMeshTexture>();
			foreach (CwPaintableMeshTexture cwPaintableMeshTexture in components)
			{
				if (cwPaintableMeshTexture.Slot.Name == slotName)
				{
					return cwPaintableMeshTexture;
				}
			}
			return base.gameObject.AddComponent<CwPaintableMeshTexture>();
		}

		public void ExecuteSurfaceOperation(SurfaceOperation op)
		{
			switch (op)
			{
			case SurfaceOperation.ClearDirt:
				ClearDirt(sync: false);
				break;
			case SurfaceOperation.ApplyFullDirt:
				ApplyFullDirt(sync: false);
				break;
			case SurfaceOperation.ClearRust:
				ClearRust(sync: false);
				break;
			case SurfaceOperation.ApplyFullRust:
				ApplyFullRust(sync: false);
				break;
			case SurfaceOperation.ClearPaint:
				ClearPaint(sync: false);
				break;
			case SurfaceOperation.ApplyFullPaint:
				ApplyFullPaint(sync: false);
				break;
			case SurfaceOperation.ResetPaintColor:
				ResetPaintColor(sync: false);
				break;
			case SurfaceOperation.ClearPolish:
				ClearPolish(sync: false);
				break;
			case SurfaceOperation.ApplyFullPolish:
				ApplyFullPolish(sync: false);
				break;
			case SurfaceOperation.ClearMetal:
				ClearMetal(sync: false);
				break;
			case SurfaceOperation.ApplyFullMetal:
				ApplyFullMetal(sync: false);
				break;
			case SurfaceOperation.ResetToInitialState:
				ResetToInitialState(sync: false);
				break;
			case SurfaceOperation.FullRestoration:
				FullRestoration(sync: false);
				break;
			case SurfaceOperation.PristineFinish:
				PristineFinish(sync: false);
				break;
			}
		}

		public void ClearDirt(bool sync = true)
		{
			if (_dirtMaskTexture != null && _dirtMaskTexture.Activated)
			{
				_dirtMaskTexture.Clear(null, Color.black);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ClearDirt);
			}
		}

		public void ApplyFullDirt(bool sync = true)
		{
			if (_dirtMaskTexture != null && _dirtMaskTexture.Activated)
			{
				_dirtMaskTexture.Clear(null, Color.white);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ApplyFullDirt);
			}
		}

		public void ClearRust(bool sync = true)
		{
			if (_rustMaskTexture != null && _rustMaskTexture.Activated)
			{
				_rustMaskTexture.Clear(null, Color.black);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ClearRust);
			}
		}

		public void ApplyFullRust(bool sync = true)
		{
			if (_rustMaskTexture != null && _rustMaskTexture.Activated)
			{
				_rustMaskTexture.Clear(null, Color.white);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ApplyFullRust);
			}
		}

		public void ClearPaint(bool sync = true)
		{
			if (_paintMaskTexture != null && _paintMaskTexture.Activated)
			{
				_paintMaskTexture.Clear(null, Color.black);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ClearPaint);
			}
		}

		public void ApplyFullPaint(bool sync = true)
		{
			if (_paintMaskTexture != null && _paintMaskTexture.Activated)
			{
				_paintMaskTexture.Clear(null, Color.white);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ApplyFullPaint);
			}
		}

		public void ResetPaintColor(bool sync = true)
		{
			if (_paintColorTexture != null && _paintColorTexture.Activated)
			{
				_paintColorTexture.Clear(null, _initialPaintColor);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ResetPaintColor);
			}
		}

		public void ClearPolish(bool sync = true)
		{
			if (_polishMaskTexture != null && _polishMaskTexture.Activated)
			{
				_polishMaskTexture.Clear(null, Color.black);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ClearPolish);
			}
		}

		public void ApplyFullPolish(bool sync = true)
		{
			if (_polishMaskTexture != null && _polishMaskTexture.Activated)
			{
				_polishMaskTexture.Clear(null, Color.white);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ApplyFullPolish);
			}
		}

		public void ClearMetal(bool sync = true)
		{
			if (_metalMaskTexture != null && _metalMaskTexture.Activated)
			{
				_metalMaskTexture.Clear(null, Color.black);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ClearMetal);
			}
		}

		public void ApplyFullMetal(bool sync = true)
		{
			if (_metalMaskTexture != null && _metalMaskTexture.Activated)
			{
				_metalMaskTexture.Clear(null, Color.white);
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ApplyFullMetal);
			}
		}

		public void ResetToInitialState(bool sync = true)
		{
			CwPaintableMeshTexture dirtMaskTexture = _dirtMaskTexture;
			if ((object)dirtMaskTexture != null && dirtMaskTexture.Activated)
			{
				_dirtMaskTexture.Clear(null, new Color(_initialDirtCoverage, 0f, 0f, 1f));
			}
			CwPaintableMeshTexture rustMaskTexture = _rustMaskTexture;
			if ((object)rustMaskTexture != null && rustMaskTexture.Activated)
			{
				_rustMaskTexture.Clear(null, new Color(_initialRustCoverage, 0f, 0f, 1f));
			}
			CwPaintableMeshTexture paintMaskTexture = _paintMaskTexture;
			if ((object)paintMaskTexture != null && paintMaskTexture.Activated)
			{
				_paintMaskTexture.Clear(null, new Color(_initialPaintCoverage, 0f, 0f, 1f));
			}
			CwPaintableMeshTexture polishMaskTexture = _polishMaskTexture;
			if ((object)polishMaskTexture != null && polishMaskTexture.Activated)
			{
				_polishMaskTexture.Clear(null, new Color(_initialPolishCoverage, 0f, 0f, 1f));
			}
			CwPaintableMeshTexture paintColorTexture = _paintColorTexture;
			if ((object)paintColorTexture != null && paintColorTexture.Activated)
			{
				_paintColorTexture.Clear(null, _initialPaintColor);
			}
			CwPaintableMeshTexture metalMaskTexture = _metalMaskTexture;
			if ((object)metalMaskTexture != null && metalMaskTexture.Activated)
			{
				_metalMaskTexture.Clear(null, new Color(_initialMetalCoverage, 0f, 0f, 1f));
			}
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.ResetToInitialState);
			}
		}

		public void FullRestoration(bool sync = true)
		{
			ClearDirt(sync: false);
			ClearRust(sync: false);
			ClearPaint(sync: false);
			ClearPolish(sync: false);
			ClearMetal(sync: false);
			ResetPaintColor(sync: false);
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.FullRestoration);
			}
		}

		public void PristineFinish(bool sync = true)
		{
			ClearDirt(sync: false);
			ClearRust(sync: false);
			ApplyFullPaint(sync: false);
			ApplyFullPolish(sync: false);
			if (sync)
			{
				_networkSync?.SyncSurfaceOperation(_surfaceIndex, SurfaceOperation.PristineFinish);
			}
		}

		public void Save()
		{
			if (Application.isPlaying)
			{
				string saveKey = SaveKey;
				if (_paintColorTexture != null)
				{
					EvilSave.SaveRaw(saveKey + "_paintColor", _paintColorTexture.GetPngData());
				}
				if (_dirtMaskTexture != null)
				{
					EvilSave.SaveRaw(saveKey + "_dirtMask", _dirtMaskTexture.GetPngData());
				}
				if (_rustMaskTexture != null)
				{
					EvilSave.SaveRaw(saveKey + "_rustMask", _rustMaskTexture.GetPngData());
				}
				if (_paintMaskTexture != null)
				{
					EvilSave.SaveRaw(saveKey + "_paintMask", _paintMaskTexture.GetPngData());
				}
				if (_polishMaskTexture != null)
				{
					EvilSave.SaveRaw(saveKey + "_polishMask", _polishMaskTexture.GetPngData());
				}
				if (_metalMaskTexture != null)
				{
					EvilSave.SaveRaw(saveKey + "_metalMask", _metalMaskTexture.GetPngData());
				}
				EvilSave.Save(saveKey + "_paintAge", _currentPaintAge);
			}
		}

		public void Load()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			string saveKey = SaveKey;
			if (HasSaveData())
			{
				if (_paintColorTexture != null && EvilSave.HasKey(saveKey + "_paintColor"))
				{
					_paintColorTexture.LoadFromData(EvilSave.LoadRaw(saveKey + "_paintColor"));
				}
				if (_dirtMaskTexture != null && EvilSave.HasKey(saveKey + "_dirtMask"))
				{
					_dirtMaskTexture.LoadFromData(EvilSave.LoadRaw(saveKey + "_dirtMask"));
				}
				if (_rustMaskTexture != null && EvilSave.HasKey(saveKey + "_rustMask"))
				{
					_rustMaskTexture.LoadFromData(EvilSave.LoadRaw(saveKey + "_rustMask"));
				}
				if (_paintMaskTexture != null && EvilSave.HasKey(saveKey + "_paintMask"))
				{
					_paintMaskTexture.LoadFromData(EvilSave.LoadRaw(saveKey + "_paintMask"));
				}
				if (_polishMaskTexture != null && EvilSave.HasKey(saveKey + "_polishMask"))
				{
					_polishMaskTexture.LoadFromData(EvilSave.LoadRaw(saveKey + "_polishMask"));
				}
				if (_metalMaskTexture != null && EvilSave.HasKey(saveKey + "_metalMask"))
				{
					_metalMaskTexture.LoadFromData(EvilSave.LoadRaw(saveKey + "_metalMask"));
				}
				if (EvilSave.HasKey(saveKey + "_paintAge"))
				{
					SetPaintAge(EvilSave.Load(saveKey + "_paintAge", 0f));
				}
				_networkSync?.SyncSurfaceTextures(_surfaceIndex);
			}
		}

		public bool HasSaveData()
		{
			return EvilSave.HasKey(SaveKey + "_paintColor");
		}

		public void DeleteSaveData()
		{
			string saveKey = SaveKey;
			EvilSave.DeleteKey(saveKey + "_paintColor");
			EvilSave.DeleteKey(saveKey + "_dirtMask");
			EvilSave.DeleteKey(saveKey + "_rustMask");
			EvilSave.DeleteKey(saveKey + "_paintMask");
			EvilSave.DeleteKey(saveKey + "_polishMask");
			EvilSave.DeleteKey(saveKey + "_metalMask");
			EvilSave.DeleteKey(saveKey + "_paintAge");
		}

		private void EditorSave()
		{
			Save();
		}

		private void EditorLoad()
		{
			Load();
		}

		private void EditorDeleteSave()
		{
			DeleteSaveData();
		}

		private void EditorShowSaveInfo()
		{
		}
	}
}
