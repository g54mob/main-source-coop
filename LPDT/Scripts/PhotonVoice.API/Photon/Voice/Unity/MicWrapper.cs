using System;
using UnityEngine;

namespace Photon.Voice.Unity
{
	public class MicWrapper : IAudioReader<float>, IDataReader<float>, IDisposable, IAudioDesc
	{
		private AudioClip mic;

		private string device;

		private ILogger logger;

		private int micPrevPos;

		private int micLoopCnt;

		private int readAbsPos;

		public int SamplingRate
		{
			get
			{
				if (Error != null)
				{
					return 0;
				}
				return mic.frequency;
			}
		}

		public int Channels
		{
			get
			{
				if (Error != null)
				{
					return 0;
				}
				return mic.channels;
			}
		}

		public string Error { get; private set; }

		public MicWrapper(string device, int suggestedFrequency, ILogger logger)
		{
			try
			{
				this.device = device;
				this.logger = logger;
				Error = UnityMicrophone.CheckDevice(logger, "[PV] MicWrapper: ", device, suggestedFrequency, out var frequency);
				if (Error == null)
				{
					mic = UnityMicrophone.Start(device, loop: true, 1, frequency);
					logger.Log(LogLevel.Info, "[PV] MicWrapper: microphone '{0}' initialized, frequency = {1}, channels = {2}.", device, mic.frequency, mic.channels);
				}
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				if (Error == null)
				{
					Error = "Exception in MicWrapper constructor";
				}
				logger.Log(LogLevel.Error, "[PV] MicWrapper: " + Error);
			}
		}

		public void Dispose()
		{
			UnityMicrophone.End(device);
		}

		public bool Read(float[] buffer)
		{
			if (Error != null)
			{
				return false;
			}
			int position = UnityMicrophone.GetPosition(device);
			if (position < micPrevPos)
			{
				micLoopCnt++;
			}
			micPrevPos = position;
			int num = micLoopCnt * mic.samples + position;
			if (mic.channels == 0)
			{
				Error = "Number of channels is 0 in Read()";
				logger.Log(LogLevel.Error, "[PV] MicWrapper: " + Error);
				return false;
			}
			int num2 = buffer.Length / mic.channels;
			int num3 = readAbsPos + num2;
			if (num3 < num)
			{
				mic.GetData(buffer, readAbsPos % mic.samples);
				readAbsPos = num3;
				return true;
			}
			return false;
		}
	}
}
