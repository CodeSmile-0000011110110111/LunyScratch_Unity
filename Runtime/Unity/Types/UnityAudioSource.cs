using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed class UnityAudioSource : IAudioSource
	{
		private readonly AudioSource _audioSource;

		public UnityAudioSource(AudioSource audioSource) => _audioSource = audioSource;

		public void Play() => _audioSource.Play();
	}
}
