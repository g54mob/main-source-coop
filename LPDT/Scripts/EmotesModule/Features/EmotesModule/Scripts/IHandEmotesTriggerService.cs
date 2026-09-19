namespace Features.EmotesModule.Scripts
{
	public interface IHandEmotesTriggerService
	{
		void TriggerHandEmote(HandEmoteType handEmote);

		void TriggerRandomHandEmote();
	}
}
