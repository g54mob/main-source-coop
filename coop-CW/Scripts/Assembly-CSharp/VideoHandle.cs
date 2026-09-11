using System;

public struct VideoHandle : IComparable<VideoHandle>, IEquatable<VideoHandle>
{
	public Guid id;

	public static VideoHandle Invalid => new VideoHandle(Guid.Empty);

	public VideoHandle(Guid id)
	{
		this.id = id;
	}

	public int CompareTo(VideoHandle other)
	{
		return id.CompareTo(other.id);
	}

	public bool Equals(VideoHandle other)
	{
		return id.Equals(other.id);
	}

	public override bool Equals(object obj)
	{
		if (obj is VideoHandle other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return id.GetHashCode();
	}

	public override string ToString()
	{
		return id.ToString();
	}
}
