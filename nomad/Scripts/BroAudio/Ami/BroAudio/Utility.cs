using System;
using System.Collections.Generic;
using Ami.BroAudio.Data;
using Ami.BroAudio.Runtime;
using Ami.BroAudio.Tools;
using Ami.Extension;
using UnityEngine;
using UnityEngine.Audio;

namespace Ami.BroAudio
{
	public static class Utility
	{
		public const int LastAudioType = 16;

		[Obsolete("IDs are irrelevant", true)]
		public const int IDCapacity = 268435456;

		public static readonly float[] BroVolumeSplitPoints = new float[9] { -80f, -60f, -36f, -24f, -12f, -6f, 0f, 6f, 20f };

		public const string LogTitle = "<b><color=#F3E9D7>[BroAudio] </color></b>";

		public const int UnityEverythingFlag = -1;

		[Obsolete("IDs are irrelevant", true)]
		public static int FinalIDLimit => BroAudioType.VoiceOver.GetInitialID() + 268435456;

		public static Vector3 GloballyPlayedPosition => Vector3.negativeInfinity;

		[Obsolete("IDs are irrelevant", true)]
		public static int GetInitialID(this BroAudioType audioType)
		{
			if (audioType == BroAudioType.None)
			{
				return 0;
			}
			if (audioType == BroAudioType.All || audioType < BroAudioType.None)
			{
				return 2147483647;
			}
			int num = 0;
			for (int num2 = (int)audioType; num2 > 0; num2 >>= 1)
			{
				num += 268435456;
			}
			return num;
		}

		public static BroAudioType ToNext(this BroAudioType current)
		{
			if (current == BroAudioType.None)
			{
				return current + 1;
			}
			int num = (int)current << 1;
			if (num > 16)
			{
				return BroAudioType.All;
			}
			return (BroAudioType)num;
		}

		[Obsolete("Type is direct on the entity now", true)]
		public static BroAudioType GetAudioType(int id)
		{
			if (id >= FinalIDLimit)
			{
				return BroAudioType.None;
			}
			BroAudioType broAudioType = BroAudioType.None;
			BroAudioType broAudioType2 = broAudioType.ToNext();
			while (broAudioType2 <= BroAudioType.VoiceOver && (id < broAudioType.GetInitialID() || id >= broAudioType2.GetInitialID()))
			{
				broAudioType = broAudioType2;
				broAudioType2 = broAudioType2.ToNext();
			}
			return broAudioType;
		}

		public static bool IsConcrete(this BroAudioType audioType, bool checkFlags = false)
		{
			if (audioType == BroAudioType.None || audioType == BroAudioType.All)
			{
				return false;
			}
			if (checkFlags && FlagsExtension.GetFlagsOnCount((int)audioType) > 1)
			{
				return false;
			}
			return true;
		}

		public static void ForeachConcreteAudioType(Action<BroAudioType> loopCallback)
		{
			for (BroAudioType broAudioType = BroAudioType.None; broAudioType <= BroAudioType.VoiceOver; broAudioType = broAudioType.ToNext())
			{
				if (broAudioType.IsConcrete())
				{
					loopCallback?.Invoke(broAudioType);
				}
			}
		}

		public static void ForeachConcreteAudioType<T>(T processor) where T : IAudioTypeIterable
		{
			for (BroAudioType broAudioType = BroAudioType.None; broAudioType <= BroAudioType.VoiceOver; broAudioType = broAudioType.ToNext())
			{
				if (broAudioType.IsConcrete())
				{
					processor.OnEachAudioType(broAudioType);
				}
			}
		}

		public static bool Validate(string name, IReadOnlyList<IBroAudioClip> clips)
		{
			if (clips == null || clips.Count == 0)
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>" + name.ToWhiteBold() + " has no audio clips, please assign or delete the entity.");
				return false;
			}
			for (int i = 0; i < clips.Count; i++)
			{
				if (!clips[i].IsValid())
				{
					Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>Audio clip has not been assigned! please check " + name.ToWhiteBold() + " in Library Manager.");
					return false;
				}
			}
			return true;
		}

		public static float SliderToVolume(SliderType sliderType, float normalizedValue, bool allowBoost)
		{
			normalizedValue = Mathf.Max(normalizedValue, 0.0001f);
			switch (sliderType)
			{
			case SliderType.Logarithmic:
			{
				float b = (allowBoost ? 1f : 0f);
				float p = Mathf.Lerp(-4f, b, normalizedValue);
				return Mathf.Pow(10f, p);
			}
			case SliderType.BroVolume:
			case SliderType.BroVolumeNoField:
				return SliderToBroVolume(normalizedValue, allowBoost);
			default:
				if (!allowBoost)
				{
					return normalizedValue;
				}
				return normalizedValue * 10f;
			}
		}

