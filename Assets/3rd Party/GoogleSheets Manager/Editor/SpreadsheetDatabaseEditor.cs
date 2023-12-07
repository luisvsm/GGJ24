using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GoogleSheetManager
{

    [CustomEditor(typeof(SpreadsheetDatabase))]
    public class SpreadsheetDatabaseEditor : Editor
    {

        private SpreadsheetDatabase Script;

        public void OnEnable()
        {
            Script = (SpreadsheetDatabase)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(Script.DatabaseName);
            EditorGUILayout.Space(10);

            float width = Screen.width/2 * 0.9f;
            EditorGUILayout.BeginHorizontal();
            foreach (string header in Script.Headers)
			{
                GUILayout.Label(header, GUILayout.Width(width / Script.Headers.Length));
			}
            EditorGUILayout.EndHorizontal();
            foreach (SpreadsheetDataSet dataset in Script.DataSets)
			{
                EditorGUILayout.BeginHorizontal();
				for (int i = 0; i < dataset.Values.Length; i++)
				{
                    dataset.Values[i] = GUILayout.TextField(dataset.Values[i], GUILayout.Width(width / Script.Headers.Length));
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(Script);
            }
        }
    }

}