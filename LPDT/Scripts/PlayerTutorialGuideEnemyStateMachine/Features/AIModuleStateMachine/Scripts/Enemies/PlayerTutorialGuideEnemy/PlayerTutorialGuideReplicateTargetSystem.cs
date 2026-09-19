using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.DeadPartsModule.Data;
using Features.DeadPartsModule.Scripts;
using Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling;
using Features.PlayerStatsVisualizationModule.Scripts;
using Features.SkinConfiguration.Scripts;
using Features.TeethModule.Scripts.Tooth;
using Fusion;
using Obi;
using PlayerCustomization;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerTutorialGuideReplicateTargetSystem : MonoSystem
	{
		private static readonly int ColorProperty = Shader.PropertyToID("_Color");

		private static readonly int ObiBaseColorProperty = Shader.PropertyToID("_BaseColor");

		private static readonly int UpperColorProperty = Shader.PropertyToID("_UpperColor");

		private static readonly int BottomColorProperty = Shader.PropertyToID("_BottomColor");

		private static readonly int BaseMapProperty = Shader.PropertyToID("_BaseMap");

		private static readonly int NormalMapProperty = Shader.PropertyToID("_NormalMap");

		private static readonly int MaskProperty = Shader.PropertyToID("_Mask");

		private static readonly int MetallicProperty = Shader.PropertyToID("_Metallic");

		private static readonly int DestructionFactorProperty = Shader.PropertyToID("_DestructionFactor");

		[SerializeField]
		private List<SkinnedMeshRenderer> _deadPartRenderers;

		[SerializeField]
		private ParticleStatsVisualizer _particleStatsVisualizer;

		[SerializeField]
		private Transform _skinParent;

		[SerializeField]
		private Transform _characterRootBone;

		[SerializeField]
		private SkinnedMeshRenderer _characterSkinnedMesh;

		[SerializeField]
		private List<Renderer> _renderers = new List<Renderer>();

		[SerializeField]
		private List<Renderer> _maskedRenderers = new List<Renderer>();

		[SerializeField]
		private List<Renderer> _variableColorRenderers = new List<Renderer>();

		[SerializeField]
		private List<Renderer> _universalRenderers = new List<Renderer>();

		[SerializeField]
		private List<ObiRopeExtrudedRenderer> _obiRopeExtrudedRenderers = new List<ObiRopeExtrudedRenderer>();

		[SerializeField]
		private List<PlayerTooth> _playerTeeth = new List<PlayerTooth>();

		[SerializeField]
		private CustomizationAnimationReactor _reactor;

		[SerializeField]
		private List<Renderer> _fullSkinRenderers = new List<Renderer>();

		[SerializeField]
		private List<Renderer> _partSkinRenderers = new List<Renderer>();

		[Range(0f, 100f)]
		[SerializeField]
		private float _lostTeethChance;

		private PlayerTutorialGuideEnemyContext _context;

		private ButtTexturePresetConfiguration _buttTexturePresetConfiguration;

		private ButtMeshPresetConfiguration _buttMeshPresetConfiguration;

		private SkinsConfiguration _skinsConfiguration;

		private PlayerDeadPartsConfiguration _playerDeadPartsConfiguration;

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _hatPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _torsoPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _bottomPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private List<SkinType> _availableSkins;

		private MultipleVisibilityHandler _activeBottomVisibility;

		private int _appliedSeed;

		private int _appliedBottomPartUsageCount = -1;

		private bool _enabled;

		public PlayerCustomizationSlotData CurrentAppearanceSlotData { get; private set; }

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(PlayerTutorialGuideEnemyContext context, SkinsConfiguration skinsConfiguration, ButtTexturePresetConfiguration buttTexturePresetConfiguration, ButtMeshPresetConfiguration buttMeshPresetConfiguration, PlayerDeadPartsConfiguration playerDeadPartsConfiguration)
		{
			_context = context;
			_skinsConfiguration = skinsConfiguration;
			_buttTexturePresetConfiguration = buttTexturePresetConfiguration;
			_buttMeshPresetConfiguration = buttMeshPresetConfiguration;
			_playerDeadPartsConfiguration = playerDeadPartsConfiguration;
		}

		public override void Enable()
		{
			_enabled = true;
		}

		public override void Disable()
		{
			_enabled = false;
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Initialized && _enabled && base.HasStateAuthority && _context.NeedGenerateAppearance)
			{
				_context.SetAppearanceSeed(Random.Range(1, int.MaxValue));
				_context.NeedGenerateAppearance = false;
			}
		}

		private void Update()
		{
			if (base.Initialized)
			{
				if (_context.AppearanceSeed > 0 && _context.AppearanceSeed != _appliedSeed)
				{
					ApplyRandomAppearance(_context.AppearanceSeed);
				}
				if (_context.BottomPartUsageCount != _appliedBottomPartUsageCount)
				{
					ApplyBottomPartDestructionFactor(_context.BottomPartUsageCount);
					_appliedBottomPartUsageCount = _context.BottomPartUsageCount;
				}
			}
		}

		private void ApplyBottomPartDestructionFactor(int usageCount)
		{
			if (_context.DeadPartType == DeadPartType.None)
			{
				return;
			}
			int num = _playerDeadPartsConfiguration.MaxDeadPartUsageCount[_context.DeadPartType];
			float value = Mathf.Clamp01((float)usageCount / (float)num);
			foreach (SkinnedMeshRenderer deadPartRenderer in _deadPartRenderers)
			{
				deadPartRenderer.material.SetFloat(DestructionFactorProperty, value);
			}
		}

		private void ApplyRandomAppearance(int seed)
		{
			Random.State state = Random.state;
			Random.InitState(seed);
			Color color = RandomColor();
			Color color2 = RandomColor();
			SkinType skinId = RandomSkin();
			SkinType skinId2 = RandomSkin();
			SkinType skinType = RandomSkin();
			Random.state = state;
			CurrentAppearanceSlotData = new PlayerCustomizationSlotData
			{
				VariableColor = color2,
				BottomPartSkinId = skinType
			};
			foreach (Renderer fullSkinRenderer in _fullSkinRenderers)
			{
				fullSkinRenderer.gameObject.SetActive(value: false);
			}
			foreach (Renderer partSkinRenderer in _partSkinRenderers)
			{
				partSkinRenderer.gameObject.SetActive(value: true);
			}
			MultipleVisibilityHandler multipleVisibilityHandler = ActivateHatPartSkin(skinId);
			MultipleVisibilityHandler multipleVisibilityHandler2 = ActivateTorsoPartSkin(skinId2);
			MultipleVisibilityHandler multipleVisibilityHandler3 = ActivateBottomPartSkin(skinType);
			if (!(multipleVisibilityHandler == null) && !(multipleVisibilityHandler2 == null) && !(multipleVisibilityHandler3 == null))
			{
				multipleVisibilityHandler.EnableHatRenderObjectOnly();
				multipleVisibilityHandler2.EnableTorsoRenderObjectOnly();
				multipleVisibilityHandler3.EnableBottomRenderObjectOnly();
				_activeBottomVisibility = multipleVisibilityHandler3;
				ApplyTopPartSkinColor(color);
				ApplyBottomPartSkinColor(color2);
				DisableRandomTeeth(_lostTeethChance);
				_appliedSeed = seed;
			}
		}

		public void SetBottomSkinVisible(bool visible)
		{
			if (!(_activeBottomVisibility == null))
			{
				if (visible)
				{
					_activeBottomVisibility.EnableBottomRenderObjectOnly();
				}
				else
				{
					_activeBottomVisibility.DisableRenderObject();
				}
			}
		}

		private Color RandomColor()
		{
			return new Color(Random.value, Random.value, Random.value);
		}

		private SkinType RandomSkin()
		{
			if (_availableSkins == null)
			{
				_availableSkins = _skinsConfiguration.PlayerSkins.Keys.Where((SkinType skin) => skin != SkinType.None && skin != SkinType.Empty).ToList();
			}
			if (_availableSkins.Count == 0)
			{
				return SkinType.None;
			}
			return _availableSkins[Random.Range(0, _availableSkins.Count)];
		}

		private TKey RandomKey<TKey>(ICollection<TKey> keys, TKey fallback)
		{
			if (keys == null || keys.Count == 0)
			{
				return fallback;
			}
			return keys.ElementAt(Random.Range(0, keys.Count));
		}

		private void ApplyButtPreset(ButtMeshPreset currentPreset)
		{
			if (currentPreset != ButtMeshPreset.None && _buttMeshPresetConfiguration.MeshPresets.TryGetValue(currentPreset, out var value) && value != null)
			{
				AssignMesh(_deadPartRenderers, value.Mesh);
			}
		}

		private void ApplyButtTexturePreset(ButtTexturePreset buttTexturePreset)
		{
			if (_buttTexturePresetConfiguration.TexturePresets.TryGetValue(buttTexturePreset, out var value) && value != null)
			{
				AssignShaderTexture(_variableColorRenderers, value.BaseTexture, BaseMapProperty);
				AssignShaderTexture(_variableColorRenderers, value.NormalMap, NormalMapProperty);
				AssignShaderTexture(_variableColorRenderers, value.Mask, MaskProperty);
				AssignShaderFloat(_variableColorRenderers, value.Metallic, MetallicProperty);
			}
		}

		private void AssignMesh(List<SkinnedMeshRenderer> deadPartRenderers, Mesh mesh)
		{
			foreach (SkinnedMeshRenderer deadPartRenderer in deadPartRenderers)
			{
				deadPartRenderer.sharedMesh = UnityEngine.Object.Instantiate(mesh);
			}
		}

		private void AssignShaderFloat(List<Renderer> renderers, float value, int shaderFloatProperty)
		{
			foreach (Renderer renderer in renderers)
			{
				Material material = new Material(renderer.material);
				material.SetFloat(shaderFloatProperty, value);
				renderer.material = material;
			}
		}

		private void DisableRandomTeeth(float chancePercent)
		{
			if (!base.Object.HasStateAuthority)
			{
				return;
			}
			foreach (PlayerTooth playerTooth in _playerTeeth)
			{
				if (Random.Range(0f, 100f) <= chancePercent)
				{
					playerTooth.DisableToothRPC();
				}
			}
		}

		private MultipleVisibilityHandler ActivateHatPartSkin(SkinType skinId)
		{
			return ActivatePartSkin(skinId, _hatPartCachedSkins);
		}

		private MultipleVisibilityHandler ActivateTorsoPartSkin(SkinType skinId)
		{
			return ActivatePartSkin(skinId, _torsoPartCachedSkins);
		}

		private MultipleVisibilityHandler ActivateBottomPartSkin(SkinType skinId)
		{
			return ActivatePartSkin(skinId, _bottomPartCachedSkins);
		}

		private MultipleVisibilityHandler ActivatePartSkin(SkinType skinId, Dictionary<SkinType, MultipleVisibilityHandler> cachedSkins)
		{
			foreach (MultipleVisibilityHandler value2 in cachedSkins.Values)
			{
				value2.DisableRenderObject();
			}
			if (!cachedSkins.TryGetValue(skinId, out var value))
			{
				PlayerSkin playerSkin = UnityEngine.Object.Instantiate(_skinsConfiguration.PlayerSkins[skinId], _skinParent, worldPositionStays: false);
				playerSkin.Initialize(_reactor);
				SkinnedMeshRenderer[] componentsInChildren = playerSkin.GetComponentsInChildren<SkinnedMeshRenderer>();
				foreach (SkinnedMeshRenderer obj in componentsInChildren)
				{
					obj.rootBone = _characterRootBone;
					obj.bones = _characterSkinnedMesh.bones;
				}
				value = (cachedSkins[skinId] = playerSkin.GetComponent<MultipleVisibilityHandler>());
			}
			value.EnableRenderObject();
			return value;
		}

		private void ApplyTopPartSkinColor(Color color)
		{
			AssignColor(_renderers, color);
			AssignShaderColor(_maskedRenderers, color, ColorProperty);
			AssignShaderColor(_universalRenderers, color, UpperColorProperty);
			AssignObiShaderColor(_obiRopeExtrudedRenderers, color, ObiBaseColorProperty);
		}

		private void ApplyBottomPartSkinColor(Color color)
		{
			AssignShaderColor(_variableColorRenderers, color, ColorProperty);
			AssignShaderColor(_universalRenderers, color, BottomColorProperty);
		}

		private void AssignColor(List<Renderer> renderers, Color color)
		{
			foreach (Renderer renderer in renderers)
			{
				Material material = new Material(renderer.material)
				{
					color = color
				};
				renderer.material = material;
			}
		}

		private void AssignShaderColor(List<Renderer> renderers, Color color, int shaderColorProperty)
		{
			foreach (Renderer renderer in renderers)
			{
				Material material = new Material(renderer.material);
				material.SetColor(shaderColorProperty, color);
				renderer.material = material;
			}
		}

		private void AssignObiShaderColor(List<ObiRopeExtrudedRenderer> obiRenderers, Color color, int shaderColorProperty)
		{
			foreach (ObiRopeExtrudedRenderer obiRenderer in obiRenderers)
			{
				Material material = new Material(obiRenderer.material);
				material.SetColor(shaderColorProperty, color);
				obiRenderer.material = material;
				obiRenderer.OnValidate();
			}
		}

		private void AssignShaderTexture(List<Renderer> renderers, Texture texture, int shaderTextureProperty)
		{
			foreach (Renderer renderer in renderers)
			{
				Material material = new Material(renderer.material);
				material.SetTexture(shaderTextureProperty, texture);
				renderer.material = material;
			}
		}

		public override void Clear()
		{
			foreach (MultipleVisibilityHandler value in _hatPartCachedSkins.Values)
			{
				if (value != null && value.gameObject != null)
				{
					UnityEngine.Object.Destroy(value.gameObject);
				}
			}
			_hatPartCachedSkins.Clear();
			foreach (MultipleVisibilityHandler value2 in _torsoPartCachedSkins.Values)
			{
				if (value2 != null && value2.gameObject != null)
				{
					UnityEngine.Object.Destroy(value2.gameObject);
				}
			}
			_torsoPartCachedSkins.Clear();
			foreach (MultipleVisibilityHandler value3 in _bottomPartCachedSkins.Values)
			{
				if (value3 != null && value3.gameObject != null)
				{
					UnityEngine.Object.Destroy(value3.gameObject);
				}
			}
			_bottomPartCachedSkins.Clear();
			_appliedSeed = 0;
			_appliedBottomPartUsageCount = -1;
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
