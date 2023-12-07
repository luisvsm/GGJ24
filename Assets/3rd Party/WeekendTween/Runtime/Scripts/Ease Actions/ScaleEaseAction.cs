using UnityEngine;

namespace noWeekend
{
	[System.Serializable]
    public class ScaleEaseAction : EaseAction
    {
		public override string EaseName => "Ease Scale";
		public Vector3 fromScale;
        public Vector3 toScale;
		public override WeekendTween.EaseActionType EaseActionType => WeekendTween.EaseActionType.Scale;

		public ScaleEaseAction(Vector3 scales)
        {
            this.fromScale = scales;
            this.toScale = scales;
        }


        public ScaleEaseAction(Vector3 fromScale, Vector3 toScale)
        {
            this.fromScale = fromScale;
            this.toScale = toScale;
        }

        protected override void ApplyEase(Transform targetTransform, float value)
        {
            targetTransform.localScale = Vector3.LerpUnclamped(fromScale, toScale, Ease.GetEase(easeType, value));
        }
        protected override void ApplyEase(EaseRefererceTransform targetTransform, float value)
        {
            targetTransform.localScale = Vector3.LerpUnclamped(fromScale, toScale, Ease.GetEase(easeType, value));
        }

        public override void SetStart(Transform targetTransform)
        {
            targetTransform.localScale = fromScale;
        }
        public override void SetStart(EaseRefererceTransform targetTransform)
        {
            targetTransform.localScale = fromScale;
        }

        public override void SetEnd(Transform targetTransform)
        {
            targetTransform.localScale = toScale;
        }
        public override void SetEnd(EaseRefererceTransform targetTransform)
        {
            targetTransform.localScale = toScale;
        }

		public void SetFromScaleToCurrent(Transform targetTransform)
		{
			toScale = targetTransform.localScale;

		}

		public void SetToScaleToCurrent(Transform targetTransform)
		{
			toScale = targetTransform.localScale;
		}
	}
}

