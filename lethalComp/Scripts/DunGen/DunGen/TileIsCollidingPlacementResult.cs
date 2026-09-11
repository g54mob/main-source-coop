namespace DunGen
{
	public sealed class TileIsCollidingPlacementResult : TileTemplatePlacementResult
	{
		public override string DisplayName => "Collision";

		public TileIsCollidingPlacementResult(TileProxy tileTemplate)
			: base(tileTemplate)
		{
		}
	}
}
