using UnityEngine;
using UnityEngine.UI.ProceduralImage;

[ModifierID("Only One Edge")]
public class OnlyOneEdgeModifier : ProceduralImageModifier
{
	public enum ProceduralImageEdge
	{
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right = 3
	}

	[SerializeField]
	private float radius;

	[SerializeField]
	private ProceduralImageEdge side;

	public float Radius
	{
		get
		{
			return radius;
		}
		set
		{
			radius = value;
			base._Graphic.SetVerticesDirty();
		}
	}

	public ProceduralImageEdge Side
	{
		get
		{
			return side;
		}
		set
		{
			side = value;
		}
	}

	public override Vector4 CalculateRadius(Rect imageRect)
	{
		return side switch
		{
			ProceduralImageEdge.Top => new Vector4(radius, radius, 0f, 0f), 
			ProceduralImageEdge.Right => new Vector4(0f, radius, radius, 0f), 
			ProceduralImageEdge.Bottom => new Vector4(0f, 0f, radius, radius), 
			ProceduralImageEdge.Left => new Vector4(radius, 0f, 0f, radius), 
			_ => new Vector4(0f, 0f, 0f, 0f), 
		};
	}
}
