using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public string rightMove;
    public string leftMove;
    public string jump;
    public int health;
    public float mSpeed;
    public float jumpHeight;
    public float mAirSpeed;
    public float mTeleportCooldown;
    public float mTeleportDistance;
    public bool mSlipping;
    public bool isPoofing;
    private bool mSpacePressed = false;
    private bool mInAir = false;
    private bool mFreeze;
    private bool playingJump = false;
    private bool isTeleporting = false;
    private bool mFacingRight = true;
    private bool moving;
    private bool canJump = true;
    private bool wasClicking = false;
    private bool startPoofAnimation = false;
    private bool startPoofTimer = false;
    private float currTime = 0f;
    private float heldTime = 0f;
    private float distToGround = 0;
    private int jumpCount = 0;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode poofKey;
    private Vector3 previousPos;
    private Vector3 poofPosition;
    private float mPoofTime = 0;
    private Animator anim;

    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator>();
        GetComponent<Rigidbody2D>().freezeRotation = true;
        GetComponent<Chandli>().mCamera.GetComponent<CameraFollow>().mFollowCamera = GetComponent<Chandli>().mFollowCamera;
        distToGround = GetComponent<BoxCollider2D>().size.y;
        int controlScheme = GetComponent<Chandli>().controlScheme;

        if(controlScheme ==0)
        {
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            downKey = KeyCode.S;
            upKey = KeyCode.W;
            poofKey = KeyCode.LeftShift;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        Vector3 currPos = transform.position;
        currTime = Time.deltaTime;
     
     
        //This handles timing for animations and such when chandli is poofing
        if(startPoofTimer)
        {
           
            mFreeze = false;
           
            //this is just reusing a timer for efficency
            mPoofTime += Time.deltaTime;
            
            if (mPoofTime >= 0.4)
            {
                transform.position = poofPosition;
                anim.SetBool("StartPoof", false);
                GetComponent<SpriteRenderer>().enabled = true;
                
                //GetComponent<Chandli>().mCamera.GetComponent<CameraFollow>().mFollowCamera = true;
            }
            else if(mPoofTime >= .2 && mPoofTime < 0.5)
            {
               
            }

            if (mPoofTime >= 0.45)
            {
                anim.SetTrigger("EndPoof");
                isPoofing = false;
                startPoofTimer = false;
                mPoofTime = 0;
                wasClicking = false;
            }
        }

        //This is a delay to allow the starting animation for the poof to play
        if(startPoofAnimation)
        {
           
            mPoofTime += Time.deltaTime;
            if(mPoofTime >0.33)
            {
                Poof(currPos);
            }
        }

        if (wasClicking && mFreeze)
        {
            if (Input.GetKey(poofKey))
            {
                if (Input.GetKey(leftKey) || Input.GetKey(rightKey) || Input.GetKey(downKey) || Input.GetKey(upKey))
                {
                 
                    Poof(currPos);
                }
            }
            else 
            {
                mFreeze = false;
                wasClicking = false;
            }
        }

        if (!mFreeze && !mSlipping && !startPoofTimer)
        {
            //Check to see if they are still inputting direction, and have been

            if (Input.GetKeyDown(poofKey) && !wasClicking && !startPoofTimer)
            {
                wasClicking = true;
                mFreeze = true;
            }
            else
            { 
                if (Input.GetKey(rightMove))
                {
                    if (!mFacingRight)
                    {
                        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                    }
                    mFacingRight = true;
                    float finalSpeed = mSpeed;
                    if (mInAir)
                    {
                        finalSpeed = mAirSpeed * 0.75f;
                    }

                    //currPos.x += finalSpeed * Time.deltaTime;
                    Vector2 newVel = GetComponent<Rigidbody2D>().velocity;
                    newVel.x = finalSpeed;
                    GetComponent<Rigidbody2D>().velocity = newVel;
                    if (!moving && mInAir == false)
                    {
                        moving = true;
                        anim.SetTrigger("Run");
                    }
                    else if(mInAir)
                    {
                        if (!playingJump)
                        {
                            //anim.SetTrigger("Jump");
                            playingJump = true;
                        }
                    }
                }
                else if (Input.GetKey(leftMove))
                {
                    if (mFacingRight)
                    {
                        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                    }
                    mFacingRight = false;
                    float finalSpeed = mSpeed;
                    if (mInAir)
                    {
                        finalSpeed = mAirSpeed * 0.75f;
                    }

                    Vector2 newVel = GetComponent<Rigidbody2D>().velocity;
                    newVel.x = -finalSpeed;
                    GetComponent<Rigidbody2D>().velocity = newVel;

                    if (!moving && mInAir == false)
                    {
                        moving = true;
                        anim.SetTrigger("Run");
                    }
                    else if(mInAir)
                    {

                        if (!playingJump)
                        {
                           // anim.SetTrigger("Jump");
                            playingJump = true;
                        }
                    }
                }
                else
                {
                    if (moving)
                    {
                        Vector2 newVel = GetComponent<Rigidbody2D>().velocity;
                        newVel.x = 0;
                        GetComponent<Rigidbody2D>().velocity = newVel;
                        moving = false;
                        anim.SetTrigger("Idle");
                    }
                }

                if (GetComponent<Rigidbody2D>().velocity.y == 0)
                {
                    mInAir = false;
                    playingJump = false;
                }
                
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                
                wasClicking = false;
            }

            CheckForJump();
        }
        else if(mSlipping)
        {
            if (!mFacingRight)
                GetComponent<SpriteRenderer>().flipX = false;

            currPos.x += mSpeed * Time.deltaTime;
            transform.position = currPos;
            
            CheckForJump();
        }
        else if(startPoofTimer)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        if (GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            

           if (IsGrounded())
           {
                anim.SetBool("Jump", false);
                mInAir = false;
                heldTime = 0;
                jumpCount = 0;
                canJump = false;
           }
        }
        anim.SetFloat("JumpVelocity", GetComponent<Rigidbody2D>().velocity.y);
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        LayerMask groundLayer = LayerMask.GetMask("Platforms");
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    void CheckForJump()
    {

        if (!mInAir)
        {

            if (!playingJump)
            {
                anim.SetBool("Jump", true);
                playingJump = true;
            }

        }


        mSpacePressed = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            jumpCount++;
            canJump = true;

        }
        else if(Input.GetKeyUp(KeyCode.Space) && canJump)
        {
            canJump = true;
        }

        if (heldTime <= .3 && mSpacePressed == true && jumpCount <= 1 && canJump)
        {
      
            Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            velocity.y = Mathf.Sqrt(4 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            velocity.y += Physics2D.gravity.y * Time.deltaTime;
            GetComponent<Rigidbody2D>().velocity = velocity;
            heldTime += Time.deltaTime;
            mInAir = true;
            
            //    heldTime += currTime;
            //    Vector3 vel = GetComponent<Rigidbody2D>().velocity; vel.y += 5f * currTime;
            //    GetComponent<Rigidbody2D>().velocity = vel;
        }
    }

    private void Poof(Vector3 currPos)
    {
        if (!startPoofAnimation)
        {
            isTeleporting = true;
            isPoofing = true;
            KeyCode directionKey = 0;
            Vector2 poofDir = Vector2.zero;


            if (Input.GetKey(rightMove))
            {
                directionKey = KeyCode.D;
                //currPos.x = currPos.x + mTeleportDistance;
                poofDir.x += 1;
                startPoofTimer = true;
            }
            if (Input.GetKey(leftMove))
            {
                directionKey = KeyCode.A;
                poofDir.x += -1;
                //currPos.x = currPos.x - mTeleportDistance;
                startPoofTimer = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                poofDir.y += 1;
                directionKey = KeyCode.W;
               // currPos.y = currPos.y + mTeleportDistance;
                startPoofTimer = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                poofDir.y += -1;
                directionKey = KeyCode.S;
                //currPos.y = currPos.y - mTeleportDistance;
                startPoofTimer = true;
            }
            
            poofDir *= mTeleportDistance;
            currPos.x += poofDir.x;
            currPos.y += poofDir.y + 0.05f;

           

            GetComponent<SpriteRenderer>().enabled = false;

            Vector3 positionPreTeleport = transform.position;
            transform.position = currPos;

            ContactFilter2D platformFilter = new ContactFilter2D();
            platformFilter.useLayerMask = true;
            LayerMask platformLayer = LayerMask.GetMask("Platforms");
            platformFilter.SetLayerMask(platformLayer);
            Collider2D[] results = new Collider2D[5];
            results[0] = null;
            GetComponent<Rigidbody2D>().OverlapCollider(platformFilter, results);
            //bool top, bottom, left, right = false;


            if (results[0] != null)
            {
                float halfWidth = GetComponent<BoxCollider2D>().size.x / 2;
                float halfHeight = GetComponent<BoxCollider2D>().size.y / 2;
                float playerMinX = transform.position.x - halfWidth;
                float playerMinY = transform.position.y - halfHeight;
                float playerMaxX = transform.position.x + halfWidth;
                float playerMaxY = transform.position.y + halfHeight;

                float boxWidth = results[0].transform.localScale.x / 2;
                float boxHeight = results[0].transform.localScale.y / 2;
                float boxMinX = results[0].transform.position.x - boxWidth;
                float boxMinY = results[0].transform.position.y - boxHeight;
                float boxMaxX = results[0].transform.position.x + boxWidth;
                float boxMaxY = results[0].transform.position.y + boxHeight;

                Vector3 vel = GetComponent<Rigidbody2D>().velocity;
                
                float maxVel = Mathf.Max(Mathf.Abs(vel.x), Mathf.Abs(vel.y));
                Vector3 newPos = transform.position;


                if (directionKey == KeyCode.D || directionKey == KeyCode.A)
                {
                    if (directionKey == KeyCode.D)
                    {
                        newPos.x = boxMinX - 0.5f;
                    }
                    else
                    {
                        newPos.x = boxMaxX + 0.5f;
                    }
                    //GetComponent<BoxCollider2D>().size.y;
                }
                else
                {
          
                    if (directionKey == KeyCode.W)
                    {
                        newPos.y = boxMinY - 0.5f;
                    }
                    else
                    {
                        newPos.y = boxMaxY + 0.5f;
                    }
                }

                poofPosition = newPos; // positionPreTeleport;
            }
            else
            {
               
                poofPosition = currPos;
            }
            anim.SetBool("StartPoof", true);
            GetComponent<SpriteRenderer>().enabled = true;
            startPoofAnimation = true;
            mFreeze = true;
            transform.position = positionPreTeleport;
        }
        else
        {
          
            transform.position = poofPosition;
            GetComponent<SpriteRenderer>().enabled = false;
            mPoofTime = 0;
            startPoofAnimation = false;
            startPoofTimer = true;
            
        }
        
    }

    public void FreezeMovement(bool freeze)
    {
        mFreeze = freeze;
    }
    
    public bool IsTeleporting()
    {
        return isTeleporting;
    }
    
    public bool IsFacingRight()
    {
        return mFacingRight;
    }
}

////Platform Collision
//ContactFilter2D platformFilter = new ContactFilter2D();
//platformFilter.useLayerMask = true;
//LayerMask platformLayer = LayerMask.GetMask("Platforms");
//platformFilter.SetLayerMask(platformLayer);
//Collider2D[] results = new Collider2D[5];
//results[0] = null;
//GetComponent<Rigidbody2D>().OverlapCollider(platformFilter, results);
//results[0] = null;
//GetComponent<Rigidbody2D>().OverlapCollider(platformFilter, results);
//if (mInAir == false && results[0] != null)
//{
//    //transform.position = colPos;
//    //while(results[0] != null)
//    //{
//    //    Vector3 incPos = transform.position;
//    //    incPos.y += .01f;
//    //    transform.position = incPos;
//    //    results[0] = null;
//    //    GetComponent<Rigidbody2D>().OverlapCollider(platformFilter, results);
//    //}
//}