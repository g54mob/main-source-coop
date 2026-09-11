using EasyTextEffects.Editor.EditorDocumentation;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace EasyTextEffects
{
	public abstract class TextEffect_Base : ScriptableObject
	{
		[FormerlySerializedAs("effectName")]
		[FoldBox("Effect Tag Explained", new string[] { "Used for 'Tag Effects' -- effects only applied to the text based on rich text tags.\n\nThe format is <link=effectTag>text<link>\n\n To add multiple effects, the format is <link=effectTag1+effectTag2>text</link>. Don't include \"+\" in effect tags for this reason." }, new FoldBoxAttribute.ContentType[] { FoldBoxAttribute.ContentType.Text }, true)]
		public string effectTag;

		[HideInInspector]
		public int startCharIndex;

		[HideInInspector]
		public int charLength;

		public abstract void ApplyEffect(TMP_TextInfo _textInfo, int _charIndex, int _startVertex = 0, int _endVertex = 3);

		protected Vector3 CharCenter(TMP_CharacterInfo _charInfo, Vector3[] _verts)
		{
			int vertexIndex = _charInfo.vertexIndex;
			int num = vertexIndex + 2;
			return (_verts[vertexIndex] + _verts[num]) / 2f;
		}
	}
}
