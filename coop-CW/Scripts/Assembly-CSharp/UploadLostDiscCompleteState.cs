using System;

public class UploadLostDiscCompleteState : UploadVideoStationState
{
	public UploadCompleteUI m_ui;

	public UploadLostDiscCompleteState(UploadCompleteUI ui)
	{
		m_ui = ui;
	}

	public override void Enter()
	{
		base.Enter();
		m_ui.gameObject.SetActive(value: true);
	}

	public override void Exit()
	{
		base.Exit();
		m_ui.gameObject.SetActive(value: false);
	}

	public void PlayLostFootage(LostFootageHandle footage)
	{
		m_ui.PlayVideo(footage, 0, Array.Empty<Comment>(), null);
	}
}
