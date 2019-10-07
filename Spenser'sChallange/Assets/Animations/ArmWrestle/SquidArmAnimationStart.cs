using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquidArmAnimationStart : MonoBehaviour {

    public GameObject ArmCoil;
    public GameObject Hand;
    public GameObject Arm;
    public GameObject BobbleCoil;
    public Animator ChandliAni;
    public Image blackScreen;
    public float coilAnimationTime;
    public float handANimationTime;
    private float currentAnimationTime = 0;
    private bool animatingCoil = true;
    private bool animating = false;
    private bool stopBrighten = false;
	// Use this for initialization
	void Start () {
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        GetComponent<Animator>().enabled = false;
        ChandliAni.enabled = false;
        Hand.GetComponent<Animator>().enabled = false;
        Hand.GetComponent<MeshRenderer>().enabled = false;
        ArmCoil.GetComponent<MeshRenderer>().enabled = false;
        ArmCoil.GetComponent<Animator>().enabled = false;
        BobbleCoil.GetComponent<MeshRenderer>().enabled = false;
        BobbleCoil.GetComponent<Animator>().enabled = false;
        Color col = blackScreen.color;
        col.a = 1;
        blackScreen.color = col;
    }
	
	// Update is called once per frame
	void Update () {
        if (animating)
        {
            currentAnimationTime += Time.deltaTime;
            if(currentAnimationTime >= coilAnimationTime-1.125 && animatingCoil)
            {
                Hand.GetComponent<Animator>().enabled = true;
                Hand.GetComponent<MeshRenderer>().enabled = true;

            }
        
            if (currentAnimationTime >= coilAnimationTime && animatingCoil)
            {
                currentAnimationTime = 0;
                ArmCoil.GetComponent<Animator>().enabled = false;
                //BobbleCoil.GetComponent<Animator>().enabled = false;
                //BobbleCoil.GetComponent<MeshRenderer>().enabled = false;

                animatingCoil = false;
            }
           
            else if (currentAnimationTime >= handANimationTime)
            {
                Hand.GetComponent<Animator>().enabled = false;
                GetComponent<Animator>().enabled = true;
                GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                ChandliAni.enabled = true;
                //Arm.GetComponent<MeshRenderer>().enabled = false;
                //Hand.GetComponent<MeshRenderer>().enabled = false;

                Destroy(ArmCoil);
                Destroy(BobbleCoil);
                Destroy(Hand);
                Destroy(Arm);
                animating = false;
                
                GetComponentInParent<ArmAnimation>().NewStart();
            }
        }
        else
        {
            if (!stopBrighten)
                BrightenScreen();
        }
	}

    void BrightenScreen()
    {
        
        Color color = blackScreen.color;
        color.a -= 0.005f;
        
        if (color.a <= 0.005)
        {
            stopBrighten = true;
            ArmCoil.GetComponent<MeshRenderer>().enabled = true;
            ArmCoil.GetComponent<Animator>().enabled = true;
            BobbleCoil.GetComponent<MeshRenderer>().enabled = true;
            BobbleCoil.GetComponent<Animator>().enabled = true;
            color.a = 0;
            animating = true;
        }
        blackScreen.color = color;
    }
}
