using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable] public struct VecTrack
{
    [Range(-1, 1)]
    public float curva;
    public float distance;

    public VecTrack (float curva, float distance)
    {
        this.curva = curva;
        this.distance = distance;
    }
}


public class CorridaTeste : MonoBehaviour
{
    public Transform Car;
    public Transform Background;
    public int screenWidth = 160;
    public int screenHeight = 100;
    public SpriteRenderer pixelPref, treePref;
    public Color grassColor, grassLowColor, shoulderColor, shoulderLowColor, roadColor, lineColor, lineLowColor, squareColor, squareLowColor;
    public Sprite mushroomSprite, treeSprite, emptySprite;
    private GameObject grassHolder, shoulderHolder, roadHolder, treeHolder, lineHolder, squareHolderFirst, squareHolderSecond;

    private List<Vector2> defaultRoadPoses = new List<Vector2>();
    private List<Vector2> defaultShoulderPoses = new List<Vector2>();
    private List<Vector2> defaultGrassPoses = new List<Vector2>();
    private List<Vector2> defaultTreePoses = new List<Vector2>();
    private List<Vector2> defaultLinePoses = new List<Vector2>();
    private List<Vector2> defaultSquareFirstPoses = new List<Vector2>();
    private List<Vector2> defaultSquareSecondPoses = new List<Vector2>();

    private float fCarPos = 0.0f;
    private float fDistance = 0.0f;
    private float fSpeed = 0.0f;
    private float fMaxSpeed = 1.0f;
    private Animator kartAnimation;

    private float fCurvature = 0.0f;
    private float fTrackCurvature = 0.0f;
    private float fPlayerCurvature = 0.0f;

    public List <VecTrack> vecTrack = new List<VecTrack> ();
    public float sumDistance = 100f;

    public List <SpriteRenderer> grassList = new List<SpriteRenderer>();    
    public List <SpriteRenderer> roadList = new List<SpriteRenderer>();
    public List <SpriteRenderer> shoulderList = new List<SpriteRenderer>();
    public List <SpriteRenderer> treeList = new List<SpriteRenderer>();
    public List <SpriteRenderer> lineList = new List<SpriteRenderer>();
    public List<SpriteRenderer> squareFirstList = new List<SpriteRenderer>();
    public List<SpriteRenderer> squareSecondList = new List<SpriteRenderer>();



