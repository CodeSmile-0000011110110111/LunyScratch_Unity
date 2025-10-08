using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed partial class UnityActions
	{
		public void ShowMessage(String message, Double duration) => Debug.Log($"[Say] {message}");
	}
}
