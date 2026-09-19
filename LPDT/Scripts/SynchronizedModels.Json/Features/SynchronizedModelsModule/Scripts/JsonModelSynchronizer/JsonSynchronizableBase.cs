using System;
using System.Text;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Interfaces;
using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer
{
	public abstract class JsonSynchronizableBase<TModel> : IJsonSynchronizable, IInitializable where TModel : class
	{
		protected IJsonConvertor _jsonConvertor;

		public bool IsSynchronizedOnStart;

		public bool IsNeedToCallOnSynchronized;

		private string _defaultModelValue;

		public abstract RPCType RPCType { get; }

		public abstract bool IsNeedToSynchronizeOnSpawn { get; }

		public event Action<IJsonSynchronizable> CallSynchronize;

		public event Action OnSynchronized;

		[Inject]
		private void InjectDependencies(IJsonConvertor jsonConvertor)
		{
			_jsonConvertor = jsonConvertor;
		}

		public void Initialize()
		{
			_defaultModelValue = _jsonConvertor.Convert(this);
		}

		bool IJsonSynchronizable.TryGetValue(out byte[] value)
		{
			PrepareForSynchronize();
			string text = _jsonConvertor.Convert(this);
			string text2 = text.Replace("\"IsSynchronizedOnStart\":true", "\"IsSynchronizedOnStart\":false");
			value = Encoding.UTF8.GetBytes(text);
			return text2 != _defaultModelValue;
		}

		void IJsonSynchronizable.SynchronizeInternal(byte[] value)
		{
			TModel val = _jsonConvertor.Convert<TModel>(Encoding.UTF8.GetString(value));
			JsonSynchronizableBase<TModel> jsonSynchronizableBase = val as JsonSynchronizableBase<TModel>;
			SetNewValues(val, jsonSynchronizableBase.IsSynchronizedOnStart);
			IsSynchronizedOnStart = false;
			if (jsonSynchronizableBase.IsNeedToCallOnSynchronized)
			{
				IsNeedToCallOnSynchronized = false;
				this.OnSynchronized?.Invoke();
			}
		}

		void IJsonSynchronizable.ReSynchronizeInternal()
		{
			TModel val = _jsonConvertor.Convert<TModel>(_defaultModelValue);
			JsonSynchronizableBase<TModel> jsonSynchronizableBase = val as JsonSynchronizableBase<TModel>;
			SetNewValues(val, jsonSynchronizableBase.IsSynchronizedOnStart);
			IsSynchronizedOnStart = false;
			if (jsonSynchronizableBase.IsNeedToCallOnSynchronized)
			{
				IsNeedToCallOnSynchronized = false;
				this.OnSynchronized?.Invoke();
			}
		}

		public void Synchronize(bool isWithTrigggerEvent = false)
		{
			IsNeedToCallOnSynchronized = isWithTrigggerEvent;
			this.CallSynchronize?.Invoke(this);
		}

		void IJsonSynchronizable.SynchronizeOnStart()
		{
			IsSynchronizedOnStart = true;
			this.CallSynchronize?.Invoke(this);
		}

		protected abstract void SetNewValues(TModel synchronizable, bool isSynchronizedOnStart);

		protected virtual void PrepareForSynchronize()
		{
		}

		public virtual void OnSpawned()
		{
		}

		public virtual void OnDespawned()
		{
		}

		public virtual void OnDisconnect()
		{
		}
	}
}
