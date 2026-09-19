using System;
using UnityEngine;
using Zenject;

namespace Infrastructure.Injection.Factories
{
	public sealed class InjectedComponentFactory : PlaceholderFactory<Type, GameObject, Component>
	{
	}
}
