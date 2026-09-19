using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Settings;
using Features.DeadPartsModule.Data;
using Features.GrabModule.Scripts;
using Features.PlayerGrabModule.Scripts;
using Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling;
using Features.PlayerStatsVisualizationModule.Scripts;
using Features.SkinConfiguration.Scripts;
using Features.TeethModule.Scripts.Tooth;
using Fusion;
using Obi;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class BartenderReplicateTargetSystem : MonoSystem
	{
		private static readonly int _colorProperty = Shader.PropertyToID("_Color");

		private static readonly int _obiBaseColorProperty = Shader.PropertyToID("_BaseColor");

		private static readonly int _dissolveBaseColorProperty = Shader.PropertyToID("_BaseColor");

		private static readonly int _upperColorProperty = Shader.PropertyToID("_UpperColor");

		private static readonly int _bottomColorProperty = Shader.PropertyToID("_BottomColor");

		private static readonly int _baseMapProperty = Shader.PropertyToID("_BaseMap");

		private static readonly int _normalMapProperty = Shader.PropertyToID("_NormalMap");

		private static readonly int _maskProperty = Shader.PropertyToID("_Mask");

		private static readonly int _metallicProperty = Shader.PropertyToID("_Metallic");

		[SerializeField]
		private SimplePointGrabable _simplePointGrabableOutline;

		[SerializeField]
		private List<SkinnedMeshRenderer> _deadPartRenderers;

		[SerializeField]
		private ParticleStatsVisualizer _particleStatsVisualizer;

		private BartenderEnemyContext _context;

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
		private List<Renderer> _universalRenderers;

		[SerializeField]
		private List<Renderer> _eyelidRenderers = new List<Renderer>();

		[SerializeField]
		private List<ObiRopeExtrudedRenderer> _obiRopeExtrudedRenderers = new List<ObiRopeExtrudedRenderer>();

		[SerializeField]
		private List<PlayerTooth> _playerTeeth = new List<PlayerTooth>();

		[SerializeField]
		private CustomizationAnimationReactor _reactor;

		[SerializeField]
		private EnemyDeathDissolveEffect _enemyDeathDissolveEffect;

		[SerializeField]
		private List<Renderer> _fullSkinRenderers = new List<Renderer>();

		[SerializeField]
		private List<Renderer> _partSkinRenderers = new List<Renderer>();

		[Range(0f, 100f)]
		[SerializeField]
		private float _lostTeethChance;

		[SerializeField]
		private uint _renderingLayerMask;

		private ButtTexturePresetConfiguration _buttTexturePresetConfiguration;

		private ButtMeshPresetConfiguration _buttMeshPresetConfiguration;

		private SkinsConfiguration _skinsConfiguration;

		private BartenderAppearanceSettings _appearanceSettings;

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _hatPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _torsoPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _bottomPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private List<SkinType> _availableSkins;

		private int _appliedSeed;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(BartenderEnemyContext context, SkinsConfiguration skinsConfiguration, BartenderAppearanceSettings appearanceSettings, ButtTexturePresetConfiguration buttTexturePresetConfiguration, ButtMeshPresetConfiguration buttMeshPresetConfigurationl)
		{
			_context = context;
			_skinsConfiguration = skinsConfiguration;
			_appearanceSettings = appearanceSettings;
			_buttTexturePresetConfiguration = buttTexturePresetConfiguration;
			_buttMeshPresetConfiguration = buttMeshPresetConfigurationl;
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
			if (base.Initialized && _context.AppearanceSeed > 0 && _context.AppearanceSeed != _appliedSeed)
			{
				ApplyRandomAppearance(_context.AppearanceSeed);
			}
		}

		private void ApplyRandomAppearance(int seed)
		{
			Random.State state = Random.state;
			Random.InitState(seed);
			Color color = RandomColor();
			SkinType skinType = RandomSkinFromPool();
			Random.state = state;
			SetBodySkinMode(isFullSkin: true);
			MultipleVisibilityHandler multipleVisibilityHandler = ActivateHatPartSkin(skinType);
			MultipleVisibilityHandler multipleVisibilityHandler2 = ActivateTorsoPartSkin(skinType);
			MultipleVisibilityHandler multipleVisibilityHandler3 = ActivateBottomPartSkin(skinType);
			if (multipleVisibilityHandler == null || multipleVisibilityHandler2 == null || multipleVisibilityHandler3 == null)
			{
				Debug.LogWarning($"[Bartender] Failed to activate skin '{skinType}'. " + $"skinsCfg={_skinsConfiguration != null} parent={_skinParent != null} mesh={_characterSkinnedMesh != null}");
				return;
			}
			multipleVisibilityHandler.EnableHatRenderObjectOnly();
			multipleVisibilityHandler.DisableTorsoRenderObject();
			multipleVisibilityHandler.DisableBottomRenderObject();
			multipleVisibilityHandler2.EnableTorsoRenderObjectOnly();
			multipleVisibilityHandler2.DisableHatRenderObject();
			multipleVisibilityHandler2.DisableBottomRenderObject();
			multipleVisibilityHandler3.EnableBottomRenderObjectOnly();
			multipleVisibilityHandler3.DisableHatRenderObject();
			multipleVisibilityHandler3.DisableTorsoRenderObject();
			ApplyTopPartSkinColor(color);
			ApplyBottomPartSkinColor(color);
			DisableRandomTeeth(_lostTeethChance);
			RefreshDissolveMaterials();
			_appliedSeed = seed;
		}

		private void SetBodySkinMode(bool isFullSkin)
		{
			if (isFullSkin)
			{
				foreach (Renderer fullSkinRenderer in _fullSkinRenderers)
				{
					if (fullSkinRenderer != null)
					{
						fullSkinRenderer.gameObject.SetActive(value: true);
					}
				}
				{
					foreach (Renderer partSkinRenderer in _partSkinRenderers)
					{
						if (partSkinRenderer != null)
						{
							partSkinRenderer.gameObject.SetActive(value: false);
						}
					}
					return;
				}
			}
			foreach (Renderer fullSkinRenderer2 in _fullSkinRenderers)
			{
				if (fullSkinRenderer2 != null)
				{
					fullSkinRenderer2.gameObject.SetActive(value: false);
				}
			}
			foreach (Renderer partSkinRenderer2 in _partSkinRenderers)
			{
				if (partSkinRenderer2 != null)
				{
					partSkinRenderer2.gameObject.SetActive(value: true);
				}
			}
		}

		private Color RandomColor()
		{
			return new Color(Random.value, Random.value, Random.value);
		}

		private SkinType RandomSkinFromPool()
		{
			if (_availableSkins == null)
			{
				_availableSkins = new List<SkinType>();
				IReadOnlyList<SkinType> readOnlyList = ((_appearanceSettings != null) ? _appearanceSettings.PossibleSkins : null);
				if (readOnlyList != null)
				{
					for (int i = 0; i < readOnlyList.Count; i++)
					{
						SkinType skinType = readOnlyList[i];
						if (skinType != SkinType.None && skinType != SkinType.Empty && _skinsConfiguration != null && _skinsConfiguration.PlayerSkins.ContainsKey(skinType))
						{
							_availableSkins.Add(skinType);
						}
					}
				}
				if (_availableSkins.Count == 0)
				{
					_availableSkins.Add(SkinType.Skins10);
				}
			}
			return _availableSkins[Random.Range(0, _availableSkins.Count)];
		}

		private void RefreshDissolveMaterials()
		{
			_enemyDeathDissolveEffect?.RefreshMaterials();
		}

		private void ApplyButtPreset(ButtMeshPreset currentPreset)
		{
			if (currentPreset != ButtMeshPreset.None && _buttMeshPresetConfiguration.MeshPresets.TryGetValue(currentPreset, out var value) && value != null)
			{
				AssignMesh(_deadPartRenderers, value.Mesh);
			}
		}

		private void ApplyButtTexturePreset(ButtTexturePreset buttTexturePreset, bool isFullSkin)
		{
			if (_buttTexturePresetConfiguration.TexturePresets.TryGetValue(buttTexturePreset, out var value) && value != null)
			{
				List<Renderer> deadPartRenderers = (isFullSkin ? _fullSkinRenderers : _variableColorRenderers);
				AssignShaderTexture(deadPartRenderers, value.BaseTexture, _baseMapProperty);
				AssignShaderTexture(deadPartRenderers, value.NormalMap, _normalMapProperty);
				AssignShaderTexture(deadPartRenderers, value.Mask, _maskProperty);
				AssignShaderFloat(deadPartRenderers, value.Metallic, _metallicProperty);
			}
		}

		private void AssignMesh(List<SkinnedMeshRenderer> deadPartRenderers, Mesh mesh)
		{
			foreach (SkinnedMeshRenderer deadPartRenderer in deadPartRenderers)
			{
				deadPartRenderer.sharedMesh = UnityEngine.Object.Instantiate(mesh);
			}
		}

		private void AssignShaderFloat(List<Renderer> deadPartRenderers, float value, int shaderFloatProperty)
		{
			foreach (Renderer deadPartRenderer in deadPartRenderers)
			{
				Material material = new Material(deadPartRenderer.material);
				material.SetFloat(shaderFloatProperty, value);
				deadPartRenderer.material = material;
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
			MultipleVisibilityHandler multipleVisibilityHandler = ActivatePartSkin(skinId, _torsoPartCachedSkins);
			if (multipleVisibilityHandler != null && _simplePointGrabableOutline != null && multipleVisibilityHandler.TryGetComponent<PlayerOutlineRegistrar>(out var component) && component.Outline != null)
			{
				_simplePointGrabableOutline.Outline = component.Outline;
			}
			return multipleVisibilityHandler;
		}

		private MultipleVisibilityHandler ActivateBottomPartSkin(SkinType skinId)
		{
			return ActivatePartSkin(skinId, _bottomPartCachedSkins);
		}

		private MultipleVisibilityHandler ActivatePartSkin(SkinType skinId, Dictionary<SkinType, MultipleVisibilityHandler> cachedSkins)
		{
			foreach (MultipleVisibilityHandler value3 in cachedSkins.Values)
			{
				if (!(value3 == null))
				{
					value3.DisableRenderObject();
					_enemyDeathDissolveEffect?.RemoveRenderers(value3.GetComponentsInChildren<Renderer>(includeInactive: true));
				}
			}
			if (!cachedSkins.TryGetValue(skinId, out var value) || value == null)
			{
				if (_skinsConfiguration == null || !_skinsConfiguration.PlayerSkins.TryGetValue(skinId, out var value2) || value2 == null || _skinParent == null || _characterSkinnedMesh == null)
				{
					return null;
				}
				PlayerSkin playerSkin = UnityEngine.Object.Instantiate(value2, _skinParent, worldPositionStays: false);
				playerSkin.Initialize(_reactor);
				SkinnedMeshRenderer[] componentsInChildren = playerSkin.GetComponentsInChildren<SkinnedMeshRenderer>();
				foreach (SkinnedMeshRenderer obj in componentsInChildren)
				{
					obj.rootBone = _characterRootBone;
					obj.bones = _characterSkinnedMesh.bones;
					obj.renderingLayerMask = _renderingLayerMask;
				}
				value = playerSkin.GetComponent<MultipleVisibilityHandler>();
				if (value == null)
				{
					return null;
				}
				cachedSkins[skinId] = value;
			}
			value.EnableRenderObject();
			_enemyDeathDissolveEffect?.AddRenderers(value.GetComponentsInChildren<Renderer>(includeInactive: true));
			return value;
		}

		private void ApplyTopPartSkinColor(Color color)
		{
			AssignColor(_renderers, color);
			AssignShaderColor(_maskedRenderers, color, _colorProperty);
			AssignShaderColor(_eyelidRenderers, color, _dissolveBaseColorProperty);
			AssignShaderColor(_universalRenderers, color, _upperColorProperty);
			AssignObiShaderColor(_obiRopeExtrudedRenderers, color, _obiBaseColorProperty);
		}

		private void ApplyBottomPartSkinColor(Color color)
		{
			AssignShaderColor(_variableColorRenderers, color, _colorProperty);
			AssignShaderColor(_universalRenderers, color, _bottomColorProperty);
		}

		private void AssignColor(List<Renderer> playerRenderers, Color color)
		{
			if (playerRenderers == null)
			{
				return;
			}
			foreach (Renderer playerRenderer in playerRenderers)
			{
				Material material = new Material(playerRenderer.material)
				{
					color = color
				};
				playerRenderer.material = material;
			}
		}

		private void AssignShaderColor(List<Renderer> playerRenderers, Color color, int shaderColorProperty)
		{
			if (playerRenderers == null)
			{
				return;
			}
			foreach (Renderer playerRenderer in playerRenderers)
			{
				Material material = new Material(playerRenderer.material);
				material.SetColor(shaderColorProperty, color);
				playerRenderer.material = material;
			}
		}

		private void AssignObiShaderColor(List<ObiRopeExtrudedRenderer> obiRenderers, Color color, int shaderColorProperty)
		{
			if (obiRenderers == null)
			{
				return;
			}
			foreach (ObiRopeExtrudedRenderer obiRenderer in obiRenderers)
			{
				Material material = new Material(obiRenderer.material);
				material.SetColor(shaderColorProperty, color);
				obiRenderer.material = material;
				obiRenderer.OnValidate();
			}
		}

		private void AssignShaderTexture(List<Renderer> deadPartRenderers, Texture texture, int shaderTextureProperty)
		{
			foreach (Renderer deadPartRenderer in deadPartRenderers)
			{
				Material material = new Material(deadPartRenderer.material);
				material.SetTexture(shaderTextureProperty, texture);
				deadPartRenderer.material = material;
			}
		}

		private void AssignShaderVector(List<Renderer> deadPartRenderers, Vector4 vector, int shaderTextureProperty)
		{
			foreach (Renderer deadPartRenderer in deadPartRenderers)
			{
				Material material = new Material(deadPartRenderer.material);
				material.SetVector(shaderTextureProperty, vector);
				deadPartRenderer.material = material;
			}
		}

		private void AssignMaterial(List<Renderer> deadPartRenderers, Material material, int colorTextureProperty)
		{
			foreach (Renderer deadPartRenderer in deadPartRenderers)
			{
				Material material2 = new Material(material);
				material.SetColor(colorTextureProperty, deadPartRenderer.material.GetColor(colorTextureProperty));
				deadPartRenderer.material = material2;
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
			_availableSkins = null;
			_appliedSeed = 0;
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
