using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class CZntoWJJsAvVgoxFAlonSDYUcvaF
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private unsafe delegate int zsEDnTGtqJBqYLtfUoqITVPPnWcj(void* deviceInstance, IntPtr data);

	private readonly IntPtr rOPcNAsgPMjuQIZrrouKkGtiPfSR;

	private readonly zsEDnTGtqJBqYLtfUoqITVPPnWcj tgDRFsxXeUUUllGmqgwOsBuSZgNt;

	[CompilerGenerated]
	private List<AZmRVMBCLWjdSJdmldWMNaRORDDE> IJwWStTDeaZPjVazrMlmdIMwAZHM;

	public IntPtr EVChJggwDRKnNoJaTIxuCTdiquTsb => rOPcNAsgPMjuQIZrrouKkGtiPfSR;

	public List<AZmRVMBCLWjdSJdmldWMNaRORDDE> kGXvsLvrBQUupEukadEIrnDTZZkO
	{
		[CompilerGenerated]
		get
		{
			return IJwWStTDeaZPjVazrMlmdIMwAZHM;
		}
		[CompilerGenerated]
		private set
		{
			IJwWStTDeaZPjVazrMlmdIMwAZHM = iJwWStTDeaZPjVazrMlmdIMwAZHM;
		}
	}

	public unsafe CZntoWJJsAvVgoxFAlonSDYUcvaF()
	{
		tgDRFsxXeUUUllGmqgwOsBuSZgNt = TWdSCTyvvIclBJOEZjSfRroQyWuA;
		rOPcNAsgPMjuQIZrrouKkGtiPfSR = Marshal.GetFunctionPointerForDelegate(tgDRFsxXeUUUllGmqgwOsBuSZgNt);
		kGXvsLvrBQUupEukadEIrnDTZZkO = new List<AZmRVMBCLWjdSJdmldWMNaRORDDE>();
	}

	[MonoPInvokeCallback(typeof(zsEDnTGtqJBqYLtfUoqITVPPnWcj))]
	private unsafe static int TWdSCTyvvIclBJOEZjSfRroQyWuA(void* P_0, IntPtr P_1)
	{
		uint instanceId = (uint)P_1.ToInt32();
		if (!ObjectInstanceTracker.Default.TryGetInstance<CZntoWJJsAvVgoxFAlonSDYUcvaF>(instanceId, out var instance))
		{
			return 1;
		}
		AZmRVMBCLWjdSJdmldWMNaRORDDE aZmRVMBCLWjdSJdmldWMNaRORDDE = new AZmRVMBCLWjdSJdmldWMNaRORDDE();
		aZmRVMBCLWjdSJdmldWMNaRORDDE.MIBVslPnEtGuXCdIkqDtzZrOfNXv(ref *(AZmRVMBCLWjdSJdmldWMNaRORDDE.VOAzpIRZQzmCfUpaxcBCJkEVRioKA*)P_0);
		instance.kGXvsLvrBQUupEukadEIrnDTZZkO.Add(aZmRVMBCLWjdSJdmldWMNaRORDDE);
		return 1;
	}
}
