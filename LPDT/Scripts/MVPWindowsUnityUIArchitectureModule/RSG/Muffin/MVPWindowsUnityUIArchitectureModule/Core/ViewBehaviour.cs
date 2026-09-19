using System;
using UnityEngine;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public abstract class ViewBehaviour : MonoBehaviour
	{
		public bool IsShown { get; protected set; }

		public bool IsEnabled { get; private set; }

		public bool IsViewDisposed { get; private set; }

		public event Action OnDispose;

		public event Action OnDisabled;

		public event Action OnEnabled;

		private void Awake()
		{
			IsShown = base.gameObject.activeSelf;
		}

		private void OnDestroy()
		{
			DisposeView();
		}

		public virtual void ShowView()
		{
			base.gameObject.SetActive(value: true);
			IsShown = true;
		}

		protected virtual void OnEnable()
		{
			IsEnabled = true;
			this.OnEnabled?.Invoke();
		}

		protected virtual void OnDisable()
		{
			if (IsEnabled)
			{
				IsEnabled = false;
				this.OnDisabled?.Invoke();
			}
		}

		public virtual void HideView()
		{
			base.gameObject.SetActive(value: false);
			IsShown = false;
		}

		public virtual void DisposeView()
		{
			if (!IsViewDisposed)
			{
				IsViewDisposed = true;
				this.OnDispose?.Invoke();
			}
		}

		public void DisableView()
		{
			OnDisable();
		}

		public Transform GetRoot()
		{
			return base.gameObject.transform;
		}
	}
}
