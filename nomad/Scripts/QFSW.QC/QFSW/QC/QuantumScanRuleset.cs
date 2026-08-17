using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QFSW.QC
{
	public class QuantumScanRuleset
	{
		private readonly IQcScanRule[] _scanRules;

		public QuantumScanRuleset(IEnumerable<IQcScanRule> scanRules)
		{
			_scanRules = scanRules.ToArray();
		}

		public QuantumScanRuleset()
			: this(new InjectionLoader<IQcScanRule>().GetInjectedInstances())
		{
		}

		public bool ShouldScan<T>(T entity) where T : ICustomAttributeProvider
		{
			bool result = true;
			IQcScanRule[] scanRules = _scanRules;
			for (int i = 0; i < scanRules.Length; i++)
			{
				switch (scanRules[i].ShouldScan(entity))
				{
				case ScanRuleResult.Reject:
					result = false;
					break;
				case ScanRuleResult.ForceAccept:
					return true;
				}
			}
			return result;
		}
	}
}
