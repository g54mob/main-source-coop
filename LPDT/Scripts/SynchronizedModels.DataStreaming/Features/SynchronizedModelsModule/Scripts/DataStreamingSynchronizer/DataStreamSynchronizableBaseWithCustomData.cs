using System;
using System.Security.Cryptography;
using System.Text;
using Fusion.Sockets;

namespace Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer
{
	public abstract class DataStreamSynchronizableBaseWithCustomData<TModel, TData1> : DataStreamSynchronizableBase<TModel>, ICustomDataStreamSynchronizable where TModel : class
	{
		public TData1 Data1 { get; set; }

		public event Action<ICustomDataStreamSynchronizable> CallCustomSynchronize;

		public event Action<ICustomDataStreamSynchronizable, PlayerIdDataHolder> CustomSynchronizeRequest;

		byte[] ICustomDataStreamSynchronizable.GetCustomDataInBytes()
		{
			string s = _jsonConvertor.Convert(Data1);
			return Encoding.UTF8.GetBytes(s);
		}

		void ICustomDataStreamSynchronizable.SetCustomDataInBytes(byte[] synchronizableData)
		{
			TData1 newCustomValues = _jsonConvertor.Convert<TData1>(Encoding.UTF8.GetString(synchronizableData));
			SetNewCustomValues(newCustomValues);
		}

		ReliableKey ICustomDataStreamSynchronizable.GetCustomReliableKey()
		{
			byte[] typeHash = GetTypeHash(GetType());
			byte[] typeHash2 = GetTypeHash(typeof(TData1));
			byte[] array = new byte[typeHash.Length + typeHash2.Length];
			Array.Copy(typeHash, 0, array, 0, typeHash.Length);
			Array.Copy(typeHash2, 0, array, typeHash.Length, typeHash2.Length);
			byte[] sourceArray;
			using (SHA256 sHA = SHA256.Create())
			{
				sourceArray = sHA.ComputeHash(array);
			}
			byte[] array2 = new byte[16];
			Array.Copy(sourceArray, 0, array2, 0, 16);
			byte[] value = new Guid(array2).ToByteArray();
			int key = BitConverter.ToInt32(value, 0);
			int key2 = BitConverter.ToInt32(value, 4);
			int key3 = BitConverter.ToInt32(value, 8);
			int key4 = BitConverter.ToInt32(value, 12);
			return ReliableKey.FromInts(key, key2, key3, key4);
		}

		private byte[] GetTypeHash(Type type)
		{
			string fullName = type.FullName;
			using MD5 mD = MD5.Create();
			return mD.ComputeHash(Encoding.UTF8.GetBytes(fullName));
		}

		public void CustomSynchronize()
		{
			this.CallCustomSynchronize?.Invoke(this);
		}

		private void SetNewCustomValues(TData1 data)
		{
			Data1 = data;
			if (Data1 != null)
			{
				OnSetNewCustomValues(data);
			}
		}

		public void RequestCustomSynchronizationFromMasterClient()
		{
			if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				this.CustomSynchronizeRequest?.Invoke(this, new PlayerIdDataHolder(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId));
			}
		}

		protected abstract void OnSetNewCustomValues(TData1 data);
	}
}
