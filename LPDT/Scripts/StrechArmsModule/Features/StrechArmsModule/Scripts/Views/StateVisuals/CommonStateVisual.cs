namespace Features.StrechArmsModule.Scripts.Views.StateVisuals
{
	public class CommonStateVisual : ArmStateVisual
	{
		public override void Enable()
		{
			base.gameObject.SetActive(value: true);
		}

		public override void Disable()
		{
			base.gameObject.SetActive(value: false);
		}

		public override void SetProgress(float progress)
		{
		}
	}
}
