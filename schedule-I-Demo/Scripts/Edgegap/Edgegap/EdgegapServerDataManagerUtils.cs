using IO.Swagger.Model;
using UnityEngine;
using UnityEngine.UIElements;

namespace Edgegap
{
	internal static class EdgegapServerDataManagerUtils
	{
		public static Label GetHeader(string text)
		{
			Label label = new Label(text);
			label.AddToClassList("label__header");
			return label;
		}

		public static VisualElement GetHeaderRow()
		{
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList("row__port-table");
			visualElement.AddToClassList("label__header");
			visualElement.Add(new Label("Name"));
			visualElement.Add(new Label("External"));
			visualElement.Add(new Label("Internal"));
			visualElement.Add(new Label("ProtocolStr"));
			visualElement.Add(new Label("Link"));
			return visualElement;
		}

		public static VisualElement GetRowFromPortResponse(PortMapping port)
		{
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList("row__port-table");
			visualElement.AddToClassList("focusable");
			visualElement.Add(new Label(port.Name));
			visualElement.Add(new Label(port.External.ToString()));
			visualElement.Add(new Label(port.Internal.ToString()));
			visualElement.Add(new Label(port.Protocol));
			visualElement.Add(GetCopyButton("Copy", port.Link));
			return visualElement;
		}

		public static Button GetCopyButton(string btnText, string copiedText)
		{
			Button button = new Button();
			button.text = btnText;
			button.clickable.clicked += delegate
			{
				GUIUtility.systemCopyBuffer = copiedText;
			};
			return button;
		}

		public static Button GetLinkButton(string btnText, string targetUrl)
		{
			Button button = new Button();
			button.text = btnText;
			button.clickable.clicked += delegate
			{
				UnityEngine.Application.OpenURL(targetUrl);
			};
			return button;
		}

		public static Label GetInfoText(string innerText)
		{
			Label label = new Label(innerText);
			label.AddToClassList("label__info-text");
			return label;
		}
	}
}