		public static float VolumeToSlider(SliderType sliderType, float volume, bool allowBoost)
		{
			switch (sliderType)
			{
			case SliderType.Logarithmic:
			{
				float b = (allowBoost ? 1f : 0f);
				return Mathf.InverseLerp(-4f, b, Mathf.Log10(volume));
			}
			case SliderType.BroVolume:
			case SliderType.BroVolumeNoField:
				return BroVolumeToSlider(volume, allowBoost);
			default:
			{
				float b = (allowBoost ? 10f : 1f);
				return Mathf.InverseLerp(0.0001f, b, volume);
			}
			}
		}

		public static float BroVolumeToSlider(float vol, bool allowBoost = true)
		{
			float num = vol.ToDecibel(allowBoost);
			int num2 = (allowBoost ? BroVolumeSplitPoints.Length : (GetFullVolumeSplitPointIndex() + 1));
			float broVolumeStep = GetBroVolumeStep(allowBoost);
			for (int i = 0; i < num2; i++)
			{
				if (i + 1 >= num2)
				{
					return 1f;
				}
				if (num >= BroVolumeSplitPoints[i] && num < BroVolumeSplitPoints[i + 1])
				{
					float num3 = broVolumeStep * (float)i;
					float num4 = Mathf.Abs(BroVolumeSplitPoints[i + 1] - BroVolumeSplitPoints[i]);
					float num5 = Mathf.Abs(num - BroVolumeSplitPoints[i]) / num4;
					return num3 + num5 * broVolumeStep;
				}
			}
			return 0f;
		}

		public static float SliderToBroVolume(float sliderValue, bool allowBoost = true)
		{
			if (sliderValue == 1f)
			{
				if (!allowBoost)
				{
					return 1f;
				}
				return 10f;
			}
			float broVolumeStep = GetBroVolumeStep(allowBoost);
			int num = (int)(sliderValue / broVolumeStep);
			float num2 = sliderValue % broVolumeStep / broVolumeStep;
			float num3 = Mathf.Abs(BroVolumeSplitPoints[num + 1] - BroVolumeSplitPoints[num]);
			return (BroVolumeSplitPoints[num] + num3 * num2).ToNormalizeVolume(allowBoost);
		}

		public static float GetBroVolumeStep(bool allowBoost)
		{
			int num = (allowBoost ? BroVolumeSplitPoints.Length : (GetFullVolumeSplitPointIndex() + 1));
			return 1f / (float)(num - 1);
		}

		private static int GetFullVolumeSplitPointIndex()
		{
			for (int i = 0; i < BroVolumeSplitPoints.Length; i++)
			{
				if (BroVolumeSplitPoints[i] == 0f)
				{
					return i;
				}
			}
			return -1;
		}

		public static bool Contains(this BroAudioType flags, BroAudioType targetFlag)
		{
			return (flags & targetFlag) != 0;
		}

		public static bool Contains(this RandomFlag flags, RandomFlag targetFlag)
		{
			return (flags & targetFlag) != 0;
		}

		public static BroAudioType ConvertEverythingFlag(this BroAudioType audioType)
		{
			if (audioType == (BroAudioType)(-1))
			{
				return BroAudioType.All;
			}
			return audioType;
		}

		public static float GetDeltaTime()
		{
			if (SoundManager.Instance.Setting.UpdateMode == AudioMixerUpdateMode.UnscaledTime)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		public static int GetSample(int sampleRate, float seconds)
		{
			return (int)((float)sampleRate * seconds);
		}

		public static bool IsDefaultCurve(this AnimationCurve curve, float defaultValue)
		{
			if (curve == null || curve.length == 0)
			{
				return true;
			}
			if (curve.length == 1 && curve[0].value == defaultValue)
			{
				return true;
			}
			return false;
		}

		public static void SetCustomCurveOrResetDefault(this AudioSource audioSource, AnimationCurve curve, AudioSourceCurveType curveType)
		{
			if (curveType == AudioSourceCurveType.CustomRolloff)
			{
				Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>" + $"Don't use this method on {AudioSourceCurveType.CustomRolloff}, please use RolloffMode to detect if is default or not");
				return;
			}
			float curveDefaultValue = GetCurveDefaultValue(curveType);
			if (!curve.IsDefaultCurve(curveDefaultValue))
			{
				audioSource.SetCustomCurve(curveType, curve);
				return;
			}
			switch (curveType)
			{
			case AudioSourceCurveType.SpatialBlend:
				audioSource.spatialBlend = curveDefaultValue;
				break;
			case AudioSourceCurveType.ReverbZoneMix:
				audioSource.reverbZoneMix = curveDefaultValue;
				break;
			case AudioSourceCurveType.Spread:
				audioSource.spread = curveDefaultValue;
				break;
			}
		}

		public static float GetCurveDefaultValue(AudioSourceCurveType curveType)
		{
			return curveType switch
			{
				AudioSourceCurveType.SpatialBlend => 0f, 
				AudioSourceCurveType.ReverbZoneMix => 1f, 
				AudioSourceCurveType.Spread => 0f, 
				_ => 0f, 
			};
		}

		public static IAudioEffectModifier CreateAudioEffectProxy<T>(T component) where T : Behaviour
		{
			if (!(component is AudioHighPassFilter source))
			{
				if (!(component is AudioLowPassFilter source2))
				{
					if (!(component is AudioReverbFilter source3))
					{
						if (!(component is AudioDistortionFilter source4))
						{
							if (!(component is AudioEchoFilter source5))
							{
								if (component is AudioChorusFilter source6)
								{
									return new AudioChorusFilterProxy(source6);
								}
								return LogAndReturnNull();
							}
							return new AudioEchoFilterProxy(source5);
						}
						return new AudioDistortionFilterProxy(source4);
					}
					return new AudioReverbFilterProxy(source3);
				}
				return new AudioLowPassFilterProxy(source2);
			}
			return new AudioHighPassFilterProxy(source);
			static IAudioEffectModifier LogAndReturnNull()
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>No proxy implementation found for " + typeof(T).Name);
				return null;
			}
		}

