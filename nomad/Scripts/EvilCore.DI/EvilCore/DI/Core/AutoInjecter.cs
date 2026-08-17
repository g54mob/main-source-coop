using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace EvilCore.DI.Core
{
	[DefaultExecutionOrder(-900)]
	public class AutoInjecter : MonoBehaviour
	{
		public enum InjectionTiming
		{
			Awake = 0,
			Start = 1,
			OnEnable = 2,
			Manual = 3
		}

		public enum DebugLogLevel
		{
			None = 0,
			Errors = 1,
			Normal = 2,
			Verbose = 3
		}

		[Header("Injection Settings")]
		[Tooltip("When to perform the injection")]
		[SerializeField]
		private InjectionTiming injectionTiming = InjectionTiming.Start;

		[Tooltip("Whether to wait for LifetimeScope to be available")]
		[SerializeField]
		private bool waitForLifetimeScope = true;

		[Tooltip("Maximum time to wait for LifetimeScope (in seconds) - 0 means unlimited")]
		[SerializeField]
		private float maxWaitTime = 5f;

		[Tooltip("Whether to retry injection if it fails initially")]
		[SerializeField]
		private bool retryIfFailed = true;

		[Tooltip("Number of retry attempts (if retry is enabled)")]
		[SerializeField]
		private int maxRetries = 3;

		[Tooltip("Delay between retry attempts (in seconds)")]
		[SerializeField]
		private float retryDelay = 0.5f;

		[Header("Scope Settings")]
		[Tooltip("Whether to use a specific LifetimeScope (by name) instead of first found")]
		[SerializeField]
		private bool useNamedScope;

		[Tooltip("Name of the LifetimeScope to use (if useNamedScope is true)")]
		[SerializeField]
		private string scopeName = "GameLifetimeScope";

		[Header("Behavior Settings")]
		[Tooltip("Whether to inject child GameObjects")]
		[SerializeField]
		private bool injectChildren = true;

		[Tooltip("Whether to destroy this component after successful injection")]
		[SerializeField]
		private bool destroyAfterInjection = true;

		[Tooltip("Event to trigger after successful injection")]
		[SerializeField]
		private bool triggerEventAfterInjection;

		[Header("Debug Settings")]
		[Tooltip("Whether to log debug information")]
		[SerializeField]
		private bool debugLog = true;

		[Tooltip("Debug log level")]
		[SerializeField]
		private DebugLogLevel logLevel = DebugLogLevel.Normal;

		private bool _injectionComplete;

		private int _retryCount;

		private IObjectResolver _cachedResolver;

		public event Action<GameObject> OnInjectionComplete;

		private void Awake()
		{
			if (injectionTiming == InjectionTiming.Awake)
			{
				PerformInjection();
			}
		}

		private void OnEnable()
		{
			if (injectionTiming == InjectionTiming.OnEnable)
			{
				PerformInjection();
			}
		}

		private void Start()
		{
			if (injectionTiming == InjectionTiming.Start)
			{
				PerformInjection();
			}
		}

		public void PerformInjection()
		{
			if (!_injectionComplete)
			{
				if (waitForLifetimeScope)
				{
					StartCoroutine(WaitForLifetimeScope());
				}
				else
				{
					InjectGameObject();
				}
			}
		}

		public void ForceReinjection()
		{
			_injectionComplete = false;
			_retryCount = 0;
			PerformInjection();
		}

		private IEnumerator WaitForLifetimeScope()
		{
			float startTime = Time.time;
			LifetimeScope lifetimeScope = FindAppropriateLifetimeScope();
			while (lifetimeScope == null || lifetimeScope.Container == null)
			{
				if (maxWaitTime > 0f && Time.time - startTime > maxWaitTime)
				{
					if (retryIfFailed && _retryCount < maxRetries)
					{
						_retryCount++;
						yield return new WaitForSeconds(retryDelay);
						StartCoroutine(WaitForLifetimeScope());
					}
					yield break;
				}
				yield return null;
				lifetimeScope = FindAppropriateLifetimeScope();
			}
			_cachedResolver = lifetimeScope.Container;
			InjectGameObject(lifetimeScope);
		}

		private LifetimeScope FindAppropriateLifetimeScope()
		{
			LifetimeScope[] array = UnityEngine.Object.FindObjectsOfType<LifetimeScope>();
			LifetimeScope[] array2;
			if (useNamedScope)
			{
				array2 = array;
				foreach (LifetimeScope lifetimeScope in array2)
				{
					if (lifetimeScope.name == scopeName)
					{
						return lifetimeScope;
					}
				}
			}
			Scene scene = base.gameObject.scene;
			array2 = array;
			foreach (LifetimeScope lifetimeScope2 in array2)
			{
				if (lifetimeScope2.Parent != null && lifetimeScope2.gameObject.scene == scene)
				{
					return lifetimeScope2;
				}
			}
			array2 = array;
			foreach (LifetimeScope lifetimeScope3 in array2)
			{
				if (lifetimeScope3.Parent != null)
				{
					return lifetimeScope3;
				}
			}
			if (array.Length == 0)
			{
				return null;
			}
			return array[0];
		}

		private void InjectGameObject(LifetimeScope lifetimeScope = null)
		{
			IObjectResolver objectResolver = _cachedResolver;
			if (objectResolver == null)
			{
				if (lifetimeScope == null)
				{
					lifetimeScope = FindAppropriateLifetimeScope();
				}
				if (lifetimeScope != null && lifetimeScope.Container != null)
				{
					objectResolver = (_cachedResolver = lifetimeScope.Container);
				}
			}
			if (objectResolver != null)
			{
				try
				{
					if (injectChildren)
					{
						objectResolver.InjectGameObject(base.gameObject);
					}
					else
					{
						MonoBehaviour[] components = base.gameObject.GetComponents<MonoBehaviour>();
						foreach (MonoBehaviour monoBehaviour in components)
						{
							if (monoBehaviour != this)
							{
								objectResolver.Inject(monoBehaviour);
							}
						}
					}
					_injectionComplete = true;
					_retryCount = 0;
					if (triggerEventAfterInjection)
					{
						this.OnInjectionComplete?.Invoke(base.gameObject);
					}
					if (destroyAfterInjection)
					{
						UnityEngine.Object.Destroy(this);
					}
					return;
				}
				catch (Exception)
				{
					if (retryIfFailed && _retryCount < maxRetries)
					{
						_retryCount++;
						StartCoroutine(RetryInjection());
					}
					return;
				}
			}
			if (retryIfFailed && _retryCount < maxRetries)
			{
				_retryCount++;
				StartCoroutine(RetryInjection());
			}
		}

		private IEnumerator RetryInjection()
		{
			yield return new WaitForSeconds(retryDelay);
			if (!_injectionComplete)
			{
				PerformInjection();
			}
		}

		private void LogMessage(string message, DebugLogLevel messageLevel)
		{
			if (debugLog && logLevel != DebugLogLevel.None && messageLevel <= logLevel && messageLevel != DebugLogLevel.Errors)
			{
				_ = messageLevel - 2;
				_ = 1;
			}
		}

		public bool IsInjectionComplete()
		{
			return _injectionComplete;
		}

		public static AutoInjecter AddToGameObject(GameObject gameObject, bool injectImmediately = true)
		{
			AutoInjecter autoInjecter = gameObject.GetComponent<AutoInjecter>();
			if (autoInjecter == null)
			{
				autoInjecter = gameObject.AddComponent<AutoInjecter>();
				autoInjecter.injectionTiming = ((!injectImmediately) ? InjectionTiming.Start : InjectionTiming.Manual);
			}
			if (injectImmediately)
			{
				autoInjecter.PerformInjection();
			}
			return autoInjecter;
		}
	}
}
