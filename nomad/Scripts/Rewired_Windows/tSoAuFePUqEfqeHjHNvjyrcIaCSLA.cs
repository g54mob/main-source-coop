using System.Collections.Generic;
using Rewired.Utils;

internal class tSoAuFePUqEfqeHjHNvjyrcIaCSLA : NOJZCogaHTLiuIrHVflAYfKhuPdJ
{
	private List<MgksRcfrCFgwczVjSvhFamUZHzXt> BwhmTfNUWRFgMiAORGsrmEXkmBno;

	private MgksRcfrCFgwczVjSvhFamUZHzXt[] rRhULnViFKyImPIMhTyIxeFqUAFq;

	private bool nvJPJZpKdXeVCQVxUQIIzoIskUMl;

	public tSoAuFePUqEfqeHjHNvjyrcIaCSLA()
	{
		BwhmTfNUWRFgMiAORGsrmEXkmBno = new List<MgksRcfrCFgwczVjSvhFamUZHzXt>();
	}

	public virtual void VjbNEIYvFVVavSGRbtLEXFyfUXtm(MgksRcfrCFgwczVjSvhFamUZHzXt P_0)
	{
		BwhmTfNUWRFgMiAORGsrmEXkmBno.Add(P_0);
	}

	public float IcMCQfMtfMCEJnpccEDXKUPAkKMM(int P_0)
	{
		if (P_0 < 0 || P_0 >= rRhULnViFKyImPIMhTyIxeFqUAFq.Length)
		{
			return 0f;
		}
		return hlmzMEGGifaxtHzywCLyBJvQeLcDA(rRhULnViFKyImPIMhTyIxeFqUAFq[P_0].haJuScBkUAteDWgDvyZABWCLIREh);
	}

	public int zeakccAttMgLEGhFtxFxhozCFmsNA(int P_0)
	{
		if (P_0 < 0 || P_0 >= rRhULnViFKyImPIMhTyIxeFqUAFq.Length)
		{
			return 0;
		}
		return (int)rRhULnViFKyImPIMhTyIxeFqUAFq[P_0].gKYavNiuTEHqRomqtnSIwjisgjEqA;
	}

	public virtual void UvhPMRZbvKdOrKKQPjKyPTaiogtj()
	{
		if (!nvJPJZpKdXeVCQVxUQIIzoIskUMl)
		{
			nvJPJZpKdXeVCQVxUQIIzoIskUMl = true;
			rRhULnViFKyImPIMhTyIxeFqUAFq = BwhmTfNUWRFgMiAORGsrmEXkmBno.ToArray();
			BwhmTfNUWRFgMiAORGsrmEXkmBno = null;
		}
	}

	private static float hlmzMEGGifaxtHzywCLyBJvQeLcDA(int P_0)
	{
		if (P_0 == 0)
		{
			return 0f;
		}
		return MathTools.Clamp((float)MathTools.Abs(P_0) / 65535f * (float)MathTools.Sign(P_0), -1f, 1f);
	}
}
