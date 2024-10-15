using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerImageWrapper
{
    public static string ControllerImageBinary { get; private set; }
    private const string BinPath = "/Plugins/ControllerImage/Lib";

    #if UNITY_STANDALONE_WIN
        private const string LibName = "ControllerImage.dll";
    #elif UNITY_STANDALONE_LINUX
        private const string LibName = "libControllerImage.so";
    #elif UNITY_STANDALONE_OSX
        private const string LibName = "libControllerImage.dylib";
    #endif

    [DllImport(LibName)]
    private static extern int ControllerImage_Init();

    [DllImport(LibName)]
    private static extern void ControllerImage_Quit();  

    [DllImport(LibName)]
    public static extern IntPtr ControllerImage_CreateGamepadDevice(IntPtr devicePointer);
    
    [DllImport(LibName)]
    public static extern IntPtr ControllerImage_CreateGamepadDeviceByIndex(int index);

    [DllImport(LibName)]
    public static extern void ControllerImage_DestroyDevice(IntPtr device);
    
    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int ControllerImage_AddDataFromFile(string path);
    
    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)] // "ControllerImage"
    public static extern IntPtr ControllerImage_CreateSurfaceForButton(IntPtr imgDev, int button, int width, int height);
    
    public void InitControllerImage()
    {
        ControllerImageBinary = Application.dataPath + BinPath + "/controllerimage-standard.bin";
        
        if (ControllerImage_Init() == -1)
        {
            Debug.LogError("ControllerImage_Init() failed");
        }

        if (ControllerImage_AddDataFromFile(ControllerImageBinary) == -1)
        {
            Debug.LogError("ControllerImage_AddDataFromFile() failed");
        }
    }
    
    public bool LoadControllerImageData(string filePath)
    {
        int result = ControllerImage_AddDataFromFile(filePath);
        if (result == -1)
        {
            Debug.LogError("Failed to load controller image data: " + filePath);
            return false;
        }
        return true;
    }

    public void OnApplicationQuit()
    {
        ControllerImage_Quit();
    }
}