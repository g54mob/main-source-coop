using FishNet.Managing.Predicting;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Component.Prediction
{
	public class OfflineRigidbody : MonoBehaviour
	{
		[Tooltip("Type of prediction movement which is being used.")]
		[SerializeField]
		private RigidbodyType _rigidbodyType;

		private Transform _graphicalObject;

		[Tooltip("True to also get rigidbody components within children.")]
		[SerializeField]
		private bool _getInChildren;

		private RigidbodyPauser _rigidbodyPauser = new RigidbodyPauser();

		private PredictionManager _predictionManager;

		public void SetGraphicalObject(Transform value)
		{
			_graphicalObject = value;
			UpdateRigidbodies();
		}

		private void Awake()
		{
			InitializeOnce();
		}

		private void OnDestroy()
		{
			ChangeSubscription(subscribe: false);
		}

		private void InitializeOnce()
		{
			_predictionManager = InstanceFinder.PredictionManager;
			UpdateRigidbodies();
			ChangeSubscription(subscribe: true);
		}

		public void SetPredictionManager(PredictionManager pm)
		{
			if (!(pm == _predictionManager))
			{
				ChangeSubscription(subscribe: false);
				_predictionManager = pm;
				ChangeSubscription(subscribe: true);
			}
		}

		public void UpdateRigidbodies()
		{
			_rigidbodyPauser.UpdateRigidbodies(base.transform, _rigidbodyType, _getInChildren, _graphicalObject);
		}

		private void ChangeSubscription(bool subscribe)
		{
			if (!(_predictionManager == null))
			{
				if (subscribe)
				{
					_predictionManager.OnPreReconcile += _predictionManager_OnPreReconcile;
					_predictionManager.OnPostReconcile += _predictionManager_OnPostReconcile;
				}
				else
				{
					_predictionManager.OnPreReconcile -= _predictionManager_OnPreReconcile;
					_predictionManager.OnPostReconcile -= _predictionManager_OnPostReconcile;
				}
			}
		}

		private void _predictionManager_OnPreReconcile(NetworkBehaviour obj)
		{
			_rigidbodyPauser.Pause();
		}

		private void _predictionManager_OnPostReconcile(NetworkBehaviour obj)
		{
			_rigidbodyPauser.Unpause();
		}
	}
}
