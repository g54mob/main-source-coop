using System;
using System.Collections.Generic;
using Rewired.Utils.Classes.Data;

namespace Rewired.Platforms.Custom
{
	public abstract class CustomPlatformUnifiedKeyboardSource : CustomPlatformUnifiedControllerSource
	{
		public sealed class KeyPropertyMap
		{
			public struct Key
			{
				public KeyboardKeyCode keyCode;

				public string label;
			}

			private IndexedDictionary<int, string> TqDSuhvlrPKTVIrdHnntrUWrKgzd;

			private bool OmPpOeeBTezoRjQDilpyKkYQOyhw;

			internal bool miAMjoswAFGAulHJCVCopTFBTxcK
			{
				get
				{
					return OmPpOeeBTezoRjQDilpyKkYQOyhw;
				}
				set
				{
					OmPpOeeBTezoRjQDilpyKkYQOyhw = omPpOeeBTezoRjQDilpyKkYQOyhw;
				}
			}

			public KeyPropertyMap()
			{
				TqDSuhvlrPKTVIrdHnntrUWrKgzd = new IndexedDictionary<int, string>();
				IList<int> keyboardKeyValues = Consts.keyboardKeyValues;
				IList<string> keyboardKeyNames = Consts.keyboardKeyNames;
				for (int i = 0; i < 132; i++)
				{
					TqDSuhvlrPKTVIrdHnntrUWrKgzd.Add(keyboardKeyValues[i], keyboardKeyNames[i]);
				}
				OmPpOeeBTezoRjQDilpyKkYQOyhw = true;
			}

			public KeyPropertyMap(KeyPropertyMap P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("other");
				}
				TqDSuhvlrPKTVIrdHnntrUWrKgzd = new IndexedDictionary<int, string>(P_0.TqDSuhvlrPKTVIrdHnntrUWrKgzd);
				OmPpOeeBTezoRjQDilpyKkYQOyhw = true;
			}

			public Key Get(KeyboardKeyCode keyCode)
			{
				if (!TqDSuhvlrPKTVIrdHnntrUWrKgzd.TryGetValue((int)keyCode, out var value))
				{
					return default(Key);
				}
				return new Key
				{
					keyCode = keyCode,
					label = value
				};
			}

			public void Set(Key key)
			{
				TqDSuhvlrPKTVIrdHnntrUWrKgzd.SetValue((int)key.keyCode, key.label);
				OmPpOeeBTezoRjQDilpyKkYQOyhw = true;
			}

			public Key[] Get()
			{
				Key[] array = new Key[TqDSuhvlrPKTVIrdHnntrUWrKgzd.Count];
				int count = TqDSuhvlrPKTVIrdHnntrUWrKgzd.Count;
				for (int i = 0; i < count; i++)
				{
					array[i] = new Key
					{
						keyCode = (KeyboardKeyCode)TqDSuhvlrPKTVIrdHnntrUWrKgzd.GetKeyAt(i),
						label = TqDSuhvlrPKTVIrdHnntrUWrKgzd[i]
					};
				}
				return array;
			}

			public void Set(ICollection<Key> keys)
			{
				if (keys == null)
				{
					throw new ArgumentNullException("keys");
				}
				foreach (Key key in keys)
				{
					TqDSuhvlrPKTVIrdHnntrUWrKgzd.SetValue((int)key.keyCode, key.label);
				}
				OmPpOeeBTezoRjQDilpyKkYQOyhw = true;
			}
		}

		private KeyPropertyMap CVRHiYVoGMklebtRnCcOXExXkbvg;

		public KeyPropertyMap keyPropertyMap
		{
			get
			{
				if (CVRHiYVoGMklebtRnCcOXExXkbvg == null)
				{
					CVRHiYVoGMklebtRnCcOXExXkbvg = new KeyPropertyMap();
				}
				return CVRHiYVoGMklebtRnCcOXExXkbvg;
			}
			set
			{
				if (value == null)
				{
					value = new KeyPropertyMap();
				}
				CVRHiYVoGMklebtRnCcOXExXkbvg = value;
				DmSBiCoCWjcrRHnvtGLtmRAHKMMEA();
			}
		}

		public CustomPlatformUnifiedKeyboardSource()
			: base(0, Consts._keyboardKeyValues.Length)
		{
		}

		protected void SetKeyValue(KeyboardKeyCode keyCode, bool value)
		{
			int buttonIndex = ReInput.controllers.Keyboard.GetButtonIndex(keyCode);
			if (buttonIndex >= 0)
			{
				SetButtonValue(buttonIndex, value);
			}
		}

		internal virtual void NuTJXpaIwxkBftjlLBcRGJyFcGsKA()
		{
			base.teTpmqVqEpUVjdZLwlCvmChBpAaf();
			if (CVRHiYVoGMklebtRnCcOXExXkbvg != null && CVRHiYVoGMklebtRnCcOXExXkbvg.miAMjoswAFGAulHJCVCopTFBTxcK)
			{
				DmSBiCoCWjcrRHnvtGLtmRAHKMMEA();
			}
		}

		private void DmSBiCoCWjcrRHnvtGLtmRAHKMMEA()
		{
			HardwareControllerMap_Game uNRIOyvPojfCPrjRsEYcHBwwkZqS = ReInput.controllers.Keyboard.UNRIOyvPojfCPrjRsEYcHBwwkZqS;
			int totalCount = uNRIOyvPojfCPrjRsEYcHBwwkZqS.elementIdentifiers.TotalCount;
			for (int i = 0; i < totalCount; i++)
			{
				if (uNRIOyvPojfCPrjRsEYcHBwwkZqS.elementIdentifiers.TryGetValueAt(i, out var value))
				{
					KeyboardKeyCode keyCode = (KeyboardKeyCode)Consts.keyboardKeyValues[value.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid];
					string label = CVRHiYVoGMklebtRnCcOXExXkbvg.Get(keyCode).label;
					if (!string.Equals(value.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename, label))
					{
						value.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename = label;
					}
				}
			}
			CVRHiYVoGMklebtRnCcOXExXkbvg.miAMjoswAFGAulHJCVCopTFBTxcK = false;
		}
	}
}
