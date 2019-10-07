using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {

    public int inputState;
    
    public bool notAnimating = false;
    public ArmAnimation mArm;
    public ArmAnimation mEnemyArm;
    public TextBoxComponents option1Components;
    public TextBoxComponents option2Components;
    public TextBoxWrestle mTextBox;
    public Sprite invertedTextBox;
    public Sprite defaultTextBox;
    public KeyCode option1 = KeyCode.None;
    public KeyCode option2 = KeyCode.None;
    public KeyCode badKey;
    private KeyCode buttonMash;
    private KeyCode oneMash;
    private Color currentTextColor = Color.black;
    private Sprite currentTextBox;
    private float totalPower = 0;
    private float inputTimer = 0;
    private float maxRotation = 0;
    private bool isApproachingZero = true;
    private bool wasPressed = false;
    private bool notDetectingChoice = true;
    public bool badChoice = false;
    public bool frameDelay = false;
    private bool firstInput = true;
    // Use this for initialization
    void Start () {
		if(inputState ==0)
        {
            buttonMash = KeyCode.Space;
        }
        currentTextBox = invertedTextBox;
        defaultTextBox = option1Components.TextBox.GetComponent<Image>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
        if (notAnimating && !GameInformation.isPaused)
        {
            inputTimer += Time.deltaTime;
            if (Input.GetKeyDown(buttonMash) && notDetectingChoice)
            {
                if(oneMash == buttonMash)
                {
                    if (currentTextColor == Color.black)
                    {
                        currentTextColor = Color.white;
                        option1Components.Text.GetComponent<Text>().color = currentTextColor;
                        option1Components.TextBox.GetComponent<Image>().sprite = invertedTextBox;
                    }
                    else
                    {
                        currentTextColor = Color.black;
                        option1Components.Text.GetComponent<Text>().color = currentTextColor;
                        option1Components.TextBox.GetComponent<Image>().sprite = defaultTextBox;
                    }
                    
                }
                else
                {
                    if (currentTextColor == Color.black)
                    {
                        currentTextColor = Color.white;
                        option2Components.Text.GetComponent<Text>().color = currentTextColor;
                        option2Components.TextBox.GetComponent<Image>().sprite = invertedTextBox;
                    }
                    else
                    {
                        currentTextColor = Color.black;
                        option2Components.Text.GetComponent<Text>().color = currentTextColor;
                        option2Components.TextBox.GetComponent<Image>().sprite = defaultTextBox;
                    }
                    
                }
                wasPressed = true;
            }
            if (inputTimer >= .1)
            {
                ProcessInput();
            }
            if (option1 != KeyCode.None)
            {
                notDetectingChoice = false;
                DetectChoice();
            }
        }
        
    }
    
    void ProcessInput()
    {
        totalPower = 0;
        
        if(wasPressed)
        {
         
            totalPower += .25f;
        }

        if (badChoice)
        {
            totalPower -= .17f;
        }
        else
        {
           totalPower -= .13f;
           
        }

        if (mArm.mPower > 0)
        {
            mArm.mPower -= totalPower;//* 1.5f  ;
        }
        else
        {
            mArm.mPower -= totalPower;// +.025f;
        }

        if (totalPower > 0)
        {
            mEnemyArm.mPower += totalPower;
        }
        else
        {
            mEnemyArm.mPower +=  totalPower;
        }

        //if (!frameDelay)
        //{
            mArm.InfluenceJoints();
            mEnemyArm.InfluenceJoints();
        //}
        //else
        //{
        //    frameDelay = false;
        //}
        wasPressed = false;
        inputTimer = 0;

        if(isApproachingZero)
        {

            if (mArm.mJoints[0].transform.rotation.eulerAngles.x <= maxRotation || mArm.mJoints[0].transform.rotation.eulerAngles.x >= 300)
            {
                float rotDifference = maxRotation - mArm.mJoints[0].transform.rotation.eulerAngles.x;
                Vector3 ea = mArm.mJoints[0].transform.rotation.eulerAngles;
                ea.x = maxRotation;
                mArm.mJoints[0].transform.rotation = Quaternion.Euler(ea);
                for (int i = 1; i < mArm.mJoints.Capacity; i++)
                {
                    ea = mArm.mJoints[i].transform.rotation.eulerAngles;
                    ea.x += rotDifference;
                    mArm.mJoints[i].transform.rotation = Quaternion.Euler(ea);
                }
            }
        }
        else
        {
       
            if (mArm.mJoints[0].transform.rotation.eulerAngles.x <= maxRotation && mArm.mJoints[0].transform.rotation.eulerAngles.x > 200)
            {
                float rotDifference = maxRotation - mArm.mJoints[0].transform.rotation.eulerAngles.x;
                Vector3 ea = mArm.mJoints[0].transform.rotation.eulerAngles;
                ea.x = maxRotation;
                mArm.mJoints[0].transform.rotation = Quaternion.Euler(ea);
                for (int i = 1; i < mArm.mJoints.Capacity; i++)
                {
                    ea = mArm.mJoints[i].transform.rotation.eulerAngles;
                    ea.x += rotDifference;
                    mArm.mJoints[i].transform.rotation = Quaternion.Euler(ea);
                }
            }
        }
        
        if (((mArm.mJoints[0].transform.rotation.eulerAngles.x >= 85 || mArm.mJoints[0].transform.rotation.eulerAngles.x <= -75) && mArm.mJoints[0].transform.rotation.eulerAngles.x < 100) || maxRotation <= 300)
        {
            
            if(mArm.mJoints[0].transform.rotation.eulerAngles.x >= 85 && mArm.mJoints[0].transform.rotation.eulerAngles.x < 200)
            {
                mTextBox.EndGame(false);
                GameInformation.failedMinigame = true;
                Destroy(mArm.GetComponentInChildren<ArmCapsule>());
                Destroy(mEnemyArm.GetComponentInChildren<ArmCapsule>());
                Destroy(mArm);
                Destroy(mEnemyArm);
                Destroy(this);
            }
            else if(mArm.mJoints[0].transform.rotation.eulerAngles.x <= 300 && maxRotation >= 200)
            {
                Debug.Log(mArm.mJoints[0].transform.rotation.eulerAngles.x);
                mTextBox.EndGame(true);
                GameInformation.failedMinigame = false;
                Destroy(mArm.GetComponentInChildren<ArmCapsule>());
                Destroy(mEnemyArm.GetComponentInChildren<ArmCapsule>());
                Destroy(mArm);
                Destroy(mEnemyArm);
                Destroy(this);
            }
            
            
            
        }
    }

    void DetectChoice()
    {
        if(Input.GetKeyDown(option1))
        {
            if(option1 == badKey)
            {
                badChoice = true;
            }
            buttonMash = option1;
            oneMash = option1;
            option1 = KeyCode.None;
            option2 = KeyCode.None;
            option2Components.Text.GetComponent<Text>().enabled = false;
            option2Components.TextBox.GetComponent<Image>().enabled = false;
            notDetectingChoice = true;
            mTextBox.waitingForChoice = false;

            if (firstInput)
            {
                int pos = mTextBox.mTextPos;
                while (mTextBox.mText[pos] != '\n')
                {
                    pos++;
                }
                int subFactor = pos - mTextBox.mTextPos - 10;
                if (subFactor < 0)
                {
                    if ((pos - mTextBox.mTextPos) < 5)
                    {
                        subFactor = 0;
                    }
                    else
                    {
                        subFactor = 5;
                    }
                }
                mTextBox.mTextPos += subFactor;
            }
            firstInput = false;
        }
        else if(Input.GetKeyDown(option2))
        {
            if (option2 == badKey)
            {
                badChoice = true;
            }
            buttonMash = option2;
            option1 = KeyCode.None;
            option2 = KeyCode.None;
            option1Components.Text.GetComponent<Text>().enabled = false;
            option1Components.TextBox.GetComponent<Image>().enabled = false;
            notDetectingChoice = true;
            mTextBox.waitingForChoice = false;

            if (firstInput)
            {
                int pos = mTextBox.mTextPos;
                while (mTextBox.mText[pos] != '\n')
                {
                    pos++;
                }
                int subFactor = pos - mTextBox.mTextPos - 10;
                if (subFactor < 0)
                {
                    if ((pos - mTextBox.mTextPos) < 5)
                    {
                        subFactor = 0;
                    }
                    else
                    {
                        subFactor = 5;
                    }
                }
                mTextBox.mTextPos += subFactor;
            }
            firstInput = false;
        }
    }

    public void ChangeLimit(float newLimit, bool approachingZero)
    {
        if (maxRotation == 0)
        {
            maxRotation = 360 - newLimit;
        }
        else
        {
            maxRotation -= newLimit;
        }
        isApproachingZero = approachingZero;
    }
}

