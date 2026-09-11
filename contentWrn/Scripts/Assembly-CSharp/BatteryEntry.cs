using Unity.Mathematics;
using UnityEngine;
using Zorro.Core.Serizalization;

public class BatteryEntry : ItemDataEntry, IHaveUIData
{
	public float m_charge;

	public float m_maxCharge;

	private int m_percentage;

	private string m_info;

	private string m_batteryText;

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteFloat(m_charge);
		binarySerializer.WriteFloat(m_maxCharge);
	}

	public void AddCharge(float chargeToAdd)
	{
		m_charge += chargeToAdd;
		m_charge = Mathf.Clamp(m_charge, 0f, m_maxCharge);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		m_charge = binaryDeserializer.ReadFloat();
		m_maxCharge = binaryDeserializer.ReadFloat();
	}

	public string GetString()
	{
		int num = Mathf.CeilToInt(GetPercentage() * 100f);
		if (num != m_percentage || string.IsNullOrEmpty(m_info))
		{
			m_percentage = num;
			m_info = $"{m_percentage}% {m_batteryText}";
		}
		return m_info;
	}

	public void UpdateLocale()
	{
		m_batteryText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Battery);
	}

	public float GetPercentage()
	{
		return math.saturate(m_charge / m_maxCharge);
	}
}
