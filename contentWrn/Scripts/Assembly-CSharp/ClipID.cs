using System;

public struct ClipID : IComparable<ClipID>, IEquatable<ClipID>
{
	public Guid id;

	public ClipID(Guid id)
	{
		this.id = id;
	}

	public int CompareTo(ClipID other)
	{
		return id.CompareTo(other.id);
	}

	public bool Equals(ClipID other)
	{
		return id.Equals(other.id);
	}

	public override string ToString()
	{
		return id.ToString();
	}

	public string ToShortString()
	{
		return id.ToString().Substring(0, 8);
	}

	public string ToMiniString()
	{
		return id.ToString().Substring(0, 3);
	}
}
