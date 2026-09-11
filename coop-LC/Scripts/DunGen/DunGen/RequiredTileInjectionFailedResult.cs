namespace DunGen
{
	public sealed class RequiredTileInjectionFailedResult : TilePlacementResult
	{
		public override string DisplayName => "Tile Injection Failed";

		public TileSet InjectedTileSet { get; private set; }

		public RequiredTileInjectionFailedResult(TileSet injectedTileSet)
		{
			InjectedTileSet = injectedTileSet;
		}
	}
}
