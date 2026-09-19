namespace Photon.Voice
{
	public class BufferReaderPushAdapterAsyncPoolShortToFloat : BufferReaderPushAdapterBase<short>
	{
		private short[] buffer = new short[0];

		public BufferReaderPushAdapterAsyncPoolShortToFloat(IDataReader<short> reader)
			: base(reader)
		{
		}

		public override void Service(LocalVoice localVoice)
		{
			LocalVoiceFramed<float> localVoiceFramed = (LocalVoiceFramed<float>)localVoice;
			float[] array = localVoiceFramed.BufferFactory.New();
			if (buffer.Length != array.Length)
			{
				buffer = new short[array.Length];
			}
			while (reader.Read(buffer))
			{
				AudioUtil.Convert(buffer, array, array.Length);
				localVoiceFramed.PushDataAsync(array);
				array = localVoiceFramed.BufferFactory.New();
			}
			localVoiceFramed.BufferFactory.Free(array, array.Length);
		}
	}
}
