using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed partial class UnityActions
	{
		public void PlaySound(String soundName, Double volume) => Debug.Log($"[PlaySound] {soundName} @ {volume}");
	}
}
