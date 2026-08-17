using System;
using UnityEngine;

namespace Rewired.Components
{
	[Serializable]
	[AddComponentMenu("")]
	public abstract class ComponentWrapper<T> : MonoBehaviour where T : class
	{
		[NonSerialized]
		private T eyyVWBZaxsCJCQMmwhCWcagfYfzWA;

		[NonSerialized]
		private bool obpzIVquVRQseulcTcTZaHvizTjRA;

		protected T source => eyyVWBZaxsCJCQMmwhCWcagfYfzWA;

		protected bool initialized => obpzIVquVRQseulcTcTZaHvizTjRA;

		[CustomObfuscation(rename = false)]
		private void Awake()
		{
			OnAwake();
			OnAwakeFinished();
		}

		[CustomObfuscation(rename = false)]
		private void Start()
		{
			OnStart();
		}

		[CustomObfuscation(rename = false)]
		private void OnEnable()
		{
			OnEnabled();
		}

		[CustomObfuscation(rename = false)]
		private void OnDisable()
		{
			OnDisabled();
		}

		[CustomObfuscation(rename = false)]
		private void OnDestroy()
		{
			OnDestroyed();
		}

		[CustomObfuscation(rename = false)]
		private void Reset()
		{
			OnReset();
		}

		[CustomObfuscation(rename = false)]
		private void OnValidate()
		{
			OnValidated();
		}

		protected virtual void OnAwake()
		{
			ReInput.InitializedEvent += qHfIfclSXqkBQKmYDsKypGlEnSlG;
			Initialize();
		}

		protected virtual void OnAwakeFinished()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnEnabled()
		{
		}

		protected virtual void OnDisabled()
		{
		}

		protected virtual void OnDestroyed()
		{
			Unsubscribe();
			ReInput.InitializedEvent -= qHfIfclSXqkBQKmYDsKypGlEnSlG;
		}

		protected virtual void OnReset()
		{
		}

		protected virtual void OnValidated()
		{
		}

		protected virtual void Initialize()
		{
			if (TryInitialize())
			{
				obpzIVquVRQseulcTcTZaHvizTjRA = true;
				PostInitialize();
			}
		}

		protected virtual bool TryInitialize()
		{
			if (obpzIVquVRQseulcTcTZaHvizTjRA)
			{
				return false;
			}
			eyyVWBZaxsCJCQMmwhCWcagfYfzWA = CreateSource(GetCreateSourceArgs());
			if (eyyVWBZaxsCJCQMmwhCWcagfYfzWA == null)
			{
				Logger.LogError("Failed to create source object.");
				return false;
			}
			obpzIVquVRQseulcTcTZaHvizTjRA = true;
			return true;
		}

		protected abstract T CreateSource(object args);

		protected abstract object GetCreateSourceArgs();

		protected virtual void PostInitialize()
		{
			Subscribe();
		}

		protected virtual void Deinitialize()
		{
			obpzIVquVRQseulcTcTZaHvizTjRA = false;
			Unsubscribe();
			eyyVWBZaxsCJCQMmwhCWcagfYfzWA = null;
		}

		protected virtual void Subscribe()
		{
			Unsubscribe();
			ReInput.ShutDownEvent += iFJDpJvSySehuvNroLXFdrVZGkIs;
		}

		protected virtual void Unsubscribe()
		{
			ReInput.ShutDownEvent -= iFJDpJvSySehuvNroLXFdrVZGkIs;
		}

		private void iFJDpJvSySehuvNroLXFdrVZGkIs()
		{
			Deinitialize();
		}

		private void qHfIfclSXqkBQKmYDsKypGlEnSlG()
		{
			Initialize();
		}
	}
}
