using UnityEngine;
using System.Collections;
using System.Text;

public class LogitechSteeringWheel : MonoBehaviour
{

    LogitechGSDK.LogiControllerPropertiesData properties;
    private string actualState;
    private string activeForces;
    private string propertiesEdit;
    private string buttonStatus;
    private string forcesLabel;
    string[] activeForceAndEffect;

    // Use this for initialization
    void Start()
    {
        activeForces = "";
        propertiesEdit = "";
        actualState = "";
        buttonStatus = "";
        forcesLabel = "Press the following keys to activate forces and effects on the steering wheel / gaming controller \n";
        forcesLabel += "Spring force : S\n";
        forcesLabel += "Constant force : C\n";
        forcesLabel += "Damper force : D\n";
        forcesLabel += "Side collision : Left or Right Arrow\n";
        forcesLabel += "Front collision : Up arrow\n";
        forcesLabel += "Dirt road effect : I\n";
        forcesLabel += "Bumpy road effect : B\n";
        forcesLabel += "Slippery road effect : L\n";
        forcesLabel += "Surface effect : U\n";
        forcesLabel += "Car Airborne effect : A\n";
        forcesLabel += "Soft Stop Force : O\n";
        forcesLabel += "Set example controller properties : PageUp\n";
        forcesLabel += "Play Leds : P\n";
        activeForceAndEffect = new string[9];
        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));
    }

    void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }

    void OnGUI()
    {
        activeForces = GUI.TextArea(new Rect(10, 10, 180, 200), activeForces, 400);
        propertiesEdit = GUI.TextArea(new Rect(200, 10, 200, 200), propertiesEdit, 400);
        actualState = GUI.TextArea(new Rect(410, 10, 300, 200), actualState, 1000);
        buttonStatus = GUI.TextArea(new Rect(720, 10, 300, 200), buttonStatus, 1000);
        GUI.Label(new Rect(10, 400, 800, 400), forcesLabel);
    }

    // Update is called once per frame
    void Update()
    {
        //All the test functions are called on the first device plugged in(index = 0)
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {

            //CONTROLLER PROPERTIES
            StringBuilder deviceName = new StringBuilder(256);
            LogitechGSDK.LogiGetFriendlyProductName(0, deviceName, 256);
            propertiesEdit = "Current Controller : " + deviceName + "\n";
            propertiesEdit += "Current controller properties : \n\n";
            LogitechGSDK.LogiControllerPropertiesData actualProperties = new LogitechGSDK.LogiControllerPropertiesData();
            LogitechGSDK.LogiGetCurrentControllerProperties(0, ref actualProperties);
            propertiesEdit += "forceEnable = " + actualProperties.forceEnable + "\n";
            propertiesEdit += "overallGain = " + actualProperties.overallGain + "\n";
            propertiesEdit += "springGain = " + actualProperties.springGain + "\n";
            propertiesEdit += "damperGain = " + actualProperties.damperGain + "\n";
            propertiesEdit += "defaultSpringEnabled = " + actualProperties.defaultSpringEnabled + "\n";
            propertiesEdit += "combinePedals = " + actualProperties.combinePedals + "\n";
            propertiesEdit += "wheelRange = " + actualProperties.wheelRange + "\n";
            propertiesEdit += "gameSettingsEnabled = " + actualProperties.gameSettingsEnabled + "\n";
            propertiesEdit += "allowGameSettings = " + actualProperties.allowGameSettings + "\n";

            //CONTROLLER STATE
            actualState = "Steering wheel current state : \n\n";
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            actualState += "x-axis position :" + rec.lX + "\n";
            actualState += "y-axis position :" + rec.lY + "\n";
            actualState += "z-axis position :" + rec.lZ + "\n";
            actualState += "x-axis rotation :" + rec.lRx + "\n";
            actualState += "y-axis rotation :" + rec.lRy + "\n";
            actualState += "z-axis rotation :" + rec.lRz + "\n";
            actualState += "extra axes positions 1 :" + rec.rglSlider[0] + "\n";
            actualState += "extra axes positions 2 :" + rec.rglSlider[1] + "\n";
            switch (rec.rgdwPOV[0])
            {
                case (0): actualState += "POV : UP\n"; break;
                case (4500): actualState += "POV : UP-RIGHT\n"; break;
                case (9000): actualState += "POV : RIGHT\n"; break;
                case (13500): actualState += "POV : DOWN-RIGHT\n"; break;
                case (18000): actualState += "POV : DOWN\n"; break;
                case (22500): actualState += "POV : DOWN-LEFT\n"; break;
                case (27000): actualState += "POV : LEFT\n"; break;
                case (31500): actualState += "POV : UP-LEFT\n"; break;
                default: actualState += "POV : CENTER\n"; break;
            }

            //Button status :

            buttonStatus = "Button pressed : \n\n";
            for (int i = 0; i < 128; i++)
            {
                if (rec.rgbButtons[i] == 128)
                {
                    buttonStatus += "Button " + i + " pressed\n";
                }

            }

            /* THIS AXIS ARE NEVER REPORTED BY LOGITECH CONTROLLERS 
             * 
             * actualState += "x-axis velocity :" + rec.lVX + "\n";
             * actualState += "y-axis velocity :" + rec.lVY + "\n";
             * actualState += "z-axis velocity :" + rec.lVZ + "\n";
             * actualState += "x-axis angular velocity :" + rec.lVRx + "\n";
             * actualState += "y-axis angular velocity :" + rec.lVRy + "\n";
             * actualState += "z-axis angular velocity :" + rec.lVRz + "\n";
             * actualState += "extra axes velocities 1 :" + rec.rglVSlider[0] + "\n";
             * actualState += "extra axes velocities 2 :" + rec.rglVSlider[1] + "\n";
             * actualState += "x-axis acceleration :" + rec.lAX + "\n";
             * actualState += "y-axis acceleration :" + rec.lAY + "\n";
             * actualState += "z-axis acceleration :" + rec.lAZ + "\n";
             * actualState += "x-axis angular acceleration :" + rec.lARx + "\n";
             * actualState += "y-axis angular acceleration :" + rec.lARy + "\n";
             * actualState += "z-axis angular acceleration :" + rec.lARz + "\n";
             * actualState += "extra axes accelerations 1 :" + rec.rglASlider[0] + "\n";
             * actualState += "extra axes accelerations 2 :" + rec.rglASlider[1] + "\n";
             * actualState += "x-axis force :" + rec.lFX + "\n";
             * actualState += "y-axis force :" + rec.lFY + "\n";
             * actualState += "z-axis force :" + rec.lFZ + "\n";
             * actualState += "x-axis torque :" + rec.lFRx + "\n";
             * actualState += "y-axis torque :" + rec.lFRy + "\n";
             * actualState += "z-axis torque :" + rec.lFRz + "\n";
             * actualState += "extra axes forces 1 :" + rec.rglFSlider[0] + "\n";
             * actualState += "extra axes forces 2 :" + rec.rglFSlider[1] + "\n";
             */

            int shifterTipe = LogitechGSDK.LogiGetShifterMode(0);
            string shifterString = "";
            if (shifterTipe == 1) shifterString = "Gated";
            else if (shifterTipe == 0) shifterString = "Sequential";
            else shifterString = "Unknown";
            actualState += "\nSHIFTER MODE:" + shifterString;




            // FORCES AND EFFECTS 
            activeForces = "Active forces and effects :\n";

            //Spring Force -> S
            if (Input.GetKeyUp(KeyCode.S))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SPRING))
                {
                    LogitechGSDK.LogiStopSpringForce(0);
                    activeForceAndEffect[0] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlaySpringForce(0, 50, 50, 50);
                    activeForceAndEffect[0] = "Spring Force\n ";
                }
            }

            //Constant Force -> C
            if (Input.GetKeyUp(KeyCode.C))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_CONSTANT))
                {
                    LogitechGSDK.LogiStopConstantForce(0);
                    activeForceAndEffect[1] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlayConstantForce(0, 50);
                    activeForceAndEffect[1] = "Constant Force\n ";
                }
            }

            //Damper Force -> D
            if (Input.GetKeyUp(KeyCode.D))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_DAMPER))
                {
                    LogitechGSDK.LogiStopDamperForce(0);
                    activeForceAndEffect[2] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlayDamperForce(0, 50);
                    activeForceAndEffect[2] = "Damper Force\n ";
                }
            }

            //Side Collision Force -> left or right arrow
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                LogitechGSDK.LogiPlaySideCollisionForce(0, 60);
            }

            //Front Collision Force -> up arrow
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                LogitechGSDK.LogiPlayFrontalCollisionForce(0, 60);
            }

            //Dirt Road Effect-> I
            if (Input.GetKeyUp(KeyCode.I))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_DIRT_ROAD))
                {
                    LogitechGSDK.LogiStopDirtRoadEffect(0);
                    activeForceAndEffect[3] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlayDirtRoadEffect(0, 50);
                    activeForceAndEffect[3] = "Dirt Road Effect\n ";
                }

            }

            //Bumpy Road Effect-> B
            if (Input.GetKeyUp(KeyCode.B))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_BUMPY_ROAD))
                {
                    LogitechGSDK.LogiStopBumpyRoadEffect(0);
                    activeForceAndEffect[4] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlayBumpyRoadEffect(0, 50);
                    activeForceAndEffect[4] = "Bumpy Road Effect\n";
                }

            }

            //Slippery Road Effect-> L
            if (Input.GetKeyUp(KeyCode.L))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SLIPPERY_ROAD))
                {
                    LogitechGSDK.LogiStopSlipperyRoadEffect(0);
                    activeForceAndEffect[5] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlaySlipperyRoadEffect(0, 50);
                    activeForceAndEffect[5] = "Slippery Road Effect\n ";
                }
            }

            //Surface Effect-> U
            if (Input.GetKeyUp(KeyCode.U))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SURFACE_EFFECT))
                {
                    LogitechGSDK.LogiStopSurfaceEffect(0);
                    activeForceAndEffect[6] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlaySurfaceEffect(0, LogitechGSDK.LOGI_PERIODICTYPE_SQUARE, 50, 1000);
                    activeForceAndEffect[6] = "Surface Effect\n";
                }
            }

            //Car Airborne -> A
            if (Input.GetKeyUp(KeyCode.A))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_CAR_AIRBORNE))
                {
                    LogitechGSDK.LogiStopCarAirborne(0);
                    activeForceAndEffect[7] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlayCarAirborne(0);
                    activeForceAndEffect[7] = "Car Airborne\n ";
                }
            }

            //Soft Stop Force -> O
            if (Input.GetKeyUp(KeyCode.O))
            {
                if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SOFTSTOP))
                {
                    LogitechGSDK.LogiStopSoftstopForce(0);
                    activeForceAndEffect[8] = "";
                }
                else
                {
                    LogitechGSDK.LogiPlaySoftstopForce(0, 20);
                    activeForceAndEffect[8] = "Soft Stop Force\n";
                }
            }

            //Set preferred controller properties -> PageUp
            if (Input.GetKeyUp(KeyCode.PageUp))
            {
                //Setting example values
                properties.wheelRange = 90;
                properties.forceEnable = true;
                properties.overallGain = 80;
                properties.springGain = 80;
                properties.damperGain = 80;
                properties.allowGameSettings = true;
                properties.combinePedals = false;
                properties.defaultSpringEnabled = true;
                properties.defaultSpringGain = 80;
                LogitechGSDK.LogiSetPreferredControllerProperties(properties);

            }

            //Play leds -> P
            if (Input.GetKeyUp(KeyCode.P))
            {
                LogitechGSDK.LogiPlayLeds(0, 20, 20, 20);
            }

            for (int i = 0; i < 9; i++)
            {
                activeForces += activeForceAndEffect[i];
            }

        }
        else if (!LogitechGSDK.LogiIsConnected(0))
        {
            actualState = "PLEASE PLUG IN A STEERING WHEEL OR A FORCE FEEDBACK CONTROLLER";
        }
        else
        {
            actualState = "THIS WINDOW NEEDS TO BE IN FOREGROUND IN ORDER FOR THE SDK TO WORK PROPERLY";
        }
    }



}
