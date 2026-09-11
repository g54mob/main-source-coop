using UnityEngine.UIElements;

public class RoomStatsCell : VisualElement
{
	public RoomStatsCell(VisualTreeAsset roomStatsCell, string defaultText)
	{
		roomStatsCell.CloneTree(this);
	}
}
