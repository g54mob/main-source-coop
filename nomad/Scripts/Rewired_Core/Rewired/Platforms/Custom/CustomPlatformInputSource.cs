using System;
using System.Collections.Generic;

namespace Rewired.Platforms.Custom
{
	public abstract class CustomPlatformInputSource : CustomInputSource
	{
		public new abstract class Joystick : CustomInputSource.Joystick
		{
			protected Joystick(string P_0, long P_1, int P_2, int P_3)
				: base(P_0, P_1, P_2, P_3)
			{
				_isConnected = true;
			}
		}

		public sealed class InitOptions
		{
			public CustomPlatformUnifiedKeyboardSource unifiedKeyboardSource;

			public CustomPlatformUnifiedMouseSource unifiedMouseSource;
		}

		private readonly CustomPlatformConfigVars FqqJaWbFCXsqiVpaZmtQQghUiMeb;

		private readonly bool sjLRaCxVaXdxorIILrVBklWYXpzD;

		private readonly bool QPteCHXrLdemCbmeAQnhUphDFxlkA;

		private bool xIZbUabqctztLoMETitEeNhYgZfp;

		protected CustomPlatformInputSource(CustomPlatformConfigVars P_0, InitOptions P_1)
			: base(100, (P_1 != null && P_1.unifiedKeyboardSource != null && P_0.useNativeKeyboard) ? new ciERiDAEWPhXXWpMLnWLAnQKhQHu(P_1.unifiedKeyboardSource) : null, (P_1 != null && P_1.unifiedMouseSource != null && P_0.useNativeMouse) ? new NjHPSkAaaQsjZmiwmamffZbpVuDA(P_1.unifiedMouseSource) : null)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("configVars");
			}
			FqqJaWbFCXsqiVpaZmtQQghUiMeb = P_0;
			if (P_1 == null || P_1.unifiedKeyboardSource == null)
			{
				P_0.useNativeKeyboard = false;
			}
			if (P_1 == null || P_1.unifiedMouseSource == null)
			{
				P_0.useNativeMouse = false;
			}
			sjLRaCxVaXdxorIILrVBklWYXpzD = P_0.useNativeKeyboard;
			QPteCHXrLdemCbmeAQnhUphDFxlkA = P_0.useNativeMouse;
		}

		internal virtual void MZEpgRNSQeUBWtGbGHhZvfwUemck()
		{
			base.BrcOHbBoQMdzRAEoYlowdclvaIsvA();
			if (sjLRaCxVaXdxorIILrVBklWYXpzD && CgmdcXLZYwNvCEAMqCnUidmgmAhq() is DnBSVBokqhfUxYQWpdpecLUsgVJu)
			{
				(CgmdcXLZYwNvCEAMqCnUidmgmAhq() as DnBSVBokqhfUxYQWpdpecLUsgVJu).rXYxKYFdGeHSghPTzTsaFHJPaVXcb();
			}
			if (QPteCHXrLdemCbmeAQnhUphDFxlkA && HcrPovgnNSheDcZTplDJNufqleGA() is DnBSVBokqhfUxYQWpdpecLUsgVJu)
			{
				(HcrPovgnNSheDcZTplDJNufqleGA() as DnBSVBokqhfUxYQWpdpecLUsgVJu).rXYxKYFdGeHSghPTzTsaFHJPaVXcb();
			}
		}

		internal virtual void amJJGqEsakKupgHjoyMETzDXRfqS()
		{
			base.nmiHBHNfCudAZvaUpaMnJnprHnMy();
			IList<CustomInputSource.Joystick> joysticks = GetJoysticks();
			int count = joysticks.Count;
			for (int i = 0; i < count; i++)
			{
				joysticks[i].qWsfrgdHVSOtLdhBEgSUxhNFQhKA();
				joysticks[i].Update();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!xIZbUabqctztLoMETitEeNhYgZfp)
			{
				xIZbUabqctztLoMETitEeNhYgZfp = true;
				base.Dispose(disposing);
			}
		}
	}
}
