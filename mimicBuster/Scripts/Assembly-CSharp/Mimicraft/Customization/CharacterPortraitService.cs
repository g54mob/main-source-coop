using System;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class CharacterPortraitService
	{
		public static string StudioSceneName
		{
			get
			{
				return PortraitService<CharacterPortraitStudio, CharacterData>.StudioSceneName;
			}
			set
			{
				PortraitService<CharacterPortraitStudio, CharacterData>.StudioSceneName = value;
			}
		}

		public static float KeepLoadedSeconds
		{
			get
			{
				return PortraitService<CharacterPortraitStudio, CharacterData>.KeepLoadedSeconds;
			}
			set
			{
				PortraitService<CharacterPortraitStudio, CharacterData>.KeepLoadedSeconds = value;
			}
		}

		public static Vector3 StudioOffset
		{
			get
			{
				return PortraitService<CharacterPortraitStudio, CharacterData>.StudioOffset;
			}
			set
			{
				PortraitService<CharacterPortraitStudio, CharacterData>.StudioOffset = value;
			}
		}

		public static int Pending => PortraitService<CharacterPortraitStudio, CharacterData>.Pending;

		public static event Action<string> PortraitWritten
		{
			add
			{
				PortraitService<CharacterPortraitStudio, CharacterData>.PortraitWritten += value;
			}
			remove
			{
				PortraitService<CharacterPortraitStudio, CharacterData>.PortraitWritten -= value;
			}
		}

		static CharacterPortraitService()
		{
			PortraitService<CharacterPortraitStudio, CharacterData>.StudioSceneName = "CharacterStudio";
		}

		public static void Request(string characterFilePath, CharacterData data)
		{
			PortraitService<CharacterPortraitStudio, CharacterData>.Request(characterFilePath, data);
		}

		public static int RequestMissing(CharacterRigDefinition rig, bool all = false)
		{
			int num = 0;
			foreach (CharacterListEntry item in CharacterStorage.List())
			{
				if (all || !SavedThumbnails.Exists(item.FilePath))
				{
					CharacterData characterData = CharacterStorage.Load(item.FilePath, rig);
					if (characterData != null)
					{
						Request(item.FilePath, characterData);
						num++;
					}
				}
			}
			return num;
		}
	}
}
