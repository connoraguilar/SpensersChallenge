using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxWrestle : MonoBehaviour {
    public TextAsset mNPCDialouge;
    public TextAsset mGoodEnd;
    public TextAsset mBadEnd;
    public TextAsset mChoices;
    public ArmAnimation mPlayerArm;
    public ArmAnimation mEnemyArm;
    public PlayerInput mInput;
    public Door returnDoor;
    public List<bool> badChoices;
    public Text mTBText;
    public TextBoxComponents option1, option2;
    public float mDisplayTime;
    public float textSpeed;
    private float mTextTimer;
    public int mTextPos;
    private int mChoicesEntry = 0;
    public bool readingFromScript = true;
    public bool waitingForChoice = false;
    private bool startDisplayTimer = false;
    private bool displayingChoice = false;
    private bool endGame = false;
    private bool checkForEnd = false;
    public string mText;
    private string[] choices;
    // Use this for initialization
    void Start()
    {
        mTextTimer = 0;
        choices = mChoices.text.Split("\n"[0]);
        UpdateChoice();
        mText = mNPCDialouge.text;
        StartCoroutine(UpdateTextBox());
        //mEnemyArm.mLimit = 115;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!readingFromScript && !waitingForChoice)
        {
            readingFromScript = true;
            waitingForChoice = true;
            
            StartCoroutine(UpdateTextBox());
        }
        

        if(endGame)
        {
            ReturnToOverworld();
        }
    }

    public IEnumerator UpdateTextBox()
    {
       
        mTextTimer = 0;

        string str = "";
        int currentTextBoxSize = 0;
        bool display = true;
        float timer = 0;
        float displayTime = 1;
        while (mText[mTextPos] != '\n' || display)
        {
            currentTextBoxSize++;
            if (currentTextBoxSize >= 200 || mText[mTextPos] == '*')
            {
                mTextPos++;
                mTBText.text = "";
                str = "";
                currentTextBoxSize = 0;
            }
            if (mText[mTextPos] == '\n')
            {
                if (checkForEnd)
                {
                    endGame = true;
                }
                timer += Time.deltaTime;
                if(timer>= displayTime)
                {
                    display = false;
                }
            }
            else
            {
                str += mText[mTextPos];
                mTextPos++;
                mTBText.text = str;
            }
            
            yield return new WaitForSeconds(textSpeed);
        }
        //mTextPos++;
       

        if (!displayingChoice)
        {
           
            displayingChoice = true;
            if (mInput.badChoice)
            {
                //% is the bad line
                while (mText[mTextPos] != '%')
                {
                    mTextPos++;
                }
                mTextPos++;
            }
            else
            {
                //$ is the good line
                while (mText[mTextPos] != '$')
                {
                    mTextPos++;
                    int removeIndex = mText.IndexOf('%');
                    int endIndex = removeIndex;
                    int count = 0;
                    while (mText[endIndex] != '\n')
                    {
                        endIndex++;
                        count++;
                    }
                    mText.Remove(removeIndex, count);

                }
                mTextPos++;
            }
            waitingForChoice = false;
            readingFromScript = false;
            startDisplayTimer = true;
        }
        else
        {
            mEnemyArm.isPausing = false;
            mPlayerArm.isPausing = false;

            UpdateChoice();

            mPlayerArm.mPower = 0;
            mEnemyArm.mPower = 0;
            //mInput.frameDelay = true;
            mInput.ChangeLimit(30, false);

            option1.Text.GetComponent<Text>().enabled = true;
            option1.Text.GetComponent<Text>().color = Color.black;
            option1.TextBox.GetComponent<Image>().enabled = true;
            option1.TextBox.GetComponent<Image>().sprite = mInput.defaultTextBox;
            option2.Text.GetComponent<Text>().enabled = true;
            option2.Text.GetComponent<Text>().color = Color.black;
            option2.TextBox.GetComponent<Image>().enabled = true;
            option2.TextBox.GetComponent<Image>().sprite = mInput.defaultTextBox;
            displayingChoice = false;
            readingFromScript = false;
        }

    }

    void UpdateChoice()
    {
        KeyCode keycode = KeyCode.Keypad0;

        //while ((int)keycode == 37 || ((int)keycode >= 65 && (int)keycode <= 90) || ((int)keycode >= 123 && (int)keycode <= 272))
        //{
        //    //keycode = (KeyCode)Random.Range(32, 296);
        //}
        
        keycode = (KeyCode)Random.Range(97, 122);
        while(keycode == mInput.option1)
        {
            keycode = (KeyCode)Random.Range(97, 122);
        }
        string kc = keycode.ToString();
        option1.Text.GetComponent<Text>().text = choices[mChoicesEntry] + "\n" + kc;
       
        KeyCode forbidden = keycode;
        mInput.option1 = keycode;

        keycode = (KeyCode)Random.Range(97, 122);
        while (keycode == forbidden)
        {
            keycode = (KeyCode)Random.Range(97, 122);
        }

        mInput.option2 = keycode;

        if (badChoices[0])
        {
            mInput.badKey = forbidden;
        }
        else
        {
            mInput.badKey = keycode;
        }

        kc = keycode.ToString();
        option2.Text.GetComponent<Text>().text = choices[++mChoicesEntry] + "\n" + kc;
        mChoicesEntry++;
    }

    public void EndGame(bool good)
    {
        if (good)
        {
            mText = mGoodEnd.text;
            
        }
        else
        {
            mText = mBadEnd.text;
        }

        checkForEnd = true;
        mTextPos = 0;
        StopAllCoroutines();
        StartCoroutine(UpdateTextBox());
    }

    private void ReturnToOverworld()
    {
        returnDoor.Interact();
    }
}
