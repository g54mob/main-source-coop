namespace pworld.Scripts.Extensions
{
	public class Nullable<T>
	{
		private bool isNull = true;

		private T t;

		public bool HasValue => !isNull;

		public T Value
		{
			get
			{
				if (isNull)
				{
					return default(T);
				}
				return t;
			}
			set
			{
				t = value;
				isNull = false;
			}
		}

		public void FakeNullIt()
		{
			isNull = true;
		}
	}
}
