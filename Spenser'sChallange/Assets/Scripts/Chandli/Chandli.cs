using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Chandli : MonoBehaviour {

    public GameObject mCamera;
    public bool mFollowCamera;
    public int mFullHealth;
    public int controlScheme;
    public float interactDistance;
    public string SaveFileName;
    public Vector3 mRespawnPosition;
    public Vector3 SpawnPosition;
    private int mHealth;
    private KeyCode interact;
    
    private bool checkForVisibility = false;
    //private bool interacting = false;
    private bool isOverlapping = false;
    private float visibleTimer = 0;
    private GameObject destroyObject;
    private GameObject overlapObject;
	// Use this for initialization
	void Start () {
        GameInformation.player = this;
        if(SceneManager.GetActiveScene().name == GameInformation.levelLoadedIn)
        {
            //transform.position = GameInformation.playerLoadPos;
        }
        SpawnPosition pos = FindObjectOfType<SpawnPosition>();
        mRespawnPosition = pos.transform.position;
        mHealth = mFullHealth;
        Physics.IgnoreLayerCollision(0, 10);
        if(controlScheme ==0)
        {
            interact = KeyCode.E;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameInformation.isPaused)
        {
            CheckForOverlap();
            if (checkForVisibility)
            {
                DestroyEnemyHandler();
            }
            if (!mFollowCamera)
            {
                CameraCheck();
            }
            if (Input.GetKeyDown(interact))
            {
                Interact();
                //interacting = true;
            }
            //else if(Input.GetKeyUp(interact))
            //{
            //    interacting = false;
            //}

        }
    }

    public void Interact()
    {
        if (overlapObject != null)
        {
            if (overlapObject.tag.Equals("Interactable"))
            {
                overlapObject.GetComponent<Interactable>().Interact();
            }
        }

        Vector3 forwardVector = transform.right.normalized;
        bool turnAround = false;
        if (!GetComponent<PlayerMove>().IsFacingRight())
        {
            forwardVector *= -1;
           
        }
        else
        {
            turnAround = true;
        }
        forwardVector *= interactDistance;
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        LayerMask NPCMask = LayerMask.GetMask("NPC");
        filter.SetLayerMask(NPCMask);
        RaycastHit2D[] results = new RaycastHit2D[1];
        Physics2D.Raycast(transform.position, forwardVector, filter, results, interactDistance);
        if (results[0])
        {
            results[0].rigidbody.GetComponentInParent<NPC>().ReadDialouge();
            if(turnAround)
            {
                results[0].rigidbody.GetComponentInParent<NPC>().facing = true;
                turnAround = false;
                results[0].rigidbody.gameObject.GetComponent<SpriteRenderer>().flipX = !results[0].rigidbody.gameObject.GetComponent<SpriteRenderer>().flipX;
            }
            else
            {
                results[0].rigidbody.GetComponentInParent<NPC>().facing = false;
            }
        }
       // }
    }

    public void CameraCheck()
    {
        //if(transform.position.x >= (mCamera.transform.position.x + 8))
        //{

        //    mCamera.GetComponent<CameraFollow>().ChangePosition(transform.position.x + 7.5f);
        //}
        //else if(transform.position.x <= (mCamera.transform.position.x - 8))
        //{

        //    mCamera.GetComponent<CameraFollow>().ChangePosition(transform.position.x - 7.5f);
        //}
    }

    public void DestroyEnemyHandler()
    {
        if (GetComponent<SpriteRenderer>().isVisible)
        {
            visibleTimer += Time.deltaTime;
            destroyObject.GetComponent<BoxCollider2D>().enabled = false;
            if (visibleTimer >= 0.1)
            {
                visibleTimer = 0;
                checkForVisibility = false;
                Destroy(destroyObject);
            }
        }
    }

    public void RemoveHealth()
    {
        mHealth--;
        if(mHealth == 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = mRespawnPosition;
        mHealth = mFullHealth;
      //  mCamera.GetComponent<CameraFollow>().Respawn();
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this, GameInformation.playerPath);
    }

    private void CheckForOverlap()
    {
        if (overlapObject != null)
        {
            if (overlapObject.tag.Equals("Projectile") && GetComponent<PlayerMove>().isPoofing == false)
            {
                Respawn();
            }
            else if (!checkForVisibility && overlapObject.tag.Equals("Enemy"))
            {
                if (GetComponent<PlayerMove>().isPoofing == false)
                {
                    Respawn();
                }
                else
                {
                    checkForVisibility = true;
                    destroyObject =overlapObject;
                }
            }
            else if(overlapObject.tag.Equals("Respawn"))
            {
                mRespawnPosition = overlapObject.transform.position;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        overlapObject = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        overlapObject = null;
    }
}
