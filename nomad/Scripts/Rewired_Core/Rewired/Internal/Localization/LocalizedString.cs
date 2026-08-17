using System;
using Rewired.Interfaces;

namespace Rewired.Internal.Localization
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class LocalizedString
	{
		public const uint INVALID_VERSION = 0u;

		private uint NzfQvDgWDqXpLQYBMkfXvpnwZEXg;

		private uint CRyylMGjPerCwodnGLWLmNuCkoMD;

		private string tEtFztkJLfhhXhModcqsOwEwksZy;

		private bool IceHxgbygNNpjNLMCQQkaRHnjDHL;

		public bool hasCachedValue => IceHxgbygNNpjNLMCQQkaRHnjDHL;

		public string cachedValue
		{
			get
			{
				return tEtFztkJLfhhXhModcqsOwEwksZy;
			}
			set
			{
				IceHxgbygNNpjNLMCQQkaRHnjDHL = true;
				tEtFztkJLfhhXhModcqsOwEwksZy = value;
			}
		}

		public LocalizedString()
		{
			NzfQvDgWDqXpLQYBMkfXvpnwZEXg = 0u;
			CRyylMGjPerCwodnGLWLmNuCkoMD = 0u;
		}

		public LocalizedString(LocalizedString P_0)
		{
			NzfQvDgWDqXpLQYBMkfXvpnwZEXg = P_0.NzfQvDgWDqXpLQYBMkfXvpnwZEXg;
			CRyylMGjPerCwodnGLWLmNuCkoMD = P_0.CRyylMGjPerCwodnGLWLmNuCkoMD;
			tEtFztkJLfhhXhModcqsOwEwksZy = P_0.tEtFztkJLfhhXhModcqsOwEwksZy;
			IceHxgbygNNpjNLMCQQkaRHnjDHL = P_0.IceHxgbygNNpjNLMCQQkaRHnjDHL;
		}

		public void Clear()
		{
			NzfQvDgWDqXpLQYBMkfXvpnwZEXg = 0u;
			CRyylMGjPerCwodnGLWLmNuCkoMD = 0u;
			tEtFztkJLfhhXhModcqsOwEwksZy = null;
			IceHxgbygNNpjNLMCQQkaRHnjDHL = false;
		}

		public bool TryGetLocalizedValue(string key, ILocalizedStringProvider localizer, uint localizerVersion, uint userVersion, out bool versionChanged, out string result)
		{
			versionChanged = NzfQvDgWDqXpLQYBMkfXvpnwZEXg != ((localizer != null) ? localizerVersion : 0) || userVersion != CRyylMGjPerCwodnGLWLmNuCkoMD;
			if (versionChanged)
			{
				Clear();
				NzfQvDgWDqXpLQYBMkfXvpnwZEXg = localizerVersion;
				CRyylMGjPerCwodnGLWLmNuCkoMD = userVersion;
			}
			if (!versionChanged || localizer == null)
			{
				result = (IceHxgbygNNpjNLMCQQkaRHnjDHL ? tEtFztkJLfhhXhModcqsOwEwksZy : null);
				return IceHxgbygNNpjNLMCQQkaRHnjDHL;
			}
			if (string.IsNullOrEmpty(key))
			{
				result = null;
				return false;
			}
			try
			{
				IceHxgbygNNpjNLMCQQkaRHnjDHL = localizer.TryGetLocalizedString(key, out tEtFztkJLfhhXhModcqsOwEwksZy) && !string.IsNullOrEmpty(tEtFztkJLfhhXhModcqsOwEwksZy);
			}
			catch (Exception exception)
			{
				ReInput.HandleExternalInterfaceException(typeof(ILocalizedStringProvider).Name + ".TryGetLocalizedString", exception);
			}
			result = (IceHxgbygNNpjNLMCQQkaRHnjDHL ? tEtFztkJLfhhXhModcqsOwEwksZy : null);
			return IceHxgbygNNpjNLMCQQkaRHnjDHL;
		}
	}
}
