using System.Collections.Generic;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.MainMenuModule.Scripts.Credits
{
	public class CreditsSectionPresenter : PresenterBehaviour<CreditsSectionViewBase>
	{
		private readonly CreditsWindow _creditsWindow;

		private readonly DiContainer _container;

		private readonly List<CreditsEntryPresenter> _entryPresenters = new List<CreditsEntryPresenter>();

		public CreditsSectionPresenter(CreditsWindow creditsWindow, DiContainer container)
		{
			_creditsWindow = creditsWindow;
			_container = container;
		}

		public void Setup(CreditsSection section)
		{
			base.View.SetTitle(section.Title);
			BuildEntries(section);
		}

		public void SetParent(Transform parent)
		{
			base.View.transform.SetParent(parent, worldPositionStays: false);
		}

		public void DestroyView()
		{
			ClearEntries();
			Object.Destroy(base.View.gameObject);
		}

		protected override void OnDisposed()
		{
			ClearEntries();
		}

		private void BuildEntries(CreditsSection section)
		{
			if (section.Entries == null)
			{
				return;
			}
			foreach (CreditsEntryData entry in section.Entries)
			{
				CreditsEntryPresenter creditsEntryPresenter = CreateEntry();
				creditsEntryPresenter.Setup(entry);
				_entryPresenters.Add(creditsEntryPresenter);
			}
		}

		private CreditsEntryPresenter CreateEntry()
		{
			CreditsEntryViewBase component = _container.InstantiatePrefab(base.View.EntryPrefab.gameObject).GetComponent<CreditsEntryViewBase>();
			_creditsWindow.AddView(component.transform, worldPositionStays: false);
			CreditsEntryPresenter presenterForView = _creditsWindow.GetPresenterForView<CreditsEntryPresenter>(component);
			presenterForView.SetParent(base.View.EntriesContainer);
			return presenterForView;
		}

		private void ClearEntries()
		{
			foreach (CreditsEntryPresenter entryPresenter in _entryPresenters)
			{
				entryPresenter.DestroyView();
			}
			_entryPresenters.Clear();
		}
	}
}
