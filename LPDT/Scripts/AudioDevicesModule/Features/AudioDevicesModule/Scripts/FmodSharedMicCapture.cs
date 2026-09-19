using System;
using System.Runtime.InteropServices;
using FMOD;
using FMODUnity;
using Photon.Voice;

namespace Features.AudioDevicesModule.Scripts
{
	public class FmodSharedMicCapture : IDisposable
	{
		private sealed class SharedMicReader : IAudioReader<short>, IDataReader<short>, IDisposable, IAudioDesc
		{
			private readonly FmodSharedMicCapture capture;

			public int SamplingRate => capture.SamplingRate;

			public int Channels => capture.Channels;

			public string Error => capture.Error;

			public SharedMicReader(FmodSharedMicCapture capture)
			{
				this.capture = capture;
			}

			public bool Read(short[] buffer)
			{
				return capture.Read(buffer);
			}

			public void Dispose()
			{
			}
		}

		private const int BufferLengthMs = 500;

		private const string LogPrefix = "[FmodSharedMicCapture] ";

		private readonly MicrophoneModel _microphoneModel;

		private readonly RnnoiseMicProcessor _rnnoiseMicProcessor = new RnnoiseMicProcessor();

		private FMOD.System coreSystem;

		private Sound recordSound;

		private int deviceId = -1;

		private int samplingRate;

		private int channels;

		private int bufLengthSamples;

		private uint recordSoundLengthPcm;

		private uint micPrevPos;

		private int micLoopCnt;

		private uint readAbsPos;

		public bool IsActive
		{
			get
			{
				if (recordSound.hasHandle())
				{
					return Error == null;
				}
				return false;
			}
		}

		public Sound RecordSound => recordSound;

		public int SamplingRate => samplingRate;

		public int Channels => channels;

		public uint RecordSoundLengthPcm => recordSoundLengthPcm;

		public string Error { get; private set; }

		public event Action<short[], int, int> OnSamplesRead;

		public FmodSharedMicCapture(MicrophoneModel microphoneModel)
		{
			_microphoneModel = microphoneModel;
		}

		public IAudioReader<short> AcquireReader(int device, int suggestedFrequency, ILogger logger)
		{
			EnsureCapture(device, suggestedFrequency, logger);
			return new SharedMicReader(this);
		}

		public void EnsureCapture(int device, int suggestedFrequency, ILogger logger)
		{
			if (IsActive && deviceId == device)
			{
				return;
			}
			ReleaseCapture();
			coreSystem = RuntimeManager.CoreSystem;
			deviceId = device;
			samplingRate = ((suggestedFrequency > 0) ? suggestedFrequency : 48000);
			RESULT recordDriverInfo = coreSystem.getRecordDriverInfo(deviceId, out var _, 0, out var _, out var systemrate, out var _, out var speakermodechannels, out var _);
			if (recordDriverInfo == RESULT.OK && systemrate > 0)
			{
				samplingRate = systemrate;
			}
			if (recordDriverInfo == RESULT.OK && speakermodechannels > 0)
			{
				channels = speakermodechannels;
			}
			else
			{
				channels = 1;
			}
			bufLengthSamples = samplingRate * 500 / 1000;
			CREATESOUNDEXINFO exinfo = new CREATESOUNDEXINFO
			{
				cbsize = Marshal.SizeOf(typeof(CREATESOUNDEXINFO)),
				numchannels = channels,
				format = SOUND_FORMAT.PCM16,
				defaultfrequency = samplingRate,
				length = (uint)(bufLengthSamples * channels * 2)
			};
			recordDriverInfo = coreSystem.createSound("", MODE.LOOP_NORMAL | MODE.OPENUSER, ref exinfo, out recordSound);
			if (recordDriverInfo != RESULT.OK)
			{
				Error = "createSound failed: " + recordDriverInfo;
				logger?.Log(LogLevel.Error, "[FmodSharedMicCapture] " + Error);
				return;
			}
			recordDriverInfo = coreSystem.recordStart(deviceId, recordSound, loop: true);
			if (recordDriverInfo != RESULT.OK)
			{
				Error = "recordStart failed: " + recordDriverInfo;
				logger?.Log(LogLevel.Error, "[FmodSharedMicCapture] " + Error);
				recordSound.release();
				recordSound.clearHandle();
				return;
			}
			recordSound.getLength(out recordSoundLengthPcm, TIMEUNIT.PCM);
			micPrevPos = 0u;
			micLoopCnt = 0;
			readAbsPos = 0u;
			Error = null;
			logger?.Log(LogLevel.Info, "[FmodSharedMicCapture] Recording started. device={0}, rate={1}, ch={2}, bufSamples={3}", deviceId, samplingRate, channels, bufLengthSamples);
		}

		internal bool Read(short[] buffer)
		{
			if (!IsActive)
			{
				return false;
			}
			RESULT recordPosition = coreSystem.getRecordPosition(deviceId, out var position);
			if (recordPosition != RESULT.OK)
			{
				Error = "getRecordPosition failed: " + recordPosition;
				return false;
			}
			if (position < micPrevPos)
			{
				micLoopCnt++;
			}
			micPrevPos = position;
			long num = (long)micLoopCnt * (long)bufLengthSamples + position;
			int num2 = buffer.Length / channels;
			long num3 = readAbsPos + num2;
			if (num3 >= num)
			{
				return false;
			}
			uint offset = (uint)(readAbsPos % bufLengthSamples * channels * 2);
			uint length = (uint)(buffer.Length * 2);
			recordPosition = recordSound.@lock(offset, length, out var ptr, out var ptr2, out var len, out var len2);
			if (recordPosition != RESULT.OK)
			{
				Error = "lock failed: " + recordPosition;
				return false;
			}
			int num4 = (int)len / 2;
			int length2 = (int)len2 / 2;
			Marshal.Copy(ptr, buffer, 0, num4);
			if (ptr2 != IntPtr.Zero)
			{
				Marshal.Copy(ptr2, buffer, num4, length2);
			}
			recordPosition = recordSound.unlock(ptr, ptr2, len, len2);
			if (recordPosition != RESULT.OK)
			{
				Error = "unlock failed: " + recordPosition;
				return false;
			}
			readAbsPos = (uint)num3;
			if (_microphoneModel.IsNoiseSuppressionEnabled)
			{
				_rnnoiseMicProcessor.Process(buffer, samplingRate, channels);
			}
			this.OnSamplesRead?.Invoke(buffer, samplingRate, channels);
			return true;
		}

		private void ReleaseCapture()
		{
			if (coreSystem.hasHandle() && deviceId >= 0 && recordSound.hasHandle())
			{
				coreSystem.recordStop(deviceId);
			}
			if (recordSound.hasHandle())
			{
				recordSound.release();
			}
			recordSound.clearHandle();
			deviceId = -1;
			readAbsPos = 0u;
			micPrevPos = 0u;
			micLoopCnt = 0;
			_rnnoiseMicProcessor.Reset();
		}

		public void Dispose()
		{
			ReleaseCapture();
			_rnnoiseMicProcessor.Dispose();
		}
	}
}
