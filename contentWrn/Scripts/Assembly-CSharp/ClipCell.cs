using UnityEngine.UIElements;

public class ClipCell : VisualElement
{
	public enum Background
	{
		INVALID = 0,
		Recording = 1,
		Local = 2,
		Encoded = 3,
		Remote = 4,
		Error = 5,
		Recieved = 6
	}

	private Clip m_clip;

	private Background m_background;

	private VisualElement m_backgroundElement;

	public ClipCell(VisualTreeAsset visualTreeAsset, Clip clip)
	{
		m_clip = clip;
		visualTreeAsset.CloneTree(this);
		Label label = this.Q<Label>("ID");
		m_backgroundElement = this.Q("Cell");
		label.text = clip.clipID.ToMiniString();
	}

	public void Update()
	{
		Background background = Background.INVALID;
		background = ((!m_clip.Valid) ? Background.Error : (m_clip.local ? (m_clip.isRecording ? Background.Recording : ((!m_clip.encoded) ? Background.Local : Background.Encoded)) : ((!m_clip.hasBeenRecieved) ? Background.Remote : Background.Recieved)));
		SetBackground(background);
	}

	public void SetBackground(Background background)
	{
		if (m_background != background)
		{
			string backgroundStyle = GetBackgroundStyle(m_background);
			m_backgroundElement.RemoveFromClassList(backgroundStyle);
			backgroundStyle = GetBackgroundStyle(background);
			m_backgroundElement.AddToClassList(backgroundStyle);
			m_background = background;
		}
	}

	private string GetBackgroundStyle(Background style)
	{
		return style switch
		{
			Background.Recording => "recording", 
			Background.Local => "local", 
			Background.Encoded => "encoded", 
			Background.Remote => "remote", 
			Background.Error => "error", 
			Background.Recieved => "recieved", 
			_ => "invalid", 
		};
	}
}
