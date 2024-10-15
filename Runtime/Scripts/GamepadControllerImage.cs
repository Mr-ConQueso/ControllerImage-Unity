using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamepadControllerImage : MonoBehaviour
{
    [SerializeField] private RawImage image;
    private IntPtr _imgDev;
    private IntPtr _gamepad;

    private ControllerImageWrapper _controllerImageWrapper;

    private Texture2D _buttonTexture;

    private void Start()
    {
        _controllerImageWrapper = new ControllerImageWrapper();
        _controllerImageWrapper.InitControllerImage();
        
        if (!_controllerImageWrapper.LoadControllerImageData(ControllerImageWrapper.ControllerImageBinary))
        {
            Debug.LogError("Failed to load controller image data.");
            return;
        }

        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            _gamepad = ControllerImageWrapper.ControllerImage_CreateGamepadDeviceByIndex(0);
            
            if (_gamepad == IntPtr.Zero)
            {
                Debug.LogError("Failed to create ControllerImage device");
            }
            
            _imgDev = ControllerImageWrapper.ControllerImage_CreateGamepadDevice(_gamepad);
            
            if (_imgDev == IntPtr.Zero)
            {
                Debug.LogError("Failed to create ControllerImage device");
            }
            else
            {
                // Example: Render an image for the A button (button 0 is A on Xbox)
                IntPtr buttonSurface = ControllerImageWrapper.ControllerImage_CreateSurfaceForButton(_imgDev, 0, 200, 200); // Size 100x100

                if (buttonSurface != IntPtr.Zero)
                {
                    _buttonTexture = ConvertSDL_SurfaceToTexture(buttonSurface);
                    image.texture = _buttonTexture;
                    Debug.Log("Created controller image surface");
                }
            }
        }
    }

    // Conversion function from SDL_Surface to Unity Texture2D
    private Texture2D ConvertSDL_SurfaceToTexture(IntPtr sdlSurface)
    {
        // Assuming SDL_Surface has pixels and width/height data.
        // This is a simplified example; you would need to copy the pixel data to a Texture2D in Unity.
        SDL_Surface surface = Marshal.PtrToStructure<SDL_Surface>(sdlSurface);
        Texture2D texture = new Texture2D(surface.w, surface.h, TextureFormat.RGBA32, false);

        // Here we would copy pixel data from SDL_Surface to Unity Texture2D.
        // ... (Copy pixel logic)
        // Apply texture changes
        texture.Apply();

        return texture;
    }

    private void OnGUI()
    {
        if (_buttonTexture != null)
        {
            // Display the button texture on screen at a fixed position
            GUI.DrawTexture(new Rect(10, 10, 100, 100), _buttonTexture);
        }
    }

    private void OnDestroy()
    {
        if (_imgDev != IntPtr.Zero)
        {
            ControllerImageWrapper.ControllerImage_DestroyDevice(_imgDev);
        }

        _controllerImageWrapper.OnApplicationQuit();
    }
}

[StructLayout(LayoutKind.Sequential)]
// ReSharper disable once InconsistentNaming
public struct SDL_Surface
{
    public int w, h;
    public IntPtr pixels;
}