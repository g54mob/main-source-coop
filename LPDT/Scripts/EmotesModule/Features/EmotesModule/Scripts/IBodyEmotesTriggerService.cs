namespace Features.EmotesModule.Scripts
{
	public interface IBodyEmotesTriggerService
	{
		void TriggerEmote(BodyEmoteType emoteType);

		void TriggerRandomEmote();
	}
}
