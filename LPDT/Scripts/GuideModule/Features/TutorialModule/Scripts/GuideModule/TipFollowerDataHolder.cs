using System;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class TipFollowerDataHolder
	{
		private TipFollower _tipFollower;

		public TipFollower TipFollower
		{
			get
			{
				return _tipFollower;
			}
			set
			{
				_tipFollower = value;
				this.OnTipFollowerChanged?.Invoke(_tipFollower);
			}
		}

		public event Action<TipFollower> OnTipFollowerChanged;
	}
}
