using System.Collections.Generic;
using System.Linq;
using EasyTextEffects.Editor.MyBoxCopy.Attributes;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using EasyTextEffects.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace EasyTextEffects
{
	[ExecuteAlways]
	public class TextEffect : MonoBehaviour
	{
		public TMP_Text text;

		[Space(5f)]
		public bool usePreset;

		[ConditionalField("usePreset", false, new object[] { })]
		public TagEffectsPreset preset;

		public List<TextEffectEntry> tagEffects;

		[Space(5f)]
		[FormerlySerializedAs("effectsList")]
		public List<GlobalTextEffectEntry> globalEffects;

		[Space(5f)]
		[Range(1f, 120f)]
		public int updatesPerSecond = 30;

		private List<TextEffectEntry> allTagEffects_;

		private List<TextEffectEntry> onStartTagEffects_;

		private List<TextEffectEntry> manualTagEffects_;

		private List<GlobalTextEffectEntry> onStartEffects_;

		private List<GlobalTextEffectEntry> manualEffects_;

		private List<TextEffectInstance> entryEffectsCopied_;

		private float nextUpdateTime_;

		public void UpdateStyleInfos()
		{
			if (!(text == null) && text.textInfo != null)
			{
				TMP_TextInfo textInfo = text.textInfo;
				TMP_LinkInfo[] linkInfo = textInfo.linkInfo;
				int linkCount = textInfo.linkCount;
				CopyGlobalEffects(textInfo);
				AddTagEffects(linkInfo, linkCount);
				StartOnStartEffects();
			}
		}

		private void CopyGlobalEffects(TMP_TextInfo textInfo)
		{
			onStartEffects_ = new List<GlobalTextEffectEntry>();
			manualEffects_ = new List<GlobalTextEffectEntry>();
			if (globalEffects == null)
			{
				return;
			}
			globalEffects.ForEach(delegate(GlobalTextEffectEntry _entry)
			{
				if (!(_entry.effect == null))
				{
					GlobalTextEffectEntry globalTextEffectEntry = new GlobalTextEffectEntry();
					globalTextEffectEntry.effect = _entry.effect.Instantiate();
					globalTextEffectEntry.effect.startCharIndex = 0;
					globalTextEffectEntry.effect.charLength = textInfo.characterCount;
					globalTextEffectEntry.overrideTagEffects = _entry.overrideTagEffects;
					globalTextEffectEntry.onEffectCompleted = _entry.onEffectCompleted;
					if (_entry.triggerWhen == TextEffectEntry.TriggerWhen.OnStart)
					{
						onStartEffects_.Add(globalTextEffectEntry);
					}
					else
					{
						manualEffects_.Add(globalTextEffectEntry);
					}
				}
			});
		}

		private void AddTagEffects(TMP_LinkInfo[] styles, int linkCount)
		{
			onStartTagEffects_ = new List<TextEffectEntry>();
			manualTagEffects_ = new List<TextEffectEntry>();
			if (tagEffects == null)
			{
				return;
			}
			allTagEffects_ = new List<TextEffectEntry>(tagEffects);
			if (usePreset && preset != null)
			{
				allTagEffects_.AddRange(preset.tagEffects);
			}
			for (int i = 0; i < linkCount; i++)
			{
				TMP_LinkInfo tMP_LinkInfo = styles[i];
				if (tMP_LinkInfo.GetLinkID() == string.Empty)
				{
					continue;
				}
				foreach (TextEffectEntry item in GetTagEffectsByName(tMP_LinkInfo.GetLinkID()))
				{
					if (!(item.effect == null))
					{
						TextEffectEntry textEffectEntry = new TextEffectEntry();
						textEffectEntry.effect = item.effect.Instantiate();
						textEffectEntry.effect.startCharIndex = tMP_LinkInfo.linkTextfirstCharacterIndex;
						textEffectEntry.effect.charLength = tMP_LinkInfo.linkTextLength;
						if (item.triggerWhen == TextEffectEntry.TriggerWhen.OnStart)
						{
							onStartTagEffects_.Add(textEffectEntry);
						}
						else
						{
							manualTagEffects_.Add(textEffectEntry);
						}
					}
				}
			}
		}

		private List<TextEffectEntry> GetTagEffectsByName(string _effectName)
		{
			List<TextEffectEntry> list = new List<TextEffectEntry>();
			string[] array = _effectName.Split('+');
			foreach (string effectName in array)
			{
				List<TextEffectEntry> list2 = allTagEffects_.FindAll((TextEffectEntry _entry) => _entry.effect?.effectTag == effectName);
				if (list2.Count >= 1)
				{
					list.Add(list2[0]);
				}
			}
			return list;
		}

		public void Refresh()
		{
			if (!(text == null))
			{
				text.ForceMeshUpdate();
				UpdateStyleInfos();
			}
		}

		private void Reset()
		{
			text = GetComponent<TMP_Text>();
		}

		private void OnValidate()
		{
		}

		private void OnEnable()
		{
			Refresh();
		}

		private void OnDisable()
		{
		}

		public void Update()
		{
			if (!text)
			{
				return;
			}
			float time = TimeUtil.GetTime();
			if (time < nextUpdateTime_)
			{
				return;
			}
			nextUpdateTime_ = time + 1f / (float)updatesPerSecond;
			text.ForceMeshUpdate();
			TMP_TextInfo textInfo = text.textInfo;
			for (int i = 0; i < textInfo.characterCount; i++)
			{
				if (textInfo.characterInfo[i].isVisible)
				{
					int capturedI = i;
					onStartEffects_.Where((GlobalTextEffectEntry _entry) => !_entry.overrideTagEffects).ForEach(delegate(GlobalTextEffectEntry _entry)
					{
						_entry.effect.ApplyEffect(textInfo, capturedI);
					});
					manualEffects_.Where((GlobalTextEffectEntry _entry) => !_entry.overrideTagEffects).ForEach(delegate(GlobalTextEffectEntry _entry)
					{
						_entry.effect.ApplyEffect(textInfo, capturedI);
					});
					onStartTagEffects_.ForEach(delegate(TextEffectEntry _entry)
					{
						_entry.effect.ApplyEffect(textInfo, capturedI);
					});
					manualTagEffects_.ForEach(delegate(TextEffectEntry _entry)
					{
						_entry.effect.ApplyEffect(textInfo, capturedI);
					});
					onStartEffects_.Where((GlobalTextEffectEntry _entry) => _entry.overrideTagEffects).ForEach(delegate(GlobalTextEffectEntry _entry)
					{
						_entry.effect.ApplyEffect(textInfo, capturedI);
					});
					manualEffects_.Where((GlobalTextEffectEntry _entry) => _entry.overrideTagEffects).ForEach(delegate(GlobalTextEffectEntry _entry)
					{
						_entry.effect.ApplyEffect(textInfo, capturedI);
					});
				}
			}
			for (int num = 0; num < textInfo.meshInfo.Length; num++)
			{
				TMP_MeshInfo tMP_MeshInfo = textInfo.meshInfo[num];
				tMP_MeshInfo.mesh.colors32 = tMP_MeshInfo.colors32;
				tMP_MeshInfo.mesh.vertices = tMP_MeshInfo.vertices;
				text.UpdateGeometry(tMP_MeshInfo.mesh, num);
			}
		}

		public void StartOnStartEffects()
		{
			onStartEffects_.ForEach(delegate(GlobalTextEffectEntry _entry)
			{
				_entry.StartEffect();
			});
			onStartTagEffects_.ForEach(delegate(TextEffectEntry _entry)
			{
				_entry.StartEffect();
			});
			nextUpdateTime_ = 0f;
		}

		public void StartManualEffects()
		{
			manualEffects_.ForEach(delegate(GlobalTextEffectEntry _entry)
			{
				_entry.StartEffect();
			});
		}

		public void StopManualEffects()
		{
			manualEffects_.ForEach(delegate(GlobalTextEffectEntry _entry)
			{
				_entry.effect.StopEffect();
			});
		}

		public void StartManualTagEffects()
		{
			manualTagEffects_.ForEach(delegate(TextEffectEntry _entry)
			{
				_entry.StartEffect();
			});
		}

		public void StopManualTagEffects()
		{
			manualTagEffects_.ForEach(delegate(TextEffectEntry _entry)
			{
				_entry.effect.StopEffect();
			});
		}

		public void StartManualEffect(string _effectName)
		{
			GlobalTextEffectEntry globalTextEffectEntry = manualEffects_.Find((GlobalTextEffectEntry _entry) => _entry.effect.effectTag == _effectName);
			List<string> values = manualEffects_.Select((GlobalTextEffectEntry _entry) => _entry.effect.effectTag).ToList();
			if (globalTextEffectEntry != null)
			{
				globalTextEffectEntry.StartEffect();
				return;
			}
			Debug.LogWarning("Effect " + _effectName + " not found");
			Debug.Log("Available effects: " + string.Join(", ", values));
		}

		public void StartManualTagEffect(string _effectName)
		{
			TextEffectEntry textEffectEntry = manualTagEffects_.Find((TextEffectEntry _entry) => _entry.effect.effectTag == _effectName);
			List<string> values = manualTagEffects_.Select((TextEffectEntry _entry) => _entry.effect.effectTag).ToList();
			if (textEffectEntry != null)
			{
				textEffectEntry.StartEffect();
				return;
			}
			Debug.LogWarning("Effect " + _effectName + " not found");
			Debug.Log("Available effects: " + string.Join(", ", values));
		}

		public List<TextEffectStatus> QueryEffectStatuses(TextEffectType _effectType, TextEffectEntry.TriggerWhen _triggerWhen)
		{
			IReadOnlyList<TextEffectEntry> source;
			if (_effectType != TextEffectType.Global)
			{
				IReadOnlyList<TextEffectEntry> readOnlyList = ((_triggerWhen == TextEffectEntry.TriggerWhen.OnStart) ? onStartTagEffects_ : manualTagEffects_);
				source = readOnlyList;
			}
			else
			{
				IReadOnlyList<TextEffectEntry> readOnlyList = ((_triggerWhen == TextEffectEntry.TriggerWhen.OnStart) ? onStartEffects_ : manualEffects_);
				source = readOnlyList;
			}
			return source.Select((TextEffectEntry _entry) => new TextEffectStatus
			{
				Tag = _entry.effect.effectTag,
				Started = _entry.effect.started,
				IsComplete = _entry.effect.IsComplete
			}).ToList();
		}

		public List<TextEffectStatus> QueryEffectStatusesByTag(TextEffectType _effectType, TextEffectEntry.TriggerWhen _triggerWhen, string _tag)
		{
			IReadOnlyList<TextEffectEntry> source;
			if (_effectType != TextEffectType.Global)
			{
				IReadOnlyList<TextEffectEntry> readOnlyList = ((_triggerWhen == TextEffectEntry.TriggerWhen.OnStart) ? onStartTagEffects_ : manualTagEffects_);
				source = readOnlyList;
			}
			else
			{
				IReadOnlyList<TextEffectEntry> readOnlyList = ((_triggerWhen == TextEffectEntry.TriggerWhen.OnStart) ? onStartEffects_ : manualEffects_);
				source = readOnlyList;
			}
			return (from _entry in source
				where _entry.effect.effectTag == _tag
				select new TextEffectStatus
				{
					Tag = _entry.effect.effectTag,
					Started = _entry.effect.started,
					IsComplete = _entry.effect.IsComplete
				}).ToList();
		}
	}
}
