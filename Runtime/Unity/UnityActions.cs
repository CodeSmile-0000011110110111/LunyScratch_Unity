using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LunyScratch
{
	internal sealed class UnityActions : IEngineActions
	{
		// DEBUG
		public void Log(String message) => Debug.Log(message);

		// LOOKS
		public void ShowMessage(String message, Double duration) => Debug.Log($"[Say] {message}");

		// SOUND
		public void PlaySound(String soundName, Double volume) => Debug.Log($"[PlaySound] {soundName} @ {volume}");

		// TIME
		public Double GetCurrentTimeInSeconds() => Time.time;
		public Double GetDeltaTimeInSeconds() => Time.deltaTime;
		public Double GetFixedDeltaTimeInSeconds() => Time.fixedDeltaTime;

		// INPUT - KEYBOARD
		public Boolean IsKeyPressed(Key key)
		{
			var keyboard = Keyboard.current;
			if (keyboard == null) return false;

			if (key == Key.Any)
			{
				if (keyboard.anyKey.wasPressedThisFrame)
					Debug.Log("ANY pressed");
				return keyboard.anyKey.isPressed;
			}

			var inputSystemKey = Remap.ToInputSystemKey(key);
			return keyboard[inputSystemKey].isPressed;
		}

		public Boolean IsKeyJustPressed(Key key)
		{
			var keyboard = Keyboard.current;
			if (keyboard == null) return false;

			if (key == Key.Any)
			{
				if (keyboard.anyKey.wasPressedThisFrame)
					Debug.Log("ANY just pressed");
				return keyboard.anyKey.wasPressedThisFrame;
			}

			var inputSystemKey = Remap.ToInputSystemKey(key);
			return keyboard[inputSystemKey].wasPressedThisFrame;
		}

		public Boolean IsKeyJustReleased(Key key)
		{
			var keyboard = Keyboard.current;
			if (keyboard == null) return false;

			if (key == Key.Any)
				return keyboard.anyKey.wasReleasedThisFrame;

			var inputSystemKey = Remap.ToInputSystemKey(key);
			return keyboard[inputSystemKey].wasReleasedThisFrame;
		}

		// INPUT - MOUSE
		public Boolean IsMouseButtonPressed(MouseButton button)
		{
			var mouse = Mouse.current;
			if (mouse == null) return false;

			return button switch
			{
				MouseButton.Left => mouse.leftButton.isPressed,
				MouseButton.Right => mouse.rightButton.isPressed,
				MouseButton.Middle => mouse.middleButton.isPressed,
				MouseButton.Forward => mouse.forwardButton.isPressed,
				MouseButton.Back => mouse.backButton.isPressed,
				var _ => false,
			};
		}

		public Boolean IsMouseButtonJustPressed(MouseButton button)
		{
			var mouse = Mouse.current;
			if (mouse == null) return false;

			return button switch
			{
				MouseButton.Left => mouse.leftButton.wasPressedThisFrame,
				MouseButton.Right => mouse.rightButton.wasPressedThisFrame,
				MouseButton.Middle => mouse.middleButton.wasPressedThisFrame,
				MouseButton.Forward => mouse.forwardButton.wasPressedThisFrame,
				MouseButton.Back => mouse.backButton.wasPressedThisFrame,
				var _ => false,
			};
		}

		public Boolean IsMouseButtonJustReleased(MouseButton button)
		{
			var mouse = Mouse.current;
			if (mouse == null) return false;

			return button switch
			{
				MouseButton.Left => mouse.leftButton.wasReleasedThisFrame,
				MouseButton.Right => mouse.rightButton.wasReleasedThisFrame,
				MouseButton.Middle => mouse.middleButton.wasReleasedThisFrame,
				MouseButton.Forward => mouse.forwardButton.wasReleasedThisFrame,
				MouseButton.Back => mouse.backButton.wasReleasedThisFrame,
				var _ => false,
			};
		}

		public void RotateTransform(ITransform transform, Single x, Single y, Single z)
		{
			var unityTransform = ((UnityTransform)transform).Transform;
			unityTransform.Rotate(x, y, z, Space.Self);
		}
	}
}
