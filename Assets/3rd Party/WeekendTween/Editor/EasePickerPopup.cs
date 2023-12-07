using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

namespace noWeekend
{
	public class EasePickerPopup : EditorWindow
	{
		public static EasePickerPopup instance;

		private const int cellSize = 100;
		private const int grindColumns = 6;
		private const int titleBoxHeight = 20;
		private const int margin = 3;
		private const float lineWidth = 4;
		private const int lineResolution = 32;
		private const int borderSize = 15;
		private const int gridSize = (grindColumns) * cellSize;

		private EaseAction easeAction;

		//GUI Styles
		GUIStyle boxStyle;

		//Create a window object
		public static EasePickerPopup Open(EaseAction easeAction)
		{
			//Close a copy of this window if one is currently open;
			if (EasePickerPopup.instance != null)
			{
				EasePickerPopup.instance.CloseWindow();
			}
			var win = (EasePickerPopup)EditorWindow.GetWindow(typeof(EasePickerPopup), true, "Ease Picker");
			win.easeAction = easeAction;
			win.ShowUtility();
			win.minSize = new Vector2(gridSize + borderSize * 2, gridSize  + borderSize* 2);
			win.maxSize = win.minSize;

			instance = win;
			return win;
		}

		/// Colors used to draw window
		public static class EaseColors
		{
			//Background color.
			public static Color Background = new Color(0.82f, 0.18f, 0.24f, 1f);

			public static Color SquareBackground = new Color(0.14f, 0.24f, 0.32f, 1f);
			public static Color SelectedBackground = new Color(0.01f, 0.88f, 0.75f, 1f);

			public static Color Line = Color.white;
			public static Color SelectedLine = Color.black;

			public static Color Title = Color.white;
			public static Color SelectedTitle = SelectedBackground;
		}

		//On GUI
		void OnGUI()
		{
			//If there alredy is a window open, then close it
			if (easeAction == null) CloseWindow();

			//Create the main rect
			Rect mainRect = GUILayoutUtility.GetRect(gridSize, gridSize, GUILayout.ExpandWidth(false));
			mainRect.y = borderSize;
			mainRect.x = borderSize;

			//Draw Background
			GUI.color = EaseColors.Background;
			GUI.Box(mainRect, "");

			//Get all the ease tyles
			EaseType[] easeTypes = (EaseType[])Enum.GetValues(typeof(EaseType));

			//Draw each ease type box
			for (int i = 0; i < easeTypes.Length; i++)
			{
				//Get the ease type
				EaseType easeType = easeTypes[i];

				//We are not drawing animation ease types yet
				if(easeType == EaseType.Animation) continue;

				//Check if it's the current ease type of the selected ease action
				bool current = easeAction.easeType == easeType;

				//Work out the x and y coordinates of the box
				int x = i % grindColumns;
				int y = Mathf.FloorToInt(i / (float)grindColumns);

				//Get size and positions of the title and body rects
				Rect titleRect = new Rect(mainRect.x + x * cellSize , mainRect.y + y * cellSize , cellSize, titleBoxHeight);
				Rect bobyRect = new Rect(mainRect.x + x * cellSize + margin, mainRect.y + y * cellSize + titleBoxHeight, cellSize - margin *2, cellSize- titleBoxHeight);

				//Draw the title box
				GUI.color = current ? EaseColors.SelectedTitle : EaseColors.Title;
				GUI.Box(titleRect, Ease.EaseName(easeType));

				//Draw a hidden button behind each box
				if (GUI.Button(bobyRect, ""))
				{
					easeAction.easeType = easeType;
				}

				//Draw each main ease box
				DrawEase(bobyRect, easeType, current);
			}
		}

		//Draw the ease box
		public void DrawEase(Rect rect, EaseType easeType,bool current, int _resolution = lineResolution)
		{
			DrawRect(rect, current ? EaseColors.SelectedBackground: EaseColors.SquareBackground, Color.white);
			DrawCurve(rect, easeType, 50, current ? EaseColors.SelectedLine : EaseColors.Line, lineWidth);
		}

		//Draw a curve inside the given rect
		void DrawCurve(Rect rect,EaseType easeType, int resolution, Color color, float width)
		{
			//Draw a smaller rect with a border
			rect.y += 18;
			rect.x += 5;
			rect.width -= 10;
			rect.height -= 36;

			//plot all the point for the line to be drawn
			Vector3[] lineVertices = new Vector3[resolution];

			for (int i = 0; i < resolution; i++)
			{
				float time = i / (float)(resolution -1);
				float value = Ease.GetEase(easeType, time) * rect.height;

				lineVertices[i] = new Vector2((time) * rect.width, 1- value) + new Vector2(rect.xMin, rect.yMax + EaseLineOffset(easeType));
			}

			//Draw the line
			Handles.color = color;
			Handles.DrawAAPolyLine(width, lineVertices);
		}

		//Some of the eases break outside the box, this moved them up or down to compensate
		private float EaseLineOffset(EaseType easeType)
		{
			if (easeType == EaseType.ElasticIn)
			{
				return -14;
			}
			else if (easeType == EaseType.ElasticOut)
			{
				 return 10;
			}
			return 0;
		}

		//Draw a rect
		void DrawRect(Rect rect, Color fill, Color line)
		{
			Handles.color = fill;
			Handles.DrawSolidRectangleWithOutline(rect, fill, line);
		}

		//Close the window
		public void CloseWindow()
		{
			instance = null;
			Close();
		}
	}
}