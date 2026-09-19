namespace Obi
{
	public class ObiResourceHandle<T> where T : class
	{
		public T owner;

		public int index = -1;

		private int referenceCount;

		public bool isValid => index >= 0;

		public void Invalidate()
		{
			index = -1;
			referenceCount = 0;
		}

		public void Reference()
		{
			referenceCount++;
		}

		public bool Dereference()
		{
			return --referenceCount == 0;
		}

		public ObiResourceHandle(int index = -1)
		{
			this.index = index;
			owner = null;
		}
	}
}
