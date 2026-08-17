using UnityEngine;

namespace VisualDesignCafe.Nature
{
	internal class NotificationBox
	{
		private readonly GUIContent _title;

		private readonly GUIContent _text;

		private readonly GUIContent _buttonContent;

		private readonly string _buttonUrl;

		private bool _stylesLoaded;

		private GUIStyle _backgroundStyle;

		private GUIStyle _textStyle;

		private GUIStyle _titleStyle;

		private GUIStyle _buttonStyle;

		public bool IsSceneView { get; set; }

		public float MarginBottom { get; set; }

		public NotificationBox(string title, string text)
		{
			_title = new GUIContent(title);
			_text = new GUIContent(text);
		}

		public NotificationBox(string title, string text, string button, string url)
		{
			_title = new GUIContent(title);
			_text = new GUIContent(text);
			_buttonContent = new GUIContent(button);
			_buttonUrl = url;
		}

		private void LoadStyles()
		{
			_stylesLoaded = true;
			int num = 15;
			_backgroundStyle = new GUIStyle();
			_backgroundStyle.normal.background = Texture2D.whiteTexture;
			_backgroundStyle.padding = new RectOffset(num, num + 2, num, num - 2);
			_backgroundStyle.margin = new RectOffset(10, 0, 0, 10);
			_buttonStyle = new GUIStyle("Button");
			_buttonStyle.padding = new RectOffset(25, 25, 2, 2);
			_buttonStyle.margin = new RectOffset(num, 0, 0, 0);
			_buttonStyle.stretchHeight = true;
			_textStyle = new GUIStyle(GUI.skin.label)
			{
				fontSize = (Application.isEditor ? 11 : 15),
				fontStyle = FontStyle.Normal,
				margin = new RectOffset(0, 0, 2, 0),
				border = new RectOffset(0, 0, 0, 0),
				padding = new RectOffset(0, 0, 0, 0),
				wordWrap = false
			};
			_titleStyle = new GUIStyle(GUI.skin.label)
			{
				fontSize = (Application.isEditor ? 9 : 11),
				margin = new RectOffset(0, 0, 0, 0),
				border = new RectOffset(0, 0, 0, 0),
				padding = new RectOffset(0, 0, 0, 0)
			};
			Color textColor = new Color(0.5f, 0.5f, 0.5f, 1f);
			_titleStyle.normal.textColor = textColor;
			_titleStyle.richText = true;
		}

		public void Draw()
		{
			if (!_stylesLoaded)
			{
				LoadStyles();
			}
			_textStyle.CalcMinMaxWidth(_text, out var _, out var maxWidth);
			float num = _textStyle.CalcHeight(_text, maxWidth);
			_titleStyle.CalcMinMaxWidth(_title, out var _, out var maxWidth2);
			float num2 = _titleStyle.CalcHeight(_title, maxWidth2);
			float num3 = num + num2 + (float)_backgroundStyle.padding.vertical + (float)_backgroundStyle.margin.vertical;
			float num4 = Mathf.Max(maxWidth2, maxWidth) + (float)_backgroundStyle.padding.horizontal + (float)_backgroundStyle.margin.horizontal;
			if (_buttonContent != null && !string.IsNullOrEmpty(_buttonUrl))
			{
				_buttonStyle.CalcMinMaxWidth(_buttonContent, out var _, out var maxWidth3);
				num4 += maxWidth3;
			}
			float num5 = ((!IsSceneView) ? 1f : ((Screen.dpi > 0f) ? (Screen.dpi / 96f) : 1f));
			Rect screenRect = new Rect(_backgroundStyle.margin.left, (float)Screen.height / num5 - num3 - (float)_backgroundStyle.margin.bottom, num4, num3);
			screenRect.y -= MarginBottom;
			using (new GUILayout.AreaScope(screenRect))
			{
				GUILayout.FlexibleSpace();
				GUI.backgroundColor = new Color32(40, 40, 40, byte.MaxValue);
				using (new GUILayout.VerticalScope(_backgroundStyle, GUILayout.MaxWidth(maxWidth)))
				{
					using (new GUILayout.HorizontalScope())
					{
						using (new GUILayout.VerticalScope())
						{
							GUILayout.Label(_title, _titleStyle);
							GUILayout.Label(_text, _textStyle);
						}
						using (new GUILayout.VerticalScope())
						{
							GUI.backgroundColor = Color.white;
							if (_buttonContent != null && !string.IsNullOrEmpty(_buttonUrl) && GUILayout.Button(_buttonContent, _buttonStyle, GUILayout.Height(num2 + num + 1f)))
							{
								Application.OpenURL(_buttonUrl);
							}
						}
					}
				}
				GUILayout.Space(30f);
			}
		}
	}
}
