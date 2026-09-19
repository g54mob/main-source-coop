using System;
using System.Runtime.InteropServices;
using FMOD;

namespace Photon.Voice.FMOD
{
	public class AudioInReader<T> : IAudioReader<T>, IDataReader<T>, IDisposable, IAudioDesc
	{
		private readonly int sizeofT = Marshal.SizeOf(default(T));

		private const int BUF_LENGTH_MS = 2000;

		private const string LOG_PREFIX = "[PV] [FMOD] AudioIn: ";

		private int device;

		private ILogger logger;

		private global::FMOD.System coreSystem;

		private Sound sound;

		private readonly SOUND_FORMAT soundFormat;

		public bool isRecording;

		private int samplingRate;

		private int channels;

		private int bufLengthSamples;

		private uint micPrevPos;

		private int micLoopCnt;

		private uint readAbsPos;

		public int SamplingRate
		{
			get
			{
				if (Error != null)
				{
					return 0;
				}
				return samplingRate;
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
				return channels;
			}
		}

		public string Error { get; private set; }

		public AudioInReader(global::FMOD.System coreSystem, int device, int suggestedFrequency, ILogger logger)
		{
			if (sizeofT == 2)
			{
				soundFormat = SOUND_FORMAT.PCM16;
			}
			else
			{
				if (sizeofT != 4)
				{
					Error = "only float and short buffers are supported: " + typeof(T);
					logger.Log(LogLevel.Error, "[PV] [FMOD] AudioIn: " + Error);
					return;
				}
				soundFormat = SOUND_FORMAT.PCMFLOAT;
			}
			try
			{
				this.coreSystem = coreSystem;
				this.device = device;
				this.logger = logger;
				samplingRate = suggestedFrequency;
				RESULT recordDriverInfo = this.coreSystem.getRecordDriverInfo(device, out var _, 1, out var _, out var _, out var _, out channels, out var _);
				if (recordDriverInfo != RESULT.OK)
				{
					Error = "failed to getRecordDriverInfo: " + recordDriverInfo;
					logger.Log(LogLevel.Error, "[PV] [FMOD] AudioIn: " + Error);
					return;
				}
				CREATESOUNDEXINFO exinfo = default(CREATESOUNDEXINFO);
				exinfo.cbsize = Marshal.SizeOf(exinfo);
				exinfo.numchannels = channels;
				exinfo.format = soundFormat;
				exinfo.defaultfrequency = samplingRate;
				bufLengthSamples = samplingRate * 2000 / 1000;
				exinfo.length = (uint)(bufLengthSamples * channels * sizeofT);
				MODE mode = MODE.LOOP_NORMAL | MODE.OPENUSER;
				recordDriverInfo = this.coreSystem.createSound("Photon AudioIn", mode, ref exinfo, out sound);
				if (recordDriverInfo != RESULT.OK)
				{
					Error = "failed to createSound: " + recordDriverInfo;
					logger.Log(LogLevel.Error, "[PV] [FMOD] AudioIn: " + Error);
					return;
				}
				recordDriverInfo = this.coreSystem.recordStart(device, sound, loop: true);
				if (recordDriverInfo != RESULT.OK)
				{
					Error = "failed to startrecord: " + recordDriverInfo;
					logger.Log(LogLevel.Error, "[PV] [FMOD] AudioIn: " + Error);
					return;
				}
				isRecording = true;
				logger.Log(LogLevel.Info, "[PV] [FMOD] Mic: microphone '{0}' initialized, frequency = {1}, channels = {2}.", device, samplingRate, channels);
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				if (Error == null)
				{
					Error = "Exception in [FMOD] Mic constructor";
				}
				logger.Log(LogLevel.Error, "[PV] [FMOD] AudioIn: " + Error);
			}
		}

		public void Dispose()
		{
			coreSystem.recordStop(device);
			sound.release();
		}

		public bool Read(T[] readBuf)
		{
			if (Error != null)
			{
				return false;
			}
			RESULT recordPosition = coreSystem.getRecordPosition(device, out var position);
			if (recordPosition != RESULT.OK)
			{
				Error = "failed to getRecordPosition: " + recordPosition;
				logger.Log(LogLevel.Error, "[PV] [FMOD] AudioIn: " + Error);
				return false;
			}
			if (position < micPrevPos)
			{
				micLoopCnt++;
			}
			micPrevPos = position;
			long num = micLoopCnt * bufLengthSamples + position;
			long num2 = readAbsPos + readBuf.Length / channels;
			if (num2 < num)
			{
				recordPosition = sound.@lock((uint)(readAbsPos % bufLengthSamples * sizeofT * channels), (uint)(readBuf.Length * sizeofT), out var ptr, out var ptr2, out var len, out var len2);
				if (recordPosition != RESULT.OK)
				{
					Error = "failed to lock sound buffer: " + recordPosition;
					logger.Log(LogLevel.Error, "[PV] [FMOD] AudioIn: " + Error);
					return false;
				}
				int num3 = (int)len / sizeofT;
				int length = (int)len2 / sizeofT;
				if (soundFormat == SOUND_FORMAT.PCM16)
				{
					Marshal.Copy(ptr, readBuf as short[], 0, num3);
					if (ptr2 != IntPtr.Zero)
					{
						Marshal.Copy(ptr2, readBuf as short[], num3, length);
					}
				}
				else if (soundFormat == SOUND_FORMAT.PCMFLOAT)
				{
					Marshal.Copy(ptr, readBuf as float[], 0, num3);
					if (ptr2 != IntPtr.Zero)
					{
						Marshal.Copy(ptr2, readBuf as float[], num3, length);
					}
				}
				recordPosition = sound.unlock(ptr, ptr2, len, len2);
				if (recordPosition != RESULT.OK)
				{
					Error = "failed to unlock sound buffer: " + recordPosition;
					logger.Log(LogLevel.Error, "[PV] [FMOD] AudioIn: " + Error);
					return false;
				}
				readAbsPos = (uint)num2;
				return true;
			}
			return false;
		}
	}
}
