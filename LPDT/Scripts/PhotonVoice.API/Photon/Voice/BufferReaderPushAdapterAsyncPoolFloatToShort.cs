namespace Photon.Voice
{
	public class BufferReaderPushAdapterAsyncPoolFloatToShort : BufferReaderPushAdapterBase<float>
	{
		private float[] buffer;

		public BufferReaderPushAdapterAsyncPoolFloatToShort(IDataReader<float> reader)
			: base(reader)
		{
			buffer = new float[0];
		}

		public override void Service(LocalVoice localVoice)
		{
			LocalVoiceFramed<short> localVoiceFramed = (LocalVoiceFramed<short>)localVoice;
			short[] array = localVoiceFramed.BufferFactory.New();
			if (buffer.Length != array.Length)
			{
				buffer = new float[array.Length];
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
