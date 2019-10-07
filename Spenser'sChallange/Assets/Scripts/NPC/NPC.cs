using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public List<TextAsset> mDialouge;
    public Canvas mTextBox;
    public Image mImage;
    public PlayerMove mPlayer;
    public int maxBoxCharacters = 20;
    public int NPCIndex;
    private int dialougeEntry = 0;
    //right is true, left is false
    public bool facing;
    protected bool finishedReading = false;
    protected bool reading = false;
    protected bool interrupted = false;
    protected bool hasBeenTalkedTo = false;
    protected bool locked = false;
    private bool changeDialouge = false;
    protected string currentDialouge;
    protected int tolerance;
    private string path;


     ~NPC()
    {
        
        SaveSystem.SaveNPC(this);
    }
    // Use this for initialization
    public virtual void Start()
    {
        if (NPCIndex < GameInformation.NPCData.Count)
        {
            locked = GameInformation.NPCData[NPCIndex].locked;
            tolerance = GameInformation.NPCData[NPCIndex].tolerance;
            hasBeenTalkedTo = GameInformation.NPCData[NPCIndex].hasBeenTalkedTo;
        }
        if(locked)
        {
            dialougeEntry = mDialouge.Count - 2;
        }
        if(hasBeenTalkedTo)
        {
            dialougeEntry = mDialouge.Count - 3;
        }
        path = Application.persistentDataPath + "/" + name + ".211";
        mTextBox.enabled = false;
        mImage.enabled = false;
        SaveSystem.SaveNPC(this);
        //ReadDialouge();
    }



    // Update is called once per frame
    public virtual void Update()
    {
        if (!GameInformation.isPaused)
        {
            //SaveSystem.LoadNPC(path);
            CheckForInterrupt();
        }
    }

    public virtual void ReadDialouge()
    {
        if (!reading )
        {
            mImage.enabled = true;
            mTextBox.enabled = true;
            reading = true;
            currentDialouge = mDialouge[dialougeEntry].text;
            StartCoroutine(AnimateText(currentDialouge));
        }
    }

    IEnumerator AnimateText(string strComplete)
    {
        
        string str = "";
        int i = 0;
        int lengthCounter = 0;
        i = 0;
        int inturruptCount = mDialouge[mDialouge.Count - 1].text.Length;
        while (i < strComplete.Length)
        {
            if(interrupted && !changeDialouge && strComplete[i] == ' ' )
            {
                inturruptCount = mDialouge[mDialouge.Count - 1].text.Length;
                changeDialouge = true;
                mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<Text>().text = "";
                str = "";
                lengthCounter = 0;
                if (tolerance >= 0)
                {
                    //mDialouge[mDialouge.Capacity - 1].text
                   
                    string a  =strComplete.Substring(0, i);
                    string b = strComplete.Substring(i, strComplete.Length-i);
                    strComplete = a + mDialouge[mDialouge.Count - 1].text + b;
                }
                else
                {
                    inturruptCount = mDialouge[mDialouge.Count - 2].text.Length;
                    strComplete = "";
                    strComplete = mDialouge[mDialouge.Count - 2].text;
                    i = 0;
                    locked = true;
                }
            }

            if(interrupted)
            {
                inturruptCount--;
                if(inturruptCount ==0)
                {
                    interrupted = false;
                   
                }
            }

            str += strComplete[i++];
            lengthCounter++;
            mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<Text>().text = str;
            if(lengthCounter >= maxBoxCharacters && (strComplete[i-1]== '*'))
            {
                Debug.Log("equals");
                mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<Text>().text = "";
                str = "";
                lengthCounter = 0;
            }
            //Debug.Log(mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<Text>().);
            yield return new WaitForSeconds(0.05F);
        }
        finishedReading = true;
        if(!locked)
        {
            dialougeEntry++;
        }
        else
        {
            dialougeEntry = mDialouge.Count - 2 ;
        }
        //dialougeEntry++;
        reading = false;
        hasBeenTalkedTo = true;
        SaveSystem.SaveNPC(this);
        SaveSystem.SaveGame(GameInformation.player, GameInformation.playerPath);
    }

    void CheckForInterrupt()
    {
        if(reading && !locked && !hasBeenTalkedTo)
        {
            if(mPlayer.IsFacingRight() && !facing && !interrupted)
            {
                changeDialouge = false;
                interrupted = true;
                tolerance--;
                SaveSystem.SaveNPC(this);
              
            }
            else if(!mPlayer.IsFacingRight() && facing && !interrupted)
            {
                changeDialouge = false;
                interrupted = true;
                tolerance--;
                SaveSystem.SaveNPC(this);
            }
            else
            {
                //interrupted = false;
                //changeDialouge = false;
            }
        }
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void SetLocked(bool isLocked)
    {
        locked = isLocked;
    }

    public bool HasBeenTalkedTo()
    {
        return hasBeenTalkedTo;
    }

    public void SetHasBeenTalkedTo(bool talk)
    {
        hasBeenTalkedTo = talk;
    }

    public int GetTolerance()
    {
        return tolerance;
    }

    public void SetTolerance(int t)
    {
        tolerance = t;
    }
}




//int maxLineLength = 0;
//int currentLineCount = 0;
//while (i < strComplete.Length)
//{
//    currentLineCount++;

//    if (strComplete[i++].Equals('\n'))
//    {
//        //strComplete.Insert(i, "\n");
//        //Debug.Log(currentLineCount);
//        if (currentLineCount > maxLineLength)
//            maxLineLength = currentLineCount;
//        currentLineCount = 0;
//        lineCount++;
//    }
//}
////18, 10
//Debug.Log(lineCount + " " + maxLineLength);
//Vector2 textLeftBottom = mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<RectTransform>().offsetMin;
//Vector2 textRightTop = mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<RectTransform>().offsetMax;
//Vector2 textRect = mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<RectTransform>().sizeDelta;
//Debug.Log(lineCount + " " + maxLineLength);

//float horSizePerLetter = mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<Text>().fontSize / 2f;
//float vertSizePerLetter = mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<Text>().fontSize / 2.5f;
//textRect.x = horSizePerLetter * maxLineLength;
//textRect.y = 18 * lineCount;
//textRightTop.x += horSizePerLetter * maxLineLength;
//textLeftBottom.y -= vertSizePerLetter * lineCount;
//mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<RectTransform>().sizeDelta = textRect;
//mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<RectTransform>().offsetMin = textLeftBottom;
//mTextBox.GetComponent<TextBoxComponents>().Text.GetComponent<RectTransform>().offsetMax = textRightTop;


//Vector2 leftBottom = mTextBox.GetComponent<TextBoxComponents>().TextBox.GetComponent<RectTransform>().offsetMin;
//Vector2 rightTop = mTextBox.GetComponent<TextBoxComponents>().TextBox.GetComponent<RectTransform>().offsetMax;
//rightTop.x += horSizePerLetter * maxLineLength;
//leftBottom.y -= vertSizePerLetter * lineCount;
//mTextBox.GetComponent<TextBoxComponents>().TextBox.GetComponent<RectTransform>().offsetMin = leftBottom;
//mTextBox.GetComponent<TextBoxComponents>().TextBox.GetComponent<RectTransform>().offsetMax = rightTop;