using System.Text;

namespace QFSW.QC.Pooling
{
	public class ConcurrentStringBuilderPool : StringBuilderPool<ConcurrentPool<StringBuilder>>
	{
	}
}
