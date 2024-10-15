using System;
using System.Runtime.InteropServices;
using UnityEngine;

// ReSharper disable once InconsistentNaming
public class SDLTest : MonoBehaviour
{
    [DllImport("SDL3")]
    private static extern int SDL_Init(uint flags);

    [DllImport("SDL3")]
    private static extern void SDL_Quit();

    private const uint SdlInitVideo = 0x00000020;

    private void Start()
    {
        if (SDL_Init(SdlInitVideo) < 0)
        {
            Debug.LogError("SDL could not initialize! SDL_Error: " + Marshal.PtrToStringAuto(SDL_GetError()));
        }
        else
        {
            Debug.Log("SDL initialized successfully.");
        }
    }

    private void OnDestroy()
    {
        SDL_Quit();
    }

    [DllImport("SDL3")]
    private static extern IntPtr SDL_GetError();
}