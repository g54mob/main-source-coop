public abstract class Objective
{
	protected string TextToShow;

	public abstract string GetObjectiveDescription();

	public bool ShouldShow()
	{
		return !ObjectiveDatabase.HasObjectiveBeenCompleted(this);
	}
}
