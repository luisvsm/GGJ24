using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SaveController
{
    public static readonly string Demo_Int_Key = "demo_int_key";
	public static readonly string Demo_String_Key = "demo_string_key";
	public static readonly string Demo_Float_Key = "demo_float_key";
	public static readonly string Demo_Objects_Key = "demo_objects_key";

	public static void ClearData()
	{
		PlayerPrefs.DeleteAll();
	}

	// Standard int save/load
	public static void SaveDemoInt(int demoValue)
    {
        PlayerPrefs.SetInt(Demo_Int_Key, demoValue);
    }

    public static int LoadDemoInt()
    {
		return PlayerPrefs.GetInt(Demo_Int_Key, 0);
	}

	// Standard string save/load
	public static void SaveDemoString(string demoValue)
	{
		PlayerPrefs.SetString(Demo_String_Key, demoValue);
	}

	public static string LoadDemoString()
	{
		return PlayerPrefs.GetString(Demo_String_Key, "");
	}

	// Standard float save/load
	public static void SaveDemoFloat(int demoValue)
	{
		PlayerPrefs.SetFloat(Demo_Float_Key, demoValue);
	}

	public static float LoadDemoFloat()
	{
		return PlayerPrefs.GetFloat(Demo_Float_Key, 0);
	}

	// Standard object save/load

	[SerializeField]
	private static List<DemoObject> demoObjects;

	public static List<DemoObject> LoadDemoObjects()
	{
		if(demoObjects == null)
		{
			LoadDemoObjectsFromPlayerPrefs();
		}

		return demoObjects;
	}

	public static void AddDemoObject(DemoObject demoObject, bool doSave = true)
	{
		if (demoObjects == null)
		{
			LoadDemoObjectsFromPlayerPrefs();
		}

		demoObjects.Add(demoObject);

		if (doSave)
		{
			SaveDemoObjects();
		}
	}

	public static void SaveDemoObjects()
	{
		if (demoObjects == null)
		{
			LoadDemoObjectsFromPlayerPrefs();
		}

		string saveString = string.Join('~', demoObjects);

		PlayerPrefs.SetString(Demo_Objects_Key, saveString);
	}

	private static void LoadDemoObjectsFromPlayerPrefs()
	{
		string loadString = PlayerPrefs.GetString(Demo_Objects_Key, "");

		if (string.IsNullOrEmpty(loadString))
		{
			demoObjects = new();
			return;
		}

		demoObjects = loadString
		.Split('~')
		.Select(item =>
		{
			string[] moreSplit = item.Split(',');
			return new DemoObject(int.Parse(moreSplit[0]), moreSplit[1]);
		})
		.ToList();

		string[] loadStringSplit = loadString.Split('~');
	}  

	public class DemoObject
	{
		public int value1;
		public string value2;

		public DemoObject(int value1, string value2)
		{
			this.value1 = value1;
			this.value2 = value2;
		}

		public override string ToString()
		{
			return $"{value1},{value2}";
		}
	}

	

}
