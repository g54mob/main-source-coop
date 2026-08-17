using System;

namespace VContainer.Internal
{
	internal class BuilderCallbackDisposable : IDisposable
	{
		private readonly IObjectResolver container;

		public event Action<IObjectResolver> Disposing;

		public BuilderCallbackDisposable(IObjectResolver container)
		{
			this.container = container;
		}

		public void Dispose()
		{
			if (this.Disposing != null)
			{
				this.Disposing(container);
			}
		}
	}
}
