namespace Features.TutorialModule.Scripts.GuideModule
{
	public readonly struct GuideLineHandle
	{
		public readonly int Id;

		public bool IsValid => Id > 0;

		public static GuideLineHandle Invalid => default(GuideLineHandle);

		public GuideLineHandle(int id)
		{
			Id = id;
		}
	}
}
