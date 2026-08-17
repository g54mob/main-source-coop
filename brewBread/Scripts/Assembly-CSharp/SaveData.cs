using System;

[Serializable]
public class SaveData
{
	public float[] breadPosition;

	public float[] fredPosition;

	public float[] timer;

	public SaveData()
	{
		breadPosition = new float[3];
		breadPosition[0] = StaticInstance<Bread>.Instance.PenguinTransform.position.x;
		breadPosition[1] = StaticInstance<Bread>.Instance.PenguinTransform.position.y;
		breadPosition[2] = StaticInstance<Bread>.Instance.PenguinTransform.position.z;
		fredPosition = new float[3];
		fredPosition[0] = StaticInstance<Fred>.Instance.PenguinTransform.position.x;
		fredPosition[1] = StaticInstance<Fred>.Instance.PenguinTransform.position.y;
		fredPosition[2] = StaticInstance<Fred>.Instance.PenguinTransform.position.z;
	}
}
