using Unity.Collections;
using UnityEngine;

namespace Features.CollectingModule.Scripts
{
	public class CollectItemBellClapperCollisionFilter : MonoBehaviour
	{
		[SerializeField]
		private Transform _itemRoot;

		[SerializeField]
		private Transform _innerColliderRoot;

		[SerializeField]
		private Collider _clapperCollider;

		[SerializeField]
		private Rigidbody _clapperBody;

		[SerializeField]
		private Rigidbody _bellBody;

		private int _clapperBodyId;

		private int _bellBodyId;

		private void Awake()
		{
			_clapperBodyId = _clapperBody.GetInstanceID();
			_bellBodyId = _bellBody.GetInstanceID();
			_clapperCollider.hasModifiableContacts = true;
			Collider[] componentsInChildren = _innerColliderRoot.GetComponentsInChildren<Collider>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].hasModifiableContacts = true;
			}
		}

		private void OnEnable()
		{
			Physics.ContactModifyEvent += OnContactModify;
			Physics.ContactModifyEventCCD += OnContactModify;
		}

		private void OnDisable()
		{
			Physics.ContactModifyEvent -= OnContactModify;
			Physics.ContactModifyEventCCD -= OnContactModify;
		}

		private void Start()
		{
			Collider[] componentsInChildren = _itemRoot.GetComponentsInChildren<Collider>(includeInactive: true);
			foreach (Collider collider in componentsInChildren)
			{
				if (!(collider == _clapperCollider) && !IsInnerBellCollider(collider))
				{
					Physics.IgnoreCollision(_clapperCollider, collider, ignore: true);
				}
			}
		}

		private void OnContactModify(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
		{
			for (int i = 0; i < pairs.Length; i++)
			{
				ModifiableContactPair value = pairs[i];
				bool num = value.bodyInstanceID == _clapperBodyId;
				bool flag = value.otherBodyInstanceID == _clapperBodyId;
				if (!num && !flag)
				{
					continue;
				}
				bool flag2 = value.bodyInstanceID == _bellBodyId;
				bool flag3 = value.otherBodyInstanceID == _bellBodyId;
				if (flag2 || flag3)
				{
					ModifiableMassProperties massProperties = value.massProperties;
					if (flag2)
					{
						massProperties.inverseMassScale = 0f;
						massProperties.inverseInertiaScale = 0f;
					}
					else
					{
						massProperties.otherInverseMassScale = 0f;
						massProperties.otherInverseInertiaScale = 0f;
					}
					value.massProperties = massProperties;
					pairs[i] = value;
				}
			}
		}

		private bool IsInnerBellCollider(Collider otherCollider)
		{
			Transform transform = otherCollider.transform;
			if (!(transform == _innerColliderRoot))
			{
				return transform.IsChildOf(_innerColliderRoot);
			}
			return true;
		}
	}
}
