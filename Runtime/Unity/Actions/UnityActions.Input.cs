using System;
using UnityEngine.InputSystem;

namespace LunyScratch
{
	internal sealed partial class UnityActions
	{
		public Boolean IsKeyPressed(Key key)
		{
			var keyboard = Keyboard.current;
			if (keyboard == null)
				return false;

			if (key == Key.Any)
				return keyboard.anyKey.isPressed;

			var inputSystemKey = Remap.ToInputSystemKey(key);
			return keyboard[inputSystemKey].isPressed;
		}

		public Boolean IsKeyJustPressed(Key key)
		{
			var keyboard = Keyboard.current;
			if (keyboard == null)
				return false;

			if (key == Key.Any)
				return keyboard.anyKey.wasPressedThisFrame;

			var inputSystemKey = Remap.ToInputSystemKey(key);
			return keyboard[inputSystemKey].wasPressedThisFrame;
		}

		public Boolean IsKeyJustReleased(Key key)
		{
			var keyboard = Keyboard.current;
			if (keyboard == null)
				return false;

			if (key == Key.Any)
				return keyboard.anyKey.wasReleasedThisFrame;

			var inputSystemKey = Remap.ToInputSystemKey(key);
			return keyboard[inputSystemKey].wasReleasedThisFrame;
		}

		public Boolean IsMouseButtonPressed(MouseButton button)
		{
			var mouse = Mouse.current;
			if (mouse == null)
				return false;

			return Remap.ToInputSystemMouseButton(button, mouse);
		}

		public Boolean IsMouseButtonJustPressed(MouseButton button)
		{
			var mouse = Mouse.current;
			if (mouse == null)
				return false;

			return Remap.ToInputSystemMouseButton(button, mouse);
		}

		public Boolean IsMouseButtonJustReleased(MouseButton button)
		{
			var mouse = Mouse.current;
			if (mouse == null)
				return false;

			return Remap.ToInputSystemMouseButton(button, mouse);
		}
	}
}
