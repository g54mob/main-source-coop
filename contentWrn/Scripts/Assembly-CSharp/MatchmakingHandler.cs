using UnityEngine;
using Zorro.Core;
using Zorro.UI;

public class MatchmakingHandler : Singleton<MatchmakingHandler>
{
	private MatchmakingStateMachine m_stateMachine;

	private void Start()
	{
		m_stateMachine = new MatchmakingStateMachine();
		m_stateMachine.RegisterState(new MatchmakingIdleState());
		m_stateMachine.RegisterState(new MatchmakingJoinRandomState(GetComponent<UIPageHandler>()));
		m_stateMachine.SwitchState<MatchmakingIdleState>();
	}

	public void StartMatchmaking()
	{
		if (!(m_stateMachine.CurrentState is MatchmakingIdleState))
		{
			Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_MatchError_Title), LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_MatchError_Body));
			Debug.LogError("Matchmaking is already in progress");
		}
		else
		{
			m_stateMachine.SwitchState<MatchmakingJoinRandomState>().StartMatchmaking();
		}
	}

	public void StopMatchmaking()
	{
		m_stateMachine.SwitchState<MatchmakingIdleState>();
	}
}
