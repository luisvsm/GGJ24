using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;

namespace GoogleSheetManager
{

    [CustomEditor(typeof(GoogleSheetManager))]
    public class GoogleSheetManagerEditor : Editor
    {
        //Constants for parsing downloaded CSV's
        public static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        public static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        public static readonly char[] TRIM_CHARS = { '\"' };
        private GoogleSheetManager Script;

        private int CurrentlyViewedTable = int.MaxValue;

        public void OnEnable()
        {
            this.serializedObject.Update();
            Script = (GoogleSheetManager)target;
        }

        public override void OnInspectorGUI()
        {
            //Sheet Settings
            EditorGUILayout.LabelField("Google Sheets Downloader", EditorStyles.boldLabel);
            Script.SpreadSheetID = EditorGUILayout.TextField("Google Sheet ID:", Script.SpreadSheetID);
            Script.DatabasePath = EditorGUILayout.TextField("Database Path:", Script.DatabasePath);
            GUILayout.Space(30);

            //Header

            EditorGUILayout.LabelField("Pages", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);

            //Loop through all the saved sheets
            for (int i = 0; i < Script.DataSheets.Count; i++)
            {
                //Settings for the sheet
                Script.DataSheets[i].DatabaseName = EditorGUILayout.TextField("Database Name:", Script.DataSheets[i].DatabaseName);
                Script.DataSheets[i].TabId = EditorGUILayout.TextField("Tab GUI:", Script.DataSheets[i].TabId);

                if (!string.IsNullOrEmpty(Script.DataSheets[i].CSV))
                {
                    GUILayout.BeginHorizontal();
                }

                if (GUILayout.Button("Open Sheet", GUILayout.Width(100)))
                {
                    Application.OpenURL("https://docs.google.com/spreadsheets/d/" + Script.SpreadSheetID + "/edit#gid=" + Script.DataSheets[i].TabId);
                }

                //Draw the table of data
                if (!string.IsNullOrEmpty(Script.DataSheets[i].CSV))
                {
                    if (CurrentlyViewedTable == i)
                    {
                        if (GUILayout.Button("Hide Data", GUILayout.Width(100)))
                        {
                            CurrentlyViewedTable = int.MaxValue;
                        }
                        GUILayout.EndHorizontal();
                        DrawTable(Script.DataSheets[i].CSV);
                    }
                    else
                    {
                        if (GUILayout.Button("Show Data", GUILayout.Width(100)))
                        {
                            CurrentlyViewedTable = i;
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                GUILayout.Space(10);

                //Download and Delete Buttons
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Download"))
                {
                    if (string.IsNullOrEmpty(Script.DataSheets[i].TabId))
                    {
                        EditorUtility.DisplayDialog("Tab ID Missing", "Plase add a Tab Id", "OK");
                    }
                    else
                    {
                        DownloadSpreadsheet(Script.SpreadSheetID, Script.DataSheets[i].TabId, Script.DataSheets[i]);
                        CurrentlyViewedTable = i;
                        Script.DataSheets[i].IsUptodate = false;
                    }
                }
                if (GUILayout.Button("Delete"))
                {
                    if (EditorUtility.DisplayDialog("Delete Sheer?", "Are you sure you wish to delete this sheet?", "Yes!", "No!"))
                    {
                        Script.DataSheets.RemoveAt(i);
                        EditorUtility.SetDirty(Script);
                        return;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);

                //Database reference and create database button
                if (!string.IsNullOrEmpty(Script.DataSheets[i].CSV))
                {
                    GUILayout.BeginHorizontal();
                    Script.DataSheets[i].DatabaseObject = (SpreadsheetDatabase)EditorGUILayout.ObjectField("Database", Script.DataSheets[i].DatabaseObject, typeof(SpreadsheetDatabase), false);
                    if (Script.DataSheets[i].DatabaseObject == null)
                    {
                        if (GUILayout.Button("Create Database"))
                        {
                            if (string.IsNullOrEmpty(Script.DataSheets[i].DatabaseName))
                            {
                                EditorUtility.DisplayDialog("Database Name Missing", "Plase add a database name", "OK");
                            }
                            else
                            {
                                Script.DataSheets[i].DatabaseObject = CreateDatebaseScriptableObject(Script.DataSheets[i].DatabaseName);
                                EditorUtility.SetDirty(Script);
                            }
                        }
                    }
                    else
                    {


                        if (!Script.DataSheets[i].IsUptodate)
                        {
                            GUI.backgroundColor = Color.green;
                        }

                        if (GUILayout.Button("Update Database" + (Script.DataSheets[i].IsUptodate ? "" : " *")))
                        {
                            if (string.IsNullOrEmpty(Script.DataSheets[i].CSV))
                            {
                                EditorUtility.DisplayDialog("No Data Loaded", "There is no data loaded from this spreadsheet, please download the data", "OK");
                            }
                            else
                            {
                                Script.DataSheets[i].DatabaseObject.LoadData(ParseCSV(Script.DataSheets[i].CSV,false), Script.DataSheets[i].DatabaseName);
                                EditorUtility.SetDirty(Script.DataSheets[i].DatabaseObject);

                                Script.DataSheets[i].IsUptodate = true;
                                Debug.Log("Updated database");
                            }
                        }

                        GUI.backgroundColor = Color.white;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Space(10);
            }

            //Add Sheet Button
            if (GUILayout.Button("+"))
            {
                Script.DataSheets.Add(new DataSheet());
            }

            //On changed update the scriptable object
            if (GUI.changed)
            {
                EditorUtility.SetDirty(Script);
            }

            this.serializedObject.ApplyModifiedProperties();
        }


        //Draw the table of contents
        public void DrawTable(string csv)
        {
            if (String.IsNullOrEmpty(csv))
            {
                return;
            }
            List<List<string>> parsedCSV = ParseCSV(csv, false);

            float width = Screen.width * 0.9f;

            for (int i = 0; i < parsedCSV.Count; i++)
            {
                GUILayout.BeginHorizontal();
                foreach (string field in parsedCSV[i])
                {
                    GUILayout.Label(field, GUILayout.Width(width / parsedCSV[i].Count));
                }
                GUILayout.EndHorizontal();
            }
        }

        //Download the spreadsheet
        public void DownloadSpreadsheet(string sheetID, string tabID, DataSheet dataSheet)
        {
            Action<string> commCallback = (csv) =>
            {
                LoadCSVText(dataSheet, csv);
                EditorUtility.SetDirty(Script);
            };

            EditorCoroutineUtility.StartCoroutineOwnerless(DownloadCSV(sheetID, tabID, commCallback));

        }

        //Create a new database scriptable object
        public SpreadsheetDatabase CreateDatebaseScriptableObject(string databaseName)
        {
            SpreadsheetDatabase newDatabase = CreateInstance<SpreadsheetDatabase>();
            string path = Script.DatabasePath + Path.DirectorySeparatorChar + databaseName + ".asset";
            AssetDatabase.CreateAsset(newDatabase, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return newDatabase;
        }

        //Loads downloaded data into a data sheet 
        public void LoadCSVText(DataSheet dataSheet, string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                dataSheet.CSV = text;

                Debug.Log("Downloaded Data \n" + text);
            }
            else
            {
                Debug.LogWarning("Could not read data or is empty");
            }
        }

        //Clean CSV
        public string CleanReturnInCsvTexts(string text)
        {
            text = text.Replace("\"\"", "'");

            if (text.IndexOf("\"") > -1)
            {
                string clean = "";
                bool insideQuote = false;
                for (int j = 0; j < text.Length; j++)
                {
                    if (!insideQuote && text[j] == '\"')
                    {
                        insideQuote = true;
                    }
                    else if (insideQuote && text[j] == '\"')
                    {
                        insideQuote = false;
                    }
                    else if (insideQuote)
                    {
                        if (text[j] == '\n')
                            clean += "<br>";
                        else if (text[j] == ',')
                            clean += "<c>";
                        else
                            clean += text[j];
                    }
                    else
                    {
                        clean += text[j];
                    }
                }
                text = clean;
            }
            return text;
        }

        //Download the CSV
        public IEnumerator DownloadCSV(string docId, string sheetId, Action<string> callback)
        {
            string url = "https://docs.google.com/spreadsheets/d/" + docId + "/export?format=csv";

            if (!string.IsNullOrEmpty(sheetId))
                url += "&gid=" + sheetId;


            var loaded = new UnityWebRequest(url);
            loaded.downloadHandler = new DownloadHandlerBuffer();
            yield return loaded.SendWebRequest();


            if (!string.IsNullOrEmpty(loaded.error))
            {
                Debug.Log("Error downloading: " + loaded.error);
            }
            else
            {
                callback(loaded.downloadHandler.text);
            }

        }

        public List<List<string>> ParseCSV(string text, bool ignoreHeaders = true)
        {
            text = CleanReturnInCsvTexts(text);

            var list = new List<List<string>>();
            var lines = Regex.Split(text, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;

            var header = Regex.Split(lines[0], SPLIT_RE);

            bool jumpedFirst = ignoreHeaders;

            foreach (var line in lines)
            {
                if (jumpedFirst)
                {
                    jumpedFirst = false;
                    continue;
                }
                var values = Regex.Split(line, SPLIT_RE);

                var entry = new List<string>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    var value = values[j];
                    value = DecodeSpecialCharsFromCSV(value);
                    entry.Add(value);
                }
                list.Add(entry);
            }
            return list;
        }

        public static string DecodeSpecialCharsFromCSV(string value)
        {
            value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "").Replace("<br>", "\n").Replace("<c>", ",");
            return value;
        }
    }
}