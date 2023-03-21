using UnityEngine;
using System.Collections;
using KhushbuPlugin;

public class DatabaseScript : MonoBehaviour
{
    //************************************************** All Methods Or/And Events *************************************************************
    
    internal static void SetDataWithoutKey(string key, string value, bool removeOldEntry = true)
    {
        if (removeOldEntry)
        {
            DBPlayerPrefs.DeleteKey(key.ToString());
        }
        DBPlayerPrefs.SetString(key.ToString(), value.ToString());
    }  
    internal static string GetDataWithoutKey(string key, string defaultValue = "")
    {
        string val = "";
        val = DBPlayerPrefs.GetString(key.ToString());
        if (val == "")
        {
            val = defaultValue;
        }
        return val;
    }
    
    internal static void DeleteDataWithoutKey(string key)
    {
        try
        {
            string chk = GetDataWithoutKey(key);
            if (chk != "")
            {
                DBPlayerPrefs.DeleteKey(key.ToString());
            }
        }
        catch
        {
        }
    }
    internal static void ClearAllData(bool isSure)//Again Check Are You Sure.Becasue this is testing purpose.
    {
        //********************** Important Note ************************************************
        ////This Method Call Only Our Testing.Becasue this method clear all database.
        if (isSure)
        {
            DBPlayerPrefs.DeleteAll();
        }
    }
}