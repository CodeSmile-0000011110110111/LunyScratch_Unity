using System;
using UnityEngine.InputSystem;

namespace LunyScratch
{
	internal sealed class Remap
	{
		internal static UnityEngine.InputSystem.Key ToInputSystemKey(Key key) => key switch
		{
			// Letters
			Key.A => UnityEngine.InputSystem.Key.A,
			Key.B => UnityEngine.InputSystem.Key.B,
			Key.C => UnityEngine.InputSystem.Key.C,
			Key.D => UnityEngine.InputSystem.Key.D,
			Key.E => UnityEngine.InputSystem.Key.E,
			Key.F => UnityEngine.InputSystem.Key.F,
			Key.G => UnityEngine.InputSystem.Key.G,
			Key.H => UnityEngine.InputSystem.Key.H,
			Key.I => UnityEngine.InputSystem.Key.I,
			Key.J => UnityEngine.InputSystem.Key.J,
			Key.K => UnityEngine.InputSystem.Key.K,
			Key.L => UnityEngine.InputSystem.Key.L,
			Key.M => UnityEngine.InputSystem.Key.M,
			Key.N => UnityEngine.InputSystem.Key.N,
			Key.O => UnityEngine.InputSystem.Key.O,
			Key.P => UnityEngine.InputSystem.Key.P,
			Key.Q => UnityEngine.InputSystem.Key.Q,
			Key.R => UnityEngine.InputSystem.Key.R,
			Key.S => UnityEngine.InputSystem.Key.S,
			Key.T => UnityEngine.InputSystem.Key.T,
			Key.U => UnityEngine.InputSystem.Key.U,
			Key.V => UnityEngine.InputSystem.Key.V,
			Key.W => UnityEngine.InputSystem.Key.W,
			Key.X => UnityEngine.InputSystem.Key.X,
			Key.Y => UnityEngine.InputSystem.Key.Y,
			Key.Z => UnityEngine.InputSystem.Key.Z,

			// Numbers
			Key.Digit0 => UnityEngine.InputSystem.Key.Digit0,
			Key.Digit1 => UnityEngine.InputSystem.Key.Digit1,
			Key.Digit2 => UnityEngine.InputSystem.Key.Digit2,
			Key.Digit3 => UnityEngine.InputSystem.Key.Digit3,
			Key.Digit4 => UnityEngine.InputSystem.Key.Digit4,
			Key.Digit5 => UnityEngine.InputSystem.Key.Digit5,
			Key.Digit6 => UnityEngine.InputSystem.Key.Digit6,
			Key.Digit7 => UnityEngine.InputSystem.Key.Digit7,
			Key.Digit8 => UnityEngine.InputSystem.Key.Digit8,
			Key.Digit9 => UnityEngine.InputSystem.Key.Digit9,

			// Function keys
			Key.F1 => UnityEngine.InputSystem.Key.F1,
			Key.F2 => UnityEngine.InputSystem.Key.F2,
			Key.F3 => UnityEngine.InputSystem.Key.F3,
			Key.F4 => UnityEngine.InputSystem.Key.F4,
			Key.F5 => UnityEngine.InputSystem.Key.F5,
			Key.F6 => UnityEngine.InputSystem.Key.F6,
			Key.F7 => UnityEngine.InputSystem.Key.F7,
			Key.F8 => UnityEngine.InputSystem.Key.F8,
			Key.F9 => UnityEngine.InputSystem.Key.F9,
			Key.F10 => UnityEngine.InputSystem.Key.F10,
			Key.F11 => UnityEngine.InputSystem.Key.F11,
			Key.F12 => UnityEngine.InputSystem.Key.F12,

			// Arrow keys
			Key.LeftArrow => UnityEngine.InputSystem.Key.LeftArrow,
			Key.RightArrow => UnityEngine.InputSystem.Key.RightArrow,
			Key.UpArrow => UnityEngine.InputSystem.Key.UpArrow,
			Key.DownArrow => UnityEngine.InputSystem.Key.DownArrow,

			// Special keys
			Key.Space => UnityEngine.InputSystem.Key.Space,
			Key.Enter => UnityEngine.InputSystem.Key.Enter,
			Key.Escape => UnityEngine.InputSystem.Key.Escape,
			Key.Tab => UnityEngine.InputSystem.Key.Tab,
			Key.Backspace => UnityEngine.InputSystem.Key.Backspace,
			Key.Delete => UnityEngine.InputSystem.Key.Delete,
			Key.LeftShift => UnityEngine.InputSystem.Key.LeftShift,
			Key.RightShift => UnityEngine.InputSystem.Key.RightShift,
			Key.LeftCtrl => UnityEngine.InputSystem.Key.LeftCtrl,
			Key.RightCtrl => UnityEngine.InputSystem.Key.RightCtrl,
			Key.LeftAlt => UnityEngine.InputSystem.Key.LeftAlt,
			Key.RightAlt => UnityEngine.InputSystem.Key.RightAlt,

			// Numpad
			Key.Numpad0 => UnityEngine.InputSystem.Key.Numpad0,
			Key.Numpad1 => UnityEngine.InputSystem.Key.Numpad1,
			Key.Numpad2 => UnityEngine.InputSystem.Key.Numpad2,
			Key.Numpad3 => UnityEngine.InputSystem.Key.Numpad3,
			Key.Numpad4 => UnityEngine.InputSystem.Key.Numpad4,
			Key.Numpad5 => UnityEngine.InputSystem.Key.Numpad5,
			Key.Numpad6 => UnityEngine.InputSystem.Key.Numpad6,
			Key.Numpad7 => UnityEngine.InputSystem.Key.Numpad7,
			Key.Numpad8 => UnityEngine.InputSystem.Key.Numpad8,
			Key.Numpad9 => UnityEngine.InputSystem.Key.Numpad9,
			Key.NumpadEnter => UnityEngine.InputSystem.Key.NumpadEnter,
			Key.NumpadPlus => UnityEngine.InputSystem.Key.NumpadPlus,
			Key.NumpadMinus => UnityEngine.InputSystem.Key.NumpadMinus,
			Key.NumpadMultiply => UnityEngine.InputSystem.Key.NumpadMultiply,
			Key.NumpadDivide => UnityEngine.InputSystem.Key.NumpadDivide,

			var _ => UnityEngine.InputSystem.Key.None,
		};

		internal static Boolean ToInputSystemMouseButton(MouseButton button, Mouse mouse) => button switch
		{
			MouseButton.Left => mouse.leftButton.isPressed,
			MouseButton.Right => mouse.rightButton.isPressed,
			MouseButton.Middle => mouse.middleButton.isPressed,
			MouseButton.Forward => mouse.forwardButton.isPressed,
			MouseButton.Back => mouse.backButton.isPressed,
			var _ => false,
		};
	}
}
