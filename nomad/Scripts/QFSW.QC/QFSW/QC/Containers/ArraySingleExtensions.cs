namespace QFSW.QC.Containers
{
	public static class ArraySingleExtensions
	{
		public static ArraySingle<T> AsArraySingle<T>(this T data)
		{
			return new ArraySingle<T>(data);
		}
	}
}
