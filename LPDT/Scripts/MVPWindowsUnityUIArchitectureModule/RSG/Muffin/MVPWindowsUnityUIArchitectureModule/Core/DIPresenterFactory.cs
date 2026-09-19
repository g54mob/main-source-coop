using System;
using Zenject;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public sealed class DIPresenterFactory : IPresenterFactory
	{
		private const string VIEWBASE_ENDING = "ViewBase";

		private string PRESENTER_ENDING = "Presenter";

		private readonly DiContainer _container;

		public DIPresenterFactory(DiContainer container)
		{
			_container = container ?? throw new ArgumentNullException("container");
		}

		public PresenterBehaviour GetPresenter(ViewBehaviour view)
		{
			Type baseType = view.GetType().BaseType;
			if (baseType == null)
			{
				throw new ViewIsNotSuitableException(view.GetType());
			}
			try
			{
				return _container.Instantiate(GetPresenterType(baseType)) as PresenterBehaviour;
			}
			catch (Exception innerException)
			{
				throw new PresenterCannotBeCreatedException(view.GetType(), innerException);
			}
		}

		private Type GetPresenterType(Type baseType)
		{
			return Type.GetType(baseType.ToString().Replace("ViewBase", PRESENTER_ENDING) + ", " + baseType.Assembly);
		}
	}
}
