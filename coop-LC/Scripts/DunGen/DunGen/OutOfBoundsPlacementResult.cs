namespace DunGen
{
	public sealed class OutOfBoundsPlacementResult : TileTemplatePlacementResult
	{
		public override string DisplayName => "Out-of-Bounds";

		public OutOfBoundsPlacementResult(TileProxy tileTemplate)
			: base(tileTemplate)
		{
		}
	}
}
