using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Diagnostics;
using VContainer.Internal;

namespace VContainer.Unity
{
	[DefaultExecutionOrder(-5000)]
	public class LifetimeScope : MonoBehaviour, IDisposable
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public readonly struct ParentOverrideScope : IDisposable
		{
			public ParentOverrideScope(LifetimeScope nextParent)
			{
				lock (SyncRoot)
				{
					GlobalOverrideParents.Push(nextParent);
				}
			}

			public void Dispose()
			{
				lock (SyncRoot)
				{
					GlobalOverrideParents.Pop();
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public readonly struct ExtraInstallationScope : IDisposable
		{
			public ExtraInstallationScope(IInstaller installer)
			{
				lock (SyncRoot)
				{
					GlobalExtraInstallers.Push(installer);
				}
			}

			void IDisposable.Dispose()
			{
				lock (SyncRoot)
				{
					GlobalExtraInstallers.Pop();
				}
			}
		}

		private static readonly List<LifetimeScope> WaitingList = new List<LifetimeScope>();

		[SerializeField]
		public ParentReference parentReference;

		[SerializeField]
		public bool autoRun = true;

		[SerializeField]
		protected List<GameObject> autoInjectGameObjects;

		private string scopeName;

		private static readonly Stack<LifetimeScope> GlobalOverrideParents = new Stack<LifetimeScope>();

		private static readonly Stack<IInstaller> GlobalExtraInstallers = new Stack<IInstaller>();

		private static readonly object SyncRoot = new object();

		private readonly List<IInstaller> localExtraInstallers = new List<IInstaller>();

		public IObjectResolver Container { get; private set; }

		public LifetimeScope Parent { get; private set; }

		public bool IsRoot
		{
			get
			{
				if (VContainerSettings.Instance != null)
				{
					return VContainerSettings.Instance.IsRootLifetimeScopeInstance(this);
				}
				return false;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void SubscribeSceneEvents()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void EnqueueAwake(LifetimeScope lifetimeScope)
		{
			WaitingList.Add(lifetimeScope);
		}

		private static void CancelAwake(LifetimeScope lifetimeScope)
		{
			WaitingList.Remove(lifetimeScope);
		}

		private static void AwakeWaitingChildren(LifetimeScope awakenParent)
		{
			if (WaitingList.Count <= 0)
			{
				return;
			}
			List<LifetimeScope> buffer;
			using (ListPool<LifetimeScope>.Get(out buffer))
			{
				for (int num = WaitingList.Count - 1; num >= 0; num--)
				{
					LifetimeScope lifetimeScope = WaitingList[num];
					if (lifetimeScope.parentReference.Type == awakenParent.GetType())
					{
						lifetimeScope.parentReference.Object = awakenParent;
						WaitingList.RemoveAt(num);
						buffer.Add(lifetimeScope);
					}
				}
				foreach (LifetimeScope item in buffer)
				{
					item.Awake();
				}
			}
		}

		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (WaitingList.Count <= 0)
			{
				return;
			}
			List<LifetimeScope> buffer;
			using (ListPool<LifetimeScope>.Get(out buffer))
			{
				for (int num = WaitingList.Count - 1; num >= 0; num--)
				{
					LifetimeScope lifetimeScope = WaitingList[num];
					if (lifetimeScope.gameObject.scene == scene)
					{
						WaitingList.RemoveAt(num);
						buffer.Add(lifetimeScope);
					}
				}
				foreach (LifetimeScope item in buffer)
				{
					item.Awake();
				}
			}
		}

		public static LifetimeScope Create(IInstaller installer = null, string name = null)
		{
			GameObject obj = new GameObject(name ?? "LifetimeScope");
			obj.SetActive(value: false);
			LifetimeScope lifetimeScope = obj.AddComponent<LifetimeScope>();
			if (installer != null)
			{
				lifetimeScope.localExtraInstallers.Add(installer);
			}
			obj.SetActive(value: true);
			return lifetimeScope;
		}

		public static LifetimeScope Create(Action<IContainerBuilder> configuration, string name = null)
		{
			return Create(new ActionInstaller(configuration), name);
		}

		public static ParentOverrideScope EnqueueParent(LifetimeScope parent)
		{
			return new ParentOverrideScope(parent);
		}

		public static ExtraInstallationScope Enqueue(Action<IContainerBuilder> installing)
		{
			return new ExtraInstallationScope(new ActionInstaller(installing));
		}

		public static ExtraInstallationScope Enqueue(IInstaller installer)
		{
			return new ExtraInstallationScope(installer);
		}

		[Obsolete("LifetimeScope.PushParent is obsolete. Use LifetimeScope.EnqueueParent instead.", false)]
		public static ParentOverrideScope PushParent(LifetimeScope parent)
		{
			return new ParentOverrideScope(parent);
		}

		[Obsolete("LifetimeScope.Push is obsolete. Use LifetimeScope.Enqueue instead.", false)]
		public static ExtraInstallationScope Push(Action<IContainerBuilder> installing)
		{
			return Enqueue(installing);
		}

		[Obsolete("LifetimeScope.Push is obsolete. Use LifetimeScope.Enqueue instead.", false)]
		public static ExtraInstallationScope Push(IInstaller installer)
		{
			return Enqueue(installer);
		}

		public static LifetimeScope Find<T>(Scene scene) where T : LifetimeScope
		{
			return Find(typeof(T), scene);
		}

		public static LifetimeScope Find<T>() where T : LifetimeScope
		{
			return Find(typeof(T));
		}

		private static LifetimeScope Find(Type type, Scene scene)
		{
			List<GameObject> buffer;
			using (ListPool<GameObject>.Get(out buffer))
			{
				scene.GetRootGameObjects(buffer);
				foreach (GameObject item in buffer)
				{
					LifetimeScope lifetimeScope = item.GetComponentInChildren(type) as LifetimeScope;
					if (lifetimeScope != null)
					{
						return lifetimeScope;
					}
				}
			}
			return null;
		}

		private static LifetimeScope Find(Type type)
		{
			return (LifetimeScope)UnityEngine.Object.FindAnyObjectByType(type);
		}

		protected virtual void Awake()
		{
			if (VContainerSettings.DiagnosticsEnabled && string.IsNullOrEmpty(scopeName))
			{
				scopeName = $"{base.name} ({base.gameObject.GetInstanceID()})";
			}
			try
			{
				if (autoRun)
				{
					Build();
				}
			}
			catch (VContainerParentTypeReferenceNotFound) when (!IsRoot)
			{
				if (WaitingList.Contains(this))
				{
					throw;
				}
				EnqueueAwake(this);
			}
		}

		protected virtual void OnDestroy()
		{
			DisposeCore();
		}

		protected virtual void Configure(IContainerBuilder builder)
		{
		}

		public void Dispose()
		{
			DisposeCore();
			if (this != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		public void DisposeCore()
		{
			Container?.Dispose();
			Container = null;
			CancelAwake(this);
			if (VContainerSettings.DiagnosticsEnabled)
			{
				DiagnositcsContext.RemoveCollector(scopeName);
			}
		}

		public void Build()
		{
			if (Parent == null)
			{
				Parent = GetRuntimeParent();
			}
			if (Parent != null)
			{
				if (VContainerSettings.Instance != null && Parent.IsRoot && Parent.Container == null)
				{
					Parent.Build();
				}
				Parent.Container.CreateScope(delegate(IContainerBuilder builder)
				{
					builder.RegisterBuildCallback(SetContainer);
					builder.ApplicationOrigin = this;
					builder.Diagnostics = (VContainerSettings.DiagnosticsEnabled ? DiagnositcsContext.GetCollector(scopeName) : null);
					InstallTo(builder);
				});
			}
			else
			{
				ContainerBuilder containerBuilder = new ContainerBuilder
				{
					ApplicationOrigin = this,
					Diagnostics = (VContainerSettings.DiagnosticsEnabled ? DiagnositcsContext.GetCollector(scopeName) : null)
				};
				containerBuilder.RegisterBuildCallback(SetContainer);
				InstallTo(containerBuilder);
				containerBuilder.Build();
			}
			AwakeWaitingChildren(this);
		}

		private void SetContainer(IObjectResolver container)
		{
			Container = container;
			AutoInjectAll();
		}

		public TScope CreateChild<TScope>(IInstaller installer = null, string childScopeName = null) where TScope : LifetimeScope
		{
			GameObject gameObject = new GameObject(childScopeName ?? "LifetimeScope (Child)");
			gameObject.SetActive(value: false);
			if (IsRoot)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			else
			{
				gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			}
			TScope val = gameObject.AddComponent<TScope>();
			if (installer != null)
			{
				val.localExtraInstallers.Add(installer);
			}
			val.parentReference.Object = this;
			gameObject.SetActive(value: true);
			return val;
		}

		public LifetimeScope CreateChild(IInstaller installer = null, string childScopeName = null)
		{
			return CreateChild<LifetimeScope>(installer, childScopeName);
		}

		public TScope CreateChild<TScope>(Action<IContainerBuilder> installation, string childScopeName = null) where TScope : LifetimeScope
		{
			return CreateChild<TScope>(new ActionInstaller(installation), childScopeName);
		}

		public LifetimeScope CreateChild(Action<IContainerBuilder> installation, string childScopeName = null)
		{
			return CreateChild<LifetimeScope>(new ActionInstaller(installation), childScopeName);
		}

		public TScope CreateChildFromPrefab<TScope>(TScope prefab, IInstaller installer = null) where TScope : LifetimeScope
		{
			bool activeSelf = prefab.gameObject.activeSelf;
			using (new ObjectResolverUnityExtensions.PrefabDirtyScope(prefab.gameObject))
			{
				if (activeSelf)
				{
					prefab.gameObject.SetActive(value: false);
				}
				TScope val = UnityEngine.Object.Instantiate(prefab, base.transform, worldPositionStays: false);
				if (installer != null)
				{
					val.localExtraInstallers.Add(installer);
				}
				val.parentReference.Object = this;
				if (activeSelf)
				{
					prefab.gameObject.SetActive(value: true);
					val.gameObject.SetActive(value: true);
				}
				return val;
			}
		}

		public TScope CreateChildFromPrefab<TScope>(TScope prefab, Action<IContainerBuilder> installation) where TScope : LifetimeScope
		{
			return CreateChildFromPrefab(prefab, new ActionInstaller(installation));
		}

		private void InstallTo(IContainerBuilder builder)
		{
			Configure(builder);
			foreach (IInstaller localExtraInstaller in localExtraInstallers)
			{
				localExtraInstaller.Install(builder);
			}
			localExtraInstallers.Clear();
			lock (SyncRoot)
			{
				foreach (IInstaller globalExtraInstaller in GlobalExtraInstallers)
				{
					globalExtraInstaller.Install(builder);
				}
			}
			builder.RegisterInstance(this).AsSelf();
			EntryPointsBuilder.EnsureDispatcherRegistered(builder);
		}

		protected virtual LifetimeScope FindParent()
		{
			return null;
		}

		private LifetimeScope GetRuntimeParent()
		{
			if (IsRoot)
			{
				return null;
			}
			if (parentReference.Object != null)
			{
				return parentReference.Object;
			}
			LifetimeScope lifetimeScope = FindParent();
			if (lifetimeScope != null)
			{
				if (parentReference.Type != null && parentReference.Type != lifetimeScope.GetType())
				{
					Debug.LogWarning($"FindParent returned {lifetimeScope.GetType()} but parent reference type is {parentReference.Type}. This may be unintentional.");
				}
				return lifetimeScope;
			}
			if (parentReference.Type != null && parentReference.Type != GetType())
			{
				LifetimeScope lifetimeScope2 = Find(parentReference.Type);
				if (lifetimeScope2 != null && lifetimeScope2.Container != null)
				{
					return lifetimeScope2;
				}
				throw new VContainerParentTypeReferenceNotFound(parentReference.Type, $"{base.name} could not found parent reference of type : {parentReference.Type}");
			}
			lock (SyncRoot)
			{
				if (GlobalOverrideParents.Count > 0)
				{
					return GlobalOverrideParents.Peek();
				}
			}
			if (VContainerSettings.Instance != null)
			{
				return VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance();
			}
			return null;
		}

		private void AutoInjectAll()
		{
			if (autoInjectGameObjects == null)
			{
				return;
			}
			foreach (GameObject autoInjectGameObject in autoInjectGameObjects)
			{
				if (autoInjectGameObject != null)
				{
					Container.InjectGameObject(autoInjectGameObject);
				}
			}
		}
	}
}
