using UnityEngine.UIElements;

public class ContentCell : VisualElement
{
	public ContentCell(VisualTreeAsset visualTreeAsset, string label)
	{
		visualTreeAsset.CloneTree(this);
		this.Q<Label>().text = label;
	}
}
