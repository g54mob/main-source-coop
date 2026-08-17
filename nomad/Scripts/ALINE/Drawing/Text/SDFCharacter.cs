using Unity.Mathematics;

namespace Drawing.Text
{
	internal struct SDFCharacter
	{
		public char codePoint;

		private float2 uvtopleft;

		private float2 uvbottomright;

		private float2 vtopleft;

		private float2 vbottomright;

		public float advance;

		public float2 uvTopLeft => uvtopleft;

		public float2 uvTopRight => new float2(uvbottomright.x, uvtopleft.y);

		public float2 uvBottomLeft => new float2(uvtopleft.x, uvbottomright.y);

		public float2 uvBottomRight => uvbottomright;

		public float2 vertexTopLeft => vtopleft;

		public float2 vertexTopRight => new float2(vbottomright.x, vtopleft.y);

		public float2 vertexBottomLeft => new float2(vtopleft.x, vbottomright.y);

		public float2 vertexBottomRight => vbottomright;

		public SDFCharacter(char codePoint, int x, int y, int width, int height, int originX, int originY, int advance, int textureWidth, int textureHeight, float defaultSize)
		{
			float2 float5 = new float2(textureWidth, textureHeight);
			this.codePoint = codePoint;
			float2 float6 = new float2(x, y) / float5;
			float2 float7 = new float2(x + width, y + height) / float5;
			uvtopleft = new float2(float6.x, 1f - float6.y);
			uvbottomright = new float2(float7.x, 1f - float7.y);
			float2 float8 = new float2(-originX, originY);
			vtopleft = (float8 + new float2(0f, 0f)) / defaultSize;
			vbottomright = (float8 + new float2(width, -height)) / defaultSize;
			this.advance = (float)advance / defaultSize;
		}
	}
}
