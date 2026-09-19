using System;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class GuideLineBuilderDataHolder
	{
		private GuideLineBuilder _guideLineBuilder;

		public GuideLineBuilder GuideLineBuilder
		{
			get
			{
				return _guideLineBuilder;
			}
			set
			{
				_guideLineBuilder = value;
				this.OnGuideLineBuilderChanged?.Invoke(_guideLineBuilder);
			}
		}

		public event Action<GuideLineBuilder> OnGuideLineBuilderChanged;
	}
}
