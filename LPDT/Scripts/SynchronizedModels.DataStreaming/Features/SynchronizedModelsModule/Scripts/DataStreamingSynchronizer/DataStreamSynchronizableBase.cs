using System;
using System.Security.Cryptography;
using System.Text;
using Features.MultiplayerSessionServices.Scripts;
using Fusion.Sockets;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Interfaces;
using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer
{
	public abstract class DataStreamSynchronizableBase<TModel> : IDataStreamSynchronizable, IInitializable where TModel : class
	{
		public bool IsNeedToCallOnSynchronized;

		protected IJsonConvertor _jsonConvertor;

		protected MultiplayerModel _multiplayerModel;

		private string _defaultModelValue;

		public abstract bool IsNeedToSynchronizeOnSpawn { get; }

		public event Action<IDataStreamSynchronizable> CallSynchronize;

		public event Action<IDataStreamSynchronizable, PlayerIdDataHolder> SynchronizeRequest;

		public event Action OnSynchronized;

		[Inject]
		private void InjectDependencies(IJsonConvertor jsonConvertor, MultiplayerModel multiplayerModel)
		{
			_jsonConvertor = jsonConvertor;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_defaultModelValue = _jsonConvertor.Convert(this);
		}

		protected abstract void SetNewValues(TModel model);

		public void Synchronize(bool isWithTrigggerEvent = false)
		{
			IsNeedToCallOnSynchronized = isWithTrigggerEvent;
			this.CallSynchronize?.Invoke(this);
		}

		bool IDataStreamSynchronizable.TryGetDataInBytes(out byte[] value)
		{
			string text = _jsonConvertor.Convert(this);
			string text2 = text.Replace("\"IsNeedToCallOnSynchronized\":true", "\"IsNeedToCallOnSynchronized\":false");
			value = Encoding.UTF8.GetBytes(text);
			return text2 != _defaultModelValue;
		}

		void IDataStreamSynchronizable.SetDataInBytes(byte[] synchronizableData)
		{
			TModel val = _jsonConvertor.Convert<TModel>(Encoding.UTF8.GetString(synchronizableData));
			SetNewValues(val);
			if ((val as DataStreamSynchronizableBase<TModel>).IsNeedToCallOnSynchronized)
			{
				IsNeedToCallOnSynchronized = false;
				this.OnSynchronized?.Invoke();
			}
		}

		void IDataStreamSynchronizable.ReSetDataInBytes()
		{
			TModel val = _jsonConvertor.Convert<TModel>(_defaultModelValue);
			SetNewValues(val);
			if ((val as DataStreamSynchronizableBase<TModel>).IsNeedToCallOnSynchronized)
			{
				IsNeedToCallOnSynchronized = false;
				this.OnSynchronized?.Invoke();
			}
		}

		ReliableKey IDataStreamSynchronizable.GetReliableKey()
		{
			byte[] typeHash = GetTypeHash();
			byte[] value = new Guid(typeHash).ToByteArray();
			int key = BitConverter.ToInt32(value, 0);
			int key2 = BitConverter.ToInt32(value, 4);
			int key3 = BitConverter.ToInt32(value, 8);
			int key4 = BitConverter.ToInt32(value, 12);
			return ReliableKey.FromInts(key, key2, key3, key4);
		}

		private byte[] GetTypeHash()
		{
			string fullName = GetType().FullName;
			using MD5 mD = MD5.Create();
			return mD.ComputeHash(Encoding.UTF8.GetBytes(fullName));
		}

		public void RequestSynchronizationFromMasterClient()
		{
			if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				this.SynchronizeRequest?.Invoke(this, new PlayerIdDataHolder(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId));
			}
		}

		public virtual void OnDisconnect()
		{
		}
	}
}
