using System.Globalization;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 4)]
internal struct dloYVzovOhpJTInWpsfDaglnDFcV
{
	private int XgLnpXEYlnoLgFBgGdRcxkYUXpuy;

	public ZmXltWbhqdfqHdQeeybxZILIjOaj FISdKDwGlUgdCydNnzZTSNmsChCo => (ZmXltWbhqdfqHdQeeybxZILIjOaj)(XgLnpXEYlnoLgFBgGdRcxkYUXpuy & -16776961);

	public int PDtLxAPTSYnLgAoFqhidKJthcbKfA => (XgLnpXEYlnoLgFBgGdRcxkYUXpuy >> 8) & 0xFFFF;

	public bool PCzBmgdfquvLDqTxmkSugfJGksqbB(dloYVzovOhpJTInWpsfDaglnDFcV P_0)
	{
		return P_0.XgLnpXEYlnoLgFBgGdRcxkYUXpuy == XgLnpXEYlnoLgFBgGdRcxkYUXpuy;
	}

	public bool VdXOEfWitllQazXjmNCEhoOejWrg(object P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (P_0.GetType() != typeof(dloYVzovOhpJTInWpsfDaglnDFcV))
		{
			return false;
		}
		return PCzBmgdfquvLDqTxmkSugfJGksqbB((dloYVzovOhpJTInWpsfDaglnDFcV)P_0);
	}

	public int RQnwRaaeXVwjvjmErMtrGZxrJzHp()
	{
		return XgLnpXEYlnoLgFBgGdRcxkYUXpuy;
	}

	public string RhIlcAeHxGKYkwxnwYmQoJOwRtSh()
	{
		return string.Format(CultureInfo.InvariantCulture, "Flags: {0} InstanceNumber: {1} RawId: 0x{2:X8}", FISdKDwGlUgdCydNnzZTSNmsChCo, PDtLxAPTSYnLgAoFqhidKJthcbKfA, XgLnpXEYlnoLgFBgGdRcxkYUXpuy);
	}
}
