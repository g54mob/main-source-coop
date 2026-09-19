using System;
using Features.InputModule.Scripts.Generated;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using Zenject;

namespace Features.EmotesModule.Scripts
{
	public class CallEmotesGroupByInputSystem : IInitializable, IDisposable
	{
		private readonly IInputService _inputService;

		private readonly EmotesTriggerModel _emotesTriggerModel;

		private readonly EmoteAvailabilityModel _emoteAvailabilityModel;

		public CallEmotesGroupByInputSystem(IInputService inputService, EmotesTriggerModel emotesTriggerModel, EmoteAvailabilityModel emoteAvailabilityModel)
		{
			_inputService = inputService;
			_emotesTriggerModel = emotesTriggerModel;
			_emoteAvailabilityModel = emoteAvailabilityModel;
		}

		public void Initialize()
		{
			InputDefaultActions faceEmote = _inputService.FaceEmote;
			faceEmote.Performed = (Action)Delegate.Combine(faceEmote.Performed, new Action(CallFaceEmote));
			InputDefaultActions handEmote = _inputService.HandEmote;
			handEmote.Performed = (Action)Delegate.Combine(handEmote.Performed, new Action(CallHandEmote));
			InputDefaultActions bodyEmote = _inputService.BodyEmote;
			bodyEmote.Performed = (Action)Delegate.Combine(bodyEmote.Performed, new Action(CallBodyEmote));
			InputDefaultActions hotBar_ = _inputService.HotBar_1;
			hotBar_.Performed = (Action)Delegate.Combine(hotBar_.Performed, new Action(TriggerHotBar1));
			InputDefaultActions hotBar_2 = _inputService.HotBar_2;
			hotBar_2.Performed = (Action)Delegate.Combine(hotBar_2.Performed, new Action(TriggerHotBar2));
			InputDefaultActions hotBar_3 = _inputService.HotBar_3;
			hotBar_3.Performed = (Action)Delegate.Combine(hotBar_3.Performed, new Action(TriggerHotBar3));
			InputDefaultActions hotBar_4 = _inputService.HotBar_4;
			hotBar_4.Performed = (Action)Delegate.Combine(hotBar_4.Performed, new Action(TriggerHotBar4));
			InputDefaultActions hotBar_5 = _inputService.HotBar_5;
			hotBar_5.Performed = (Action)Delegate.Combine(hotBar_5.Performed, new Action(TriggerHotBar5));
			InputDefaultActions hotBar_6 = _inputService.HotBar_6;
			hotBar_6.Performed = (Action)Delegate.Combine(hotBar_6.Performed, new Action(TriggerHotBar6));
			InputDefaultActions hotBar_7 = _inputService.HotBar_7;
			hotBar_7.Performed = (Action)Delegate.Combine(hotBar_7.Performed, new Action(TriggerHotBar7));
			InputDefaultActions hotBar_8 = _inputService.HotBar_8;
			hotBar_8.Performed = (Action)Delegate.Combine(hotBar_8.Performed, new Action(TriggerHotBar8));
			InputDefaultActions hotBar_9 = _inputService.HotBar_9;
			hotBar_9.Performed = (Action)Delegate.Combine(hotBar_9.Performed, new Action(TriggerHotBar9));
			InputDefaultActions hotBar_10 = _inputService.HotBar_0;
			hotBar_10.Performed = (Action)Delegate.Combine(hotBar_10.Performed, new Action(TriggerHotBar0));
		}

		public void Dispose()
		{
			InputDefaultActions faceEmote = _inputService.FaceEmote;
			faceEmote.Performed = (Action)Delegate.Remove(faceEmote.Performed, new Action(CallFaceEmote));
			InputDefaultActions handEmote = _inputService.HandEmote;
			handEmote.Performed = (Action)Delegate.Remove(handEmote.Performed, new Action(CallHandEmote));
			InputDefaultActions bodyEmote = _inputService.BodyEmote;
			bodyEmote.Performed = (Action)Delegate.Remove(bodyEmote.Performed, new Action(CallBodyEmote));
			InputDefaultActions hotBar_ = _inputService.HotBar_1;
			hotBar_.Performed = (Action)Delegate.Remove(hotBar_.Performed, new Action(TriggerHotBar1));
			InputDefaultActions hotBar_2 = _inputService.HotBar_2;
			hotBar_2.Performed = (Action)Delegate.Remove(hotBar_2.Performed, new Action(TriggerHotBar2));
			InputDefaultActions hotBar_3 = _inputService.HotBar_3;
			hotBar_3.Performed = (Action)Delegate.Remove(hotBar_3.Performed, new Action(TriggerHotBar3));
			InputDefaultActions hotBar_4 = _inputService.HotBar_4;
			hotBar_4.Performed = (Action)Delegate.Remove(hotBar_4.Performed, new Action(TriggerHotBar4));
			InputDefaultActions hotBar_5 = _inputService.HotBar_5;
			hotBar_5.Performed = (Action)Delegate.Remove(hotBar_5.Performed, new Action(TriggerHotBar5));
			InputDefaultActions hotBar_6 = _inputService.HotBar_6;
			hotBar_6.Performed = (Action)Delegate.Remove(hotBar_6.Performed, new Action(TriggerHotBar6));
			InputDefaultActions hotBar_7 = _inputService.HotBar_7;
			hotBar_7.Performed = (Action)Delegate.Remove(hotBar_7.Performed, new Action(TriggerHotBar7));
			InputDefaultActions hotBar_8 = _inputService.HotBar_8;
			hotBar_8.Performed = (Action)Delegate.Remove(hotBar_8.Performed, new Action(TriggerHotBar8));
			InputDefaultActions hotBar_9 = _inputService.HotBar_9;
			hotBar_9.Performed = (Action)Delegate.Remove(hotBar_9.Performed, new Action(TriggerHotBar9));
			InputDefaultActions hotBar_10 = _inputService.HotBar_0;
			hotBar_10.Performed = (Action)Delegate.Remove(hotBar_10.Performed, new Action(TriggerHotBar0));
		}

		private void CallFaceEmote()
		{
			CallEmoteView(EmoteGroup.Face);
		}

		private void CallHandEmote()
		{
			CallEmoteView(EmoteGroup.Hand);
		}

		private void CallBodyEmote()
		{
			CallEmoteView(EmoteGroup.Body);
		}

		private void TriggerHotBar1()
		{
			TriggerHotBar(1);
		}

		private void TriggerHotBar2()
		{
			TriggerHotBar(2);
		}

		private void TriggerHotBar3()
		{
			TriggerHotBar(3);
		}

		private void TriggerHotBar4()
		{
			TriggerHotBar(4);
		}

		private void TriggerHotBar5()
		{
			TriggerHotBar(5);
		}

		private void TriggerHotBar6()
		{
			TriggerHotBar(6);
		}

		private void TriggerHotBar7()
		{
			TriggerHotBar(7);
		}

		private void TriggerHotBar8()
		{
			TriggerHotBar(8);
		}

		private void TriggerHotBar9()
		{
			TriggerHotBar(9);
		}

		private void TriggerHotBar0()
		{
			TriggerHotBar(0);
		}

		private void CallEmoteView(EmoteGroup emoteGroup)
		{
			if (_emoteAvailabilityModel.IsEmoteActive)
			{
				_emotesTriggerModel.CallEmoteView(emoteGroup);
			}
		}

		private void TriggerHotBar(int hotBarIndex)
		{
			if (_emoteAvailabilityModel.IsEmoteActive)
			{
				_emotesTriggerModel.InvokeOnHotBarActivated(hotBarIndex);
			}
		}
	}
}
