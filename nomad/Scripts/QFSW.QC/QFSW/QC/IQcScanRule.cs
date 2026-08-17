using System.Reflection;

namespace QFSW.QC
{
	public interface IQcScanRule
	{
		ScanRuleResult ShouldScan<T>(T entity) where T : ICustomAttributeProvider;
	}
}
