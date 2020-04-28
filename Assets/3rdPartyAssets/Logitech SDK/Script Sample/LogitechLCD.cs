// Logitech Gaming SDK
//
// Copyright (C) 2011-2014 Logitech. All rights reserved.
// Author: Tiziano Pigliucci
// Email: devtechsupport@logitech.com

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class LogitechLCD : MonoBehaviour {
	
	byte[] pixelMatrix;

	
	// Use this for initialization
	void Start () {
		LogitechGSDK.LogiLcdInit("UNITY_TEST", 3);
	
		LogitechGSDK.LogiLcdColorSetTitle("Testing", 255,0,0);
		LogitechGSDK.LogiLcdColorSetText(0,"zero",255,255,0);
		LogitechGSDK.LogiLcdColorSetText(1,"first",0,255,0);
		LogitechGSDK.LogiLcdColorSetText(2,"second",0,255,30);
		LogitechGSDK.LogiLcdColorSetText(3,"third",0,255,50);
		LogitechGSDK.LogiLcdColorSetText(4,"fourth",0,255,90);
		LogitechGSDK.LogiLcdColorSetText(5,"fifth",0,255,140);
		LogitechGSDK.LogiLcdColorSetText(6,"sixth",0,255,200);
		LogitechGSDK.LogiLcdColorSetText(7,"seventh",0,255,255);
		LogitechGSDK.LogiLcdColorSetText(8,"eight",0,255,255);
		
		LogitechGSDK.LogiLcdMonoSetText(0,"testing");
		LogitechGSDK.LogiLcdMonoSetText(1,"mono");
		LogitechGSDK.LogiLcdMonoSetText(2,"chrome");
		LogitechGSDK.LogiLcdMonoSetText(3,"lcd");
		
	}
	
	// Update is called once per frame
	void Update () {
		//BUTTON TEST
		String colorButtons ="";
		String monoButtons ="";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_COLOR_BUTTON_CANCEL))
			colorButtons+= "Cancel";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_COLOR_BUTTON_DOWN))
			colorButtons+= "Down";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_COLOR_BUTTON_LEFT))
			colorButtons+= "Left";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_COLOR_BUTTON_MENU))
			colorButtons+= "Menu";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_COLOR_BUTTON_OK))
			colorButtons+= "Ok";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_COLOR_BUTTON_RIGHT))
			colorButtons+= "Right";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_COLOR_BUTTON_UP))
			colorButtons+= "Up";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_0))
			monoButtons+= "Button 0";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_1))
			monoButtons+= "Button 1";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_2))
			monoButtons+= "Button 2";
		if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_3))
			monoButtons+= "Button 3";
		
		LogitechGSDK.LogiLcdMonoSetText(0,monoButtons);
		LogitechGSDK.LogiLcdColorSetText(5,colorButtons,255,255,0);
		
		//LCD TYPE CONNECTED TEST
		String lcdsConnected="LCDs connected :";
		if(LogitechGSDK.LogiLcdIsConnected(LogitechGSDK.LOGI_LCD_TYPE_MONO))
			lcdsConnected += "MONO ";
		if(LogitechGSDK.LogiLcdIsConnected(LogitechGSDK.LOGI_LCD_TYPE_COLOR))
			lcdsConnected += "COLOR";
		
		LogitechGSDK.LogiLcdMonoSetText(1,lcdsConnected);
		LogitechGSDK.LogiLcdColorSetText(2,lcdsConnected,255,255,0);
		
		LogitechGSDK.LogiLcdUpdate();
		if(Input.GetKey(KeyCode.Mouse0)){
			//COLOR TEST
			pixelMatrix = new byte[307200];  
			int red = 0;
			int blue = 0;
			int green = 0;
			int alpha = 0;
			System.Random random = new System.Random();
			red = random.Next(0,255);
			blue = random.Next(0,255);
			green = random.Next(0,255);
			alpha = random.Next(0,255);
        	for (int i = 0; i < 307200; i++)
        	{
				
           		if((i%1) == 0) pixelMatrix[i] = (byte)blue; // blue
				if((i%2) == 0) pixelMatrix[i] = (byte)green; // green
				if((i%3) == 0) pixelMatrix[i] = (byte)red; // red
				if((i%4) == 0) pixelMatrix[i] = (byte)alpha; // red
				
				
       		}
			
			LogitechGSDK.LogiLcdColorSetBackground(pixelMatrix);
			LogitechGSDK.LogiLcdColorSetText(6,"color : "+red+" - "+blue+" - "+green+" - "+alpha,255,0,0);
			
		}
		if(Input.GetKey(KeyCode.Mouse1)){
			//MONO TEST
			pixelMatrix = new byte[6880];  
			int pixel;
        	for (int i = 0; i < 6880; i++)
        	{
				System.Random random = new System.Random();
				pixel = random.Next(0,255);
				pixelMatrix[i] = (byte)pixel; // red
       		}
			
			LogitechGSDK.LogiLcdMonoSetBackground(pixelMatrix);
			
		}
	}
	
	void OnDestroy () {
     	LogitechGSDK.LogiLcdShutdown();
	}
}
