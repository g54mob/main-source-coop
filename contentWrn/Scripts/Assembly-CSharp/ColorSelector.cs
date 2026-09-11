using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class ColorSelector : MonoBehaviour
{
	public Color color;

	public ProceduralImage outline;

	public ProceduralImage fill;

	public bool selected;

	private Color defaultFillColor;

	private PlayerCustomizer playerCustomizer_gp;

	private void Awake()
	{
		playerCustomizer_gp = GetComponentInParent<PlayerCustomizer>();
		defaultFillColor = fill.color;
		GetComponent<Button>().onClick.AddListener(Clicked);
	}

	private void Start()
	{
		outline.color = color;
	}

	public void Clicked()
	{
		Debug.Log("Clicked on color selector!");
		playerCustomizer_gp.SelectedColor = this;
	}

	public void Select()
	{
		selected = true;
		fill.color = color;
	}

	public void UnSelect()
	{
		selected = false;
		fill.color = defaultFillColor;
	}
}
