using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LunyScratch
{
 public sealed class ScratchMenu : ScratchUI, IEngineMenu
 {
 	public event Action<string> OnButtonClicked;
		private void OnEnable()
		{
			var buttons = RootElement.Query<Button>();
			buttons.ForEach(button => button.RegisterCallback<ClickEvent>(OnButtonPressed));
		}

		private void OnDisable()
		{
			var buttons = RootElement.Query<Button>();
			buttons.ForEach(button => button.UnregisterCallback<ClickEvent>(OnButtonPressed));
		}

		private void OnButtonPressed(ClickEvent evt)
		{
			var button = evt.currentTarget as Button;
			if (button != null)
			{
				Debug.Log($"pressed {button.name} --- {evt}, {evt.currentTarget}");
				OnButtonClicked?.Invoke(button.name);
			}
		}
	}
}
