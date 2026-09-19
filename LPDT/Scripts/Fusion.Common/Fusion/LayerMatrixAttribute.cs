namespace Fusion
{
	public class LayerMatrixAttribute : DrawerPropertyAttribute
	{
		public string LayerNamesField { get; }

		public LayerMatrixAttribute(string layerNamesField)
			: base(applyToCollection: true)
		{
			LayerNamesField = layerNamesField;
		}
	}
}
