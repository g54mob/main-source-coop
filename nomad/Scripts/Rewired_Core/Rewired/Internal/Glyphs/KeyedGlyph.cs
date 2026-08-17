using System;
using Rewired.Interfaces;

namespace Rewired.Internal.Glyphs
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class KeyedGlyph
	{
		public const uint INVALID_VERSION = 0u;

		private uint WevlQOuEnUoQBxRzwxMvjmlMsbUs;

		private uint eLLDPacdTkqkqiwVEsHTlBCaOfPSB;

		private object kycbwYNiNukRnGWnJhUKlanUEnqu;

		private bool wjLeNrltsoxALmighgClJnlWqPBIA;

		private string YePenSYVzIGxcoFTfFSpzNnEdZBU;

		public bool hasCachedValue => wjLeNrltsoxALmighgClJnlWqPBIA;

		public object cachedValue
		{
			get
			{
				return kycbwYNiNukRnGWnJhUKlanUEnqu;
			}
			set
			{
				wjLeNrltsoxALmighgClJnlWqPBIA = true;
				kycbwYNiNukRnGWnJhUKlanUEnqu = value;
				if (value == null)
				{
					YePenSYVzIGxcoFTfFSpzNnEdZBU = null;
				}
			}
		}

		public string cachedKey => YePenSYVzIGxcoFTfFSpzNnEdZBU;

		public KeyedGlyph()
		{
			WevlQOuEnUoQBxRzwxMvjmlMsbUs = 0u;
			eLLDPacdTkqkqiwVEsHTlBCaOfPSB = 0u;
		}

		public KeyedGlyph(KeyedGlyph P_0)
		{
			WevlQOuEnUoQBxRzwxMvjmlMsbUs = P_0.WevlQOuEnUoQBxRzwxMvjmlMsbUs;
			eLLDPacdTkqkqiwVEsHTlBCaOfPSB = P_0.eLLDPacdTkqkqiwVEsHTlBCaOfPSB;
			kycbwYNiNukRnGWnJhUKlanUEnqu = P_0.kycbwYNiNukRnGWnJhUKlanUEnqu;
			wjLeNrltsoxALmighgClJnlWqPBIA = P_0.wjLeNrltsoxALmighgClJnlWqPBIA;
			YePenSYVzIGxcoFTfFSpzNnEdZBU = P_0.YePenSYVzIGxcoFTfFSpzNnEdZBU;
		}

		public void Clear()
		{
			WevlQOuEnUoQBxRzwxMvjmlMsbUs = 0u;
			eLLDPacdTkqkqiwVEsHTlBCaOfPSB = 0u;
			kycbwYNiNukRnGWnJhUKlanUEnqu = null;
			wjLeNrltsoxALmighgClJnlWqPBIA = false;
			YePenSYVzIGxcoFTfFSpzNnEdZBU = null;
		}

		public bool TryGetValue(string key, IGlyphProvider glyphProvider, uint glyphProviderVersion, uint userVersion, out bool versionChanged, out object result)
		{
			versionChanged = WevlQOuEnUoQBxRzwxMvjmlMsbUs != ((glyphProvider != null) ? glyphProviderVersion : 0) || userVersion != eLLDPacdTkqkqiwVEsHTlBCaOfPSB;
			if (versionChanged)
			{
				Clear();
				WevlQOuEnUoQBxRzwxMvjmlMsbUs = glyphProviderVersion;
				eLLDPacdTkqkqiwVEsHTlBCaOfPSB = userVersion;
			}
			if (!versionChanged || glyphProvider == null)
			{
				result = (wjLeNrltsoxALmighgClJnlWqPBIA ? kycbwYNiNukRnGWnJhUKlanUEnqu : null);
				return wjLeNrltsoxALmighgClJnlWqPBIA;
			}
			if (string.IsNullOrEmpty(key))
			{
				result = null;
				return false;
			}
			try
			{
				wjLeNrltsoxALmighgClJnlWqPBIA = glyphProvider.TryGetGlyph(key, out kycbwYNiNukRnGWnJhUKlanUEnqu) && kycbwYNiNukRnGWnJhUKlanUEnqu != null;
				if (wjLeNrltsoxALmighgClJnlWqPBIA)
				{
					YePenSYVzIGxcoFTfFSpzNnEdZBU = key;
				}
			}
			catch (Exception exception)
			{
				ReInput.HandleExternalInterfaceException(typeof(IGlyphProvider).Name + ".TryGetGlyph", exception);
			}
			result = (wjLeNrltsoxALmighgClJnlWqPBIA ? kycbwYNiNukRnGWnJhUKlanUEnqu : null);
			return wjLeNrltsoxALmighgClJnlWqPBIA;
		}
	}
}
