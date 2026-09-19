using System;
using UnityEngine;

namespace Features.GameUpdaterModule
{
	public class GameUpdater : MonoBehaviour, IGameUpdater
	{
		public event Action OnUpdate;

		public event Action OnFixedUpdate;

		public event Action OnLateUpdate;

		public event Action OnOnDrawGizmos;

		private void Update()
		{
			this.OnUpdate?.Invoke();
		}

		private void FixedUpdate()
		{
			this.OnFixedUpdate?.Invoke();
		}

		private void LateUpdate()
		{
			this.OnLateUpdate?.Invoke();
		}

		private void OnDrawGizmos()
		{
			this.OnOnDrawGizmos?.Invoke();
		}

		public void ClearEvents()
		{
			this.OnUpdate = null;
			this.OnFixedUpdate = null;
			this.OnLateUpdate = null;
		}
	}
}
