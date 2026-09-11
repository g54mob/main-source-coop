using System;
using DefaultNamespace;

public struct LostFootageHandle : IComparable<LostFootageHandle>, IEquatable<LostFootageHandle>, IPlayableVideo
{
	public int index;

	public LostFootageHandle(int index)
	{
		this.index = index;
	}

	public int CompareTo(LostFootageHandle other)
	{
		return index.CompareTo(other.index);
	}

	public bool Equals(LostFootageHandle other)
	{
		return index.Equals(other.index);
	}

	public override bool Equals(object obj)
	{
		if (obj is LostFootageHandle other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return index.GetHashCode();
	}

	public override string ToString()
	{
		return index.ToString();
	}

	public bool TryGetVideoPath(out string path)
	{
		if (LostFootageDatabase.TryGetLostFootage(this, out var footage))
		{
			path = footage.fileInfo.FullName;
			return true;
		}
		path = null;
		return false;
	}
}
