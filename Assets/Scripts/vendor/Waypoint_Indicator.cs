﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace com.horizon.store
{
    public class Waypoint_Indicator : MonoBehaviour
    {
        //Version 1.2.0
        //1. Added a new tracking type: Centered Tracking.Both types may be enabled at the same time or one at a time
        //2. UI overhaul of Inspector (Use with Waypoint_Indicator_Editor.cs)
        //3. New Indicator Sprites have been added exclusively for Centered Tracking.
        //4. Ability to customize sprite colors in real time
        //5. Renamed all Indicators from "Game Object" to "Prefab"
        //6. Bug Fix #1: Fixed Game Object clones from spawning into scene when re-enabling the Prefab Indicator Option while object is on screen
        //7. Bug Fix #2: Fixed rect-popping in bottom left corner when spawning indicators



        //CANVAS - By default the script will look for "Canvas". If you have labled it something else, update the variable below to match
        //!! Make sure your Canvas > Reference Resolution is the same aspect ratio as your Play Screen or else Waypoint boundary edges may not align exactly
        private string canvas_name = "Canvas";
        private Canvas mainCanvas;
        //CAMERA - By default the script will look for Camera.main.
        //If you'd like to target the Camera by a tag, look at line 268
        private Camera mainCamera;


        private float canvasScaleFactor;
        private Vector2 canvasRefRez;

        private bool isDynamic = false; //Make this public to allow for external scripts to send Text Description copy here as "description"


        //Tracking Types
        [Header("Tracking Types")]

        [Space(10)]
        [Header("*** Custom Editor Missing! See documentation. ***")]
        [Space(10)]

        public bool enableStandardTracking = true; //Tracks over objects in view and to the edge of the screen when out of view.
        public bool enableCenteredTracking = false; //Tracks objects along a circular perimeter originating from the center of the screen.

        [Header("Parent")]
        public bool showBoundaryBox = true;
        public Color boundaryBoxColor = new Color32(255, 255, 102, 255);
        public Vector2Int parentSize = new Vector2Int(75, 75);
        private float parentPaddingX;
        private float parentPaddingY;
        private float onScreenSnapOffset_X;
        private float onScreenSnapOffset_Y;
        public float displayRange = 50f;



        [Header("Sprite Global")]
        public bool enableSprite = false;
        public int spriteDepth;
        public bool offScreenSpriteRotates = false;

        [Header("Sprite On-Screen")]
        //ON-SCREEN SPRITE
        public Sprite onScreenSprite;
        public Color onScreenSpriteColor = new Color32(255, 255, 255, 255);
        public float onScreenSpriteSize = 1f;
        public Vector2 onScreenSpriteOffset = new Vector2(0f, 0f);
        [Range(0, 360)]
        public float onScreenSpriteRotation = 0f;
        public bool onScreenSpriteFadeWithRange;
        public bool onScreenSpriteScaleWithRange;
        public bool onScreenSpriteHide = false;


        [Header("Sprite Off-Screen")]
        //OFF-SCREEN SPRITE
        public Sprite offScreenSprite;
        public Color offScreenSpriteColor = new Color32(255, 255, 255, 255);
        public float offScreenSpriteSize = 1f;
        public Vector2 offScreenSpriteOffset = new Vector2(0f, 0f);
        [Range(0, 360)]
        public float offScreenSpriteRotation = 0f;
        public bool offScreenSpriteFadeWithRange;
        public bool offScreenScaleWithRange;
        public bool offScreenSpriteHide = false;




        [Header("Game Object Global")]
        public bool enableGameObject = false;
        public int gameObjectDepth;
        public bool offScreenObjectRotates = false;

        //ON-SCREEN GAME OBJECT
        [Header("Object On-Screen")]
        public GameObject onScreenGameObject;
        public Color onScreenGameObjectColor = new Color32(255, 255, 255, 255);
        public float onScreenGameObjectSize = 1f;
        public Vector2 onScreenGameObjectOffset = new Vector2(0f, 0f);
        [Range(0, 360)]
        public float onScreenGameObjectRotation = 0f;
        public bool onScreenGameObjectFadeWithRange;
        public bool onScreenGameObjectScaleWithRange;
        public bool onScreenGameObjectHide = false;

        //OFF-SCREEN GAME OBJECT
        [Header("Object Off-Screen")]
        public GameObject offScreenGameObject;
        public Color offScreenGameObjectColor = new Color32(255, 255, 255, 255);
        public float offScreenGameObjectSize = 1f;
        public Vector2 offScreenGameObjectOffset = new Vector2(0f, 0f);
        [Range(0, 360)]
        public float offScreenGameObjectRotation = 0f;
        public bool offScreenGameObjectFadeWithRange;
        public bool offScreenGameObjectScaleWithRange;
        public bool offScreenGameObjectHide = false;



        [Header("Text Global")]
        public bool enableText = true;
        public int textDepth;
        public string textDescription = "Hello!";
        public TMP_FontAsset textFont;
        public static string description; //Text pulled from Quest scripts (if marked as dynamic)
        public float textSize = 15;
        public Color textColor = new Color32(0, 0, 0, 255);
        public enum textAlignValue
        {
            Left,
            Center,
            Right
        }
        public textAlignValue textAlign = textAlignValue.Center;
        public float textLineSpacing = 20f;
        public float textRectWidth = 80f;
        public float textRectHeight = 50f;

        [Header("Text On-Screen")]
        public bool onScreenSpriteHideDesc = false;
        public bool onScreenSpriteHideDist = false;
        public Vector2 onScreenTextOffset = new Vector2(0f, 0f);

        [Header("Text Off-Screen")]
        public bool offScreenSpriteHideDesc = false;
        public bool offScreenSpriteHideDist = false;
        public Vector2 offScreenTextOffset = new Vector2(0f, 0f);



        //CENTERED
        [Header("Diameter")]
        public bool showDiameter = false;
        public Color diameterColor = new Color32(0, 0, 0, 255);
        public float diameterSize = 50f;

        [Header("Radius")]
        public bool enableRadiusGizmo; //A circular wireframe tool for aligning crescent-shaped icons.  Scene View only. Must have Gizmos and 2D enabled.
        public Color radiusGizmoColor = new Color32(0, 0, 0, 255);
        public float radiusGizmoSize = 50f;

        [Header("Centered Icon On-Screen")]
        public float onScreenCenteredRange = 25f;
        public Color onScreenCenteredSpriteColor = new Color32(255, 255, 255, 255);
        public Sprite onScreenCenteredSprite;
        public float onScreenCenteredSpriteSize = 1f;
        [Range(0, 360)]
        public float onScreenCenteredSpriteRotation = 0f;
        public bool onScreenSpriteCenteredFadeWithRange;
        public bool onScreenSpriteCenteredScaleWithRange;


        //Custom Editor Drawer Toggle States
        [Header("Ignore (For use with Custom Editor)")]
        public bool toggleParentOptions = false;
        public bool toggleSpriteOptions = false;
        public bool toggleGameObjectOptions = false; public bool toggleTextOptions = false;
        public bool toggleDiameterOptions = false;
        public bool toggleCenteredSpriteOptions = false;
        public bool toggleRadiusGizmoOptions = false;


        //WP Parent Vars
        private GameObject wpParentGameObject;
        private RectTransform wpParent;
        private Image wpParentImage;
        private bool waypointParentCreated = false;


        //WP Sprite Indicator Vars
        private GameObject spriteIndicator; //spriteIndicator
        private RectTransform spriteIndicatorRect; //spriteIndicatorRect
        private Image spriteIndicatorImage; //spriteIndicatorImage
        private Color spriteIndicatorColor;
        private bool spriteIndicatorCreated = false;


        //WP Game Object Indicator Vars
        private GameObject gameObjectIndicator; //This is the Game Object version of the Sprite
        private bool gameObjectIndicatorCreated = false;
        private int gameObjectIndicatorOnOffScreenStatus = 0; //0 on screen, 1 off screen
        private CanvasGroup gameObjectIndicatorCanvasGroup;
        private GameObject gameObjectIndicatorChildGameObject;
        private RectTransform gameObjectIndicatorChildRect;
        private RawImage gameObjectIndicatorChildImg; //The image that controls the sprite that sits inside the game object (index 0)
        private Color gameObjectIndicatorChildIndicatorColor;


        //WP Text Vars
        private GameObject textGameObject;
        private TextMeshProUGUI textField;
        private bool waypointTextCreated = false;
        private Vector2 onScreenSpriteOriginalSize;
        private Vector2 offScreenSpriteOriginalSize;
        private Vector2 newOnScreenSize;
        private Vector2 newOffScreenSize;

        //Setup Vars Standard Tracking
        private Vector3 screenCenter;
        private Vector3 wpParentPos;
        private float angle;
        private float waypointDist;
        private int iSpriteIndicator = 0;
        private int iGameObjectIndicator = 0;
        private int iText = 0;
        private int iScreenCheck;
        private int iStandardTrackingEnabled;
        private bool parentOnScreen;
        private float iconAlphaValue;
        private float scaleValueX;
        private float scaleValueY;
        private float scaleValueGameObjectX;
        private float scaleValueGameObjectY;
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        private GameObject waypointParent;


        //Setup Vars Centered Tracking
        //WP Parent Vars
        private GameObject wpCenteredParentGameObject;
        private RectTransform wpCenteredParent;
        private Image wpCenteredParentImage;
        private bool centeredWaypointParentCreated = false;

        //WP Icon Vars
        private GameObject iconCenteredGameObject;
        private RectTransform iconCentered;
        private Image iconCenteredImage;
        private bool waypointIconCenteredCreated = false;

        //Setup Vars
        private Vector2 onScreenCenteredSpriteOriginalSize;
        private Vector2 newOnScreenCenteredSize;
        private Vector3 screenCenteredCenter;
        private Vector3 wpCenteredParentPos;
        private Color iconCenteredColor;
        private float angleCentered;
        private float waypointCenteredDist;
        private int iScreenCheckCentered;
        private int iCenteredTrackingEnabled;
        //private bool centeredParentOnScreen;
        private float iconCenteredAlphaValue;
        private float centeredScaleValueX;
        private float centeredScaleValueY;

        private GameObject centeredWaypointParent;




        void OnEnable()
        {
            mainCamera = Camera.main;
            //mainCamera = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<Camera>();
            if (mainCamera == null)
            {
                Debug.LogError("Camera missing! Waypoint Indicators is looking for \"Camera.main\" \nChange the reference variable name on line 205 or make the variable public.");
            }


            if (!GameObject.Find(canvas_name))
            {
                Debug.LogError("Canvas missing! Waypoint Indicators is looking for canvas titled \"" + canvas_name + "\"\nEither rename your canvas to  \"" + canvas_name + "\", or change the reference name in this script on line 18, or make the canvas variable public.");
            }
            else
            {
                mainCanvas = GameObject.Find(canvas_name).GetComponent<Canvas>();
            }



            if (mainCamera && mainCanvas)
            {
                //Standard Tracking
                if (enableStandardTracking)
                {
                    InstantiateWaypointParent();

                    if (textDescription == "")
                    {
                        textDescription = gameObject.name;
                        //Debug.Log("There's no text");
                    }

                    //Find this game object's Waypoint UI by name so we can target it later
                    waypointParent = GameObject.Find(wpParent.name);

                    iSpriteIndicator = 0;
                    iGameObjectIndicator = 0;
                    iText = 0;

                    #region Fill missing fields for Sprite and Game Objects if left empty to avoid errors

                    //SPRITE CHECK
                    if (onScreenSprite != null || offScreenSprite != null) //At least one icon was defined for one slot
                    {
                        //Check to see if either on or off-screen has been left empty and assign the sprite from the state that has been defined 
                        if (onScreenSprite == null)
                        {
                            onScreenSprite = offScreenSprite;
                        }
                        if (offScreenSprite == null)
                        {
                            offScreenSprite = onScreenSprite;
                        }

                        //Get the original size of the Sprite before alterations
                        onScreenSpriteOriginalSize.x = onScreenSprite.bounds.size.x;
                        onScreenSpriteOriginalSize.y = onScreenSprite.bounds.size.y;

                        offScreenSpriteOriginalSize.x = offScreenSprite.bounds.size.x;
                        offScreenSpriteOriginalSize.y = offScreenSprite.bounds.size.y;
                    }
                    else
                    {
                        if (enableSprite)
                        {
                            Debug.LogWarning("Sprite Indicators are ENABLED, but NOT ASSIGNED on: " + gameObject.name + "\nAssign a Sprite to either On/Off Screen Sprite fields. - OnEnable()");
                        }
                        //Since there's no Sprite, we need to manually enter X,Y sizes so there's no error
                        onScreenSpriteOriginalSize.x = 1f;
                        onScreenSpriteOriginalSize.y = 1f;

                        offScreenSpriteOriginalSize.x = 1f;
                        offScreenSpriteOriginalSize.y = 1f;
                    }


                    //GAME OBJECT TYPE
                    if (onScreenGameObject != null || offScreenGameObject != null) //At least one icon was defined for one slot
                    {
                        //Check to see if either on or off-screen has been left empty and assign the sprite from the state that has been defined 
                        if (onScreenGameObject == null)
                        {
                            onScreenGameObject = offScreenGameObject;
                        }
                        if (offScreenGameObject == null)
                        {
                            offScreenGameObject = onScreenGameObject;
                        }
                    }
                    else
                    {
                        if (enableGameObject)
                        {
                            Debug.LogWarning("Game Object Indicators are ENABLED, but NOT ASSIGNED on: " + gameObject.name + "\nAssign a Game Object to either On/Off Screen Game Object fields.");
                        }
                    }

                    #endregion


                    //Check if parent is on or off-screen then set the game object status accordingly
                    //This ensures the right Game Object indicator shows for on or off screen when enabled and disabled during run time
                    SetGameObjectIndicatorStatus();

                    iStandardTrackingEnabled = 1;
                }


                //Centered Tracking
                if (enableCenteredTracking)
                {
                    InstantiateCenteredWaypointParent();
                    InstantiateCenteredWaypointIcon();

                    //Find this game object's Waypoint UI by name so we can target it later
                    centeredWaypointParent = GameObject.Find(wpCenteredParent.name);



                    #region Fill missing fields for Centered Sprite if left empty to avoid errors

                    //CENTERED SPRITE CHECK
                    if (onScreenCenteredSprite != null) //At least one icon was defined for one slot
                    {
                        //Get the original size of the Sprite before alterations
                        onScreenCenteredSpriteOriginalSize.x = onScreenCenteredSprite.bounds.size.x;
                        onScreenCenteredSpriteOriginalSize.y = onScreenCenteredSprite.bounds.size.y;
                    }
                    else
                    {
                        Debug.LogWarning("Centered Tracking Sprite Missing on: " + gameObject.name + "\nAssign a Sprite to the Sprite field. - OnEnable()");

                        //Get the original size of the Sprite before alterations
                        onScreenCenteredSpriteOriginalSize.x = 1f;
                        onScreenCenteredSpriteOriginalSize.y = 1f;
                    }

                    #endregion


                    iCenteredTrackingEnabled = 1;
                }


                canvasRefRez = mainCanvas.GetComponent<CanvasScaler>().referenceResolution;
            }


        }


        void OnDisable()
        {
            if (enableStandardTracking)
            {
                waypointParentCreated = false;
                spriteIndicatorCreated = false;
                gameObjectIndicatorCreated = false;
                waypointTextCreated = false;

                //Destroy wp ui using find from OnEnable() above
                if (waypointParent != null)
                {
                    Destroy(waypointParent);
                }

                iStandardTrackingEnabled = 0;
            }

            if (enableCenteredTracking)
            {
                centeredWaypointParentCreated = false;
                waypointIconCenteredCreated = false;
                enableRadiusGizmo = false;

                //Destroy wp ui using find from OnEnable() above
                if (centeredWaypointParent != null)
                {
                    Destroy(centeredWaypointParent);
                }

                iCenteredTrackingEnabled = 0;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (mainCamera && mainCanvas)
            {
                if (enableStandardTracking)
                {
                    //Reset Standard Tracking as if it were Enable()
                    if (iStandardTrackingEnabled == 0)
                    {
                        //Run functions from Enable()
                        InstantiateWaypointParent();

                        if (textDescription == "")
                        {
                            textDescription = gameObject.name;
                            //Debug.Log("There's no text");
                        }

                        //Find this game object's Waypoint UI by name so we can target it later
                        waypointParent = GameObject.Find(wpParent.name);

                        iSpriteIndicator = 0;
                        iGameObjectIndicator = 0;
                        iText = 0;


                        #region Fill missing fields for Sprite and Game Objects if left empty to avoid errors

                        //SPRITE TYPE
                        if (onScreenSprite != null || offScreenSprite != null) //At least one icon was defined for one slot
                        {
                            //Check to see if either on or off-screen has been left empty and assign the sprite from the state that has been defined 
                            if (onScreenSprite == null)
                            {
                                onScreenSprite = offScreenSprite;
                            }
                            if (offScreenSprite == null)
                            {
                                offScreenSprite = onScreenSprite;
                            }

                            //Get the original size of the Sprite before alterations
                            onScreenSpriteOriginalSize.x = onScreenSprite.bounds.size.x;
                            onScreenSpriteOriginalSize.y = onScreenSprite.bounds.size.y;

                            offScreenSpriteOriginalSize.x = offScreenSprite.bounds.size.x;
                            offScreenSpriteOriginalSize.y = offScreenSprite.bounds.size.y;
                        }
                        else
                        {
                            if (enableSprite)
                            {
                                Debug.LogWarning("Sprite Indicators are ENABLED, but NOT ASSIGNED on: " + gameObject.name + "\nAssign a Sprite to either On/Off Screen Sprite fields. - void Update()");
                            }
                            //Get the original size of the Sprite before alterations
                            onScreenSpriteOriginalSize.x = 1f;
                            onScreenSpriteOriginalSize.y = 1f;

                            offScreenSpriteOriginalSize.x = 1f;
                            offScreenSpriteOriginalSize.y = 1f;
                        }


                        //GAME OBJECT TYPE
                        if (onScreenGameObject != null || offScreenGameObject != null) //At least one icon was defined for one slot
                        {
                            //Check to see if either on or off-screen has been left empty and assign the sprite from the state that has been defined 
                            if (onScreenGameObject == null)
                            {
                                onScreenGameObject = offScreenGameObject;
                            }
                            if (offScreenGameObject == null)
                            {
                                offScreenGameObject = onScreenGameObject;
                            }
                        }
                        else
                        {
                            if (enableGameObject)
                            {
                                Debug.LogWarning("Game Object Indicators are ENABLED, but NOT ASSIGNED on: " + gameObject.name + "\nAssign a Game Object to either On/Off Screen Game Object fields.");
                            }
                        }

                        #endregion


                        //Check if parent is on or off-screen then set the game object status accordingly
                        //This ensures the right Game Object indicator shows for on or off screen when enabled and disabled during run time
                        SetGameObjectIndicatorStatus();

                        iStandardTrackingEnabled = 1;
                    }


                    //If true, other scripts can send text description copy via: WaypointIndicator.description = "My Text";
                    if (isDynamic)
                    {
                        textDescription = description;
                    }


                    #region CHECK TOGGLE STATES
                    //SPRITE INDICATOR
                    if (enableSprite) //ENABLE
                    {
                        if (iSpriteIndicator == 0)
                        {
                            //Debug.Log("Display Sprite Ind");
                            InstantiateWaypointIcon();
                            iSpriteIndicator++;
                        }
                    }
                    if (!enableSprite) //DISABLE
                    {
                        if (iSpriteIndicator == 1)
                        {
                            //Debug.Log("Hide Sprite Ind");
                            DestroyWaypointIcon();
                            iSpriteIndicator--;
                        }
                    }


                    //GAME OBJECT INDICATOR
                    if (enableGameObject) //ENABLE
                    {
                        if (iGameObjectIndicator == 0)
                        {
                            //Debug.Log("Display Game Object Ind");
                            SetGameObjectIndicatorStatus();
                            InstantiateWaypointGameObject();
                            iGameObjectIndicator++;
                        }
                    }
                    if (!enableGameObject) //DISABLE
                    {
                        if (iGameObjectIndicator == 1)
                        {
                            //Debug.Log("Hide Game Object Ind");
                            DestroyWaypointGameObject();
                            iGameObjectIndicator--;
                        }
                    }


                    //TEXT
                    if (enableText) //ENABLE
                    {
                        if (iText == 0)
                        {
                            //Debug.Log("Display Text Field");
                            InstantiateWaypointText();
                            iText++;
                        }
                    }
                    if (!enableText) //DISABLE
                    {
                        if (iText == 1)
                        {
                            //Debug.Log("Hide Text Field");
                            DestroyWaypointText();
                            iText--;
                        }
                    }
                    #endregion

                    //Determine Distance from Game Object (that this script is attached to) Distance is from Camera
                    waypointDist = (int)Vector3.Distance(mainCamera.transform.position, transform.position);



                    #region IF ENABLE PARENT (enabled on Start)
                    //Make sure there is at least one sprite assigned to avoid nasty errors 
                    if (waypointParentCreated && wpParent != null)
                    {
                        //Reset Scale to 1
                        wpParent.localScale = new Vector3(1f, 1f, 1f);

                        //Show/Hide
                        if (showBoundaryBox)
                        {
                            wpParentImage.enabled = true;
                        }
                        else //Hide boundary box
                        {
                            wpParentImage.enabled = false;
                        }

                        //Match the PARENT UI pos to 3D MESH position (the object this script is attached to) via WorldToScreenPoint
                        if (mainCamera != null)
                        {
                            wpParentPos = mainCamera.WorldToScreenPoint(transform.position);
                            //This keeps wpParentPos.z value from going above 1 causing dissapearing if above 1000 or so
                            if (wpParentPos.z > 1)
                            {
                                wpParentPos.z = 1f;
                            }
                            else
                            {
                                wpParentPos.z = mainCamera.WorldToScreenPoint(transform.position).z;
                            }

                            canvasScaleFactor = mainCanvas.scaleFactor;
                        }


                        //ONSCREEN SNAP TO EDGE OFFSET
                        //This ensures the on screen icon boundary edge will trigger offscreen exactly when it touches screen edge
                        //Otherwise, the on screen icon will snap to edge before reaching them
                        //As long as Canvas > Reference Resolution is the same aspect ratio as the play mode screen, this will always align
                        onScreenSnapOffset_X = 22 - (Screen.width * .024f);
                        //onScreenSnapOffset_Y = 22 - (Screen.height * .024f);
                        onScreenSnapOffset_Y = 0f;



                        //Onscreen
                        if (wpParentPos.z > 0f &&
                            (wpParentPos.x / canvasScaleFactor) > (wpParent.sizeDelta.x / 2) && wpParentPos.x < (Screen.width - (wpParent.sizeDelta.x / 2)) + onScreenSnapOffset_X &&
                            wpParentPos.y / canvasScaleFactor > (wpParent.sizeDelta.y / 2) && wpParentPos.y < (Screen.height - (wpParent.sizeDelta.y / 2)) + onScreenSnapOffset_Y)
                        {
                            //Debug.Log("On");
                            if (iScreenCheck == 1)
                            {
                                //Debug.Log("On Screen");
                                parentOnScreen = true;
                                iScreenCheck--;
                            }

                            wpParent.transform.position = wpParentPos;
                        }
                        else //(Offscreen)
                        {
                            //Debug.Log("Off");
                            if (iScreenCheck == 0)
                            {
                                //Debug.Log("Off Screen");
                                parentOnScreen = false;
                                iScreenCheck++;
                            }


                            if (wpParentPos.z < 0)
                            {
                                //Flip coordinates when things are behind
                                wpParentPos *= -1;
                            }

                            //Find center of screen
                            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0f) / 2;

                            //Set 0,0 DEAD CENTER from lower left
                            wpParentPos -= screenCenter;

                            //Find angle from center of screen to mouse pos
                            angle = Mathf.Atan2(wpParentPos.y, wpParentPos.x);
                            angle -= 90f * Mathf.Deg2Rad;

                            float cos = Mathf.Cos(angle);
                            float sin = Mathf.Sin(angle);

                            //wpParentPos = screenCenter + new Vector3(sin * 150f, cos * 150f, 0f);

                            //y = mx+b format
                            float m = cos / sin;

                            Vector3 screenBounds = screenCenter;


                            //Get Offscreen Padding Offset: (1 / Reference Resolution)
                            //This positions the boundary right along the edge once its gone offscreen
                            //This stops the bouncing back and passing beyond edges when icon reaches screen edge
                            //As long as Canvas > Reference Resolution is the same aspect ratio as the play mode screen, this will always align
                            parentPaddingX = 1 / canvasRefRez.x;
                            parentPaddingY = 1 / canvasRefRez.y;

                            screenBounds.x = screenCenter.x * (.999f - (parentSize.x * parentPaddingX));
                            screenBounds.y = screenCenter.y * (.999f - (parentSize.y * parentPaddingY));


                            //Check up and down first
                            if (cos > 0f)
                            {
                                //up
                                //Debug.Log("TOP area");
                                wpParentPos = new Vector3(-screenBounds.y / m, screenBounds.y, 0f);
                            }
                            else
                            {
                                //down
                                //Debug.Log("BOT area");
                                wpParentPos = new Vector3(screenBounds.y / m, -screenBounds.y, 0f);
                            }

                            //If out of bounds, get point on appropriate side
                            if (wpParentPos.x > screenBounds.x) //Out of bounds! Must be on the right
                            {
                                //Debug.Log("LEFT area");
                                wpParentPos = new Vector3(screenBounds.x, -screenBounds.x * m, 0f);
                            }
                            else if (wpParentPos.x < -screenBounds.x) //Out of bounds! Must be on the left
                            {
                                //Debug.Log("RIGHT area");
                                wpParentPos = new Vector3(-screenBounds.x, screenBounds.x * m, 0);
                            }

                            //Remove coordinate translation
                            wpParentPos += screenCenter;


                            wpParent.transform.position = wpParentPos;
                            //Arrow point offscreen
                            //wpParent.transform.localRotation = Quaternion.Euler(0f, 0f, angle*Mathf.Rad2Deg);
                        }




                        //Size
                        wpParent.sizeDelta = parentSize;

                        //Color
                        wpParentGameObject.GetComponent<Image>().color = boundaryBoxColor;

                    }
                    #endregion


                    #region IF ENABLE SPRITE INDICATOR
                    if (enableSprite)
                    {
                        //Set Size in real time
                        newOnScreenSize.x = onScreenSpriteOriginalSize.x * onScreenSpriteSize;
                        newOnScreenSize.y = onScreenSpriteOriginalSize.y * onScreenSpriteSize;

                        newOffScreenSize.x = offScreenSpriteOriginalSize.x * offScreenSpriteSize;
                        newOffScreenSize.y = offScreenSpriteOriginalSize.y * offScreenSpriteSize;

                        if (spriteIndicatorCreated && spriteIndicatorRect != null)
                        {

                            spriteIndicator.transform.SetSiblingIndex(spriteDepth);

                            if (waypointDist < displayRange) //ICON IN RANGE
                            {
                                //SPRITE ON-SCREEN -------------------------------------------------------------
                                if (parentOnScreen) //ICON ON-SCREEN
                                {
                                    //Sprite
                                    spriteIndicatorImage.sprite = onScreenSprite;

                                    //Color
                                    spriteIndicatorColor = onScreenSpriteColor;

                                    //Position
                                    spriteIndicatorRect.position = new Vector2(wpParent.position.x + onScreenSpriteOffset.x, wpParent.position.y + onScreenSpriteOffset.y);

                                    //Rotation
                                    spriteIndicatorRect.transform.eulerAngles = new Vector3(0f, 0f, onScreenSpriteRotation);

                                    //Size
                                    if (onScreenSpriteHide) //HIDE
                                    {
                                        spriteIndicatorImage.rectTransform.localScale = new Vector2(0, 0);
                                    }
                                    else //SHOW
                                    {
                                        //FADE WITH RANGE
                                        if (onScreenSpriteFadeWithRange)
                                        {
                                            FadeSpriteWithRange();
                                        }
                                        else
                                        {
                                            //Alpha set to 0 - We use SCALE to "Hide" icons, so no need to do any adjustments here
                                            iconAlphaValue = 1;
                                            spriteIndicatorColor.a = iconAlphaValue;
                                            spriteIndicatorImage.color = spriteIndicatorColor;
                                        }

                                        //SCALE WITH RANGE
                                        if (onScreenSpriteScaleWithRange)
                                        {
                                            ScaleSpriteWithRange(newOnScreenSize);
                                        }
                                        else //Use user Scale
                                        {
                                            spriteIndicatorImage.rectTransform.localScale = new Vector2(newOnScreenSize.x, newOnScreenSize.y);
                                        }

                                    }
                                }

                                //SPRITE OFF-SCREEN -------------------------------------------------------------
                                if (!parentOnScreen)
                                {
                                    //Sprite
                                    spriteIndicatorImage.sprite = offScreenSprite;

                                    //Color
                                    spriteIndicatorColor = offScreenSpriteColor;

                                    //Position
                                    spriteIndicatorRect.position = new Vector2(wpParent.position.x + offScreenSpriteOffset.x, wpParent.position.y + offScreenSpriteOffset.y);

                                    //Rotation
                                    if (offScreenSpriteRotates)
                                    {
                                        spriteIndicatorRect.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + offScreenSpriteRotation);
                                    }
                                    else
                                    {
                                        spriteIndicatorRect.transform.eulerAngles = new Vector3(0f, 0f, offScreenSpriteRotation);
                                    }

                                    //Size
                                    if (offScreenSpriteHide) //HIDE
                                    {
                                        spriteIndicatorImage.rectTransform.localScale = new Vector2(0, 0);
                                    }
                                    else //SHOW
                                    {
                                        //FADE WITH RANGE
                                        if (offScreenSpriteFadeWithRange)
                                        {
                                            FadeSpriteWithRange();
                                        }
                                        else
                                        {
                                            //Alpha set to 0 - We use SCALE to "Hide" icons, so no need to do any adjustments here
                                            iconAlphaValue = 1;
                                            spriteIndicatorColor.a = iconAlphaValue;
                                            spriteIndicatorImage.color = spriteIndicatorColor;
                                        }

                                        //SCALE WITH RANGE
                                        if (offScreenScaleWithRange)
                                        {
                                            ScaleSpriteWithRange(newOffScreenSize);
                                        }
                                        else //Use user Scale
                                        {
                                            spriteIndicatorImage.rectTransform.localScale = new Vector2(newOffScreenSize.x, newOffScreenSize.y);
                                        }
                                    }
                                }
                            }
                            else //ICON OUT OF RANGE
                            {
                                spriteIndicatorImage.rectTransform.localScale = new Vector2(0, 0);
                            }


                        }
                    }
                    #endregion


                    #region IF ENABLE OBJECT INDICATOR
                    if (enableGameObject)
                    {
                        if (gameObjectIndicatorCreated && gameObjectIndicator != null)
                        {

                            gameObjectIndicator.transform.SetSiblingIndex(gameObjectDepth);

                            if (waypointDist < displayRange) //ICON IN RANGE
                            {
                                //GAME OBJECT INDICATOR ON-SCREEN -------------------------------------------------------------
                                if (parentOnScreen) //ICON ON-SCREEN
                                {
                                    //Object Prefab
                                    if (gameObjectIndicatorOnOffScreenStatus == 0)
                                    {
                                        Destroy(gameObjectIndicator);
                                        gameObjectIndicator = Instantiate(onScreenGameObject, wpParentPos, Quaternion.Euler(0, 0, 0)) as GameObject;
                                        gameObjectIndicator.layer = 5;
                                        gameObjectIndicator.transform.position = wpParent.position;
                                        gameObjectIndicator.transform.SetParent(wpParentGameObject.transform);
                                        gameObjectIndicator.name = wpParent.name + "-Prefab";
                                        gameObjectIndicatorCanvasGroup = gameObjectIndicator.AddComponent<CanvasGroup>();
                                        gameObjectIndicatorCanvasGroup.alpha = 1f;
                                        gameObjectIndicatorCanvasGroup.blocksRaycasts = false;
                                        gameObjectIndicatorCanvasGroup.interactable = false;
                                        gameObjectIndicatorOnOffScreenStatus = 1;

                                        //Get the game objcet's child that houses the sprite (index 0)
                                        gameObjectIndicatorChildGameObject = gameObjectIndicator.transform.GetChild(0).gameObject;
                                        gameObjectIndicatorChildRect = gameObjectIndicatorChildGameObject.GetComponent<RectTransform>();
                                        gameObjectIndicatorChildImg = gameObjectIndicatorChildRect.GetComponent<RawImage>();


                                    }

                                    //Position
                                    gameObjectIndicator.transform.localPosition = new Vector3(onScreenGameObjectOffset.x, onScreenGameObjectOffset.y, 0f);

                                    //Color
                                    gameObjectIndicatorChildImg.color = onScreenGameObjectColor;

                                    //Rotation
                                    gameObjectIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, onScreenGameObjectRotation);

                                    //Size
                                    if (onScreenGameObjectHide) //HIDE
                                    {
                                        gameObjectIndicator.transform.localScale = new Vector3(0, 0, 0);
                                    }
                                    else //Show
                                    {
                                        //FADE WITH RANGE
                                        if (onScreenGameObjectFadeWithRange)
                                        {
                                            FadeGameObjectWithRange();
                                        }
                                        else
                                        {
                                            gameObjectIndicatorCanvasGroup.alpha = 1f;
                                        }

                                        //SCALE WITH RANGE
                                        if (onScreenGameObjectScaleWithRange)
                                        {
                                            ScaleGameObjectWithRange(onScreenGameObjectSize);
                                        }
                                        else //Use user Scale
                                        {
                                            gameObjectIndicator.transform.localScale = new Vector3(onScreenGameObjectSize, onScreenGameObjectSize, 0f);

                                        }

                                    }

                                }

                                //GAME OBJECT INDICATOR OFF-SCREEN -------------------------------------------------------------
                                if (!parentOnScreen) //ICON OFF-SCREEN
                                {
                                    //Object Prefab
                                    if (gameObjectIndicatorOnOffScreenStatus == 1)
                                    {
                                        Destroy(gameObjectIndicator);
                                        gameObjectIndicator = Instantiate(offScreenGameObject, wpParentPos, Quaternion.Euler(0, 0, 0)) as GameObject;
                                        gameObjectIndicator.layer = 5;
                                        gameObjectIndicator.transform.position = wpParent.position;
                                        gameObjectIndicator.transform.SetParent(wpParentGameObject.transform);
                                        gameObjectIndicator.name = wpParent.name + "-Prefab";
                                        gameObjectIndicatorCanvasGroup = gameObjectIndicator.AddComponent<CanvasGroup>();
                                        gameObjectIndicatorCanvasGroup.alpha = 1f;
                                        gameObjectIndicatorCanvasGroup.blocksRaycasts = false;
                                        gameObjectIndicatorCanvasGroup.interactable = false;
                                        gameObjectIndicatorOnOffScreenStatus = 0;

                                        //Get the game objcet's child that houses the sprite (index 0)
                                        gameObjectIndicatorChildGameObject = gameObjectIndicator.transform.GetChild(0).gameObject;
                                        gameObjectIndicatorChildRect = gameObjectIndicatorChildGameObject.GetComponent<RectTransform>();
                                        gameObjectIndicatorChildImg = gameObjectIndicatorChildRect.GetComponent<RawImage>();
                                    }

                                    //Position
                                    gameObjectIndicator.transform.localPosition = new Vector3(offScreenGameObjectOffset.x, offScreenGameObjectOffset.y, 0f);

                                    //Color
                                    gameObjectIndicatorChildImg.color = offScreenGameObjectColor;

                                    //Rotation
                                    //gameObjectIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, offScreenGameObjectRotation);
                                    //Rotation
                                    if (offScreenObjectRotates)
                                    {
                                        gameObjectIndicator.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + offScreenGameObjectRotation);
                                    }
                                    else
                                    {
                                        gameObjectIndicator.transform.eulerAngles = new Vector3(0f, 0f, offScreenGameObjectRotation);
                                    }

                                    //Size
                                    if (offScreenGameObjectHide) //HIDE
                                    {
                                        gameObjectIndicator.transform.localScale = new Vector3(0, 0, 0);
                                    }
                                    else //Show
                                    {
                                        //FADE WITH RANGE
                                        if (offScreenGameObjectFadeWithRange)
                                        {
                                            FadeGameObjectWithRange();
                                        }
                                        else
                                        {
                                            gameObjectIndicatorCanvasGroup.alpha = 1f;
                                        }

                                        //SCALE WITH RANGE
                                        if (offScreenGameObjectScaleWithRange)
                                        {
                                            ScaleGameObjectWithRange(offScreenGameObjectSize);
                                        }
                                        else //Use user Scale
                                        {
                                            gameObjectIndicator.transform.localScale = new Vector3(offScreenGameObjectSize, offScreenGameObjectSize, 0f);

                                        }

                                    }
                                }
                            }
                            else //GAME INDICATOR OUT OF RANGE
                            {
                                gameObjectIndicator.transform.localScale = new Vector3(0, 0, 0);
                            }
                        }
                    }
                    #endregion


                    #region IF ENABLE TEXT
                    if (enableText)
                    {
                        if (waypointTextCreated && textGameObject != null)
                        {
                            textGameObject.transform.SetSiblingIndex(textDepth);

                            if (waypointDist < displayRange) //TEXT IN RANGE
                            {
                                //GLOBAL-----
                                //FONT
                                textField.font = textFont;
                                //SIZE
                                textField.fontSize = textSize;
                                //COLOR
                                textField.color = textColor;
                                //ALIGN
                                switch (textAlign)
                                {
                                    case textAlignValue.Left:
                                        //Debug.Log("Left was chosen.");
                                        textField.alignment = TextAlignmentOptions.Left;
                                        break;
                                    case textAlignValue.Center:
                                        //Debug.Log("Center was chosen.");
                                        textField.alignment = TextAlignmentOptions.Center;
                                        break;
                                    case textAlignValue.Right:
                                        //Debug.Log("Right was chosen.");
                                        textField.alignment = TextAlignmentOptions.Right;
                                        break;
                                }
                                //LINE SPACING
                                textField.lineSpacing = textLineSpacing;
                                //SIZE
                                textField.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textRectWidth);
                                textField.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textRectHeight);


                                // TEXT ONSCREEN ---------------------------------------------------
                                if (parentOnScreen) //ICON ON-SCREEN
                                {
                                    //POSITION
                                    textGameObject.transform.localPosition = onScreenTextOffset;

                                    //DESCRIPTION / DISTANCE
                                    if (!onScreenSpriteHideDesc && !onScreenSpriteHideDist) //SHOW BOTH
                                    {
                                        textField.text = textDescription + "\n" + waypointDist.ToString() + "m";
                                    }
                                    if (onScreenSpriteHideDesc && !onScreenSpriteHideDist) //SHOW DISTNACE ONLY
                                    {
                                        textField.text = waypointDist.ToString() + "m";
                                    }
                                    if (!onScreenSpriteHideDesc && onScreenSpriteHideDist) //SHOW DESCRIPTION ONLY
                                    {
                                        textField.text = textDescription;
                                    }
                                    if (onScreenSpriteHideDesc && onScreenSpriteHideDist) //SHOW NO TEXT
                                    {
                                        textField.text = "";
                                    }
                                }

                                // TEXT OFFSCREEN ---------------------------------------------------
                                if (!parentOnScreen)
                                {
                                    //POSITION
                                    textGameObject.transform.localPosition = offScreenTextOffset;

                                    //DESCRIPTION / DISTANCE
                                    if (!offScreenSpriteHideDesc && !offScreenSpriteHideDist) //SHOW BOTH
                                    {
                                        textField.text = textDescription + "\n" + waypointDist.ToString() + "m";
                                    }
                                    if (offScreenSpriteHideDesc && !offScreenSpriteHideDist) //SHOW DISTNACE ONLY
                                    {
                                        textField.text = waypointDist.ToString() + "m";
                                    }
                                    if (!offScreenSpriteHideDesc && offScreenSpriteHideDist) //SHOW DESCRIPTION ONLY
                                    {
                                        textField.text = textDescription;
                                    }
                                    if (offScreenSpriteHideDesc && offScreenSpriteHideDist) //SHOW NO TEXT
                                    {
                                        textField.text = "";
                                    }
                                }
                            }
                            else  //TEXT OUT OF RANGE
                            {
                                textField.text = "";
                            }


                        }
                    }
                    #endregion

                }
                else
                {
                    iSpriteIndicator = 0;
                    iGameObjectIndicator = 0;
                    iText = 0;
                    waypointParentCreated = false;
                    spriteIndicatorCreated = false;
                    gameObjectIndicatorCreated = false;
                    waypointTextCreated = false;

                    //Destroy wp ui using find from OnEnable() above
                    if (waypointParent != null)
                    {
                        Destroy(waypointParent);
                    }

                    iStandardTrackingEnabled = 0;
                }




                if (enableCenteredTracking)
                {
                    //Reset Centered Tracking as if it were Enable()
                    if (iCenteredTrackingEnabled == 0)
                    {
                        InstantiateCenteredWaypointParent();
                        InstantiateCenteredWaypointIcon();

                        //Find this game object's Waypoint UI by name so we can target it later
                        centeredWaypointParent = GameObject.Find(wpCenteredParent.name);


                        #region Fill missing fields for Centered Sprite if left empty to avoid errors

                        //CENTERED SPRITE CHECK
                        if (onScreenCenteredSprite != null) //At least one icon was defined for one slot
                        {
                            //Get the original size of the Sprite before alterations
                            onScreenCenteredSpriteOriginalSize.x = onScreenCenteredSprite.bounds.size.x;
                            onScreenCenteredSpriteOriginalSize.y = onScreenCenteredSprite.bounds.size.y;
                        }
                        else
                        {
                            Debug.LogWarning("Centered Tracking Sprite Missing on: " + gameObject.name + "\nAssign a Sprite to the Sprite field. - void Update()");

                            //Get the original size of the Sprite before alterations
                            onScreenCenteredSpriteOriginalSize.x = 1f;
                            onScreenCenteredSpriteOriginalSize.y = 1f;
                        }
                        #endregion

                        iCenteredTrackingEnabled = 1;
                    }


                    //Determine Distance from Game Object (that this script is attached to) Distance is from Camera
                    waypointCenteredDist = (int)Vector3.Distance(mainCamera.transform.position, transform.position);

                    #region IF ENABLE PARENT (enabled on Start)
                    if (centeredWaypointParentCreated && wpCenteredParent != null)
                    {
                        //Reset Scale to 1
                        wpCenteredParent.localScale = new Vector3(1f, 1f, 1f);

                        //Show/Hide
                        if (showDiameter)
                        {
                            wpCenteredParentImage.enabled = true;
                        }
                        else
                        {
                            wpCenteredParentImage.enabled = false;
                        }

                        //Match the PARENT UI pos to 3D MESH position (the object this script is attached to) via WorldToScreenPoint
                        if (mainCamera != null)
                        {
                            wpCenteredParentPos = mainCamera.WorldToScreenPoint(transform.position);
                            //This keeps wpCenteredParentPos.z value from going above 1 causing dissapearing if above 1000 or so
                            wpCenteredParentPos = mainCamera.WorldToScreenPoint(transform.position);
                            if (wpCenteredParentPos.z > 1)
                            {
                                wpCenteredParentPos.z = 1f;
                            }
                            else
                            {
                                wpCenteredParentPos.z = mainCamera.WorldToScreenPoint(transform.position).z;
                            }
                        }


                        //centeredParentOnScreen = false;


                        if (wpCenteredParentPos.z < 0)
                        {
                            //Flip coordinates when things are behind
                            wpCenteredParentPos *= -1;
                        }

                        //Find center of screen
                        Vector3 screenCenteredCenter = new Vector3(Screen.width, Screen.height, 0f) / 2;

                        //Set 0,0 DEAD CENTER from lower left
                        wpCenteredParentPos -= screenCenteredCenter;

                        //Find angleCentered from center of screen to mouse pos
                        angleCentered = Mathf.Atan2(wpCenteredParentPos.y, wpCenteredParentPos.x);
                        angleCentered -= 90f * Mathf.Deg2Rad;

                        float cos = Mathf.Cos(angleCentered);
                        float sin = Mathf.Sin(angleCentered);


                        //y = mx+b format
                        float m = cos / sin;

                        Vector3 screenBounds = screenCenteredCenter;
                        screenBounds.x = 0f;
                        screenBounds.y = 0f;



                        //Check up and down first
                        if (cos > 0f)
                        {
                            //up
                            //Debug.Log("TOP area");
                            wpCenteredParentPos = new Vector3(-screenBounds.y / m, screenBounds.y, 0f);
                        }
                        else
                        {
                            //down
                            //Debug.Log("BOT area");
                            wpCenteredParentPos = new Vector3(screenBounds.y / m, -screenBounds.y, 0f);
                        }

                        //If out of bounds, get point on appropriate side
                        if (wpCenteredParentPos.x > screenBounds.x) //Out of bounds! Must be on the right
                        {
                            //Debug.Log("LEFT area");
                            wpCenteredParentPos = new Vector3(screenBounds.x, -screenBounds.x * m, 0f);
                        }
                        else if (wpCenteredParentPos.x < -screenBounds.x) //Out of bounds! Must be on the left
                        {
                            //Debug.Log("RIGHT area");
                            wpCenteredParentPos = new Vector3(-screenBounds.x, screenBounds.x * m, 0);
                        }

                        //Remove coordinate translation
                        wpCenteredParentPos += screenCenteredCenter;


                        //Position
                        wpCenteredParent.transform.position = wpCenteredParentPos;

                        //Rotation
                        wpCenteredParent.transform.rotation = Quaternion.Euler(0, 0, angleCentered * Mathf.Rad2Deg);


                        //Size
                        if (diameterSize > 15f)
                        {
                            wpCenteredParent.sizeDelta = new Vector2(diameterSize / 10, diameterSize);
                        }
                        else
                        {
                            wpCenteredParent.sizeDelta = new Vector2(diameterSize, diameterSize);
                        }


                        //Color
                        wpCenteredParentGameObject.GetComponent<Image>().color = diameterColor;


                    }
                    #endregion








                    #region IF ENABLE ICON
                    //Set Size in real time
                    newOnScreenCenteredSize.x = onScreenCenteredSpriteOriginalSize.x * onScreenCenteredSpriteSize;
                    newOnScreenCenteredSize.y = onScreenCenteredSpriteOriginalSize.y * onScreenCenteredSpriteSize;


                    if (waypointIconCenteredCreated && iconCentered != null)
                    {
                        if (waypointCenteredDist < onScreenCenteredRange) //ICON IN RANGE
                        {

                            //Sprite
                            iconCenteredImage.sprite = onScreenCenteredSprite;

                            //Position
                            iconCentered.localPosition = new Vector2(0f, diameterSize / 2f);

                            //Color - This is handled in the else section of Fade with Range
                            //iconCenteredImage.color = onScreenCenteredSpriteColor;

                            //Rotation
                            iconCentered.transform.localRotation = Quaternion.Euler(0f, 0f, onScreenCenteredSpriteRotation);

                            //Size
                            //FADE WITH RANGE
                            if (onScreenSpriteCenteredFadeWithRange)
                            {
                                FadeWithRangeCentered();
                            }
                            else
                            {
                                //Alpha set to 0 - We use SCALE to "Hide" iconCentereds, so no need to do any adjustments here
                                iconCenteredAlphaValue = 1;
                                iconCenteredColor.a = iconCenteredAlphaValue;
                                iconCenteredImage.color = onScreenCenteredSpriteColor;
                            }

                            //SCALE WITH RANGE
                            if (onScreenSpriteCenteredScaleWithRange)
                            {
                                ScaleWithRangeCentered(newOnScreenCenteredSize);
                            }
                            else //Use user Scale
                            {
                                iconCenteredImage.rectTransform.localScale = new Vector2(newOnScreenCenteredSize.x, newOnScreenCenteredSize.y);
                            }


                            /*For later use
                            //ICON ON-SCREEN -------------------------------------------------------------
                            if (centeredParentOnScreen)
                            {

                            }
                            //ICON OFF-SCREEN -------------------------------------------------------------
                            if (!centeredParentOnScreen)
                            {

                            }
                            */

                        }
                        else //ICON OUT OF RANGE
                        {
                            iconCenteredImage.rectTransform.localScale = new Vector2(0, 0);
                        }


                    }
                    #endregion
                }
                else
                {
                    centeredWaypointParentCreated = false;
                    waypointIconCenteredCreated = false;
                    enableRadiusGizmo = false;

                    //Destroy wp ui using find from OnEnable() above
                    if (centeredWaypointParent != null)
                    {
                        Destroy(centeredWaypointParent);
                    }

                    iCenteredTrackingEnabled = 0;
                }
            }




        }



        #region Standard Tracking Functions
        //CREATE PARENT UI 
        void InstantiateWaypointParent()
        {
            if (mainCanvas != null)
            {
                wpParentGameObject = new GameObject();
                wpParentGameObject.layer = 5;
                wpParent = wpParentGameObject.AddComponent<RectTransform>();
                wpParent.transform.SetParent(mainCanvas.transform);
                wpParent.name = gameObject.name + "-WP" + GetInstanceID();
                wpParentImage = wpParent.gameObject.AddComponent<Image>();
                wpParent.position = new Vector3(-1000f, -1000f, 0f); //This make sure the blinking box doesn't appear in bottom left at spawn



                if (wpParentPos.z > 0f &&
                wpParentPos.x > (wpParent.sizeDelta.x / 2) && wpParentPos.x < Screen.width - (wpParent.sizeDelta.x / 2) &&
                wpParentPos.y > (wpParent.sizeDelta.x / 2) && wpParentPos.y < Screen.height - (wpParent.sizeDelta.x / 2))
                {
                    //On screen
                    if (iScreenCheck == 1)
                    {
                        //Debug.Log("On Screen");
                        gameObjectIndicatorOnOffScreenStatus = 0;
                        parentOnScreen = true;
                        iScreenCheck--;
                    }
                }
                else
                {
                    //Off screen
                    if (iScreenCheck == 0)
                    {
                        //Debug.Log("Off Screen");
                        gameObjectIndicatorOnOffScreenStatus = 1;
                        parentOnScreen = false;
                        iScreenCheck++;
                    }
                }


                waypointParentCreated = true;
            }

        }


        //CREATE ICON UI
        //This will execute on initial play, and disabling/enabling the icon

        //SPRITE ICON
        void InstantiateWaypointIcon()
        {
            if (onScreenSprite != null && offScreenSprite != null) //Make sure BOTH icons are defined before making
            {
                spriteIndicator = new GameObject();
                spriteIndicator.layer = 5;
                spriteIndicatorRect = spriteIndicator.AddComponent<RectTransform>();
                spriteIndicatorRect.position = wpParent.position;
                spriteIndicator.transform.SetParent(wpParentGameObject.transform);
                spriteIndicatorRect.name = wpParent.name + "-Sprite";
                spriteIndicatorImage = spriteIndicatorRect.gameObject.AddComponent<Image>();
                spriteIndicatorImage.sprite = onScreenSprite;
                spriteIndicatorImage.rectTransform.localScale = new Vector2(newOnScreenSize.x, newOnScreenSize.y); //user set scale

                //Set the spriteIndicatorColorarency of an image component that conatains sprites/textures
                //spriteIndicatorColor = spriteIndicatorImage.color;
                spriteIndicatorColor = new Color32(0, 0, 0, 255);
                spriteIndicatorColor.a = 1f; //Default is transparent on spawn
                spriteIndicatorImage.color = spriteIndicatorColor;

                spriteIndicator.transform.SetSiblingIndex(spriteDepth);

                spriteIndicatorCreated = true;
            }
        }


        //GAME OBJECT ICON
        void InstantiateWaypointGameObject()
        {
            if (onScreenGameObject != null && offScreenGameObject != null)
            {
                //Onscreen
                if (gameObjectIndicatorOnOffScreenStatus == 0)
                {
                    Destroy(gameObjectIndicator);
                    gameObjectIndicator = Instantiate(onScreenGameObject, wpParentPos, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg)) as GameObject;
                    gameObjectIndicatorOnOffScreenStatus = 1;
                }
                if (gameObjectIndicatorOnOffScreenStatus == 1)
                {
                    Destroy(gameObjectIndicator);
                    gameObjectIndicator = Instantiate(offScreenGameObject, wpParentPos, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg)) as GameObject;
                    gameObjectIndicatorOnOffScreenStatus = 0;
                }

                //gameObjectIndicator = onScreenGameObject;
                gameObjectIndicator.layer = 5;
                gameObjectIndicator.transform.position = wpParent.position;
                gameObjectIndicator.transform.SetParent(wpParentGameObject.transform);
                gameObjectIndicator.name = wpParent.name + "-Prefab";

                #region Set Child Object's Sprite Color - (Ref only) This means nothing as it re-instantiates form on to off screen
                //Get the game objcet's child that houses the sprite (index 0)
                gameObjectIndicatorChildGameObject = gameObjectIndicator.transform.GetChild(0).gameObject;
                gameObjectIndicatorChildRect = gameObjectIndicatorChildGameObject.GetComponent<RectTransform>();
                gameObjectIndicatorChildImg = gameObjectIndicatorChildRect.GetComponent<RawImage>();

                //Set the color of the Image
                gameObjectIndicatorChildIndicatorColor = new Color32(0, 0, 0, 255);
                gameObjectIndicatorChildIndicatorColor.a = 1f; //Default is transparent on spawn
                gameObjectIndicatorChildImg.color = gameObjectIndicatorChildIndicatorColor;
                #endregion

                gameObjectIndicatorCanvasGroup = gameObjectIndicator.AddComponent<CanvasGroup>();
                gameObjectIndicatorCanvasGroup.alpha = 1f;
                gameObjectIndicatorCanvasGroup.blocksRaycasts = false;
                gameObjectIndicatorCanvasGroup.interactable = false;

                gameObjectIndicator.transform.SetSiblingIndex(gameObjectDepth);

                gameObjectIndicatorCreated = true;
            }

        }



        //CREATE TEXT UI 
        void InstantiateWaypointText()
        {
            textGameObject = new GameObject();
            textGameObject.layer = 5;
            textGameObject.transform.SetParent(wpParentGameObject.transform);
            textField = textGameObject.AddComponent<TextMeshProUGUI>();
            textField.transform.position = wpParent.position;
            textField.name = wpParent.name + "-Text";
            textField.fontSize = textSize;
            textField.font = textFont;
            textField.color = textColor;
            textField.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textRectWidth); //!!! MAKE THIS WIDTH FIT THE TEXT CHAR LENGTH
            textField.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textRectHeight);
            textField.rectTransform.localScale = new Vector2(1f, 1f);

            textGameObject.transform.SetSiblingIndex(textDepth);

            waypointTextCreated = true;
        }








        void FadeSpriteWithRange()
        {
            if (waypointDist < displayRange)
            {
                //Alpha in real time based off dist
                iconAlphaValue = 1 - (waypointDist / displayRange); //Derrived from "Waypoint Math" on Google Sheets

                //Adjust Alpha
                spriteIndicatorColor.a = iconAlphaValue;
                spriteIndicatorImage.color = spriteIndicatorColor;
            }
            else //Waypoint is beyond the player preset, so set scale
            {
                //Alpha set to 0
                iconAlphaValue = 0;
                spriteIndicatorColor.a = iconAlphaValue;
                spriteIndicatorImage.color = spriteIndicatorColor;
            }
        }



        void FadeGameObjectWithRange()
        {

            if (waypointDist < displayRange)
            {
                //Alpha in real time based off dist
                gameObjectIndicatorCanvasGroup.alpha = 1 - (waypointDist / displayRange); //Derrived from "Waypoint Math" on Google Sheets
            }
            else //Waypoint is beyond the player preset, so set scale
            {
                //Alpha set to 0
                gameObjectIndicatorCanvasGroup.alpha = 0f;
            }

        }



        void ScaleSpriteWithRange(Vector2 OnOffScreenSize) //This should take onScreenSpriteSize and offScreenSpriteSize arguments
        {
            if (waypointDist < displayRange)
            {
                //Scale in real time based off dist
                scaleValueX = OnOffScreenSize.x - ((OnOffScreenSize.x / displayRange) * waypointDist); //Derrived from "Waypoint Math" on Google Sheets
                scaleValueY = OnOffScreenSize.y - ((OnOffScreenSize.y / displayRange) * waypointDist);

                //spriteIndicatorRect.localScale = new Vector2(scaleValueX, scaleValueY);
                spriteIndicatorImage.rectTransform.localScale = new Vector2(scaleValueX, scaleValueY);
            }
            else //Waypoint is beyond the player preset, so set scale and alpha to 0
            {
                //Scale set to 0
                spriteIndicatorImage.rectTransform.localScale = new Vector2(0f, 0f);
            }
        }


        void ScaleGameObjectWithRange(float OnOffScreenSize) //This should take onScreenSpriteSize and offScreenSpriteSize arguments
        {
            if (waypointDist < displayRange)
            {

                //Scale in real time based off dist
                scaleValueGameObjectX = OnOffScreenSize - ((OnOffScreenSize / displayRange) * waypointDist); //Derrived from "Waypoint Math" on Google Sheets
                scaleValueGameObjectY = OnOffScreenSize - ((OnOffScreenSize / displayRange) * waypointDist);

                gameObjectIndicator.transform.localScale = new Vector3(scaleValueGameObjectX, scaleValueGameObjectY, 0);
            }
            else //Waypoint is beyond the player preset, so set scale and alpha to 0
            {
                //Scale set to 0
                gameObjectIndicator.transform.localScale = new Vector3(0, 0, 0);
            }
        }




        //DESTROY WAYPOINT PARENT
        void DestroyWaypointParent()
        {
            GameObject waypointParent = GameObject.Find(wpParent.name);
            if (waypointParent != null)
            {
                Destroy(waypointParent);
            }
        }

        //DESTROY WAYPOINT ICON (Sprite & Game Object)
        void DestroyWaypointIcon()
        {
            GameObject waypointIconSprite = GameObject.Find(wpParent.name + "-Sprite");
            if (waypointIconSprite != null)
            {
                Destroy(waypointIconSprite);
            }
        }
        void DestroyWaypointGameObject()
        {
            GameObject waypointIconGameObjcet = GameObject.Find(wpParent.name + "-Prefab");
            if (waypointIconGameObjcet != null)
            {
                Destroy(waypointIconGameObjcet);
            }
        }

        //DESTROY WAYPOINT TEXT
        void DestroyWaypointText()
        {
            GameObject waypointText = GameObject.Find(wpParent.name + "-Text");
            if (waypointText != null)
            {
                Destroy(waypointText);
            }
        }

        //Check if parent is on or off-screen then set the game object status accordingly
        //This ensures the right Game Object indicator shows for on or off screen when enabled and disabled during run time
        void SetGameObjectIndicatorStatus()
        {
            if (parentOnScreen)
            {
                gameObjectIndicatorOnOffScreenStatus = 0;
            }

            if (!parentOnScreen)
            {
                gameObjectIndicatorOnOffScreenStatus = 1;
            }
        }
        #endregion




        #region Centered Tracking Functions

        private void OnDrawGizmos()
        {
            if (enableRadiusGizmo)
            {
                Gizmos.color = radiusGizmoColor;
                Gizmos.DrawWireSphere(wpCenteredParentPos, radiusGizmoSize);
            }


        }

        //CREATE PARENT UI 
        void InstantiateCenteredWaypointParent()
        {
            if (mainCanvas != null)
            {
                wpCenteredParentGameObject = new GameObject();
                wpCenteredParentGameObject.layer = 5;
                wpCenteredParent = wpCenteredParentGameObject.AddComponent<RectTransform>();
                wpCenteredParent.transform.SetParent(mainCanvas.transform);
                wpCenteredParent.name = gameObject.name + GetInstanceID() + ": wpCenteredParent";
                wpCenteredParentImage = wpCenteredParent.gameObject.AddComponent<Image>();
                wpCenteredParentImage.color = diameterColor;
                wpCenteredParent.position = new Vector3(-1000f, -1000f, 0f); //This make sure the blinking box doesn't appear in bottom left at spawn

                if (wpCenteredParentPos.z > 0f &&
                wpCenteredParentPos.x > (wpCenteredParent.sizeDelta.x / 2) && wpCenteredParentPos.x < Screen.width - (wpCenteredParent.sizeDelta.x / 2) &&
                wpCenteredParentPos.y > (wpCenteredParent.sizeDelta.x / 2) && wpCenteredParentPos.y < Screen.height - (wpCenteredParent.sizeDelta.x / 2))
                {
                    //On screen
                    if (iScreenCheckCentered == 1)
                    {
                        //Debug.Log("On Screen");
                        //centeredParentOnScreen = true;
                        iScreenCheckCentered--;
                    }
                }
                else
                {
                    //Off screen
                    if (iScreenCheckCentered == 0)
                    {
                        //Debug.Log("Off Screen");
                        //centeredParentOnScreen = false;
                        iScreenCheckCentered++;
                    }
                }


                centeredWaypointParentCreated = true;
            }

        }


        //CREATE ICON UI 
        void InstantiateCenteredWaypointIcon()
        {
            if (onScreenCenteredSprite != null)
            {
                iconCenteredGameObject = new GameObject();
                iconCenteredGameObject.layer = 5;
                iconCentered = iconCenteredGameObject.AddComponent<RectTransform>();
                iconCentered.position = wpCenteredParent.position;
                iconCenteredGameObject.transform.SetParent(wpCenteredParentGameObject.transform);
                iconCentered.name = gameObject.name + ": wpIcon";
                iconCenteredImage = iconCentered.gameObject.AddComponent<Image>();
                iconCenteredImage.sprite = onScreenCenteredSprite;
                iconCenteredImage.rectTransform.localScale = new Vector2(newOnScreenCenteredSize.x, newOnScreenCenteredSize.y); //user set scale

                //Set the iconCenteredColorarency of an image component that conatains sprites/textures
                iconCenteredColor = iconCenteredImage.color;
                iconCenteredColor.a = 1f; //Default is transparent on spawn
                iconCenteredImage.color = iconCenteredColor;

                waypointIconCenteredCreated = true;
            }

        }





        void FadeWithRangeCentered()
        {

            if (waypointCenteredDist < onScreenCenteredRange)
            {
                //Alpha in real time based off dist
                iconCenteredAlphaValue = 1 - (waypointCenteredDist / onScreenCenteredRange); //Derrived from "Waypoint Math" on Google Sheets

                //Adjust Alpha
                iconCenteredColor.a = iconCenteredAlphaValue;
                iconCenteredImage.color = iconCenteredColor;
            }
            else //Waypoint is beyond the player preset, so set scale
            {
                //Alpha set to 0
                iconCenteredAlphaValue = 0;
                iconCenteredColor.a = iconCenteredAlphaValue;
                iconCenteredImage.color = iconCenteredColor;
            }

        }




        void ScaleWithRangeCentered(Vector2 OnOffScreenSize) //This should take onScreenCenteredSpriteSize and offScreenSize arguments
        {
            if (waypointCenteredDist < onScreenCenteredRange)
            {
                //Scale in real time based off dist
                centeredScaleValueX = OnOffScreenSize.x - ((OnOffScreenSize.x / onScreenCenteredRange) * waypointCenteredDist); //Derrived from "Waypoint Math" on Google Sheets
                centeredScaleValueY = OnOffScreenSize.y - ((OnOffScreenSize.y / onScreenCenteredRange) * waypointCenteredDist);

                //iconCentered.localScale = new Vector2(centeredScaleValueX, centeredScaleValueY);
                iconCenteredImage.rectTransform.localScale = new Vector2(centeredScaleValueX, centeredScaleValueY);
            }
            else //Waypoint is beyond the player preset, so set scale and alpha to 0
            {
                //Scale set to 0
                iconCenteredImage.rectTransform.localScale = new Vector2(0f, 0f);
            }
        }





        #endregion

    }
}
