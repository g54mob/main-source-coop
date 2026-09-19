using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.LevelModule.Scripts;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.StoreModule.Scripts
{
	[NetworkBehaviourWeaved(7)]
	public class StoreCardBehaviour : NetworkBehaviour
	{
		[SerializeField]
		private LayerMask _defaultExcludeLayerMask;

		[SerializeField]
		private LayerMask _activatedExcludeLayerMask;

		[SerializeField]
		private GameObject _visual;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private MeshRenderer _meshRenderer;

		[SerializeField]
		private TMP_Text _priceText;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Collider _buyingZoneCheckCollider;

		[SerializeField]
		private PhysicsMaterial _bouncyPhysicMaterial;

		[SerializeField]
		private float _physicMaterialRevertDelay = 0.5f;

		[SerializeField]
		private float _forceStrength = 25f;

		[SerializeField]
		private float _upwardForceComponent = 0.3f;

		[SerializeField]
		private float _moveToTransformSpeed = 2f;

		[SerializeField]
		private float _revertMaskDelay = 0.2f;

		[SerializeField]
		public SoundSourceBehaviour SoundSourceBehaviour;

		private Material _meshRendererMaterial;

		private Coroutine _moveToTransformCoroutine;

		private PhysicsMaterial[] _originalPhysicMaterials;

		private StoreCardData _storeCardData;

		private StoreRewardBase _reward;

		private Color _spawnColor = Color.white;

		private int _activatedTargetPlayerId = -1;

		private int _cachedCost;

		private StorePoolConfiguration _storePoolConfiguration;

		private StoreLevelConfiguration _storeLevelConfiguration;

		private LevelModel _levelModel;

		private StoreRewardFactory _storeRewardFactory;

		private StoreCustomizationModel _storeCustomizationModel;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsActivated", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsActivated;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CardDataId", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CardDataId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HasCardData", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _HasCardData;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SpawnColor", 3, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Color _SpawnColor;

		private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

		private static readonly int Mask = Shader.PropertyToID("_Mask");

		public Rigidbody Rigidbody => _rigidbody;

		public SimplePointGrabable SimplePointGrabable => _simplePointGrabable;

		public GameObject Visual => _visual;

		public StoreCardData CardData => _storeCardData;

		public Color EffectiveSpawnColor => _spawnColor;

		[Networked]
		[OnChangedRender("OnIsActivatedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsActivated
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreCardBehaviour.IsActivated. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreCardBehaviour.IsActivated. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public bool IsActivatedForPurchase
		{
			get
			{
				if (!IsActivated)
				{
					return _activatedTargetPlayerId >= 0;
				}
				return true;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe int CardDataId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreCardBehaviour.CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreCardBehaviour.CardDataId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe bool HasCardData
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreCardBehaviour.HasCardData. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreCardBehaviour.HasCardData. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		[Networked]
		[OnChangedRender("OnSpawnColorRender")]
		[NetworkedWeaved(3, 4)]
		public unsafe Color SpawnColor
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreCardBehaviour.SpawnColor. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Color*)(Ptr + 3);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing StoreCardBehaviour.SpawnColor. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Color*)(Ptr + 3) = value;
			}
		}

		public bool IsInitialized { get; private set; }

		public event Action OnCardDataSet;

		public event Action OnActivationChanged;

		public void SetVisualsEnabled(bool isEnabled)
		{
			MeshRenderer[] componentsInChildren = ((_visual != null) ? _visual : base.gameObject).GetComponentsInChildren<MeshRenderer>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = isEnabled;
			}
		}

		[Inject]
		private void InjectDependencies(StorePoolConfiguration storePoolConfiguration, StoreLevelConfiguration storeLevelConfiguration, LevelModel levelModel, StoreRewardFactory storeRewardFactory, StoreCustomizationModel storeCustomizationModel)
		{
			_storePoolConfiguration = storePoolConfiguration;
			_storeLevelConfiguration = storeLevelConfiguration;
			_levelModel = levelModel;
			_storeRewardFactory = storeRewardFactory;
			_storeCustomizationModel = storeCustomizationModel;
		}

		public bool TryGetBuyingZoneCheckBounds(out Bounds bounds)
		{
			if (_buyingZoneCheckCollider == null)
			{
				bounds = default(Bounds);
				return false;
			}
			bounds = _buyingZoneCheckCollider.bounds;
			return true;
		}

		private void OnValidate()
		{
			if (_buyingZoneCheckCollider == null)
			{
				_buyingZoneCheckCollider = GetComponent<Collider>();
			}
		}

		private void Awake()
		{
			_meshRendererMaterial = new Material(_meshRenderer.material);
			_meshRenderer.material = _meshRendererMaterial;
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			_originalPhysicMaterials = new PhysicsMaterial[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				_originalPhysicMaterials[i] = componentsInChildren[i].material;
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			IsInitialized = true;
			if (_storeCardData == null && HasCardData)
			{
				ApplyCardData(CardDataId);
			}
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			StopMoveToTransformCoroutine();
		}

		private IEnumerator RevertPhysicsMaterialAfterDelay()
		{
			yield return new WaitForSeconds(_physicMaterialRevertDelay);
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length && i < _originalPhysicMaterials.Length; i++)
			{
				componentsInChildren[i].material = _originalPhysicMaterials[i];
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1974004566u)]
		public void SetDataRPC([RpcPayload(4)] int randomStoreCardId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1974004566u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreCardBehaviour::SetDataRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(randomStoreCardId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			AssignCardData(randomStoreCardId);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3236229630u)]
		public void SetDeadPlayerDataRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3236229630u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreCardBehaviour::SetDeadPlayerDataRPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			AssignCardData(-1);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2705672360u)]
		public void SetHealPotionDataRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2705672360u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreCardBehaviour::SetHealPotionDataRPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			AssignCardData(-2);
		}

		private void AssignCardData(int cardDataId)
		{
			if (base.HasStateAuthority)
			{
				CardDataId = cardDataId;
				HasCardData = true;
			}
			ApplyCardData(cardDataId);
		}

		private void ApplyCardData(int cardDataId)
		{
			if (cardDataId == -1)
			{
				SetData(_storePoolConfiguration.DeadPlayerStoreCardData);
			}
			else if (cardDataId == -2)
			{
				SetData(_storePoolConfiguration.HealPotionStoreCardData);
			}
			else if (cardDataId >= 0)
			{
				SetData(_storePoolConfiguration.RewardPoolWithChances[cardDataId]);
			}
		}

		public int GetCost()
		{
			return _cachedCost;
		}

		public void SetData(StoreCardData storeCardData)
		{
			_storeCardData = storeCardData;
			CacheCost();
			SetCardIcon(storeCardData);
			_reward = _storeRewardFactory.Create(_storeCardData.GetRewardData());
			if (_storeCardData.CardSprite.bounds.size.y > _storeCardData.CardSprite.bounds.size.x)
			{
				float num = _storeCardData.CardSprite.bounds.size.x / _storeCardData.CardSprite.bounds.size.y * _meshRenderer.transform.localScale.z;
				num *= _meshRenderer.transform.parent.localScale.z / _meshRenderer.transform.parent.localScale.x;
				_meshRenderer.transform.localScale = new Vector3(num, _meshRenderer.transform.localScale.y, _meshRenderer.transform.localScale.z);
			}
			else
			{
				float num2 = _storeCardData.CardSprite.bounds.size.y / _storeCardData.CardSprite.bounds.size.x * _meshRenderer.transform.localScale.x;
				num2 *= _meshRenderer.transform.parent.localScale.x / _meshRenderer.transform.parent.localScale.z;
				_meshRenderer.transform.localScale = new Vector3(_meshRenderer.transform.localScale.x, _meshRenderer.transform.localScale.y, num2);
			}
			_priceText.SetText(GetCost() + "¢");
			this.OnCardDataSet?.Invoke();
		}

		private void CacheCost()
		{
			_cachedCost = 0;
			if (_storeCardData == null)
			{
				return;
			}
			if (!_storeLevelConfiguration.StoreByLevelData.TryGetValue(_levelModel.LastLoadedLevel, out var value))
			{
				value = _storeLevelConfiguration.DefaultStoreByLevelData;
			}
			if (value.LevelRewardsSettings == null)
			{
				return;
			}
			foreach (LevelRewardSetting levelRewardsSetting in value.LevelRewardsSettings)
			{
				if (!(levelRewardsSetting.CardId != _storeCardData.Id))
				{
					_cachedCost = levelRewardsSetting.CardPrice;
					break;
				}
			}
		}

		public async Task ApplyReward(StoreRewardContext context, StoreRewardApplyMoment moment)
		{
			if (_reward != null && _reward.ApplyMoment == moment)
			{
				context.SpawnColor = _spawnColor;
				await _reward.ApplyReward(context);
			}
		}

		public void MoveToTransform(Transform target)
		{
			if (!(target == null))
			{
				MoveToWorldTarget(target.position, target.eulerAngles.y);
			}
		}

		public void MoveToWorldTarget(Vector3 worldPosition, float targetEulerY)
		{
			if (base.HasStateAuthority)
			{
				StartMoveToWorldTarget(worldPosition, targetEulerY);
			}
			else
			{
				MoveToWorldTargetRpc(worldPosition, targetEulerY);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 538916446u)]
		private void MoveToWorldTargetRpc([RpcPayload(12)] Vector3 worldPosition, [RpcPayload(4)] float targetEulerY)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(538916446u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreCardBehaviour::MoveToWorldTargetRpc(UnityEngine.Vector3,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(worldPosition, 12);
						writer.Write(targetEulerY, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			StartMoveToWorldTarget(worldPosition, targetEulerY);
		}

		private void StartMoveToWorldTarget(Vector3 worldPosition, float targetEulerY)
		{
			if (!(_rigidbody == null))
			{
				StopMoveToTransformCoroutine();
				_rigidbody.linearVelocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
				_moveToTransformCoroutine = StartCoroutine(MoveToWorldPositionCoroutine(worldPosition, targetEulerY));
			}
		}

		private void StopMoveToTransformCoroutine()
		{
			if (_moveToTransformCoroutine != null)
			{
				StopCoroutine(_moveToTransformCoroutine);
				_moveToTransformCoroutine = null;
			}
		}

		private IEnumerator MoveToWorldPositionCoroutine(Vector3 worldPosition, float targetEulerY)
		{
			Vector3 startPosition = _rigidbody.position;
			Vector3 startEuler = base.transform.eulerAngles;
			float startY = startEuler.y;
			float num = Vector3.Distance(startPosition, worldPosition);
			float duration = ((_moveToTransformSpeed > 0f) ? (num / _moveToTransformSpeed) : 0f);
			float elapsed = 0f;
			while (elapsed < duration)
			{
				if (_simplePointGrabable.GrabbedByPlayersCount > 0)
				{
					_moveToTransformCoroutine = null;
					yield break;
				}
				elapsed += Time.fixedDeltaTime;
				float t = ((duration > 0f) ? Mathf.Clamp01(elapsed / duration) : 1f);
				Vector3 position = Vector3.Lerp(startPosition, worldPosition, t);
				float y = Mathf.LerpAngle(startY, targetEulerY, t);
				Quaternion rot = Quaternion.Euler(startEuler.x, y, startEuler.z);
				_rigidbody.MovePosition(position);
				_rigidbody.MoveRotation(rot);
				yield return new WaitForFixedUpdate();
			}
			if (_simplePointGrabable.GrabbedByPlayersCount > 0)
			{
				_moveToTransformCoroutine = null;
				yield break;
			}
			_rigidbody.MovePosition(worldPosition);
			Vector3 eulerAngles = base.transform.eulerAngles;
			_rigidbody.MoveRotation(Quaternion.Euler(eulerAngles.x, targetEulerY, eulerAngles.z));
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			_moveToTransformCoroutine = null;
		}

		public void ApplyRandomForce()
		{
			ApplyRandomForceRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 949247743u)]
		private void ApplyRandomForceRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(949247743u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreCardBehaviour::ApplyRandomForceRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_rigidbody.excludeLayers = _activatedExcludeLayerMask;
			StartCoroutine(RevertIgnoreMaskAfterDelay());
			if (!(_rigidbody == null) && base.HasStateAuthority)
			{
				float f = UnityEngine.Random.Range(90f, 0f) * (MathF.PI / 180f);
				Vector3 vector = new Vector3(Mathf.Cos(f), _upwardForceComponent, Mathf.Sin(f));
				Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].material = _bouncyPhysicMaterial;
				}
				_rigidbody.angularVelocity = Vector3.zero;
				_rigidbody.AddForce(vector * _forceStrength, ForceMode.Impulse);
				StartCoroutine(RevertPhysicsMaterialAfterDelay());
			}
		}

		private IEnumerator RevertIgnoreMaskAfterDelay()
		{
			yield return new WaitForSeconds(_revertMaskDelay);
			_rigidbody.excludeLayers = _defaultExcludeLayerMask;
		}

		private void SetCardIcon(StoreCardData storeCardData)
		{
			_meshRendererMaterial.SetTexture(BaseMap, storeCardData.CardSprite.texture);
			if (storeCardData.CardMask != null)
			{
				_meshRendererMaterial.SetTexture(Mask, storeCardData.CardMask.texture);
			}
			if (storeCardData.RandomizeColor)
			{
				if (base.HasStateAuthority)
				{
					float h = (float)_storeCustomizationModel.Random.NextDouble();
					float s = Mathf.Lerp(0.5f, 1f, (float)_storeCustomizationModel.Random.NextDouble());
					float v = Mathf.Lerp(0.7f, 1f, (float)_storeCustomizationModel.Random.NextDouble());
					SpawnColor = Color.HSVToRGB(h, s, v);
				}
				ApplySpawnColor();
			}
		}

		private void OnSpawnColorRender()
		{
			ApplySpawnColor();
		}

		private void ApplySpawnColor()
		{
			_spawnColor = SpawnColor;
			_meshRendererMaterial.color = SpawnColor;
		}

		public void ActivateCard(int targetPlayerId)
		{
			ActivateCardRpc(targetPlayerId);
		}

		public void DeactivateCard()
		{
			DeactivateCardRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 4080102889u)]
		private void ActivateCardRpc([RpcPayload(4)] int targetPlayerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4080102889u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreCardBehaviour::ActivateCardRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(targetPlayerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.HasStateAuthority)
			{
				IsActivated = true;
			}
			_activatedTargetPlayerId = targetPlayerId;
			this.OnActivationChanged?.Invoke();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1579427149u)]
		private void DeactivateCardRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1579427149u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StoreModule.Scripts.StoreCardBehaviour::DeactivateCardRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.HasStateAuthority)
			{
				IsActivated = false;
			}
			_activatedTargetPlayerId = -1;
			this.OnActivationChanged?.Invoke();
		}

		private void OnIsActivatedRender()
		{
			this.OnActivationChanged?.Invoke();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsActivated = _IsActivated;
			CardDataId = _CardDataId;
			HasCardData = _HasCardData;
			SpawnColor = _SpawnColor;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsActivated = IsActivated;
			_CardDataId = CardDataId;
			_HasCardData = HasCardData;
			_SpawnColor = SpawnColor;
		}

		[NetworkRpcWeavedInvoker(1974004566u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetDataRPC_0040Invoker1974004566([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreCardBehaviour)context.TargetBehaviour).SetDataRPC(value);
		}

		[NetworkRpcWeavedInvoker(3236229630u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetDeadPlayerDataRPC_0040Invoker3236229630([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreCardBehaviour)context.TargetBehaviour).SetDeadPlayerDataRPC();
		}

		[NetworkRpcWeavedInvoker(2705672360u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetHealPotionDataRPC_0040Invoker2705672360([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreCardBehaviour)context.TargetBehaviour).SetHealPotionDataRPC();
		}

		[NetworkRpcWeavedInvoker(538916446u)]
		[Preserve]
		[WeaverGenerated]
		protected static void MoveToWorldTargetRpc_0040Invoker538916446([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out Vector3 value, 12);
			payloadReader.Read(out float value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreCardBehaviour)context.TargetBehaviour).MoveToWorldTargetRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(949247743u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ApplyRandomForceRpc_0040Invoker949247743([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreCardBehaviour)context.TargetBehaviour).ApplyRandomForceRpc();
		}

		[NetworkRpcWeavedInvoker(4080102889u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ActivateCardRpc_0040Invoker4080102889([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreCardBehaviour)context.TargetBehaviour).ActivateCardRpc(value);
		}

		[NetworkRpcWeavedInvoker(1579427149u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DeactivateCardRpc_0040Invoker1579427149([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((StoreCardBehaviour)context.TargetBehaviour).DeactivateCardRpc();
		}
	}
}
