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

	private void OnValueChanged(Variable obj)
	{
		if (_element is Label label)
			label.text = obj.AsString();
		else
			Debug.LogWarning($"Unsupported UI element type: {_element.GetType()}");
	}
}
