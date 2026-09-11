public class Comment
{
	public int Likes;

	public bool Detailed { get; set; }

	public string Text { get; set; }

	public string Player { get; set; }

	public float Time { get; set; }

	public byte Face { get; set; }

	public byte FaceColor { get; set; }

	public ushort EventID { get; set; }

	public Comment(string text)
	{
		Text = text;
	}

	public Comment(string text, string player)
	{
		Text = text;
		Player = player;
		Detailed = true;
	}
}
