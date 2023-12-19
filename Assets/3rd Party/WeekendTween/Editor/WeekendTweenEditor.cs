using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace noWeekend
{
	[CustomEditor(typeof(WeekendTween))]
	public class WeekendTweenEditor : Editor
	{
		private enum State { None, Activate, Deactivate }
		private State previewState = State.None;

		private WeekendTween myTarget;

		private SerializedProperty onActivateCompleteAction;
		private SerializedProperty onDeactivateCompleteAction;
		private SerializedProperty targetTransform;

		private void OnEnable()
		{
			myTarget = (WeekendTween)target;

			onActivateCompleteAction = serializedObject.FindProperty("onActivateCompleteAction");
			onDeactivateCompleteAction = serializedObject.FindProperty("onDeactivateCompleteAction");
			targetTransform = serializedObject.FindProperty("targetTransform");
		}

		//Update the scene view
		void OnSceneGUI()
		{
			if (myTarget == null || myTarget.targetTransform == null) return;
			if (previewState == State.None) return;
			if (Application.isPlaying) return;
			if (myTarget.activateEaseActions.Count == 0 && previewState == State.Activate) return;
			if (myTarget.deactivateEaseActions.Count == 0 && previewState == State.Deactivate) return;

			if (previewState == State.Activate)
			{
				myTarget.ActivateEditorTimer = Time.realtimeSinceStartup;
			}
			else
			{
				myTarget.DeactivateEditorTimer = Time.realtimeSinceStartup;
			}

			EaseRefererceTransform refererceTransform = new EaseRefererceTransform(myTarget.targetTransform);

			WeekendTween myObj = (WeekendTween)target;

			//Process each ease in active/deactive lists
			foreach (EaseAction easeAction in previewState == State.Activate ? myTarget.activateEaseActions : myTarget.deactivateEaseActions)
			{
				easeAction.Process(refererceTransform, myTarget.EditorTimer);
			}

			//update the references
			UpdateReferenceTransform(refererceTransform);

			//redraw the scene.
			SceneView.RepaintAll();
		}

		private void UpdateReferenceTransform(EaseRefererceTransform refererceTransform)
		{
			Vector3 dimentions = refererceTransform.isRectTransform ? GetDimentionsRect(refererceTransform) : GetDimentions(refererceTransform);
			Handles.color = Color.magenta;

			Handles.matrix = Matrix4x4.TRS(
				refererceTransform.localPosition + refererceTransform.transform.parent.position,
				refererceTransform.localRotation,
				refererceTransform.localScale);

			//Draw wire cube around object
			Handles.DrawWireCube(Vector3.zero, new Vector3(dimentions.x, dimentions.y, dimentions.z));

		}

		private Vector3 GetDimentionsRect(EaseRefererceTransform refererceTransform)
		{
			return new Vector3(
				refererceTransform.rect.width * refererceTransform.lossyScale.x,
				refererceTransform.rect.height * refererceTransform.lossyScale.y
			);
		}

		private Vector3 GetDimentions(EaseRefererceTransform refererceTransform)
		{
			if (refererceTransform.transform.GetComponent<Renderer>() != null)
			{
				return refererceTransform.transform.GetComponent<Renderer>().bounds.extents * 2;
			}

			if (refererceTransform.transform.GetComponentInChildren<Renderer>() != null)
			{
				return refererceTransform.transform.GetComponentInChildren<Renderer>().bounds.extents * 2;
			}

			return Vector3.one;
		}

		public override void OnInspectorGUI()
		{
			//Target Transform
			serializedObject.Update();
			EditorGUILayout.PropertyField(targetTransform, new GUIContent("Transform"));
			serializedObject.ApplyModifiedProperties();

			//If there is no target Transform then don't show anything else
			if (myTarget.targetTransform == null)
			{
				return;
			}

			//Select if the active tweens play when a gameobject is enabled
			myTarget.activateOnEnable = EditorGUILayout.Toggle("Activate on Enable", myTarget.activateOnEnable);
			//if (activateOnEnableValue != myTarget.activateOnEnable)
			//{
			//	myTarget.activateOnEnable = activateOnEnableValue;
			//	EditorUtility.SetDirty(myTarget);
			//}
			myTarget.hideOnEnable = EditorGUILayout.Toggle("Hide on Enable", myTarget.hideOnEnable);
			myTarget.hideAfterDisable = EditorGUILayout.Toggle("Hide after Disable", myTarget.hideAfterDisable);
			//Select if a tween initilises with the settings of 
			myTarget.initiliseActiveState = EditorGUILayout.Toggle("Initilise Active Status", myTarget.initiliseActiveState);
			//if (useInitiliseActiveStatus != myTarget.initiliseActiveState)
			//{
			//	myTarget.initiliseActiveState = useInitiliseActiveStatus;
			//	EditorUtility.SetDirty(myTarget);
			//}


			//Select if a tween uses unscaled time or not
			myTarget.useUnscaledTime = EditorGUILayout.Toggle("Use Unscaled time", myTarget.useUnscaledTime);

			//if (useUnscaledTimeValue != myTarget.useUnscaledTime)
			//{
			//	myTarget.useUnscaledTime = useUnscaledTimeValue;
			//	EditorUtility.SetDirty(myTarget);
			//}

			//space
			EditorGUILayout.Space();

			BuildEaseGroup("Activate Tweens",true, myTarget.activateEaseActions,onActivateCompleteAction);
			BuildEaseGroup("Deactivate Tweens",false, myTarget.deactivateEaseActions,onDeactivateCompleteAction);

			serializedObject.ApplyModifiedProperties();
		}

		private void BuildEaseGroup(string groupName,bool activateActions, List<EaseAction> easeActions, SerializedProperty completeAction)
		{
			//Main Vertical group
			GUILayout.BeginVertical(groupName, "window");

			//Show Tweens
			for (int i = 0; i < easeActions.Count; i++)
			{
				EaseAction easeAction = easeActions[i];	//Get Ease Action

				GUILayout.BeginVertical(easeAction.EaseName, "window");

				GUILayout.Space(10);	//space

				//Build each ease type.
				switch (easeAction.EaseActionType)
				{
					case WeekendTween.EaseActionType.Position:
						BuildPosition(easeAction);
						break;
					case WeekendTween.EaseActionType.Rotation:
						BuildRotation(easeAction);
						break;
					case WeekendTween.EaseActionType.Scale:
						BuildScale(easeAction);
						break;
					default:
						break;
				}

				GUILayout.Space(10);    //space

				//Settings and Ease type button
				GUILayout.BeginHorizontal();

				//Ease Type Window Popup
				if (GUILayout.Button(Ease.EaseName(easeAction.easeType), GUILayout.Width(100), GUILayout.Height(50)))
				{
					EasePickerPopup.Open(easeAction);
				}

				//Settings
				GUILayout.BeginVertical();
				easeAction.duration = EditorGUILayout.FloatField("Duration", easeAction.duration);
				easeAction.delay = EditorGUILayout.FloatField("Delay", easeAction.delay);
				GUILayout.EndVertical();

				// End Settings and Ease type button
				GUILayout.EndHorizontal();

				GUILayout.Space(10);	//space

				//Delete Button
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();

				if (GUILayout.Button("x", GUILayout.Width(30)))
				{
					myTarget.RemoveEaseAction(easeAction, activateActions);
					EditorUtility.SetDirty(myTarget);
				}

				//End Delete Button
				GUILayout.EndHorizontal();

				//End Settings
				GUILayout.EndVertical();
			}


			//Buttons
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("+ Position"))
			{
				myTarget.OnAddPositionEaseButtonPress(activateActions);

				EditorUtility.SetDirty(myTarget);
			}

			if (GUILayout.Button("+ Rotation"))
			{
				myTarget.OnAddRotationEaseButtonPress(activateActions);
				EditorUtility.SetDirty(myTarget);
			}

			if (GUILayout.Button("+ Scale"))
			{
				myTarget.OnAddScaleEaseButtonPress(activateActions);
				EditorUtility.SetDirty(myTarget);
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.EndVertical();

			//space
			EditorGUILayout.Space();

			//Preview ActiveTweens
			if (easeActions.Count > 0 && !Application.isPlaying)
			{
				State correctState = (activateActions ? State.Activate : State.Deactivate);

				bool preview = EditorGUILayout.Toggle("Preview", previewState == correctState);

				if(preview != (previewState == correctState))
				{
					if(preview == false)
					{
						previewState = State.None;
					}
					else
					{
						previewState = correctState;
					}
					SceneView.RepaintAll();
				}
			}

			//If editor is playing then show buttons for testing the actual tween
			if (easeActions.Count > 0 &&  Application.isPlaying)
			{
				GUILayout.BeginHorizontal();

				GUILayout.Label("Live Test");

				//Ease Type Window Popup
				if (GUILayout.Button("Test"))
				{
					if (activateActions)
					{
						myTarget.Activate();
					}
					else
					{
						myTarget.Deactivate();
					}
					
				}

				EditorGUILayout.EndHorizontal();
			}

			//space
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(completeAction);
			EditorGUILayout.Space();
		}

		//On Destory of custom inspector
		private void OnDestroy()
		{
			if (EasePickerPopup.instance != null)
			{
				EasePickerPopup.instance.CloseWindow();
			}
		}


		//Build Ease Actions
		// ----------------------------------------------------


		//Build Position Ease Action
		public void BuildPosition(EaseAction easeAction)
		{
			PositionEaseAction positionEaseAction = easeAction as PositionEaseAction;

			EditorGUILayout.BeginHorizontal();
			positionEaseAction.fromPosition = EditorGUILayout.Vector3Field("Start Position", positionEaseAction.fromPosition);

			if (GUILayout.Button("R", GUILayout.Width(20)))
			{
				positionEaseAction.SetFromPositionToCurrent(myTarget.targetTransform);
				EditorUtility.SetDirty(myTarget);
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			positionEaseAction.toPosition = EditorGUILayout.Vector3Field("End Position", positionEaseAction.toPosition);

			if (GUILayout.Button("R", GUILayout.Width(20)))
			{
				positionEaseAction.SetToPositionToCurrent(myTarget.targetTransform);
				EditorUtility.SetDirty(myTarget);
			}

			EditorGUILayout.EndHorizontal();
		}

		//Build Rotation Ease Action
		public void BuildRotation(EaseAction easeAction)
		{
			RotateEaseAction positionEaseAction = easeAction as RotateEaseAction;

			EditorGUILayout.BeginHorizontal();
			positionEaseAction.fromRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Start Rotation", positionEaseAction.fromRotation.eulerAngles));

			if (GUILayout.Button("R", GUILayout.Width(20)))
			{
				positionEaseAction.SetFromRotationToCurrent(myTarget.targetTransform);
				EditorUtility.SetDirty(myTarget);
			}

			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal();
			positionEaseAction.toRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("End Rotation", positionEaseAction.toRotation.eulerAngles));

			if (GUILayout.Button("R", GUILayout.Width(20)))
			{
				positionEaseAction.SetToRotationToCurrent(myTarget.targetTransform);
				EditorUtility.SetDirty(myTarget);
			}

			EditorGUILayout.EndHorizontal();
		}

		//Build Scale Ease Action
		public void BuildScale(EaseAction easeAction)
		{
			ScaleEaseAction positionEaseAction = easeAction as ScaleEaseAction;

			EditorGUILayout.BeginHorizontal();
			positionEaseAction.fromScale = EditorGUILayout.Vector3Field("Start Scale", positionEaseAction.fromScale);

			if (GUILayout.Button("R", GUILayout.Width(20)))
			{
				positionEaseAction.SetFromScaleToCurrent(myTarget.targetTransform);
				EditorUtility.SetDirty(myTarget);
			}

			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal();
			positionEaseAction.toScale = EditorGUILayout.Vector3Field("End Scale", positionEaseAction.toScale);

			if (GUILayout.Button("R", GUILayout.Width(20)))
			{
				positionEaseAction.SetToScaleToCurrent(myTarget.targetTransform);
				EditorUtility.SetDirty(myTarget);
			}

			EditorGUILayout.EndHorizontal();
		}



	}
}
