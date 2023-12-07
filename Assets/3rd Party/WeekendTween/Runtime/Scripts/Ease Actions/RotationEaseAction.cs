using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace noWeekend
{
	[System.Serializable]
	public class RotateEaseAction : EaseAction
	{

		public override string EaseName => "Ease Rotation";
		public Quaternion toRotation;
		public Quaternion fromRotation;
		public override WeekendTween.EaseActionType EaseActionType => WeekendTween.EaseActionType.Rotation;

		public RotateEaseAction(Quaternion rotations)
		{
			this.toRotation = rotations;
			this.fromRotation = rotations;
		}

		public RotateEaseAction(Quaternion toRotation, Quaternion fromRotation)
		{
			this.toRotation = toRotation;
			this.fromRotation = fromRotation;
		}

		protected override void ApplyEase(Transform targetTransform, float value)
		{
			targetTransform.localRotation = Quaternion.LerpUnclamped(fromRotation, toRotation, Ease.GetEase(easeType, value));
		}
		protected override void ApplyEase(EaseRefererceTransform targetTransform, float value)
		{
			targetTransform.localRotation = Quaternion.LerpUnclamped(fromRotation, toRotation, Ease.GetEase(easeType, value));
		}

		public override void SetStart(Transform targetTransform)
		{
			targetTransform.localRotation = fromRotation;
		}
		public override void SetEnd(Transform targetTransform)
		{
			targetTransform.localRotation = toRotation;
		}
		public override void SetStart(EaseRefererceTransform targetTransform)
		{
			targetTransform.localRotation = fromRotation;
		}
		public override void SetEnd(EaseRefererceTransform targetTransform)
		{
			targetTransform.localRotation = toRotation;
		}

		public void SetFromRotationToCurrent(Transform targetTransform)
		{
			toRotation = targetTransform.localRotation;

		}

		public void SetToRotationToCurrent(Transform targetTransform)
		{
			toRotation = targetTransform.localRotation;
		}
	}
}