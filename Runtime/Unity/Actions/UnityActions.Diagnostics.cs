using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed partial class UnityActions
	{
		public void LogInfo(String message) => Debug.Log(message);
		public void LogWarn(String message) => Debug.LogWarning(message);
		public void LogError(String message) => Debug.LogError(message);
	}
}
