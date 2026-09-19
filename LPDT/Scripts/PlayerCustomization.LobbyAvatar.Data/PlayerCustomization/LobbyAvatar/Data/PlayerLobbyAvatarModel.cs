using Features.DeadPartsModule.Data;
using Features.NetworkedModelCodegen.Scripts;
using Features.SkinConfiguration.Scripts;
using Fusion;

namespace PlayerCustomization.LobbyAvatar.Data
{
	[NetworkedModel(ModelScope.Lobby, ModelOwnership.Individual)]
	public sealed class PlayerLobbyAvatarModel : NetworkedModelBase
	{
		public Networked<NetworkString<_32>> Nickname { get; } = new Networked<NetworkString<_32>>();

		public Networked<int> PrimaryColor { get; } = new Networked<int>();

		public Networked<int> VariableColor { get; } = new Networked<int>();

		public Networked<SkinType> HatPartSkinId { get; } = new Networked<SkinType>();

		public Networked<SkinType> TorsoPartSkinId { get; } = new Networked<SkinType>();

		public Networked<SkinType> BottomPartSkinId { get; } = new Networked<SkinType>();

		public Networked<bool> IsFullSkin { get; } = new Networked<bool>();

		public Networked<ButtTexturePreset> ButtTexturePreset { get; } = new Networked<ButtTexturePreset>();

		public Networked<ButtMeshPreset> ButtMeshPreset { get; } = new Networked<ButtMeshPreset>();
	}
}
