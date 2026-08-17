namespace UIControllers
{
	public class InGameUIController : UIController
	{
		public override void Exit()
		{
			base.Exit();
			SaveSystem.SaveData(StaticInstance<TransitionSystem>.Instance.GetActiveScene() == Scenes.SinglePlayer.ToString());
		}
	}
}
