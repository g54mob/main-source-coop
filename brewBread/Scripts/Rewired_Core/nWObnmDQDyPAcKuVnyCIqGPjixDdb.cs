using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired;
using Rewired.Utils.Classes.Data;

internal class nWObnmDQDyPAcKuVnyCIqGPjixDdb
{
	private class GzfBWedZfWsnzoMJEZxUuNsioVAcb
	{
		public readonly InputAction uZtLhlDCJMzQQLBWRnENPjclyYIb;

		public readonly int QSZlWSsqUhpIVZjOleCIDuxHVUuQ;

		public readonly int mUeXAPkBeXCWYVMAScGMEviKqIoLA;

		public GzfBWedZfWsnzoMJEZxUuNsioVAcb(InputAction P_0, int P_1)
		{
			uZtLhlDCJMzQQLBWRnENPjclyYIb = P_0;
			QSZlWSsqUhpIVZjOleCIDuxHVUuQ = P_0.id;
			mUeXAPkBeXCWYVMAScGMEviKqIoLA = P_1;
		}
	}

	private InputAction[] kwqvcjKabUZoOqJswYiJZifnKRub;

	private ADictionary<string, GzfBWedZfWsnzoMJEZxUuNsioVAcb> PTExjiBygjfWcraeQidSjQqpMAohA;

	private GzfBWedZfWsnzoMJEZxUuNsioVAcb[] FbBbEezBXHSbesaFYkexqfqynWrQ;

	private ReadOnlyCollection<InputAction> TRuBHpDkIIeVQAFPGfuhhFfZtARN;

	private int rVUwZcSwJYddkQFBHhyviXLcZbdE;

	private int FfCZbcwKEEGYFFSiSoMXrKxhaOMR;

	private List<string> bonHAwIQzcJFRlLvGSWGSuMyNYUU;

	private List<int> mdKlJgHoUlIIvUAIprbSCmcbZGSW;

	public IList<InputAction> pKMZFxQiAHeZkCfHZqCmwzClFZlQ => TRuBHpDkIIeVQAFPGfuhhFfZtARN;

	public int aLLlpNUjcfjiKpSIORbqbGLhIBol => rVUwZcSwJYddkQFBHhyviXLcZbdE;

	public int cuIqMUSdmKIejUTXChjhBrEkVSBe => FfCZbcwKEEGYFFSiSoMXrKxhaOMR;

	public nWObnmDQDyPAcKuVnyCIqGPjixDdb(List<InputAction> P_0)
	{
		bonHAwIQzcJFRlLvGSWGSuMyNYUU = new List<string>();
		mdKlJgHoUlIIvUAIprbSCmcbZGSW = new List<int>();
		kwqvcjKabUZoOqJswYiJZifnKRub = P_0.ToArray();
		rVUwZcSwJYddkQFBHhyviXLcZbdE = kwqvcjKabUZoOqJswYiJZifnKRub.Length;
		int num = -1;
		for (int i = 0; i < rVUwZcSwJYddkQFBHhyviXLcZbdE; i++)
		{
			int id = kwqvcjKabUZoOqJswYiJZifnKRub[i].id;
			if (id > num)
			{
				num = id;
			}
		}
		FfCZbcwKEEGYFFSiSoMXrKxhaOMR = num;
		FbBbEezBXHSbesaFYkexqfqynWrQ = new GzfBWedZfWsnzoMJEZxUuNsioVAcb[num + 1];
		for (int j = 0; j < rVUwZcSwJYddkQFBHhyviXLcZbdE; j++)
		{
			InputAction inputAction = kwqvcjKabUZoOqJswYiJZifnKRub[j];
			FbBbEezBXHSbesaFYkexqfqynWrQ[inputAction.id] = new GzfBWedZfWsnzoMJEZxUuNsioVAcb(inputAction, j);
		}
		PTExjiBygjfWcraeQidSjQqpMAohA = new ADictionary<string, GzfBWedZfWsnzoMJEZxUuNsioVAcb>(rVUwZcSwJYddkQFBHhyviXLcZbdE, StringComparer.OrdinalIgnoreCase);
		for (int k = 0; k < rVUwZcSwJYddkQFBHhyviXLcZbdE; k++)
		{
			InputAction inputAction2 = kwqvcjKabUZoOqJswYiJZifnKRub[k];
			try
			{
				PTExjiBygjfWcraeQidSjQqpMAohA.Add(inputAction2.name, FbBbEezBXHSbesaFYkexqfqynWrQ[inputAction2.id]);
			}
			catch
			{
				Logger.LogError("Duplicate Action name \"" + inputAction2.name + "\" found in Action list. Duplicate Action names are not allowed. If you have edited the data manually outside the Rewired Input Manager, remove any duplicate Actions.");
			}
		}
		TRuBHpDkIIeVQAFPGfuhhFfZtARN = new ReadOnlyCollection<InputAction>(kwqvcjKabUZoOqJswYiJZifnKRub);
	}

