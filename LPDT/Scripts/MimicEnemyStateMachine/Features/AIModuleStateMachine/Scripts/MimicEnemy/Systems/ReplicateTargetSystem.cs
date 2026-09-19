using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.DeadPartsModule.Data;
using Features.GrabModule.Scripts;
using Features.PlayerGrabModule.Scripts;
using Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatsVisualizationModule.Scripts;
using Features.SkinConfiguration.Scripts;
using Features.TeethModule.Scripts.Tooth;
using Fusion;
using Obi;
using PlayerCustomization;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class ReplicateTargetSystem : MonoSystem
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

		private MimicEnemyContext _context;

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

		private SpawnedPlayersModel _spawnedPlayersModel;

		private PlayerCustomizationModel _playerCustomizationModel;

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _hatPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _torsoPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _bottomPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private int _appliedReplicateTarget = -1;

		private EnemyHeadwearModel _enemyHeadwearModel;

		private MultipleVisibilityHandler _hatVisibilityHandler;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context, SkinsConfiguration skinsConfiguration, SpawnedPlayersModel spawnedPlayersModel, PlayerCustomizationModel playerCustomizationModel, ButtTexturePresetConfiguration buttTexturePresetConfiguration, ButtMeshPresetConfiguration buttMeshPresetConfigurationl, EnemyHeadwearModel enemyHeadwearModel)
		{
			_context = context;
			_skinsConfiguration = skinsConfiguration;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerCustomizationModel = playerCustomizationModel;
			_buttTexturePresetConfiguration = buttTexturePresetConfiguration;
			_buttMeshPresetConfiguration = buttMeshPresetConfigurationl;
			_enemyHeadwearModel = enemyHeadwearModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_enemyHeadwearModel.OnEnemyHeadwearChanged += HandleEnemyHeadwearChanged;
			ApplyHeadwearHatState();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_enemyHeadwearModel.OnEnemyHeadwearChanged -= HandleEnemyHeadwearChanged;
			base.Despawned(runner, hasState);
		}

		private void HandleEnemyHeadwearChanged(uint enemyObjectId, bool isWearing)
		{
			if (!(base.Object == null) && base.Object.IsValid && base.Object.Id.Raw == enemyObjectId)
			{
				ApplyHeadwearHatState();
			}
		}

		private void ApplyHeadwearHatState()
		{
			if (!(_hatVisibilityHandler == null) && !(base.Object == null) && base.Object.IsValid)
			{
				if (_enemyHeadwearModel.IsWearing(base.Object.Id.Raw))
				{
					_hatVisibilityHandler.DisableHatRenderObject();
				}
				else
				{
					_hatVisibilityHandler.EnableHatRenderObject();
				}
			}
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
			if (base.Initialized && _enabled && !(base.Object.StateAuthority != base.Runner.LocalPlayer) && base.Runner.ActivePlayers.Any() && _context.NeedFindReplicateTarget)
			{
				PlayerRef randomPlayer = GetRandomPlayer();
				if (_spawnedPlayersModel.Players.ContainsKey(randomPlayer))
				{
					_context.SetReplicateTarget(randomPlayer.PlayerId);
					_context.NeedFindReplicateTarget = false;
				}
			}
		}

		private void Update()
		{
			if (base.Initialized && _context.ReplicateTarget > 0 && _context.ReplicateTarget != _appliedReplicateTarget)
			{
				ReplicateTarget();
			}
		}

		private void ReplicateTarget()
		{
			if (_context.ReplicateTarget == 0)
			{
				return;
			}
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData data) => data.PlayerId == _context.ReplicateTarget);
			if (playerCustomizationSlotData == null)
			{
				return;
			}
			if (!playerCustomizationSlotData.IsFullSkin)
			{
				foreach (Renderer fullSkinRenderer in _fullSkinRenderers)
				{
					fullSkinRenderer.gameObject.SetActive(value: false);
				}
				foreach (Renderer partSkinRenderer in _partSkinRenderers)
				{
					partSkinRenderer.gameObject.SetActive(value: true);
				}
			}
			else
			{
				foreach (Renderer fullSkinRenderer2 in _fullSkinRenderers)
				{
					fullSkinRenderer2.gameObject.SetActive(value: true);
				}
				foreach (Renderer partSkinRenderer2 in _partSkinRenderers)
				{
					partSkinRenderer2.gameObject.SetActive(value: false);
				}
			}
			ApplyButtPreset(playerCustomizationSlotData.ButtMeshPreset);
			MultipleVisibilityHandler multipleVisibilityHandler = ActivateHatPartSkin(playerCustomizationSlotData.HatPartSkinId);
			MultipleVisibilityHandler multipleVisibilityHandler2 = ActivateTorsoPartSkin(playerCustomizationSlotData.TorsoPartSkinId);
			MultipleVisibilityHandler multipleVisibilityHandler3 = ActivateBottomPartSkin(playerCustomizationSlotData.BottomPartSkinId);
			if (!(multipleVisibilityHandler == null) && !(multipleVisibilityHandler2 == null) && !(multipleVisibilityHandler3 == null))
			{
				multipleVisibilityHandler.EnableHatRenderObjectOnly();
				multipleVisibilityHandler2.EnableTorsoRenderObjectOnly();
				multipleVisibilityHandler3.EnableBottomRenderObjectOnly();
				_hatVisibilityHandler = multipleVisibilityHandler;
				ApplyHeadwearHatState();
				ApplyButtTexturePreset(playerCustomizationSlotData.ButtTexturePreset, playerCustomizationSlotData.IsFullSkin);
				ApplyTopPartSkinColor(playerCustomizationSlotData.PrimaryColor);
				ApplyBottomPartSkinColor(playerCustomizationSlotData.VariableColor);
				DisableRandomTeeth(_lostTeethChance);
				if (_particleStatsVisualizer != null)
				{
					_particleStatsVisualizer.InitializeTargetPlayer(_context.ReplicateTarget);
				}
				_appliedReplicateTarget = _context.ReplicateTarget;
				_context.NeedReplicateTarget = false;
				RefreshDissolveMaterials();
			}
		}

		private void RefreshDissolveMaterials()
		{
			_enemyDeathDissolveEffect.RefreshMaterials();
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
			_simplePointGrabableOutline.Outline = multipleVisibilityHandler.GetComponent<PlayerOutlineRegistrar>().Outline;
			return multipleVisibilityHandler;
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
				_enemyDeathDissolveEffect.RemoveRenderers(value2.GetComponentsInChildren<Renderer>(includeInactive: true));
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
					obj.renderingLayerMask = _renderingLayerMask;
				}
				value = (cachedSkins[skinId] = playerSkin.GetComponent<MultipleVisibilityHandler>());
			}
			value.EnableRenderObject();
			_enemyDeathDissolveEffect.AddRenderers(value.GetComponentsInChildren<Renderer>(includeInactive: true));
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

		private PlayerRef GetRandomPlayer()
		{
			return base.Runner.ActivePlayers.ElementAt(Random.Range(0, base.Runner.ActivePlayers.Count()));
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
			_appliedReplicateTarget = -1;
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
