namespace Features.TutorialModule.Scripts.GuideModule
{
	public readonly struct TipHandle
	{
		public readonly int Id;

		public bool IsValid => Id > 0;

		public static TipHandle Invalid => default(TipHandle);

		public TipHandle(int id)
		{
			Id = id;
		}
	}
}
