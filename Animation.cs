using System;

namespace GraphAnimator
{

	public abstract class Animation
	{
		public Animation(){}

		public abstract void Initialize(Graph g);

		public abstract bool isPlayable();

		public abstract void Step();

		public abstract void Play();

		public abstract void Pause();

		public abstract void Stop();

		public abstract void StepBack();

		public abstract override string ToString();
	}
}
