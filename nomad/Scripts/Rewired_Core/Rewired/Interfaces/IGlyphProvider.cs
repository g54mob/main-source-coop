namespace Rewired.Interfaces
{
	public interface IGlyphProvider
	{
		bool TryGetGlyph(string key, out object result);
	}
}
