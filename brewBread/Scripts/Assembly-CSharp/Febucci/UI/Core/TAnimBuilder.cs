using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Febucci.UI.Core
{
	public static class TAnimBuilder
	{
		[Serializable]
		internal struct TagFormatting
		{
			public char charOpeningTag;

			public char charClosingTag;

			public TagFormatting(char openingChar, char closingChar)
			{
				charOpeningTag = openingChar;
				charClosingTag = closingChar;
			}
		}

		internal static TagFormatting tag_behaviors = new TagFormatting('<', '>');

		internal static TagFormatting tag_appearances = new TagFormatting('{', '}');

		private static TAnimGlobalDataScriptable _data;

		private static bool hasData;

		private static Dictionary<string, Type> behaviorsData = new Dictionary<string, Type>();

		private static Dictionary<string, Type> appearancesData = new Dictionary<string, Type>();

		private static HashSet<string> globalDefaultActions = new HashSet<string>();

		private static HashSet<string> globalCustomActions = new HashSet<string>();

		private static bool globalDatabaseInitialized;

		internal static TAnimGlobalDataScriptable data => _data;

		public static string[] GetAllBehaviorsTags()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < behaviorsData.Count; i++)
			{
				list.Add(behaviorsData.Keys.ElementAt(i));
			}
			return list.ToArray();
		}

		public static string[] GetAllApppearancesTags()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < appearancesData.Count; i++)
			{
				list.Add(appearancesData.Keys.ElementAt(i));
			}
			return list.ToArray();
		}

		public static void InitializeGlobalDatabase()
		{
			if (globalDatabaseInitialized)
			{
				return;
			}
			globalDatabaseInitialized = true;
			TextUtilities.Initialize();
			PopulateEffectsFromAssembly<BehaviorBase>(ref behaviorsData);
			PopulateEffectsFromAssembly<AppearanceBase>(ref appearancesData);
			globalDefaultActions.Add("waitfor");
			globalDefaultActions.Add("waitinput");
			globalDefaultActions.Add("speed");
			hasData = false;
			_data = Resources.Load("TextAnimator GlobalData") as TAnimGlobalDataScriptable;
			if (!(data != null))
			{
				return;
			}
			hasData = true;
			if (data.customTagsFormatting)
			{
				if (data.tagInfo_behaviors.charOpeningTag != data.tagInfo_appearances.charOpeningTag && data.tagInfo_behaviors.charClosingTag != data.tagInfo_appearances.charClosingTag)
				{
					tag_behaviors = data.tagInfo_behaviors;
					tag_appearances = data.tagInfo_appearances;
				}
				else
				{
					Debug.LogError("Not valid");
				}
			}
			for (int i = 0; i < data.globalBehaviorPresets.Length; i++)
			{
				TryAddingPresetToDictionary(ref behaviorsData, data.globalBehaviorPresets[i].effectTag, typeof(PresetBehavior));
			}
			for (int j = 0; j < data.globalAppearancePresets.Length; j++)
			{
				TryAddingPresetToDictionary(ref appearancesData, data.globalAppearancePresets[j].effectTag, typeof(PresetAppearance));
			}
			if (data.customActions == null || data.customActions.Length == 0)
			{
				return;
			}
			for (int k = 0; k < data.customActions.Length; k++)
			{
				if (data.customActions[k].Length <= 0)
				{
					Debug.LogError($"TextAnimator: Custom action {k} has an empty tag!");
				}
				else if (globalCustomActions.Contains(data.customActions[k]))
				{
					Debug.LogError("TextAnimator: Custom feature with tag '" + data.customActions[k] + "' is already present, it won't be added to the database.");
				}
				else
				{
					globalCustomActions.Add(data.customActions[k]);
				}
			}
			static List<Type> GetAssemblyClasses<T>() where T : EffectsBase
			{
				return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
					from assemblyType in domainAssembly.GetTypes()
					where assemblyType.IsSubclassOf(typeof(T))
					where !assemblyType.IsAbstract
					select assemblyType).ToList();
			}
			static void PopulateEffectsFromAssembly<T>(ref Dictionary<string, Type> effectsList) where T : EffectsBase
			{
				List<Type> list = GetAssemblyClasses<T>();
				for (int l = 0; l < list.Count; l++)
				{
					string empty = string.Empty;
					EffectInfoAttribute customAttribute = list[l].GetCustomAttribute<EffectInfoAttribute>();
					if (customAttribute != null)
					{
						empty = customAttribute.tag;
						if (!string.IsNullOrEmpty(empty))
						{
							if (!effectsList.ContainsKey(empty))
							{
								effectsList.Add(empty, list[l]);
							}
							else
							{
								Debug.LogError("TextAnimator: not adding effect <" + empty + "> (from class '" + list[l].Name + "') to the database because an effect with the same tag has already been added (by class '" + effectsList[empty].Name + "')");
							}
						}
					}
					else
					{
						Debug.LogError("TextAnimator: skipping class " + list[l].Name + ". Please add a 'EffectInfoAttribute' on top of it.");
					}
				}
			}
		}

		internal static bool TryGetGlobalPresetBehavior(string tag, out PresetBehaviorValues result)
		{
			if (!hasData)
			{
				result = null;
				return false;
			}
			return GetPresetFromArray(tag, data.globalBehaviorPresets, out result);
		}

		internal static bool TryGetGlobalPresetAppearance(string tag, out PresetAppearanceValues result)
		{
			if (!hasData)
			{
				result = null;
				return false;
			}
			return GetPresetFromArray(tag, data.globalAppearancePresets, out result);
		}

		internal static bool GetPresetFromArray<T>(string tag, T[] presets, out T result) where T : PresetBaseValues
		{
			if (presets.Length != 0)
			{
				for (int i = 0; i < presets.Length; i++)
				{
					if (tag.Equals(presets[i].effectTag))
					{
						result = presets[i];
						return true;
					}
				}
			}
			result = null;
			return false;
		}

		internal static bool IsDefaultAction(string tag)
		{
			if (globalDefaultActions.Count > 0 && globalDefaultActions.Contains(tag))
			{
				return true;
			}
			return false;
		}

		internal static bool IsCustomAction(string tag)
		{
			if (globalCustomActions.Count > 0 && globalCustomActions.Contains(tag))
			{
				return true;
			}
			return false;
		}

		internal static bool TryGetGlobalBehaviorFromTag(string effectTag, string entireRichTextTag, out BehaviorBase effectClass)
		{
			return TryGetEffectClassFromTag<BehaviorBase>(behaviorsData, effectTag, entireRichTextTag, out effectClass);
		}

		internal static bool TryGetGlobalAppearanceFromTag(string effectTag, string entireRichTextTag, out AppearanceBase effectClass)
		{
			return TryGetEffectClassFromTag<AppearanceBase>(appearancesData, effectTag, entireRichTextTag, out effectClass);
		}

		internal static bool TryGetEffectClassFromTag<T>(Dictionary<string, Type> dictionary, string effectTag, string entireRichTextTag, out T effectClass) where T : EffectsBase
		{
			if (dictionary.ContainsKey(effectTag))
			{
				effectClass = Activator.CreateInstance(dictionary[effectTag]) as T;
				effectClass._Initialize(effectTag, entireRichTextTag);
				return true;
			}
			effectClass = null;
			return false;
		}

		internal static void TryAddingPresetToDictionary(ref Dictionary<string, Type> database, string tag, Type type)
		{
			if (string.IsNullOrEmpty(tag))
			{
				Debug.LogWarning("TextAnimator: Preset has a null or empty tag '" + tag + "'");
			}
			else if (!TextUtilities.IsTagLongEnough(tag))
			{
				Debug.LogWarning("TextAnimator: Preset has tag '" + tag + "' shorter than three characters.");
			}
			else if (database.ContainsKey(tag))
			{
				Debug.LogWarning("TextAnimator: A Preset has tag '" + tag + "' that's already present, it won't be added to the database.");
			}
			else
			{
				database.Add(tag, type);
			}
		}
	}
}
