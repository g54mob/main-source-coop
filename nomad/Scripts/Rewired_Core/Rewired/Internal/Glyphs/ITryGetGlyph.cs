namespace Rewired.Internal.Glyphs
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal interface ITryGetGlyph
	{
		bool TryGetGlyph(out object value);
	}
}
