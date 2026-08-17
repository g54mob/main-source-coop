using System;

namespace Drawing
{
	public interface IDrawGizmos
	{
		bool Exists
		{
			get
			{
				throw new NotImplementedException("This method should be overridden in the implementing class, unless it inherits from MonoBehaviour");
			}
		}

		void DrawGizmos();
	}
}
