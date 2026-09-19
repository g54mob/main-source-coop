using Features.KrakenModule.Scripts.Data;

namespace Features.KrakenModule.Scripts.Core
{
	public interface IKrakenStateMachine
	{
		KrakenStateId CurrentState { get; }

		bool CanAcceptInteraction { get; }

		bool IsVisible { get; }

		bool RequestAppear(KrakenAppearReason reason);

		bool RequestHide();

		bool TryEnterInteractionState(KrakenStateId stateId);

		void CompleteInteraction(KrakenStateId nextState = KrakenStateId.Idle);

		void ResetInactivityTimer();
	}
}
