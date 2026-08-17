using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Internal;

namespace VContainer.Unity
{
	public static class ObjectResolverUnityExtensions
	{
		public readonly struct PrefabDirtyScope : IDisposable
		{
			private readonly GameObject _prefab;

			private readonly bool _madeDirty;

			public PrefabDirtyScope(GameObject prefab)
			{
				_prefab = prefab;
				_madeDirty = false;
			}

			public void Dispose()
			{
			}
		}

		public static void InjectGameObject(this IObjectResolver resolver, GameObject gameObject)
		{
			InjectGameObjectRecursive(gameObject);
			void InjectGameObjectRecursive(GameObject current)
			{
				if (!(current == null))
				{
					List<MonoBehaviour> buffer;
					using (ListPool<MonoBehaviour>.Get(out buffer))
					{
						buffer.Clear();
						current.GetComponents(buffer);
						foreach (MonoBehaviour item in buffer)
						{
							if (item != null)
							{
								resolver.Inject(item);
							}
						}
					}
					Transform transform = current.transform;
					for (int i = 0; i < transform.childCount; i++)
					{
						InjectGameObjectRecursive(transform.GetChild(i).gameObject);
					}
				}
			}
		}

		public static T Instantiate<T>(this IObjectResolver resolver, T prefab) where T : Component
		{
			return resolver.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
		}

		public static T Instantiate<T>(this IObjectResolver resolver, T prefab, Transform parent, bool worldPositionStays = false) where T : Component
		{
			bool activeSelf = prefab.gameObject.activeSelf;
			using (new PrefabDirtyScope(prefab.gameObject))
			{
				prefab.gameObject.SetActive(value: false);
				T val = UnityEngine.Object.Instantiate(prefab, parent, worldPositionStays);
				SetName(val, prefab);
				try
				{
					resolver.InjectGameObject(val.gameObject);
				}
				finally
				{
					prefab.gameObject.SetActive(activeSelf);
					val.gameObject.SetActive(activeSelf);
				}
				return val;
			}
		}

		public static T Instantiate<T>(this IObjectResolver resolver, T prefab, Vector3 position, Quaternion rotation) where T : Component
		{
			if (resolver.ApplicationOrigin is LifetimeScope scope)
			{
				return Instantiate(scope, prefab, position, rotation);
			}
			return resolver.Instantiate(prefab, position, rotation, null);
		}

		public static T Instantiate<T>(this IObjectResolver resolver, T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
		{
			bool activeSelf = prefab.gameObject.activeSelf;
			using (new PrefabDirtyScope(prefab.gameObject))
			{
				prefab.gameObject.SetActive(value: false);
				T val = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
				SetName(val, prefab);
				try
				{
					resolver.InjectGameObject(val.gameObject);
				}
				finally
				{
					prefab.gameObject.SetActive(activeSelf);
					val.gameObject.SetActive(activeSelf);
				}
				return val;
			}
		}

		private static T Instantiate<T>(this LifetimeScope scope, T prefab, Vector3 position, Quaternion rotation) where T : Component
		{
			bool activeSelf = prefab.gameObject.activeSelf;
			using (new PrefabDirtyScope(prefab.gameObject))
			{
				prefab.gameObject.SetActive(value: false);
				T val;
				if (scope.IsRoot)
				{
					val = UnityEngine.Object.Instantiate(prefab, position, rotation);
					UnityEngine.Object.DontDestroyOnLoad(val);
				}
				else
				{
					val = UnityEngine.Object.Instantiate(prefab, position, rotation, scope.transform);
					val.transform.SetParent(null);
				}
				SetName(val, prefab);
				try
				{
					scope.Container.InjectGameObject(val.gameObject);
				}
				finally
				{
					prefab.gameObject.SetActive(activeSelf);
					val.gameObject.SetActive(activeSelf);
				}
				return val;
			}
		}

		private static GameObject Instantiate(this LifetimeScope scope, GameObject prefab, Vector3 position, Quaternion rotation)
		{
			bool activeSelf = prefab.activeSelf;
			using (new PrefabDirtyScope(prefab))
			{
				prefab.SetActive(value: false);
				GameObject gameObject;
				if (scope.IsRoot)
				{
					gameObject = UnityEngine.Object.Instantiate(prefab, position, rotation);
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				else
				{
					gameObject = UnityEngine.Object.Instantiate(prefab, position, rotation, scope.transform);
					gameObject.transform.SetParent(null);
				}
				SetName(gameObject, prefab);
				try
				{
					scope.Container.InjectGameObject(gameObject);
				}
				finally
				{
					prefab.SetActive(activeSelf);
					gameObject.SetActive(activeSelf);
				}
				return gameObject;
			}
		}

		public static GameObject Instantiate(this IObjectResolver resolver, GameObject prefab)
		{
			return resolver.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
		}

		public static GameObject Instantiate(this IObjectResolver resolver, GameObject prefab, Transform parent, bool worldPositionStays = false)
		{
			bool activeSelf = prefab.activeSelf;
			using (new PrefabDirtyScope(prefab))
			{
				prefab.SetActive(value: false);
				GameObject gameObject = null;
				try
				{
					gameObject = UnityEngine.Object.Instantiate(prefab, parent, worldPositionStays);
					SetName(gameObject, prefab);
					resolver.InjectGameObject(gameObject);
				}
				finally
				{
					prefab.SetActive(activeSelf);
					gameObject?.SetActive(activeSelf);
				}
				return gameObject;
			}
		}

		public static GameObject Instantiate(this IObjectResolver resolver, GameObject prefab, Vector3 position, Quaternion rotation)
		{
			if (resolver.ApplicationOrigin is LifetimeScope scope)
			{
				return Instantiate(scope, prefab, position, rotation);
			}
			return resolver.Instantiate(prefab, position, rotation, null);
		}

		public static GameObject Instantiate(this IObjectResolver resolver, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
		{
			bool activeSelf = prefab.activeSelf;
			using (new PrefabDirtyScope(prefab))
			{
				prefab.SetActive(value: false);
				GameObject gameObject = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
				SetName(gameObject, prefab);
				try
				{
					resolver.InjectGameObject(gameObject);
				}
				finally
				{
					prefab.SetActive(activeSelf);
					gameObject.SetActive(activeSelf);
				}
				return gameObject;
			}
		}

		private static void SetName(UnityEngine.Object instance, UnityEngine.Object prefab)
		{
			if (VContainerSettings.Instance != null && VContainerSettings.Instance.RemoveClonePostfix)
			{
				instance.name = prefab.name;
			}
		}
	}
}
