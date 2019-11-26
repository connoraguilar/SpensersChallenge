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
    public float mTeleportCooldown;
    public float mTeleportDistance;
    public float airMovement = 1;
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
    private bool poofPause = false;
    private bool startPoofPause = false;
    private float currTime = 0f;
    private float heldTime = 0f;
    private float distToGround = 0;
    private int jumpCount = 0;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode poofKey;
    private Vector2 normalCollisionSize;
    private Vector2 poofCollisionSize;
    private Vector3 previousPos;
    private Vector3 poofPosition;
    private float mPoofTime = 0;
    private float lastInput = 0;
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
        else if(controlScheme == 1)
        {
            //leftKey = KeyCode.
          
        }
        normalCollisionSize = GetComponent<BoxCollider2D>().size;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameInformation.isPaused)
        {
            Vector3 currPos = transform.position;
            currTime = Time.deltaTime;


            //This handles timing for animations and such when chandli is poofing
            if (startPoofTimer)
            {
                startPoofPause = true;
                mFreeze = false;
                poofPause = true;
                //this is just reusing a timer for efficency
                mPoofTime += Time.deltaTime;

                if (mPoofTime >= 0.3)
                {
                    transform.position = poofPosition;
                    anim.SetBool("StartPoof", false);
                    GetComponent<SpriteRenderer>().enabled = true;

                    //GetComponent<Chandli>().mCamera.GetComponent<CameraFollow>().mFollowCamera = true;
                }

                if (mPoofTime >= 0.35)
                {
                    anim.SetTrigger("EndPoof");
                    isPoofing = false;
                    startPoofTimer = false;
                    
                    wasClicking = false;
                   
                }

               
            }

            if (startPoofPause)
            {
                if(!startPoofTimer)
                {
                    mPoofTime += Time.deltaTime;
                }

                if (mPoofTime > .5)
                {
                    mPoofTime = 0;
                    poofPause = false;
                    startPoofPause = false;
                }
            }
            if (isPoofing)
            {

                poofCollisionSize = new Vector2(0.44f, 0.44f);
                GetComponent<BoxCollider2D>().size = poofCollisionSize;

            }
            else
            {
                GetComponent<BoxCollider2D>().size = normalCollisionSize;
            }

            //This is a delay to allow the starting animation for the poof to play
            if (startPoofAnimation)
            {
                mPoofTime += Time.deltaTime;
                if (mPoofTime > 0.3)
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

            if (!mFreeze && !mSlipping && !startPoofTimer && !poofPause)
            {
                //Check to see if they are still inputting direction, and have been

                if (Input.GetKeyDown(poofKey) && !wasClicking && !startPoofTimer)
                {
                    wasClicking = true;
                    mFreeze = true;
                }
                else
                {
                    float inputValueHor = Input.GetAxis("Horizontal");
                    bool dontRun = false;
                    if(Mathf.Sign(lastInput) == Mathf.Sign(inputValueHor) )
                    {
                        
                        if (Mathf.Abs(lastInput) > Mathf.Abs(inputValueHor))
                        {

                            dontRun = true;
                        }
                    }
                    lastInput = inputValueHor;
                    if (inputValueHor > 0 && !dontRun)
                    {
                        if (!mFacingRight)
                        {
                            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                        }
                        mFacingRight = true;
                        float finalSpeed = mSpeed;
                        if (mInAir)
                        {
                            finalSpeed = finalSpeed * airMovement;
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
                        else if (mInAir)
                        {
                            moving = true;
                            if (!playingJump)
                            {
                                //anim.SetTrigger("Jump");
                                playingJump = true;
                            }
                        }
                    }
                    else if (inputValueHor < 0 && !dontRun)
                    {
                        if (mFacingRight)
                        {
                            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                        }
                        mFacingRight = false;
                        float finalSpeed = mSpeed;
                        if (mInAir)
                        {
                            finalSpeed = mSpeed * 0.75f;
                        }

                        Vector2 newVel = GetComponent<Rigidbody2D>().velocity;
                        newVel.x = -finalSpeed;
                        GetComponent<Rigidbody2D>().velocity = newVel;


                        //Animation handling
                        if (!moving && mInAir == false)
                        {
                            moving = true;
                            anim.SetTrigger("Run");
                        }
                        else if (mInAir)
                        {
                            moving = true;
                            if (!playingJump)
                            {
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

                            if (!mInAir)
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
            else if (mSlipping)
            {
                Debug.Log("slipping");
                if (!mFacingRight)
                    GetComponent<SpriteRenderer>().flipX = false;
                Vector3 newVel = Vector3.zero;
                newVel.y = GetComponent<Rigidbody2D>().velocity.y;
                GetComponent<Rigidbody2D>().velocity = newVel;
                currPos.x += mSpeed * Time.deltaTime;
                transform.position = currPos;

                CheckForJump();
            }
            else if (startPoofTimer || poofPause)
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

            if (poofDir.x != 0 && poofDir.y != 0)
            {
                float poofModifier = Mathf.Sqrt(2) / 2;

                poofDir.x *= poofModifier;
                poofDir.y *= poofModifier;
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
                //float halfWidth = GetComponent<BoxCollider2D>().size.x / 2;
                //float halfHeight = GetComponent<BoxCollider2D>().size.y / 2;
                //float playerMinX = transform.position.x - halfWidth;
                //float playerMinY = transform.position.y - halfHeight;
                //float playerMaxX = transform.position.x + halfWidth;
                //float playerMaxY = transform.position.y + halfHeight;

                //float boxWidth = results[0].transform.localScale.x / 2;
                //float boxHeight = results[0].transform.localScale.y / 2;
                //float boxMinX = results[0].transform.position.x - boxWidth;
                //float boxMinY = results[0].transform.position.y - boxHeight;
                //float boxMaxX = results[0].transform.position.x + boxWidth;
                //float boxMaxY = results[0].transform.position.y + boxHeight;

                //Vector3 vel = GetComponent<Rigidbody2D>().velocity;

                //float maxVel = Mathf.Max(Mathf.Abs(vel.x), Mathf.Abs(vel.y));
                //Vector3 newPos = transform.position;


                //if (directionKey == KeyCode.D || directionKey == KeyCode.A)
                //{
                //    if (directionKey == KeyCode.D)
                //    {
                //        newPos.x = boxMinX - 0.5f;
                //    }
                //    else
                //    {
                //        newPos.x = boxMaxX + 0.5f;
                //    }
                //    //GetComponent<BoxCollider2D>().size.y;
                //}
                //else
                //{

                //    if (directionKey == KeyCode.W)
                //    {
                //        newPos.y = boxMinY - 0.5f;
                //    }
                //    else
                //    {
                //        newPos.y = boxMaxY + 0.5f;
                //    }
                //}
                Vector3 toOldLocation = positionPreTeleport - currPos;
                toOldLocation = 0.25f* toOldLocation.normalized;
                Vector3 newPos = currPos;
                bool overlapping = true;
                int loopCount = 0;
                Vector3 playerMax, playerMin, objectMax, objectMin;
                Vector3 objectPos = results[0].transform.position;
                while (overlapping)
                {

                    playerMax = GetComponent<Collider2D>().bounds.max;
                    objectMax = results[0].GetComponent<Collider2D>().bounds.max;
                    playerMin = GetComponent<Collider2D>().bounds.min;
                    objectMin = results[0].GetComponent<Collider2D>().bounds.min;

                    if ((playerMin.x <= objectMax.x && playerMax.x >= objectMin.x) && (playerMin.y <= objectMax.y && playerMax.y >= objectMin.y))
                    {
                        
                        newPos += toOldLocation;
                        transform.position = newPos;
                    }
                    else
                    {
                       
                        overlapping = false;
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
