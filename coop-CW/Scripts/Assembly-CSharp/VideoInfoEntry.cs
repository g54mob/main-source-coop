using System;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core.Serizalization;

public class VideoInfoEntry : ItemDataEntry, IHaveUIData
{
	public VideoHandle videoID;

	public float timeLeft;

	public float maxTime;

	private string m_filmLeftText;

	private int m_percentage;

	private string m_info;

	public bool isRecording { get; set; }

	public bool isFlipped { get; set; }

	public float timeRecorded => maxTime - timeLeft;

	public bool StartedRecording => videoID.id != default(Guid);

	public override void Serialize(BinarySerializer binarySerializer)
	{
		binarySerializer.WriteFloat(timeLeft);
		binarySerializer.WriteFloat(maxTime);
		binarySerializer.WriteBool(isRecording);
		binarySerializer.WriteBool(isFlipped);
		binarySerializer.WriteGuid(videoID.id);
	}

	public override void Deserialize(BinaryDeserializer binaryDeserializer)
	{
		timeLeft = binaryDeserializer.ReadFloat();
		maxTime = binaryDeserializer.ReadFloat();
		isRecording = binaryDeserializer.ReadBool();
		isFlipped = binaryDeserializer.ReadBool();
		videoID = new VideoHandle(binaryDeserializer.ReadGuid());
	}

	public string GetString()
	{
		int num = Mathf.CeilToInt(GetPercentage() * 100f);
		if (m_percentage != num || string.IsNullOrEmpty(m_info))
		{
			m_percentage = num;
			m_info = $"{m_percentage} {m_filmLeftText}";
		}
		return m_info;
	}

	public void UpdateLocale()
	{
		m_filmLeftText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.FilmLeft);
		m_info = $"{m_percentage} {m_filmLeftText}";
	}

	public float GetPercentage()
	{
		return math.saturate(timeLeft / maxTime);
	}
}
