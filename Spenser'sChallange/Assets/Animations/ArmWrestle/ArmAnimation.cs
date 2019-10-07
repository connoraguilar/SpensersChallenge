using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmAnimation : MonoBehaviour {

    
    public List<GameObject> mJoints;
    public List<TextBoxComponents> mOptions;
    public Animator mAnimator;
    public PlayerInput mInput;
    public float mLimit;
    public float mPower;
    public float animationTime;
    private float animationTimer = 0;
    private bool animatorEnabled = false;
    public bool isPausing = false;
    private bool notAdjusted = true;
	// Use this for initialization
	void Start () {

        for (int i = 0; i < mOptions.Count; i++)
        {
            mOptions[i].Text.GetComponent<Text>().enabled = false;
            mOptions[i].TextBox.GetComponent<Image>().enabled = false;
        }
    }
	public void NewStart()
    {
        //InfluenceJoints();
        

        animatorEnabled = true;
    }
	// Update is called once per frame
	void Update () { 
        if (animatorEnabled)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer > animationTime)
            {
                if(mInput != null)
                {
                    
                    mInput.notAnimating = true;
                }
                mPower = 0;
                animatorEnabled = false;
                InfluenceJoints();
                mAnimator.enabled = false;
            }
            else if(animationTimer >= (animationTime - 4.5 )&& notAdjusted)
            {
                notAdjusted = false;
                if(mJoints.Capacity == 2)
                {
                    mPower += 1.0f;
                }
                else
                {
                    mPower -= 1.0f;
                }
                InfluenceJoints();
               
            }
            if(animationTimer >= (animationTime - 2.5))
            {
                for (int i = 0; i < mOptions.Count; i++)
                {
                    mOptions[i].Text.GetComponent<Text>().enabled = true;
                    mOptions[i].TextBox.GetComponent<Image>().enabled = true;

                }
            }
        }
        
    }

    public void InfluenceJoints()
    {
        if (!isPausing)
        {
            GetComponent<Animator>().enabled = false;
            //Debug.Log(this.name + " " + mPower);
            for (int i = 0; i < mJoints.Capacity; i++)
            {
                Vector3 rot = mJoints[i].transform.rotation.eulerAngles;

                if(Mathf.Abs(mPower) > 2)
                {
                    mPower = Mathf.Sign(mPower) * 2;
                }
                rot.x += mPower;
                

                mJoints[i].transform.rotation = Quaternion.Euler(rot);

            }
        }
    }
}
