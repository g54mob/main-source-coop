namespace Den.Tools
{
	public interface ITile
	{
		bool IsNull { get; }

		void Move(Coord coord, float dist);

		void Dist(float dist);

		void Remove();
	}
}
