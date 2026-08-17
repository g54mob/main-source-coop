namespace CodeStage.AdvancedFPSCounter.CountersData
{
	public abstract class StaticCounterData : BaseCounterData
	{
		internal override void Activate()
		{
			base.Activate();
			if (inited)
			{
				UpdateValue();
			}
		}
	}
}
