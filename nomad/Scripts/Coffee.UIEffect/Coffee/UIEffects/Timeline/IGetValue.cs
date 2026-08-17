namespace Coffee.UIEffects.Timeline
{
	public interface IGetValue<out T>
	{
		T Get(float time);
	}
}
