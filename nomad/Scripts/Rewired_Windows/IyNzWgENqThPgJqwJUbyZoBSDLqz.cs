using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 4)]
internal struct IyNzWgENqThPgJqwJUbyZoBSDLqz : IEquatable<IyNzWgENqThPgJqwJUbyZoBSDLqz>
{
	private int eLQFIuOQewkCNrPxzXYVYtQgMNhv;

	public bool Equals(IyNzWgENqThPgJqwJUbyZoBSDLqz other)
	{
		return eLQFIuOQewkCNrPxzXYVYtQgMNhv == other.eLQFIuOQewkCNrPxzXYVYtQgMNhv;
	}

	bool IEquatable<IyNzWgENqThPgJqwJUbyZoBSDLqz>.Equals(IyNzWgENqThPgJqwJUbyZoBSDLqz other)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Equals
		return this.Equals(other);
	}

	public bool sXCvFoqOatMpyVEcqIvmYFyZZpEp(object P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (P_0 is IyNzWgENqThPgJqwJUbyZoBSDLqz)
		{
			return Equals((IyNzWgENqThPgJqwJUbyZoBSDLqz)P_0);
		}
		return false;
	}

	public int XuvgVQbSARtllkdoRcnTaNvituFkB()
	{
		return eLQFIuOQewkCNrPxzXYVYtQgMNhv;
	}

	[SpecialName]
	public static bool jJSETxscYMZtZlfXLHmsaXFsRvOk(IyNzWgENqThPgJqwJUbyZoBSDLqz P_0)
	{
		return P_0.eLQFIuOQewkCNrPxzXYVYtQgMNhv != 0;
	}

	public string jPAyRwDhYwTBtsYslETEWUoXJEFc()
	{
		return $"{eLQFIuOQewkCNrPxzXYVYtQgMNhv != 0}";
	}
}
