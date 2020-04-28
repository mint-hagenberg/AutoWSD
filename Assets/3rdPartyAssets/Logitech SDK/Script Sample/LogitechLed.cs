// Logitech Gaming SDK
//
// Copyright (C) 2011-2014 Logitech. All rights reserved.
// Author: Tiziano Pigliucci
// Email: devtechsupport@logitech.com

using UnityEngine;
using System.Collections;

public class LogitechLed : MonoBehaviour
{

    int red, blue, green;
    public string effectLabel;

    // Use this for initialization
    void Start()
    {

        blue = 0;
        red = 0;
        green = 0;
        LogitechGSDK.LogiLedInit();
        LogitechGSDK.LogiLedSaveCurrentLighting();
        effectLabel = "Press F to test flashing effect, P to test pulsing effect \n " +
                    "Press mouse1 to set all lighting to random color, mouse 2 to set G910 to random bitmap \n" +
"Press E to start per-key effects (F1-F12) show on supported devices \n" +
                    "Press S to stop the effects \n";
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 250, 500, 200), effectLabel);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            //On mouse click set random color backlighting. In the monochrome backlighting devices it will change the brightness.
            System.Random random = new System.Random();
            red = random.Next(0, 100);
            blue = random.Next(0, 100);
            green = random.Next(0, 100);
            LogitechGSDK.LogiLedSetLighting(red, blue, green);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            byte[] bitmap = new byte[LogitechGSDK.LOGI_LED_BITMAP_SIZE];
            System.Random random = new System.Random();
            for (int i = 0; i < LogitechGSDK.LOGI_LED_BITMAP_SIZE; i++)
            {
                bitmap[i] = (byte)random.Next(0, 255);
            }
            LogitechGSDK.LogiLedSetLightingFromBitmap(bitmap);
            red = random.Next(0, 100);
            blue = random.Next(0, 100);
            green = random.Next(0, 100);
            LogitechGSDK.LogiLedSetLightingForTargetZone(LogitechGSDK.DeviceType.Speaker, 0, red, blue, green);
        }
        if (Input.GetKey(KeyCode.F))
        {
            //Flashing preset effect
            System.Random random = new System.Random();
            red = random.Next(0, 100);
            blue = random.Next(0, 100);
            green = random.Next(0, 100);
            LogitechGSDK.LogiLedFlashLighting(red, blue, green, LogitechGSDK.LOGI_LED_DURATION_INFINITE, 200);
        }
        if (Input.GetKey(KeyCode.P))
        {
            //Pulsing preset effect
            System.Random random = new System.Random();
            red = random.Next(0, 100);
            blue = random.Next(0, 100);
            green = random.Next(0, 100);
            LogitechGSDK.LogiLedPulseLighting(red, blue, green, LogitechGSDK.LOGI_LED_DURATION_INFINITE, 100);
        }

        if (Input.GetKey(KeyCode.E))
        {
            //Pulsing preset effect
            System.Random random = new System.Random();
            red = random.Next(0, 100);
            blue = random.Next(0, 100);
            green = random.Next(0, 100);
            int finishRed = random.Next(0, 100);
            int finishBlue = random.Next(0, 100);
            int finishGreen = random.Next(0, 100);

            LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F1, finishRed, finishGreen, finishBlue, red, green, blue, 100, true);
            LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F2, finishRed, finishGreen, finishBlue, red, green, blue, 100, true);
            LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F3, finishRed, finishGreen, finishBlue, red, green, blue, 100, true);
            LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F4, finishRed, finishGreen, finishBlue, red, green, blue, 100, true);

            red = random.Next(0, 100);
            blue = random.Next(0, 100);
            green = random.Next(0, 100);
            int duration = random.Next(50, 200);

            LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F5, red, green, blue, 0, duration);
            LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F6, red, green, blue, 0, duration);
            LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F7, red, green, blue, 0, duration);
            LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F8, red, green, blue, 0, duration);
        }
        if (Input.GetKey(KeyCode.S))
        {
            LogitechGSDK.LogiLedStopEffects();
        }
    }

    void OnDestroy()
    {
        //Before quitting, we need to restore the user's backlighting settings
        LogitechGSDK.LogiLedRestoreLighting();
        LogitechGSDK.LogiLedShutdown();
    }
}
