namespace Features.EmotesModule.Scripts
{
	public interface IFaceEmotesTriggerService
	{
		void TriggerEmote(FaceEmoteType emoteType, float percent);

		void TriggerRandomEmote(float percent);
	}
}
