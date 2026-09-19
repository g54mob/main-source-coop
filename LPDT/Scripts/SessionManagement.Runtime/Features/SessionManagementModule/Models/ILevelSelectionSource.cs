namespace Features.SessionManagementModule.Models
{
	public interface ILevelSelectionSource
	{
		string SelectedLevelName { get; }

		int EnterGateWindowTicks { get; }
	}
}
