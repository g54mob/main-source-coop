using System;
using Features.HingeModule.Scripts;
using UnityEngine;

namespace Features.GuillotineModule.Scripts
{
	public class GuillotineInitializeController : MonoBehaviour
	{
		[SerializeField]
		private HingeSetupHandler _hingeSetupHandler;

		private bool _isGuillotineInitialized;

		public bool IsGuillotineInitialized
		{
			get
			{
				return _isGuillotineInitialized;
			}
			private set
			{
				_isGuillotineInitialized = value;
				if (_isGuillotineInitialized)
				{
					this.OnGuillotineInitialized?.Invoke();
				}
			}
		}

		public event Action OnGuillotineInitialized;

		private void OnEnable()
		{
			if (_hingeSetupHandler.IsJointInitialized)
			{
				IsGuillotineInitialized = true;
			}
			else
			{
				_hingeSetupHandler.OnJointInitialized += OnJointInitialized;
			}
		}

		private void OnDisable()
		{
			_hingeSetupHandler.OnJointInitialized -= OnJointInitialized;
		}

		private void OnJointInitialized()
		{
			IsGuillotineInitialized = true;
		}
	}
}
