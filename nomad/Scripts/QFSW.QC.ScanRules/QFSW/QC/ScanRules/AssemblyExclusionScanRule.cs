using System.Reflection;

namespace QFSW.QC.ScanRules
{
	public class AssemblyExclusionScanRule : IQcScanRule
	{
		public ScanRuleResult ShouldScan<T>(T entity) where T : ICustomAttributeProvider
		{
			if (entity is Assembly assembly)
			{
				string[] array = new string[9] { "System", "Unity", "Microsoft", "Mono.", "mscorlib", "NSubstitute", "JetBrains", "nunit.", "GeNa." };
				string[] array2 = new string[3] { "mcs", "AssetStoreTools", "Facepunch.Steamworks" };
				string fullName = assembly.FullName;
				string[] array3 = array;
				foreach (string value in array3)
				{
					if (fullName.StartsWith(value))
					{
						return ScanRuleResult.Reject;
					}
				}
				string name = assembly.GetName().Name;
				array3 = array2;
				foreach (string text in array3)
				{
					if (name == text)
					{
						return ScanRuleResult.Reject;
					}
				}
			}
			return ScanRuleResult.Accept;
		}
	}
}
