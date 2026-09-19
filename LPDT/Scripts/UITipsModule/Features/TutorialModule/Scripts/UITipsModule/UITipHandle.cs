namespace Features.TutorialModule.Scripts.UITipsModule
{
	public readonly struct UITipHandle
	{
		public readonly int Id;

		public bool IsValid => Id > 0;

		public static UITipHandle Invalid => default(UITipHandle);

		public UITipHandle(int id)
		{
			Id = id;
		}
	}
}
