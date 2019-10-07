using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {

    public int inputState;
    public float mSpeed;
    public TextAsset trainText;
    public TextAsset potholeText;
    public TextAsset goodEnding;
    public GameObject trainChoice;
    public GameObject potholeChoice;
    public GameObject carWheel;
    private KeyCode forward;
    private KeyCode back;
    private KeyCode left;
    private KeyCode right;
    private KeyCode choiceA;
    private KeyCode choiceB;
    private KeyCode choiceC;
    private Quaternion startingRotation;
    private float readChoiceTimer = 0;
    private float readTime;
    private float yPos;
    private float rotationSpeed = 20;
    private int hitCount = 0;
    private int choicePosition;
    private bool isHalting = false;
    private bool startChoiceTimer;
    private bool leftPressed = false;
    private bool rightPressed = false;
    public bool detectChoice = false;
    private DrivingCue mDrivingCue;
    // Use this for initialization
    void Start () {
		if(inputState ==0)
        {
            left = KeyCode.A;
            right = KeyCode.D;
            forward = KeyCode.W;
            back = KeyCode.S;
        }
        yPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        if(transform.position.y != yPos)
        {
            Vector3 pos = transform.position;
            pos.y = yPos;
            transform.position = pos;
        }
        if (!isHalting)
            ProcessInput();
        else if(detectChoice)
            ProcessChoice();

        if(startChoiceTimer)
        {

            //readChoiceTimer += Time.deltaTime;
            //readChoiceTimer >= readTime
            if (choicePosition <= GetComponentInChildren<TextBoxDriving>().mTextPos)
            {
                string f  = GetComponentInChildren<TextBoxDriving>().mText;
                string a = f;
                GetComponentInChildren<TextBoxDriving>().readingFromScript = false;
                startChoiceTimer = false;
                readChoiceTimer = 0;
                detectChoice = true;
            }
        }
	}

    void ProcessInput()
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        if(Input.GetKey(forward))
        {
            
            
            velocity.x += transform.forward.x * Time.deltaTime * mSpeed;
            velocity.y += transform.forward.y * Time.deltaTime * mSpeed;
            velocity.z += transform.forward.z * Time.deltaTime * mSpeed;

        }
        if (Input.GetKey(left))
        {
            leftPressed = true;
            //Vector3 up = new Vector3(0, 1, 0);
            //transform.Rotate(up, -1);
            //+= Time.deltaTime * mSpeed;
            //velocity.x -= Time.deltaTime * mSpeed;
            velocity.x -= transform.right.x * Time.deltaTime * mSpeed;
            velocity.y -= transform.right.y * Time.deltaTime * mSpeed;
            velocity.z -= transform.right.z * Time.deltaTime * mSpeed;
            Vector3 rotation = carWheel.transform.rotation.eulerAngles;
            rotation.x += Time.deltaTime * rotationSpeed;
            Quaternion rot = new Quaternion();
            rot.eulerAngles = rotation;
            carWheel.transform.rotation = rot;
            //if(transform.localRotation.eulerAngles.y == 90)
            //{

            //}

        }
        else
        {
            Vector3 rotation = carWheel.transform.rotation.eulerAngles;
            if (!rightPressed && rotation.x >0)
            {
                rotation = carWheel.transform.rotation.eulerAngles;
                rotation.x -= Time.deltaTime * rotationSpeed * 1.5f;
                if (rotation.x < 0)
                {
                    rotation.x = 0;
                }
                Quaternion rot = new Quaternion();
                rot.eulerAngles = rotation;
                carWheel.transform.rotation = rot;
            }
            leftPressed = false;
        }
        if (Input.GetKey(back))
        {

            //velocity.z -= Time.deltaTime * mSpeed;
            velocity.x -= transform.forward.x * Time.deltaTime * mSpeed;
            velocity.y -= transform.forward.y * Time.deltaTime * mSpeed;
            velocity.z -= transform.forward.z * Time.deltaTime * mSpeed;
            //if(velocity.z <= 0)
            //{
            //    velocity.z = 0;
            //}
            //else if(velocity.x <= 0)
            //{
            //    velocity.x = 0;
            //}
            Debug.Log(velocity.z + " " + velocity.x);
        }
        if (Input.GetKey(right))
        {
            rightPressed = true;
            velocity.x += transform.right.x * Time.deltaTime * mSpeed;
            velocity.y += transform.right.y * Time.deltaTime * mSpeed;
            velocity.z += transform.right.z * Time.deltaTime * mSpeed;
            Vector3 rotation = carWheel.transform.rotation.eulerAngles;
            rotation.x -= Time.deltaTime * rotationSpeed;
            Quaternion rot = new Quaternion();
            rot.eulerAngles = rotation;
            carWheel.transform.rotation = rot;
        }
        else
        {
            Vector3 rotation = carWheel.transform.rotation.eulerAngles;
            if (!rightPressed && rotation.x > 270)
            {
                rotation.x -= 360;
                rotation.x += Time.deltaTime * rotationSpeed * 1.5f * 2;
                
                if (rotation.x > 0)
                {
                    rotation.x = 0;
                }
                Quaternion rot = new Quaternion();
                rot.eulerAngles = rotation;
                carWheel.transform.rotation = rot;
            }
            rightPressed = false;
        }


        GetComponent<Rigidbody>().velocity = velocity;
    }

    void ProcessChoice()
    {
        if (Input.GetKeyDown(choiceA))
        {
            detectChoice = false;
            mDrivingCue.PerformFunction(choiceA, this.gameObject);
        }
        else if (Input.GetKeyDown(choiceB))
        {
            detectChoice = false;
            mDrivingCue.PerformFunction(choiceB, this.gameObject);
        }
        else if (Input.GetKeyDown(choiceC))
        {
            detectChoice = false;
            mDrivingCue.PerformFunction(choiceC, this.gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("dialougeCue"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            
            mDrivingCue = other.gameObject.GetComponent<DrivingCue>();
            List<KeyCode> keyChoices = mDrivingCue.mKeyChoices;
            choiceA = keyChoices[0];
            choiceB = keyChoices[1];
            choiceC = keyChoices[2];

            if (choiceA != KeyCode.None)
            {
                string textboxText = GetComponentInChildren<TextBoxDriving>().mText;
                string cueText = other.gameObject.GetComponent<DrivingCue>().mText;
                int mTextPos = GetComponentInChildren<TextBoxDriving>().mTextPos;
                int newLinePos = mTextPos;
                while(textboxText[newLinePos] != '\n')
                {
                    newLinePos++;
                }
                choicePosition = newLinePos;
                string a = textboxText.Substring(0,newLinePos );
                string b = textboxText.Substring(mTextPos, textboxText.Length - newLinePos);
                GetComponentInChildren<TextBoxDriving>().mText = a + cueText + b;
                //.Insert(textboxText.Length-1,cueText);
                textboxText = GetComponentInChildren<TextBoxDriving>().mText;
                //GetComponentInChildren<TextBoxDriving>().mTextPos = 0;


                startChoiceTimer = true;
            }

            
            isHalting = true;
            

            if (choiceA != KeyCode.None)
            {
                //StartCoroutine(GetComponentInChildren<TextBoxDriving>().UpdateTextBox());
            }
            else
            {
                mDrivingCue.PerformFunction(KeyCode.None, this.gameObject);
            }
            Destroy(other);
        }
        else if(other.gameObject.tag.Equals("End"))
        {
            GetComponentInChildren<TextBoxDriving>().mEndText = goodEnding;
                 isHalting = true;
            GetComponent<Collider>().enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isHalting)
        {
            if (collision.gameObject.tag.Equals("Car") && GetComponent<Rigidbody>().velocity.z != 0)
            {
                GetComponentInChildren<TextBoxDriving>().CarAlert();
                hitCount++;
            }
            else
            {
                GetComponentInChildren<TextBoxDriving>().CollisionAlert();
                hitCount++;
            }

            if(collision.gameObject == trainChoice)
            {
                GetComponentInChildren<TextBoxDriving>().mEndText = trainText;
                //  transform.Rotate()
                
                GetComponentInChildren<TextBoxDriving>().PullOver();
               
                isHalting = true;
                GetComponent<Collider>().enabled = false;
            }
            else if(collision.gameObject == potholeChoice)
            {
                GetComponentInChildren<TextBoxDriving>().mEndText = potholeText;
                GetComponentInChildren<TextBoxDriving>().PullOver();
                isHalting = true;
                GetComponent<Collider>().enabled = false;
            }

            if (hitCount > 2)
            {
                isHalting = true;
                GetComponent<Collider>().enabled = false;

                GetComponentInChildren<TextBoxDriving>().PullOver();
            }

        }
    }

    public void SetHalting(bool haltState)
    {
        isHalting = haltState;
        detectChoice = false;
    }
}


