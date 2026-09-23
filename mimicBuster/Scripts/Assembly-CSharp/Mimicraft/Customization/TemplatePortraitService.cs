using System;
using Mimicraft.VoxelEditor.Core;

namespace Mimicraft.Customization
{
	public static class TemplatePortraitService
	{
		public static string StudioSceneName
		{
			get
			{
				return PortraitService<TemplateStudio, TemplateModel>.StudioSceneName;
			}
			set
			{
				PortraitService<TemplateStudio, TemplateModel>.StudioSceneName = value;
			}
		}

		public static float KeepLoadedSeconds
		{
			get
			{
				return PortraitService<TemplateStudio, TemplateModel>.KeepLoadedSeconds;
			}
			set
			{
				PortraitService<TemplateStudio, TemplateModel>.KeepLoadedSeconds = value;
			}
		}

		public static int Pending => PortraitService<TemplateStudio, TemplateModel>.Pending;

		public static event Action<string> PortraitWritten
		{
			add
			{
				PortraitService<TemplateStudio, TemplateModel>.PortraitWritten += value;
			}
			remove
			{
				PortraitService<TemplateStudio, TemplateModel>.PortraitWritten -= value;
			}
		}

		static TemplatePortraitService()
		{
			PortraitService<TemplateStudio, TemplateModel>.StudioSceneName = "TemplateStudio";
		}

		public static void Request(string filePath, TemplateModel data)
		{
			if (data != null)
			{
				PortraitService<TemplateStudio, TemplateModel>.Request(filePath, data);
			}
		}

		public static void RequestFor(string filePath)
		{
			Request(filePath, TemplateStorage.LoadTemplate(filePath));
		}

		public static int RequestMissing(string category = null, bool all = false)
		{
			int num = 0;
			foreach (TemplateListEntry item in TemplateStorage.ListTemplates())
			{
				if ((category == null || !(item.Category != category)) && (all || !SavedThumbnails.Exists(item.FilePath)))
				{
					TemplateModel templateModel = TemplateStorage.LoadTemplate(item.FilePath);
					if (templateModel != null)
					{
						Request(item.FilePath, templateModel);
						num++;
					}
				}
			}
			return num;
		}
	}
}
