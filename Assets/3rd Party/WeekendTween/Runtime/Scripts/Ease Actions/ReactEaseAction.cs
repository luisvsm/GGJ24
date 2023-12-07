using UnityEngine;

namespace noWeekend
{
	[System.Serializable]
    public class RectEaseAction : EaseAction
    {
		public override string EaseName => "Ease Rect";
        public Rect fromRect;
        public Rect toRect;
		public override WeekendTween.EaseActionType EaseActionType => WeekendTween.EaseActionType.Rect;

		public RectEaseAction(Rect Rects)
        {
            this.fromRect = Rects;
            this.toRect = Rects;
        }

        public RectEaseAction(Rect fromRect, Rect toRect)
        {
            this.fromRect = fromRect;
            this.toRect = toRect;
        }

        protected override void ApplyEase(Transform targetTransform, float value)
        {

			Rect lerpedRect = new Rect(
				Mathf.LerpUnclamped(fromRect.x, toRect.x, value),
				Mathf.LerpUnclamped(fromRect.y, toRect.y, value),
				Mathf.LerpUnclamped(fromRect.width, toRect.width, value),
				Mathf.LerpUnclamped(fromRect.height, toRect.height, value)
			);

            ((RectTransform)targetTransform).anchoredPosition = lerpedRect.position;
            ((RectTransform)targetTransform).sizeDelta = lerpedRect.size;
		}
        protected override void ApplyEase(EaseRefererceTransform targetTransform, float value)
        {
			targetTransform.rect = new Rect(
				Mathf.LerpUnclamped(fromRect.x, toRect.x, value),
				Mathf.LerpUnclamped(fromRect.y, toRect.y, value),
				Mathf.LerpUnclamped(fromRect.width, toRect.width, value),
				Mathf.LerpUnclamped(fromRect.height, toRect.height, value)
			);
		}

        public override void SetStart(Transform targetTransform)
        {
			((RectTransform)targetTransform).anchoredPosition = fromRect.position;
			((RectTransform)targetTransform).sizeDelta = fromRect.size;
		}
        public override void SetStart(EaseRefererceTransform targetTransform)
        {
            targetTransform.rect = fromRect;
        }

        public override void SetEnd(Transform targetTransform)
        {
			((RectTransform)targetTransform).anchoredPosition = toRect.position;
			((RectTransform)targetTransform).sizeDelta = toRect.size;
		}
        public override void SetEnd(EaseRefererceTransform targetTransform)
        {
            targetTransform.rect = toRect;
        }

		public void SetFromRectToCurrent(RectTransform targetTransform)
		{
			toRect = targetTransform.rect;

		}

		public void SetToRectToCurrent(RectTransform targetTransform)
		{
			toRect = targetTransform.rect;
		}
	}
}

