using System;
using Unity.Cinemachine;

namespace LunyScratch
{
	public sealed class ScratchCamera : IEngineCamera
	{
		private readonly CinemachineCamera _camera;

		public ScratchCamera(CinemachineCamera camera) => _camera = camera;

		public void SetTrackingTarget(IEngineObject target)
		{
			if (_camera == null)
				return;

			// For now, support clearing the target and future extension for UnityGameObject
			if (target == null)
				_camera.Target.TrackingTarget = null;
			else if (target is ScratchGameObject go)
			{
				throw new NotImplementedException();
				//_camera.Target.TrackingTarget = go.Transform;
			}
		}
	}
}
