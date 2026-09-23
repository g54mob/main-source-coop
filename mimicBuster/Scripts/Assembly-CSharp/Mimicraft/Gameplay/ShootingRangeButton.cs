using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class ShootingRangeButton : MonoBehaviour
	{
		[Tooltip("Bu butonun sürdüğü istasyon. Boş bırakılırsa üst objelerde aranır.")]
		[SerializeField]
		private ShootingRangeStation station;

		[Tooltip("İstasyonun Duraklar listesindeki indeks. 0 = ilk durak.")]
		[SerializeField]
		[Min(0f)]
		private int stop;

		[Tooltip("Ekranda görünecek ad - '15 metre' gibi. Boşsa objenin adı kullanılır.")]
		[SerializeField]
		private string label = "";

		[Tooltip("Bakılınca yanan parça. Boş bırakılırsa bu objenin Renderer'ı kullanılır.")]
		[SerializeField]
		private Renderer highlight;

		[Tooltip("Bakılırken uygulanan renk.")]
		[SerializeField]
		private Color highlightColor = new Color(1f, 0.85f, 0.3f);

		private static readonly List<ShootingRangeButton> all = new List<ShootingRangeButton>();

		private MaterialPropertyBlock block;

		private Color restColor;

		private bool hasRestColor;

		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

		public ShootingRangeStation Station => station;

		public int Stop => stop;

		public string Prompt
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(label))
				{
					return label;
				}
				return base.name;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			all.Clear();
		}

		private void OnEnable()
		{
			all.Add(this);
			if (station == null)
			{
				station = GetComponentInParent<ShootingRangeStation>();
			}
			if (highlight == null)
			{
				highlight = GetComponent<Renderer>();
			}
		}

		private void OnDisable()
		{
			all.Remove(this);
			SetHighlighted(highlighted: false);
		}

		public void SetHighlighted(bool highlighted)
		{
			if (!(highlight == null))
			{
				if (block == null)
				{
					block = new MaterialPropertyBlock();
				}
				if (!hasRestColor)
				{
					hasRestColor = true;
					restColor = ((highlight.sharedMaterial != null && highlight.sharedMaterial.HasProperty(BaseColor)) ? highlight.sharedMaterial.GetColor(BaseColor) : Color.white);
				}
				highlight.GetPropertyBlock(block);
				block.SetColor(BaseColor, highlighted ? highlightColor : restColor);
				highlight.SetPropertyBlock(block);
			}
		}

		public static ShootingRangeButton Nearest(Vector3 point, float within)
		{
			ShootingRangeButton result = null;
			float num = within * within;
			foreach (ShootingRangeButton item in all)
			{
				if (!(item == null) && !(item.station == null))
				{
					float sqrMagnitude = (item.transform.position - point).sqrMagnitude;
					if (!(sqrMagnitude > num))
					{
						num = sqrMagnitude;
						result = item;
					}
				}
			}
			return result;
		}
	}
}
