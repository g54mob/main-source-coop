using Rewired.Internal;

namespace Rewired.Interfaces
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
	[CustomObfuscation(rename = false)]
	internal interface IElementIdentifierTool
	{
		void Initialize(GUIText guiText);

		void Start();

		void Update();

		void OnDestroy();
	}
}
