using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.DamageableTrackModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts
{
	public class EnemyItemDamageProcessor : MonoBehaviour
	{
		[SerializeField]
		private SimpleEnemyDamageable _simpleEnemyDamageable;

		private ItemCollisionModel _itemCollisionModel;

		private MultiplayerModel _multiplayerModel;

		[Inject]
		public void InjectDependencies(ItemCollisionModel itemCollisionModel, MultiplayerModel multiplayerModel)
		{
			_itemCollisionModel = itemCollisionModel;
			_multiplayerModel = multiplayerModel;
		}

		private void OnEnable()
		{
			_itemCollisionModel.OnCollisionAdded += ProcessItem;
		}

		private void OnDisable()
		{
			_itemCollisionModel.OnCollisionAdded -= ProcessItem;
		}

		private void ProcessItem(ItemCollisionData itemCollisionData)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient && itemCollisionData.CollisionCollider.gameObject == base.gameObject && itemCollisionData.Force > 5f && itemCollisionData.Item.DamageOnCollide > 0f)
			{
				_simpleEnemyDamageable.DamageRPC(itemCollisionData.Item.DamageOnCollide, itemCollisionData.Item.NetworkObject.StateAuthority.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForPlayerAttack(DamageType.Melee)));
			}
		}
	}
}
