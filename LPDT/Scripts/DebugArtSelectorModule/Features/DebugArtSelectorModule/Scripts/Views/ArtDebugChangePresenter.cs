using System.Collections.Generic;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugArtSelectorModule.Scripts.Views
{
	[PublicAPI]
	public class ArtDebugChangePresenter : PresenterBehaviour<ArtDebugChangeViewBase>
	{
		private readonly ArtListConfiguration _artListConfiguration;

		private readonly IArtApplierService _artApplierService;

		public ArtDebugChangePresenter(ArtListConfiguration artListConfiguration, IArtApplierService artApplierService)
		{
			_artListConfiguration = artListConfiguration;
			_artApplierService = artApplierService;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.Dropdown.options.Clear();
			List<string> list = new List<string>();
			foreach (ArtConfiguration artConfiguration in _artListConfiguration.ArtConfigurations)
			{
				list.Add(artConfiguration.name);
			}
			base.View.Dropdown.AddOptions(list);
			base.View.Button.onClick.AddListener(ApplySettings);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.Button.onClick.RemoveListener(ApplySettings);
		}

		private void ApplySettings()
		{
			_artApplierService.ApplyArtConfiguration(_artListConfiguration.ArtConfigurations[base.View.Dropdown.value]);
		}
	}
}
