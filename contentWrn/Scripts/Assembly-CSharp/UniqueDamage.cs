using UnityEngine;

public class UniqueDamage
{
	public float damage;

	public int unqiueHash;

	private float time;

	public UniqueDamage(float damage, float time)
	{
		this.damage = damage;
		this.time = time;
		unqiueHash = damage.GetHashCode() + time.GetHashCode();
	}

	public UniqueDamage(float damage, int hash)
	{
		this.damage = damage;
		unqiueHash = hash;
		time = 0f;
	}

	public bool ShouldRemove()
	{
		return Time.time - time > 3f;
	}
}