    // Start is called before the first frame update
    void Start()
    {
        kartAnimation = Car.GetComponent<Animator>();
        foreach (VecTrack track in vecTrack)
        {
            sumDistance += track.distance;
        }
       
        grassHolder = new GameObject();
        grassHolder.name = "grassHolder";
        shoulderHolder = new GameObject();
        shoulderHolder.name = "shoulderHolder";
        roadHolder = new GameObject();
        roadHolder.name = "roadHolder";
        treeHolder = new GameObject();
        treeHolder.name = "treeHolder";
        lineHolder = new GameObject();
        lineHolder.name = "lineHolder";
        squareHolderFirst = new GameObject();
        squareHolderFirst.name = "squareHolderOne";
        squareHolderSecond = new GameObject();
        squareHolderSecond.name = "squareHolderTwo";

        for(int y = 0; y < screenHeight; y++)
        {
            for(int x = -100;  x < screenWidth + 100; x++)
            {
                float fPerspective = (float)y/ (screenHeight/2.0f);

                float fMiddlePoint = 0.5f;
                float fRoadWidth = -0.05f + fPerspective * 1.2f;
                float fClipWidth = fRoadWidth * 0.15f;

                fRoadWidth *= 0.5f;

                float nLeftGrass = (fMiddlePoint - fRoadWidth - fClipWidth) * screenWidth;
                float nLeftClip = (fMiddlePoint - fRoadWidth) * screenWidth;
                float nRightGrass = (fMiddlePoint + fRoadWidth + fClipWidth) * screenWidth;
                float nRightClip = (fMiddlePoint + fRoadWidth) * screenWidth;
                int nRow = screenHeight / 2 + y;
                

                if((x >= nLeftClip - 20 && x < nLeftClip - 19) || (x <= nRightClip + 20 && x > nRightClip + 19))
                {
                    SpriteRenderer treeTemp = (SpriteRenderer)Instantiate(treePref, new Vector3(x, nRow), Quaternion.identity); 
                    treeTemp.transform.Rotate(new Vector3(180f, 0f, 0f));
                    treeTemp.transform.parent = treeHolder.transform;
                    treeList.Add(treeTemp);
                    defaultTreePoses.Add(treeTemp.transform.position);
                    treeTemp.transform.localScale = new Vector2(2 * Mathf.Pow(-1.0f + fPerspective, 3) + 3, 2 * Mathf.Pow(-1.0f + fPerspective, 3) + 3);
                    treeTemp.sortingOrder = y;
                }
            }
        }

        for(int y = 0; y < 2*screenHeight/3; y++)
        {
            for(int x = -100; x < screenWidth + 100; x++)
            {
                float fPerspective = (float)y / (screenHeight/ 2.0f);

                float fMiddlePoint = 0.5f;
                float fRoadWidth = 0.05f + fPerspective * 1.0f;
                float fClipWidth = fRoadWidth * 0.15f;

                fRoadWidth *= 0.5f;

                float nLeftGrass = (fMiddlePoint - fRoadWidth - fClipWidth) * screenWidth;
                float nLeftClip = (fMiddlePoint - fRoadWidth) * screenWidth;
                float nRightGrass = (fMiddlePoint + fRoadWidth + fClipWidth) * screenWidth;
                float nRightClip = (fMiddlePoint + fRoadWidth) * screenWidth;
                float nLeftLinePlace = (fMiddlePoint - (fRoadWidth / 3)) * screenWidth;
                float nLeftLineGirth = (fMiddlePoint - (fRoadWidth / 3) + 0.015f) * screenWidth;
                float nRightLinePlace = (fMiddlePoint + (fRoadWidth / 3)) * screenWidth;
                float nRightLineGirth = (fMiddlePoint + (fRoadWidth / 3) - 0.015f) * screenWidth;

                float nSquareOne = (nLeftClip + (nRightClip - nLeftClip) / 16) * screenWidth;
                float nSquareTwo = (nLeftClip + (2 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareThree = (nLeftClip + (3 * (nRightClip - nLeftClip) / 16)) * screenWidth;
                float nSquareFour = (nLeftClip + (4 *(nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareFive = (nLeftClip + (5 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareSix = (nLeftClip + (6 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareSeven = (nLeftClip + (7 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareEight = (nLeftClip + (8 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareNine = (nLeftClip + (9 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareTen = (nLeftClip + (10 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareEleven = (nLeftClip + (11 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareTwelve = (nLeftClip + (12 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareThirteen = (nLeftClip + (13 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareFourteen = (nLeftClip + (14 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareFifteen = (nLeftClip + (15 * (nLeftClip - nRightClip) / 16)) * screenWidth;
                float nSquareSixteen = (nLeftClip + (nLeftClip - nRightClip)) * screenWidth;
     


                int nRow = screenHeight / 2 + y;
                Color nGrassColor = Mathf.Sin(20.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) > 0.0f ? grassColor : grassLowColor;

                

             

                if(x >= -1000 && x < nLeftGrass)
                {
                    SpriteRenderer grassPixTemp = (SpriteRenderer)Instantiate(pixelPref, new Vector3(x, nRow), Quaternion.identity); grassPixTemp.color = nGrassColor;
                    grassPixTemp.transform.parent = grassHolder.transform;
                    grassList.Add(grassPixTemp);
                    defaultGrassPoses.Add(grassPixTemp.transform.position);
                }

                if (x >= nLeftGrass && x < nLeftClip)
                {
                    SpriteRenderer shoulderPixTemp = (SpriteRenderer)Instantiate(pixelPref, new Vector3(x, nRow), Quaternion.identity); shoulderPixTemp.color = shoulderColor;
                    shoulderPixTemp.transform.parent = shoulderHolder.transform;
                    shoulderList.Add(shoulderPixTemp);
                    defaultShoulderPoses.Add(shoulderPixTemp.transform.position);
                }
                
                if (sumDistance - fDistance >= 100f)
                {
                    if ((x >= nLeftClip && x < nLeftLinePlace) || (x >= nLeftLineGirth && x < nRightLineGirth) || (x >= nRightLinePlace && x < nRightClip))
                    {
                        SpriteRenderer roadPixTemp = (SpriteRenderer)Instantiate(pixelPref, new Vector3(x, nRow), Quaternion.identity); roadPixTemp.color = roadColor;
                        roadPixTemp.transform.parent = roadHolder.transform;
                        roadList.Add(roadPixTemp);
                        defaultRoadPoses.Add(roadPixTemp.transform.position);
                    }


                    if ((x >= nLeftLinePlace && x < nLeftLineGirth) || (x >= nRightLineGirth && x < nRightLinePlace))
                    {
                        SpriteRenderer linePixTemp = (SpriteRenderer)Instantiate(pixelPref, new Vector3(x, nRow), Quaternion.identity); linePixTemp.color = lineColor;
                        linePixTemp.transform.parent = lineHolder.transform;
                        lineList.Add(linePixTemp);
                        defaultLinePoses.Add(linePixTemp.transform.position);
                    }
                }

                else
                {
                    if ((x >= nLeftClip && x < nSquareOne) || (x >= nSquareTwo && x < nSquareThree) || (x >= nSquareFour && x < nSquareFive) || (x >= nSquareSix && x < nSquareSeven) || (x >= nSquareEight && x < nSquareNine) || (x >= nSquareTen && x < nSquareEleven) || (x >= nSquareTwelve && x < nSquareThirteen) || (x >= nSquareFourteen && x < nSquareFifteen))
                    {
                        SpriteRenderer squareFirstPixTemp = (SpriteRenderer)Instantiate(pixelPref, new Vector3(x, nRow), Quaternion.identity); squareFirstPixTemp.color = squareColor;
                        squareFirstPixTemp.transform.parent = squareHolderFirst.transform;
                        squareFirstList.Add(squareFirstPixTemp);
                        defaultSquareFirstPoses.Add(squareFirstPixTemp.transform.position);
                    }

                    if((x >= nSquareOne && x < nSquareTwo) || (x >= nSquareThree && x < nSquareFour) || (x >= nSquareFive && x < nSquareSix) || (x >= nSquareSeven && x < nSquareEight) || (x >= nSquareNine && x < nSquareTen) || (x >= nSquareEleven && x < nSquareTwelve) || (x >= nSquareThirteen && x < nSquareFourteen) || (x >= nSquareFifteen && x < nSquareSixteen))
                    {
                        SpriteRenderer squareSecondPixTemp = (SpriteRenderer)Instantiate(pixelPref, new Vector3(x, nRow), Quaternion.identity); squareSecondPixTemp.color = squareLowColor;
                        squareSecondPixTemp.transform.parent = squareHolderSecond.transform;
                        squareSecondList.Add(squareSecondPixTemp);
                        defaultSquareSecondPoses.Add(squareSecondPixTemp.transform.position);
                    }
                }
           
                if (x >= nRightClip && x < nRightGrass)
                {
                    SpriteRenderer shoulderPixTemp = (SpriteRenderer)Instantiate(pixelPref, new Vector3(x, nRow), Quaternion.identity); shoulderPixTemp.color = shoulderColor;
                    shoulderPixTemp.transform.parent = shoulderHolder.transform;
                    shoulderList.Add(shoulderPixTemp);
                    defaultShoulderPoses.Add(shoulderPixTemp.transform.position);
                }

                if (x >= nRightGrass && x < screenWidth + 1000)
                {
                    SpriteRenderer grassPixTemp = (SpriteRenderer)Instantiate(pixelPref, new Vector3(x, nRow), Quaternion.identity); grassPixTemp.color = nGrassColor;
                    grassPixTemp.transform.parent = grassHolder.transform;
                    grassList.Add(grassPixTemp);
                    defaultGrassPoses.Add(grassPixTemp.transform.position);
                }
            }
        }

        vecTrack.Add(new VecTrack(0, 1000));
    }

    public bool topGear = false;
    
    public Sprite SpriteChooser(float sin)
    {
        if(sin < -0.2)
        {
            return mushroomSprite;
        }

        else if (sin < 0.99)
        {
            return emptySprite;
        }
        else
        {
            return treeSprite;
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            fSpeed += 2.0f * Time.deltaTime;
        }
        
        else
        {
            fSpeed -= 0.8f * Time.deltaTime;
        }

        
        
        if (Input.GetKey(KeyCode.A) && fSpeed > 0)
        {
            fPlayerCurvature += 0.7f * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.D) && fSpeed > 0)
        {
            fPlayerCurvature -= 0.7f * Time.deltaTime;
        }

        if (Mathf.Abs(fPlayerCurvature - 0.2f - fTrackCurvature) >= 0.8f)
        {
            fSpeed -= 5.0f * Time.deltaTime;
        }

        if (fSpeed < 0.0f)
        {
            fSpeed = 0.0f;
        }

        if (fSpeed > fMaxSpeed)
        {
            fSpeed = Mathf.Lerp(fSpeed, 1.0f, 5 * Time.deltaTime);
        }


        fDistance += (50f * fSpeed) * Time.deltaTime;


        float fOffset = 0;
        int nTrackSection = 0;


        while(nTrackSection < vecTrack.Count && fOffset <= fDistance)
        {
            fOffset += vecTrack[nTrackSection].distance;
            nTrackSection++;
        }

        float fTargetCurvature = vecTrack[nTrackSection - 1].curva;

        float fTrackCurveDiff = (fTargetCurvature - fCurvature) * Time.deltaTime * fSpeed;
        fCurvature += fTrackCurveDiff;


        fTrackCurvature += fCurvature * Time.deltaTime * fSpeed;

        if (sumDistance - fDistance >= 100)
        {
            for (int i = 0; i < grassList.Count; i++)
            {
                float fPerspective = (float)(grassList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                grassList[i].color = Mathf.Sin(10.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) < 0.0f ? grassColor : grassLowColor;
                grassList[i].transform.position = new Vector2(defaultGrassPoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, grassList[i].transform.position.y);
            }

            for (int i = 0; i < shoulderList.Count; i++)
            {
                float fPerspective = (float)(shoulderList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                shoulderList[i].color = Mathf.Sin(15.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) < 0.0f ? shoulderColor : shoulderLowColor;
                shoulderList[i].transform.position = new Vector2(defaultShoulderPoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, shoulderList[i].transform.position.y);
            }


            for (int i = 0; i < roadList.Count; i++)
            {
                float fPerspective = (float)(roadList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                roadList[i].transform.position = new Vector2(defaultRoadPoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, roadList[i].transform.position.y);
            }


            for (int i = 0; i < treeList.Count; i++)
            {
                float fPerspective = (float)(treeList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                treeList[i].sprite = SpriteChooser(Mathf.Sin(10.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f));
                treeList[i].transform.position = new Vector2(defaultTreePoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, treeList[i].transform.position.y);
            }
         
            
            
            for (int i = 0; i < lineList.Count; i++)
            {
                float fPerspective = (float)(lineList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                lineList[i].color = Mathf.Sin(15.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) < 0.0f ? lineColor : lineLowColor;
                lineList[i].transform.position = new Vector2(defaultLinePoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, lineList[i].transform.position.y);
            }
            
        }

        else
        {
            for (int i = 0; i < grassList.Count; i++)
            {
                float fPerspective = (float)(grassList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                grassList[i].color = Mathf.Sin(10.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) < 0.0f ? grassColor : grassLowColor;
                grassList[i].transform.position = new Vector2(defaultGrassPoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, grassList[i].transform.position.y);
            }

            for (int i = 0; i < shoulderList.Count; i++)
            {
                float fPerspective = (float)(shoulderList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                shoulderList[i].color = Mathf.Sin(15.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) < 0.0f ? shoulderColor : shoulderLowColor;
                shoulderList[i].transform.position = new Vector2(defaultShoulderPoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, shoulderList[i].transform.position.y);
            }

            /*
            for (int i = 0; i < roadList.Count; i++)
            {
                float fPerspective = (float)(roadList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                roadList[i].transform.position = new Vector2(defaultRoadPoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, roadList[i].transform.position.y);
            }
            */
            
            for (int i = 0; i <squareFirstList.Count; i++)
            {
                float fPerspective = (float)(squareFirstList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                squareFirstList[i].color = Mathf.Sin(Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) < 0.0f ? squareColor : squareLowColor;
                squareFirstList[i].transform.position = new Vector2(defaultSquareFirstPoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, squareFirstList[i].transform.position.y);
            }

            for (int i = 0; i < squareSecondList.Count; i++)
            {
                float fPerspective = (float)(squareSecondList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                squareSecondList[i].color = Mathf.Sin(Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) < 0.0f ? squareLowColor : squareColor;
                squareSecondList[i].transform.position = new Vector2(defaultSquareSecondPoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, squareSecondList[i].transform.position.y);
            }
            

            for (int i = 0; i < treeList.Count; i++)
            {
                float fPerspective = (float)(treeList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                treeList[i].sprite = SpriteChooser(Mathf.Sin(10.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f));
                treeList[i].transform.position = new Vector2(defaultTreePoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, treeList[i].transform.position.y);
            }
            /*
            for (int i = 0; i < lineList.Count; i++)
            {
                float fPerspective = (float)(lineList[i].transform.position.y - screenHeight / 2) / (screenHeight / 2.0f);
                lineList[i].color = Mathf.Sin(15.0f * Mathf.Pow(1.0f - fPerspective, 3) + fDistance * 0.1f) < 0.0f ? lineColor : lineLowColor;
                lineList[i].transform.position = new Vector2(defaultLinePoses[i].x + (fCurvature * Mathf.Pow(1.0f - fPerspective, 3)) * screenWidth, lineList[i].transform.position.y);
            }
            */
        }


        fCarPos = fPlayerCurvature - fTrackCurvature;
        int nCarPos = screenWidth / 2 + ((int)(screenWidth * fCarPos) / 2) - 7;
        Car.position = new Vector2 (nCarPos, Car.position.y);

        kartAnimation.SetInteger("InputH", (int)(vecTrack[nTrackSection - 1].curva * 10));
        kartAnimation.SetFloat("AnimationSpeeders", fSpeed);
        
        Background.transform.position = new Vector3 (Background.transform.position.x - vecTrack[nTrackSection - 1].curva * fSpeed, Background.transform.position.y - fDistance/50000 * fSpeed);


        if (fDistance > sumDistance - 10) 
        {
            fSpeed -= 5 * Time.deltaTime;
        }
    }
}
