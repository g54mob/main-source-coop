using System;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	public abstract class HardwareControllerTemplateMap : ScriptableObject
	{
		internal struct jiuvLTxcDzWSddWpdRJbLPDWISiIA
		{
			public int IhtpnYRlqMiPrMNxIAdtxyiCeyrh;

			public int mfSwFnJqaubVGckClgYswnorqksAb;

			public int bKiSaMoXBEBBrYElcYinrFOxWVty;

			public bool dJsxBLjaWpYIjFpaxNcnMfgakBeg;
		}

		public abstract Guid Guid { get; }

		public abstract string Key { get; }

		[CustomObfuscation(rename = false)]
		public abstract ControllerTemplateElementIdentifier GetElementIdentifier(int id);

		[CustomObfuscation(rename = false)]
		public abstract bool ContainsElementIdentifier(int id);
	}
}
