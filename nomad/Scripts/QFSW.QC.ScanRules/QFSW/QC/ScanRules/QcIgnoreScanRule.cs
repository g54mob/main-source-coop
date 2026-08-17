using System.Reflection;
using System.Runtime.CompilerServices;
using QFSW.QC.Utilities;

namespace QFSW.QC.ScanRules
{
	public class QcIgnoreScanRule : IQcScanRule
	{
		public ScanRuleResult ShouldScan<T>(T entity) where T : ICustomAttributeProvider
		{
			if (entity.HasAttribute<QcIgnoreAttribute>(searchInherited: false))
			{
				return ScanRuleResult.Reject;
			}
			if (!(entity is MemberInfo) && entity.HasAttribute<CompilerGeneratedAttribute>())
			{
				return ScanRuleResult.Reject;
			}
			return ScanRuleResult.Accept;
		}
	}
}
