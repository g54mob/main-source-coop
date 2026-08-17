namespace PlayEveryWare.Common
{
	public abstract class Wrapped<T> where T : struct
	{
		protected T _value;

		public T Unwrap()
		{
			return _value;
		}
	}
}
