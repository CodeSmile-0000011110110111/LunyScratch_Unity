using LunyScratch;
using UnityEngine;
using UnityEngine.UIElements;

internal sealed class UIVariableBinding
{
	private readonly VisualElement _element;
	private readonly Variable _variable;

	public UIVariableBinding(VisualElement visualElement, Variable variable)
	{
		_element = visualElement;
		_variable = variable;

		OnValueChanged(_variable); // set initial value
		_variable.OnValueChanged += OnValueChanged;
	}

	public void Dispose() => _variable.OnValueChanged -= OnValueChanged;

	private void OnValueChanged(Variable variable)
	{
		if (_element is Label label)
		{
			if (variable.IsNumber)
				label.text = variable.AsNumber().ToString("N0");
			else
				label.text = variable.AsString();
		}
		else
			Debug.LogWarning($"Unsupported UI element type: {_element.GetType()}");
	}
}
