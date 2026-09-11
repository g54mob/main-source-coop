using UnityEngine.UIElements;

public class SharingClipCell : VisualElement
{
	private VisualElement m_backgroundElement;

	public SharingClipCell(VisualTreeAsset visualTreeAsset, ClipID clipID, bool local)
	{
		visualTreeAsset.CloneTree(this);
		Label label = this.Q<Label>("ID");
		m_backgroundElement = this.Q("Cell");
		label.text = clipID.ToMiniString();
		m_backgroundElement.AddToClassList(local ? "encoded" : "remote");
	}
}
