using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LunyScratch
{
	[RequireComponent(typeof(UIDocument))]
	[DisallowMultipleComponent]
	public abstract class ScratchUI : MonoBehaviour, IEngineUI
	{
		private readonly Dictionary<String, UIVariableBinding> _boundVariables = new();
		private VisualElement _rootElement;
		public VisualElement RootElement => _rootElement ??= GetRootVisualElement(gameObject);

		internal static VisualElement GetRootVisualElement(GameObject go)
		{
			var doc = go.GetComponent<UIDocument>();
			if (doc == null)
				Debug.LogError($"missing {nameof(UIDocument)} component on {go.name} ({go.GetInstanceID()})");
			if (go.activeInHierarchy && (doc.rootVisualElement == null || doc.rootVisualElement.childCount == 0))
				Debug.LogError($"Source Asset not assigned in UIDocument on {go.name} ({go.GetInstanceID()})");
			if (doc.panelSettings == null)
				Debug.LogError($"Panel Settings not assigned in UIDocument on {go.name} ({go.GetInstanceID()})");

			return doc.rootVisualElement;
		}

		public void Show()
		{
			if (RootElement != null)
				RootElement.style.display = DisplayStyle.Flex;
		}

		public void Hide()
		{
			if (RootElement != null)
				RootElement.style.display = DisplayStyle.None;
		}

		private void OnDestroy()
		{
			foreach (var binding in _boundVariables.Values)
				binding.Dispose();
			_boundVariables.Clear();
		}

		public void BindVariable(Variable variable)
		{
			var elementName = variable.Name;
			if (variable == null)
			{
				Debug.LogError($"Variable for {elementName} is null");
				return;
			}

			if (_boundVariables.ContainsKey(elementName))
			{
				Debug.LogWarning($"HUD element '{elementName}' already has variable binding.");
				return;
			}

			var element = RootElement.Q(elementName);
			if (element == null)
			{
				Debug.LogWarning($"HUD element named '{elementName}' not found.");
				return;
			}

			var binding = new UIVariableBinding(element, variable);
			_boundVariables.Add(elementName, binding);
		}
	}
}
