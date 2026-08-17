namespace MapMagic.Nodes
{
	public interface ISceneGizmo
	{
		bool hideDefaultToolGizmo { get; set; }

		void DrawGizmo();
	}
}
