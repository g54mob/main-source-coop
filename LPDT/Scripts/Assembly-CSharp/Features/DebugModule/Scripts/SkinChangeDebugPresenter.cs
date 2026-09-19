using System;
using Features.SkinChangeModule.Scripts;
using Features.SkinConfiguration.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public class SkinChangeDebugPresenter : PresenterBehaviour<SkinChangeDebugViewBase>
	{
		private readonly SkinChangeService _skinChangeService;

		public SkinChangeDebugPresenter(SkinChangeService skinChangeService)
		{
			_skinChangeService = skinChangeService;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			SkinChangeDebugViewBase view = base.View;
			view.OnSkinChangeRequested = (Action<SkinPartType, int>)Delegate.Combine(view.OnSkinChangeRequested, new Action<SkinPartType, int>(ChangeSkinPart));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			SkinChangeDebugViewBase view = base.View;
			view.OnSkinChangeRequested = (Action<SkinPartType, int>)Delegate.Remove(view.OnSkinChangeRequested, new Action<SkinPartType, int>(ChangeSkinPart));
		}

		private void ChangeSkinPart(SkinPartType skinPartType, int skinId)
		{
			_skinChangeService.ChangeLocalSkinPart(skinPartType, (SkinType)skinId);
		}
	}
}
