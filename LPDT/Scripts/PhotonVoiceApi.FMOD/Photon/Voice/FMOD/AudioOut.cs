using System;
using System.Runtime.InteropServices;
using FMOD;

namespace Photon.Voice.FMOD
{
	public class AudioOut<T> : AudioOutDelayControl<T>
	{
		protected int channels;

		protected int frequency;

		private global::FMOD.System coreSystem;

		private Sound sound;

		private Channel channel;

		private SOUND_FORMAT soundFormat;

		public Sound Sound => sound;

		public Channel Channel => channel;

		public override long OutPos
		{
			get
			{
				channel.getPosition(out var position, TIMEUNIT.PCMBYTES);
				return position / channels / sizeofT;
			}
		}

		public string Error { get; private set; }

		public AudioOut(global::FMOD.System coreSystem, PlayDelayConfig playDelayConfig, ILogger logger, string logPrefix, bool debugInfo)
			: base(false, playDelayConfig, logger, "[PV] [FMOD] AudioOut" + ((logPrefix == "") ? "" : " ") + logPrefix + " ", debugInfo)
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
					logger.Log(LogLevel.Error, logPrefix + Error);
					return;
				}
				soundFormat = SOUND_FORMAT.PCMFLOAT;
			}
			this.coreSystem = coreSystem;
		}

		public override void OutCreate(int samplingRate, int channels, int bufferSamples)
		{
			this.channels = channels;
			frequency = samplingRate;
			CREATESOUNDEXINFO exinfo = default(CREATESOUNDEXINFO);
			exinfo.cbsize = Marshal.SizeOf(exinfo);
			exinfo.numchannels = channels;
			exinfo.format = soundFormat;
			exinfo.defaultfrequency = samplingRate;
			exinfo.length = (uint)(bufferSamples * channels * sizeofT);
			MODE mode = MODE.LOOP_NORMAL | MODE.OPENUSER;
			RESULT rESULT = coreSystem.createSound("Photon AudioOut", mode, ref exinfo, out sound);
			if (rESULT != RESULT.OK)
			{
				Error = "failed to createSound: " + rESULT;
				logger.Log(LogLevel.Error, logPrefix + Error);
			}
			else
			{
				logger.Log(LogLevel.Info, logPrefix + "Sound Created" + sound.handle);
			}
		}

		public override void OutStart()
		{
			coreSystem.getMasterChannelGroup(out var channelgroup);
			RESULT rESULT = coreSystem.playSound(sound, channelgroup, paused: false, out channel);
			if (rESULT != RESULT.OK)
			{
				Error = "failed to playSound: " + rESULT;
				logger.Log(LogLevel.Error, logPrefix + Error);
			}
		}

		public override void OutWrite(T[] frame, int offsetSamples)
		{
			if (Error != null)
			{
				return;
			}
			RESULT rESULT = sound.@lock((uint)(offsetSamples * sizeofT * channels), (uint)(frame.Length * sizeofT), out var ptr, out var ptr2, out var len, out var len2);
			if (rESULT != RESULT.OK)
			{
				Error = "failed to lock sound buffer: " + rESULT;
				logger.Log(LogLevel.Error, logPrefix + Error);
				return;
			}
			int num = (int)len / sizeofT;
			int length = (int)len2 / sizeofT;
			if (soundFormat == SOUND_FORMAT.PCM16)
			{
				Marshal.Copy(frame as short[], 0, ptr, num);
				if (ptr2 != IntPtr.Zero)
				{
					Marshal.Copy(frame as short[], num, ptr2, length);
				}
			}
			else if (soundFormat == SOUND_FORMAT.PCMFLOAT)
			{
				Marshal.Copy(frame as float[], 0, ptr, num);
				if (ptr2 != IntPtr.Zero)
				{
					Marshal.Copy(frame as float[], num, ptr2, length);
				}
			}
			rESULT = sound.unlock(ptr, ptr2, len, len2);
			if (rESULT != RESULT.OK)
			{
				Error = "failed to unlock sound buffer: " + rESULT;
				logger.Log(LogLevel.Error, logPrefix + Error);
			}
		}

		public override void Stop()
		{
			base.Stop();
			sound.release();
		}
	}
}
