using LunyScratch;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class ScratchHUD : MonoBehaviour
{
	private readonly Dictionary<String, UIVariableBinding> _boundVariables = new();

	private VisualElement _rootElement;
	public VisualElement RootElement => _rootElement ??= GetComponent<UIDocument>().rootVisualElement;

	private void OnDestroy()
	{
		foreach (var binding in _boundVariables.Values)
			binding.Dispose();
		_boundVariables.Clear();
	}

	public void BindVariable(String elementName, Variable variable)
	{
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
