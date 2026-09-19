using System.Collections.Generic;
using Features.Movement.Scripts;
using Obi;
using UnityEngine;
using Zenject;

namespace Features.ObiCollidersChunk.Scripts
{
	public class ObiColliderChunk : MonoBehaviour
	{
		[SerializeField]
		private List<ObiCollider> _colliders;

		[SerializeField]
		private float _triggerDistance = 10f;

		[SerializeField]
		private Transform _centerTranform;

		private PlayerMovableModel _playerMovableModel;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		private void Update()
		{
			if ((Object)(object)_playerMovableModel.LocalMovable == null)
			{
				return;
			}
			bool flag = Vector3.Distance(((Component)(object)_playerMovableModel.LocalMovable).transform.position, _centerTranform.position) <= _triggerDistance;
			foreach (ObiCollider collider in _colliders)
			{
				if (!(collider == null))
				{
					collider.enabled = flag;
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (!(_centerTranform == null))
			{
				Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
				Gizmos.DrawSphere(_centerTranform.position, _triggerDistance);
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(_centerTranform.position, _triggerDistance);
			}
		}
	}
}
