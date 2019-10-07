using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxDriving : MonoBehaviour {
    public TextAsset mNPCDialouge;
    public TextAsset mCarColText;
    public TextAsset mColText;
    public TextAsset mEndText;
    public RectTransform mTextBox;
    public Image blackScreen;
    public Text mTBText;
    public GameObject dropOffPos;
    public GameObject CarObject;
    public string mText;
    private string mHazardText;
    public float mDisplayTime;
    public float textSpeed;
    public float changeTime;
    private float mTextTimer;
    private float changePosTimer = 0;
    public float mMaxX, mMinX, mMinY, mMaxY;
    public int mTextPos;
    private int hazardCount;
    public bool readingFromScript = true;
    private bool startDisplayTimer = false;
    private bool checkForEnd = false;
    private bool isDarkening = false;
    private bool hitHazard = false;
    private bool countHazard = false;
    // Use this for initialization
    void Start () {
        mTextTimer = 0;
        //mMaxX = Screen.safeArea.max.x - mTextBox.rect.width;
        //mMinX = mTextBox.rect.width*2; 
        //mMaxY = Screen.safeArea.max.y - mTextBox.rect.height;
        //mMinY= mTextBox.rect.height ;
        mText = mNPCDialouge.text;
        dropOffPos.GetComponent<MeshRenderer>().enabled = false;
        Vector2 newPosition = new Vector3(Random.Range(mMinX, mMaxX), Random.Range(mMinY, mMaxY));
        mTextBox.anchoredPosition = newPosition;
        StartCoroutine(UpdateTextBox());
        Debug.Log(mTextBox.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (/*mTextTimer >= mDisplayTime &&*/ readingFromScript)
        {
            startDisplayTimer = false;

           // StartCoroutine(UpdateTextBox());
            mTextTimer = 0;
        }
        else if(startDisplayTimer)
        {
            mTextTimer += Time.deltaTime;
        }

        if(checkForEnd)
        {
            if(mTextPos >= mText.Length )
            {
                mTBText.enabled = false;
                
                //GetComponentInParent<Transform>().gameObject.GetComponentInChildren<Camera>().
                DarkenScreen();
            }
        }

        changePosTimer += Time.deltaTime;
        if(changePosTimer >= changeTime)
        {
            Vector2 newPosition = new Vector3(Random.Range(mMinX, mMaxX), Random.Range(mMinY, mMaxY));
            mTextBox.anchoredPosition = newPosition;
            changePosTimer = 0;
        }
        
	}
    
    void DarkenScreen()
    {
        Color color = blackScreen.color;
        if(!isDarkening && color.a == 0)
        {
            isDarkening = true;
        }

        if (isDarkening)
        {

            color.a += 0.005f;

            blackScreen.color = color;
            if (color.a >= 1)
            {
                mText = "";
                GetComponentInParent<Drive>().gameObject.transform.position = dropOffPos.transform.position;
                isDarkening = false;
            }
        }
        else
        {
            BrightenScreen(color);
        }

        
    }

    void BrightenScreen(Color color)
    {
        CarObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        color.a -= 0.005f;
        if(color.a <= 0.005)
        {
            color.a = 0;
            checkForEnd = false;
        }
        blackScreen.color = color;
    }

    public IEnumerator UpdateTextBox()
    {
      
        mTextTimer = 0;
       
        

     
        string str = "";
        int currentTextBoxSize = 0;
        while (mTextPos < mText.Length && mText[mTextPos] != '\n')
        {
            if (hitHazard)
            {
                hitHazard = false;
                countHazard = true;
                string a = mText.Substring(0, mTextPos);
                string b = mText.Substring(mTextPos, mText.Length - mTextPos);
                mText = a + mHazardText + b;
                
            }
            if(countHazard)
            {
                hazardCount--;
                if(hazardCount <= 0)
                {
                    countHazard = false;
                }
            }

            currentTextBoxSize++;
            if (currentTextBoxSize >=50   || mText[mTextPos] == '*')
            {
                mTBText.text = "";
                str = "";
                currentTextBoxSize = 0;
            }
            str += mText[mTextPos];
            mTextPos++;
            mTBText.text = str;
            yield return new WaitForSeconds(textSpeed);
        }
        mTextPos++;
        startDisplayTimer = true;
    }

    public void CarAlert()
    {
        if (!countHazard && !hitHazard)
        {
            mHazardText = mCarColText.text;
            hazardCount = mHazardText.Length;
            mTextPos = 0;
            hitHazard = true;
            StopAllCoroutines();
            StartCoroutine(UpdateTextBox());
        }

    }

    public void CollisionAlert()
    {
        if (!countHazard && !hitHazard)
        {
            mHazardText = mColText.text;
            hitHazard = true;
            mTextPos = 0;
            hazardCount = mHazardText.Length;
            StopAllCoroutines();
            StartCoroutine(UpdateTextBox());
        }

    }
    public void PullOver()
    {
        GetComponentInParent<Transform>().Rotate(new Vector3(0, 0, 0));
        mText = mEndText.text;
        mTextPos = 0;
        StopAllCoroutines();
        StartCoroutine(UpdateTextBox());
        checkForEnd = true;
    }

}