	public InputAction cJlaptDDgHYEZdKZMGkPWYdKckQLA(string P_0, bool P_1 = false)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return null;
		}
		if (!PTExjiBygjfWcraeQidSjQqpMAohA.TryGetValue(P_0, out var value))
		{
			if (P_1)
			{
				gsdCCLMHzhfbrNswvXhEIpMUGnIx(P_0);
			}
			return null;
		}
		return value.uZtLhlDCJMzQQLBWRnENPjclyYIb;
	}

	public InputAction SbJXHzMCJxVIoMZqRpTsxBZFrfWP(int P_0)
	{
		if (P_0 < 0)
		{
			return null;
		}
		if (P_0 > FfCZbcwKEEGYFFSiSoMXrKxhaOMR)
		{
			return null;
		}
		if (FbBbEezBXHSbesaFYkexqfqynWrQ[P_0] == null)
		{
			return null;
		}
		return FbBbEezBXHSbesaFYkexqfqynWrQ[P_0].uZtLhlDCJMzQQLBWRnENPjclyYIb;
	}

	public InputAction qGTnUvTooyDyvIZphuDcznGqwKrR(int P_0)
	{
		if (P_0 < 0 || P_0 >= rVUwZcSwJYddkQFBHhyviXLcZbdE)
		{
			return null;
		}
		return kwqvcjKabUZoOqJswYiJZifnKRub[P_0];
	}

	public int lsWdiPZgHHfEfIiesCpnLAlcpBgUA(string P_0, bool P_1 = false)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return -1;
		}
		if (!PTExjiBygjfWcraeQidSjQqpMAohA.TryGetValue(P_0, out var value))
		{
			if (P_1)
			{
				gsdCCLMHzhfbrNswvXhEIpMUGnIx(P_0);
			}
			return -1;
		}
		return value.mUeXAPkBeXCWYVMAScGMEviKqIoLA;
	}

	public int lsWdiPZgHHfEfIiesCpnLAlcpBgUA(int P_0, bool P_1 = false)
	{
		if (P_0 < 0 || P_0 > FfCZbcwKEEGYFFSiSoMXrKxhaOMR)
		{
			if (P_0 >= 0 && P_1)
			{
				gsdCCLMHzhfbrNswvXhEIpMUGnIx(P_0);
			}
			return -1;
		}
		GzfBWedZfWsnzoMJEZxUuNsioVAcb gzfBWedZfWsnzoMJEZxUuNsioVAcb = FbBbEezBXHSbesaFYkexqfqynWrQ[P_0];
		if (gzfBWedZfWsnzoMJEZxUuNsioVAcb == null)
		{
			if (P_1)
			{
				gsdCCLMHzhfbrNswvXhEIpMUGnIx(P_0);
			}
			return -1;
		}
		return gzfBWedZfWsnzoMJEZxUuNsioVAcb.mUeXAPkBeXCWYVMAScGMEviKqIoLA;
	}

	public bool fSBMVaLfQvgqXypKYGeLWMIbARbB(string P_0, bool P_1 = false)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return false;
		}
		if (!PTExjiBygjfWcraeQidSjQqpMAohA.ContainsKey(P_0))
		{
			if (P_1)
			{
				gsdCCLMHzhfbrNswvXhEIpMUGnIx(P_0);
			}
			return false;
		}
		return true;
	}

	public bool fSBMVaLfQvgqXypKYGeLWMIbARbB(int P_0)
	{
		if (P_0 < 0 || P_0 > FfCZbcwKEEGYFFSiSoMXrKxhaOMR)
		{
			return false;
		}
		return FbBbEezBXHSbesaFYkexqfqynWrQ[P_0] != null;
	}

	public int hzfLpbQtxVGSCsRNNcbtAzJaaTLQA(string P_0, bool P_1 = false)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			return -1;
		}
		if (!PTExjiBygjfWcraeQidSjQqpMAohA.TryGetValue(P_0, out var value))
		{
			if (P_1)
			{
				gsdCCLMHzhfbrNswvXhEIpMUGnIx(P_0);
			}
			return -1;
		}
		return value.QSZlWSsqUhpIVZjOleCIDuxHVUuQ;
	}

	private void gsdCCLMHzhfbrNswvXhEIpMUGnIx(string P_0)
	{
		if (!bonHAwIQzcJFRlLvGSWGSuMyNYUU.Contains(P_0))
		{
			bonHAwIQzcJFRlLvGSWGSuMyNYUU.Add(P_0);
			Logger.LogWarning("The Action \"" + P_0 + "\" does not exist. You can create Actions in the editor.");
		}
	}

	private void gsdCCLMHzhfbrNswvXhEIpMUGnIx(int P_0)
	{
		if (!mdKlJgHoUlIIvUAIprbSCmcbZGSW.Contains(P_0))
		{
			mdKlJgHoUlIIvUAIprbSCmcbZGSW.Add(P_0);
			Logger.LogWarning("No Action exists for Action Id " + P_0 + ". You can create Actions in the editor.");
		}
	}
}
