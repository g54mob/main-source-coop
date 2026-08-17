namespace Rewired
{
	public sealed class KeyboardMapSaveData : ControllerMapSaveData
	{
		public KeyboardMap keyboardMap
		{
			get
			{
				if (ReInput._id != PbuCWLhPESGXtsXepbdkNpjZHdmp)
				{
					ReInput.CheckInitialized(PbuCWLhPESGXtsXepbdkNpjZHdmp);
					return null;
				}
				return (KeyboardMap)_map;
			}
		}

		internal KeyboardMapSaveData(Keyboard P_0, KeyboardMap P_1)
			: base(P_0, P_1)
		{
		}
	}
}
