using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.RumModule.Scripts.Views
{
	[PublicAPI]
	public class PermanentRumBuffItemPresenter : PresenterBehaviour<PermanentRumBuffItemViewBase>
	{
		private readonly RumIconsConfiguration _rumIconsConfiguration;

		public PermanentRumBuffItemPresenter(RumIconsConfiguration rumIconsConfiguration)
		{
			_rumIconsConfiguration = rumIconsConfiguration;
		}

		public void SetData(RumType rumType, int count)
		{
			base.View.SetRumIcon(_rumIconsConfiguration.GetIcon(rumType));
			base.View.SetStackCount(count);
		}

		public void SetSiblingIndex(int index)
		{
			base.View.transform.SetSiblingIndex(index);
		}

		public void DestroyView()
		{
			Object.Destroy(base.View.gameObject);
		}
	}
}
