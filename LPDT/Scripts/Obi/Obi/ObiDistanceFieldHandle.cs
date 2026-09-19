namespace Obi
{
	public class ObiDistanceFieldHandle : ObiResourceHandle<ObiDistanceField>
	{
		public ObiDistanceFieldHandle(ObiDistanceField field, int index = -1)
			: base(index)
		{
			owner = field;
		}
	}
}
