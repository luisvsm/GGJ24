using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace noWeekend
{
	[System.Serializable]
	public abstract class EaseAction
	{
		public float duration = 1;
		public float delay = 0;
		public EaseType easeType = EaseType.SineInOut;
		public float Duration => delay + duration;
		public abstract string EaseName { get; }
		public abstract WeekendTween.EaseActionType EaseActionType { get; }

		public void Process(EaseRefererceTransform targetTransform, float time)
		{

			if (time < delay || time > delay + duration)
			{
				return;
			}

			float value = Mathf.InverseLerp(delay, duration + delay, time);
			ApplyEase(targetTransform, value);
		}

		public void Process(Transform targetTransform, float time)
		{
			if (time < delay || time > delay + duration)
			{
				return;
			}

			float value = Mathf.InverseLerp(delay, duration + delay, time);
			ApplyEase(targetTransform, value);
		}
		protected virtual void ApplyEase(Transform targetTransform, float value) { }
		protected virtual void ApplyEase(EaseRefererceTransform targetTransform, float value) { }
		public virtual void SetStart(Transform targetTransform) { }
		public virtual void SetStart(EaseRefererceTransform targetTransform) { }
		public virtual void SetEnd(Transform targetTransform) { }
		public virtual void SetEnd(EaseRefererceTransform targetTransform) { }
	}

}