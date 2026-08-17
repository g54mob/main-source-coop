using System;

namespace Technie.PhysicsCreator.QHull
{
	public class CoplanarException : Exception
	{
		public CoplanarException(string msg)
			: base(msg)
		{
		}
	}
}
