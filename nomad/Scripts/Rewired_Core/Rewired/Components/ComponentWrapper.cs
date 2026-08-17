using System;
using UnityEngine;

namespace Rewired.Components
{
	[Serializable]
	[AddComponentMenu("")]
	public abstract class ComponentWrapper<T> : MonoBehaviour where T : class
	{
		[NonSerialized]
		private T XGyBNQJPCuUzWnddBPbeHUekDPjc;

		[NonSerialized]
		private bool RDoXDqpwXUcvqODjditHozArPlML;

		protected T source => XGyBNQJPCuUzWnddBPbeHUekDPjc;

		protected bool initialized => RDoXDqpwXUcvqODjditHozArPlML;

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
			ReInput.InitializedEvent += GLhOUSCOIwOAgjvTTONapTSUPIjR;
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
			ReInput.InitializedEvent -= GLhOUSCOIwOAgjvTTONapTSUPIjR;
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
				RDoXDqpwXUcvqODjditHozArPlML = true;
				PostInitialize();
			}
		}

		protected virtual bool TryInitialize()
		{
			if (RDoXDqpwXUcvqODjditHozArPlML)
			{
				return false;
			}
			XGyBNQJPCuUzWnddBPbeHUekDPjc = CreateSource(GetCreateSourceArgs());
			if (XGyBNQJPCuUzWnddBPbeHUekDPjc == null)
			{
				Logger.LogError("Failed to create source object.");
				return false;
			}
			RDoXDqpwXUcvqODjditHozArPlML = true;
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
			RDoXDqpwXUcvqODjditHozArPlML = false;
			Unsubscribe();
			XGyBNQJPCuUzWnddBPbeHUekDPjc = null;
		}

		protected virtual void Subscribe()
		{
			Unsubscribe();
			ReInput.ShutDownEvent += aeAgrchYaPaFNacTbIEinJtADiFz;
		}

		protected virtual void Unsubscribe()
		{
			ReInput.ShutDownEvent -= aeAgrchYaPaFNacTbIEinJtADiFz;
		}

		private void aeAgrchYaPaFNacTbIEinJtADiFz()
		{
			Deinitialize();
		}

		private void GLhOUSCOIwOAgjvTTONapTSUPIjR()
		{
			Initialize();
		}
	}
}
