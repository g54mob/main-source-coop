using System;
using System.Text;

namespace Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer
{
	public abstract class JsonSynchronizableBaseWithCustomData<TModel, TData1> : JsonSynchronizableBase<TModel>, ICustomJsonSynchronizable where TModel : class
	{
		public TData1 Data1 { get; set; }

		public event Action<ICustomJsonSynchronizable> OnCallCustomSynchronize;

		public void CustomSynchronize()
		{
			this.OnCallCustomSynchronize?.Invoke(this);
		}

		void ICustomJsonSynchronizable.CustomSynchronizeInternal(byte[] value)
		{
			TData1 newCustomValues = _jsonConvertor.Convert<TData1>(Encoding.UTF8.GetString(value));
			SetNewCustomValues(newCustomValues);
		}

		byte[] ICustomJsonSynchronizable.GetCustomValue()
		{
			string s = _jsonConvertor.Convert(Data1);
			return Encoding.UTF8.GetBytes(s);
		}

		protected abstract void SetNewCustomValues(TData1 data);
	}
}
