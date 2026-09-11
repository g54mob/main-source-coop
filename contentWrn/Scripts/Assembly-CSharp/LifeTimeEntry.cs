using Unity.Mathematics;
using UnityEngine;
using Zorro.Core.Serizalization;

public class LifeTimeEntry : ItemDataEntry, IHaveUIData
{
	public float m_lifeTimeLeft;

	public float m_maxLifeTime;

	private string m_info;

	private int m_percentage;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteFloat(m_lifeTimeLeft);
		binarySerializer.WriteFloat(m_maxLifeTime);
	}

	public void AddLifeTime(float lifeTimeToAdd)
	{
		m_lifeTimeLeft += lifeTimeToAdd;
		m_lifeTimeLeft = Mathf.Clamp(m_lifeTimeLeft, 0f, m_maxLifeTime);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		m_lifeTimeLeft = binaryDeserializer.ReadFloat();
		m_maxLifeTime = binaryDeserializer.ReadFloat();
	}

	public string GetString()
	{
		int num = Mathf.CeilToInt(GetPercentage() * 100f);
		if (m_percentage != num || string.IsNullOrEmpty(m_info))
		{
			m_percentage = num;
			m_info = $"{m_percentage}%";
		}
		return m_info;
	}

	public float GetPercentage()
	{
		return math.saturate(m_lifeTimeLeft / m_maxLifeTime);
	}

	public void UpdateLocale()
	{
	}
}
