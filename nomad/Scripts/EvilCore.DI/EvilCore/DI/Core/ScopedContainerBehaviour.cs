using System;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using VContainer;

namespace EvilCore.DI.Core
{
	public class ScopedContainerBehaviour : MonoBehaviour
	{
		[SerializeField]
		private bool _autoCreateOnStart;

		private ScopedContainer _scopedContainer;

		private bool _isInitialized;

		public ScopedContainer Container => _scopedContainer;

		[Inject]
		public void Initialize(ScopedContainer scopedContainer)
		{
			_scopedContainer = scopedContainer;
			_isInitialized = true;
		}

		protected virtual void Start()
		{
			if (_autoCreateOnStart && _isInitialized)
			{
				CreateScope();
			}
		}

		public void CreateScope(Action<IContainerBuilder> builder = null)
		{
			if (!_isInitialized)
			{
				EvilLogger.LogError("ScopedContainerBehaviour not initialized", "CreateScope", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\ScopedContainer.cs", 157);
			}
			else
			{
				_scopedContainer.BeginScope(builder ?? new Action<IContainerBuilder>(ConfigureContainer));
			}
		}

		protected virtual void ConfigureContainer(IContainerBuilder builder)
		{
		}

		protected virtual void OnDestroy()
		{
			if (_isInitialized)
			{
				_scopedContainer.DisposeScope();
			}
		}

		protected T Resolve<T>()
		{
			if (!_isInitialized)
			{
				EvilLogger.LogError("ScopedContainerBehaviour not initialized", "Resolve", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\ScopedContainer.cs", 190);
				return default(T);
			}
			return _scopedContainer.Resolve<T>();
		}
	}
}
