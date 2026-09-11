using UnityEngine;

public class UploadVideoClosedState : UploadVideoStationState
{
	private GameObject m_ui;

	public UploadVideoClosedState(GameObject ui)
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
