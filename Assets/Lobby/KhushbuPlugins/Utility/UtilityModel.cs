using KhushbuPlugin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityModel : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------
    //Local variables -----------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    public enum GraphicsMode { Low, Medium, High, Ultra }
    public enum GameControlMode { Touch, JoystickLeft, JoystickRight }

    private static bool Is_Music = false;
    private static bool Is_Sound = false;
    private static bool Is_Vibration = true;
    private static GraphicsMode GameGraphics = GraphicsMode.Medium;
    private static GameControlMode GameControl = GameControlMode.Touch;

    private static string Key_Music = "KPhoenix_Music";
    private static string Key_Sound = "KPhoenix_Sound";
    private static string Key_Vibration = "KPhoenix_Vibration";
    private static string Key_Graphics = "KPhoenix_Graphics";
    private static string Key_GameControl = "KPhoenix_GameControl";

    //---------------------------------------------------------------------------------------------------------
    //End Local variables -------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------

    public static void SetMusic(bool _set)
    {
        DatabaseScript.SetDataWithoutKey(Key_Music, _set.ToString().ToLower());
    }

    public static bool GetMusic()
    {
        return DefaultValues.Bool(DatabaseScript.GetDataWithoutKey(Key_Music), Is_Music.ToString());
    }

    public static void SetSound(bool _set)
    {
        DatabaseScript.SetDataWithoutKey(Key_Sound, _set.ToString().ToLower());
    }

    public static bool GetSound()
    {
        return DefaultValues.Bool(DatabaseScript.GetDataWithoutKey(Key_Sound), Is_Sound.ToString());
    }

    public static void SetVibration(bool _set)
    {
        DatabaseScript.SetDataWithoutKey(Key_Vibration, _set.ToString().ToLower());
    }

    public static bool GetVibration()
    {
        return DefaultValues.Bool(DatabaseScript.GetDataWithoutKey(Key_Vibration), Is_Vibration.ToString());
    }

    public static void SetGameGraphics(GraphicsMode _set)
    {
        DatabaseScript.SetDataWithoutKey(Key_Graphics, _set.ToString());
    }

    public static GraphicsMode GetGameGraphics()
    {
        return DefaultValues.Custom<GraphicsMode>(DatabaseScript.GetDataWithoutKey(Key_Graphics), GameGraphics);
    }

    public static void SetGameControl(GameControlMode _set)
    {
        DatabaseScript.SetDataWithoutKey(Key_GameControl, _set.ToString());
    }

    public static GameControlMode GetGameControl()
    {
        return DefaultValues.Custom<GameControlMode>(DatabaseScript.GetDataWithoutKey(Key_GameControl), GameControl);
    }
}
