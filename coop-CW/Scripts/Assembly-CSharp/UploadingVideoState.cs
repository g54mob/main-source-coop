using UnityEngine;

public class UploadingVideoState : UploadVideoStationState
{
	public GameObject m_ui;

	public UploadingVideoState(GameObject ui)
	{
		m_ui = ui;
	}

	public override void Enter()
	{
		base.Enter();
		m_ui.SetActive(value: true);
	}

	public override void Exit()
	{
		base.Exit();
		m_ui.SetActive(value: false);
	}
}
