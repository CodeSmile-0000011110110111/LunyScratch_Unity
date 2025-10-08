using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed partial class UnityActions : IEngineActions
	{
		public Double GetCurrentTimeInSeconds() => Time.time;
		public Double GetDeltaTimeInSeconds() => Time.deltaTime;
		public Double GetFixedDeltaTimeInSeconds() => Time.fixedDeltaTime;
	}
}
