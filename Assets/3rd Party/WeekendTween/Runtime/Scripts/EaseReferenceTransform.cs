using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace noWeekend
{
	public class EaseRefererceTransform
	{
		public Transform transform;
		public Vector3 localPosition;
		public Vector3 position;
		public Vector3 localScale;
		public Quaternion localRotation;
		public Rect rect;
		public Bounds bounds;
		public bool isRectTransform;

		public Vector3 lossyScale;

		public EaseRefererceTransform(Transform referenceTransform)
		{
			this.transform = referenceTransform;
			this.localPosition = referenceTransform.localPosition;
			this.localScale = referenceTransform.localScale;
			this.localRotation = referenceTransform.localRotation;
			this.lossyScale = referenceTransform.lossyScale;
			if (referenceTransform.GetType() == typeof(RectTransform))
			{
				rect = ((RectTransform)referenceTransform).rect;
				isRectTransform = true;
			}
			else
			{
				isRectTransform = false;
			}

		}
	}
}