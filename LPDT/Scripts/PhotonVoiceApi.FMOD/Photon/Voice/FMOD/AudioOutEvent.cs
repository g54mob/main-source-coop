using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;

namespace Photon.Voice.FMOD
{
	public class AudioOutEvent<T> : AudioOut<T>
	{
		private EventInstance fmodEvent;

		private int evLength;

		private long evPrevPos;

		private long evLoopCnt;

		private static int instCnt = 0;

		private static Dictionary<int, AudioOutEvent<T>> instTable = new Dictionary<int, AudioOutEvent<T>>();

		public override long OutPos
		{
			get
			{
				if (fmodEvent.handle == IntPtr.Zero)
				{
					return 0L;
				}
				fmodEvent.getTimelinePosition(out var position);
				if (evPrevPos > position)
				{
					evLoopCnt++;
				}
				evPrevPos = position;
				return (evLength * evLoopCnt + position) * frequency / 1000;
			}
		}

		public AudioOutEvent(global::FMOD.System coreSystem, EventInstance fmodEvent, PlayDelayConfig playDelayConfig, ILogger logger, string logPrefix, bool debugInfo)
			: base(coreSystem, playDelayConfig, logger, "(Event)" + ((logPrefix == "") ? "" : " ") + logPrefix, debugInfo)
		{
			this.fmodEvent = fmodEvent;
		}

		public override void OutStart()
		{
			fmodEvent.setCallback(FMODEventCallback);
			IntPtr userData;
			lock (instTable)
			{
				instTable[instCnt] = this;
				userData = new IntPtr(instCnt);
				instCnt++;
			}
			fmodEvent.setUserData(userData);
			fmodEvent.getDescription(out var description);
			description.getLength(out evLength);
			fmodEvent.start();
			logger.Log(LogLevel.Info, logPrefix + "Event Started");
		}

		[MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
		private static RESULT FMODEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
		{
			EventInstance eventInstance = default(EventInstance);
			eventInstance.handle = instance;
			eventInstance.getUserData(out var userdata);
			AudioOutEvent<T> value;
			lock (instTable)
			{
				if (!instTable.TryGetValue(userdata.ToInt32(), out value))
				{
					return RESULT.ERR_NOTREADY;
				}
			}
			return value.fmodEventCallback(type, instance, parameterPtr);
		}

		private RESULT fmodEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
		{
			logger.Log(LogLevel.Info, logPrefix + "EventCallback " + type);
			switch (type)
			{
			case EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
			{
				PROGRAMMER_SOUND_PROPERTIES structure = Marshal.PtrToStructure<PROGRAMMER_SOUND_PROPERTIES>(parameterPtr);
				structure.sound = base.Sound.handle;
				structure.subsoundIndex = -1;
				Marshal.StructureToPtr(structure, parameterPtr, fDeleteOld: false);
				logger.Log(LogLevel.Info, logPrefix + "Sound Assigned to Event Parameter");
				break;
			}
			}
			return RESULT.OK;
		}

		public override void Stop()
		{
			base.Stop();
			fmodEvent.setCallback(null);
			lock (instTable)
			{
				foreach (KeyValuePair<int, AudioOutEvent<T>> item in instTable)
				{
					if (item.Value == this)
					{
						instTable.Remove(item.Key);
						break;
					}
				}
			}
		}
	}
}
