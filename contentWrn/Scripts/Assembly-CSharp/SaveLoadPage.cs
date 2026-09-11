using UnityEngine.UIElements;
using Zorro.Core.CLI;

public class SaveLoadPage : DebugPage
{
	public SaveLoadPage()
	{
		Add(new Button(delegate
		{
			SaveSystem.SaveToDisk();
		})
		{
			text = "Save"
		});
	}
}
