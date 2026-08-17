using System;
using EvilCore.DI.Core;
using EvilCore.UI.GameMenu;
using UnityEngine;
using VContainer;

namespace EvilCore.DI.Installers
{
	[Serializable]
	public class GameMenuUIInstaller : MonoInstaller
	{
		[SerializeField]
		private GameMenuUIManager gameMenuUIManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, gameMenuUIManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IGameMenuUIManager>();
			});
		}
	}
}