		public static Type GetFilterTypeFromProxy(IAudioEffectModifier proxy)
		{
			if (!(proxy is AudioHighPassFilterProxy))
			{
				if (!(proxy is AudioLowPassFilterProxy))
				{
					if (!(proxy is AudioReverbFilterProxy))
					{
						if (!(proxy is AudioDistortionFilterProxy))
						{
							if (!(proxy is AudioEchoFilterProxy))
							{
								if (proxy is AudioChorusFilterProxy)
								{
									return typeof(AudioChorusFilter);
								}
								return LogAndReturnNull();
							}
							return typeof(AudioEchoFilter);
						}
						return typeof(AudioDistortionFilter);
					}
					return typeof(AudioReverbFilter);
				}
				return typeof(AudioLowPassFilter);
			}
			return typeof(AudioHighPassFilter);
			Type LogAndReturnNull()
			{
				Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>No filter type mapping found for " + proxy?.GetType().Name);
				return null;
			}
		}

		public static Ease GetFadeInEase(this IAudioEntity entity)
		{
			if (!entity.IsLoop(LoopType.SeamlessLoop))
			{
				return SoundManager.FadeInEase;
			}
			return SoundManager.SeamlessFadeIn;
		}

		public static Ease GetFadeOutEase(this IAudioEntity entity)
		{
			if (!entity.IsLoop(LoopType.SeamlessLoop))
			{
				return SoundManager.FadeOutEase;
			}
			return SoundManager.SeamlessFadeOut;
		}

		public static bool IsLoop(this IAudioEntity entity, LoopType targetType)
		{
			if (entity.HasLoop(out var loopType, out var _))
			{
				return loopType == targetType;
			}
			return false;
		}

		internal static T GetOrCreateDecorator<T>(ref List<AudioPlayerDecorator> list, Func<T> onCreateDecorator) where T : AudioPlayerDecorator
		{
			if (list != null && list.TryGetDecorator<T>(out var result))
			{
				return result;
			}
			result = onCreateDecorator();
			if (list == null)
			{
				list = new List<AudioPlayerDecorator>();
			}
			list.Add(result);
			return result;
		}

		internal static bool TryGetDecorator<T>(this List<AudioPlayerDecorator> list, out T result) where T : AudioPlayerDecorator
		{
			result = null;
			if (list != null)
			{
				foreach (AudioPlayerDecorator item in list)
				{
					if (item is T val)
					{
						result = val;
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsPlayedGlobally(Vector3 playPos)
		{
			if (!GloballyPlayedPosition.IsNegativeInfinity() || !playPos.IsNegativeInfinity())
			{
				if (GloballyPlayedPosition.IsPositiveInfinity())
				{
					return playPos.IsPositiveInfinity();
				}
				return false;
			}
			return true;
		}

		public static bool IsPositiveInfinity(this Vector3 v)
		{
			if (float.IsPositiveInfinity(v.x) && float.IsPositiveInfinity(v.y))
			{
				return float.IsPositiveInfinity(v.z);
			}
			return false;
		}

		public static bool IsNegativeInfinity(this Vector3 v)
		{
			if (float.IsNegativeInfinity(v.x) && float.IsNegativeInfinity(v.y))
			{
				return float.IsNegativeInfinity(v.z);
			}
			return false;
		}

		public static void Log(string message, LogType type)
		{
			switch (type)
			{
			case LogType.Error:
				Debug.LogError(message);
				break;
			case LogType.Warning:
				Debug.LogWarning(message);
				break;
			case LogType.Log:
				Debug.Log(message);
				break;
			case LogType.Exception:
				throw new BroAudioException(message);
			default:
				throw new ArgumentOutOfRangeException();
			case LogType.Assert:
				break;
			}
		}
	}
}
