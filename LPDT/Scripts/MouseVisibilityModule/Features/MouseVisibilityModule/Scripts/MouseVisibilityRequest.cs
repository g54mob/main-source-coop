namespace Features.MouseVisibilityModule.Scripts
{
	public class MouseVisibilityRequest
	{
		public bool IsMouseVisible { get; set; }

		public int Priority { get; set; }

		public MouseVisibilityRequest(int priority, bool isMouseVisible)
		{
			IsMouseVisible = isMouseVisible;
			Priority = priority;
		}
	}
}
