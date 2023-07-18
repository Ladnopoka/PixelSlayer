using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Sprite[] idleUpAnimationFrameArray;
    [SerializeField] private Sprite[] idleLeftAnimationFrameArray;
    [SerializeField] private Sprite[] idleDownAnimationFrameArray;
    [SerializeField] private Sprite[] idleRightAnimationFrameArray;
    [SerializeField] private Sprite[] idleUpLeftAnimationFrameArray;
    [SerializeField] private Sprite[] idleLeftDownAnimationFrameArray;
    [SerializeField] private Sprite[] idleDownRightAnimationFrameArray;
    [SerializeField] private Sprite[] idleUpRightAnimationFrameArray;
    [SerializeField] private Sprite[] runUpAnimationFrameArray;
    [SerializeField] private Sprite[] runLeftAnimationFrameArray;
    [SerializeField] private Sprite[] runDownAnimationFrameArray;
    [SerializeField] private Sprite[] runRightAnimationFrameArray;
    [SerializeField] private Sprite[] runUpLeftAnimationFrameArray;
    [SerializeField] private Sprite[] runLeftDownAnimationFrameArray;
    [SerializeField] private Sprite[] runDownRightAnimationFrameArray;
    [SerializeField] private Sprite[] runUpRightAnimationFrameArray;

    public enum AnimationType
    {
        IdleUp,
        IdleLeft,
        IdleDown,
        IdleRight,
        IdleUpLeft,
        IdleLeftDown,
        IdleDownRight,
        IdleUpRight,
        RunUp,
        RunLeft,
        RunDown,
        RunRight,
        RunUpLeft,
        RunLeftDown,
        RunDownRight,
        RunUpRight
    }

    //Generic version of EventHandler, pass in our specific EventArgs as in generic parameter
    public event EventHandler<OnKeyPressEventArgs> OnKeyPress;
    public class OnKeyPressEventArgs : EventArgs{
        public string keyValue;
    }

    public event EventHandler<OnAnimationChangeEventArgs> OnAnimationChange;
    public class OnAnimationChangeEventArgs : EventArgs {
        public AnimationType AnimationType;
    }

    public event EventHandler<OnCollisionEventArgs> OnCollision;
    public class OnCollisionEventArgs : EventArgs {
        public RaycastHit2D collisionVar;
    }

    [SerializeField] private Sprite[] spriteArray; 
    private int currentFrame;
    private float timer;
    private float frameRate = .11f;
    private SpriteRenderer spriteRenderer;
    private bool loop = true;
    private bool isAnimating = true;
    private int loopCounter = 0;
    private float speed = 10f;
    private int keyCount;
    private string keyValue;
    private string lastKeyValue;
    private AnimationType activeAnimationType;
    private (float, float) lastMoveKey;

    private void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I have reached the Debug.Log: ");
        PlayAnimation(AnimationType.IdleDown);
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerInput();
        HandleAnimation();
    }

    private void StopAnimating(){
        isAnimating = false;
    }

    public void PlayAnimation(Sprite[] spriteArray, float frameRate)
    {
        this.spriteArray = spriteArray;
        this.frameRate = frameRate;
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = spriteArray[currentFrame];
    }

    // Dictionary for mapping key combinations to animation types and keyValues
    private Dictionary<(float moveX, float moveY), (AnimationType animationType, AnimationType idleAnimationType, string keyValue)> keyToAnimationMap = new Dictionary<(float, float), (AnimationType, AnimationType, string)>
    {
        {(0, 1), (AnimationType.RunUp, AnimationType.IdleUp, "W")},
        {(-1, 0), (AnimationType.RunLeft, AnimationType.IdleLeft, "A")},
        {(0, -1), (AnimationType.RunDown, AnimationType.IdleDown, "S")},
        {(1, 0), (AnimationType.RunRight, AnimationType.IdleRight, "D")},
        {(-1, 1), (AnimationType.RunUpLeft, AnimationType.IdleUpLeft, "WA")},
        {(-1, -1), (AnimationType.RunLeftDown, AnimationType.IdleLeftDown, "AS")},
        {(1, -1), (AnimationType.RunDownRight, AnimationType.IdleDownRight, "SD")},
        {(1, 1), (AnimationType.RunUpRight, AnimationType.IdleUpRight, "WD")}
    };

    private void HandleMovement(float moveX, float moveY, bool isMoving)
    {
        var key = isMoving ? (moveX, moveY) : lastMoveKey; //This line is creating a tuple that stores two integer values, moveX and moveY. moveX and moveY are integers that are set based on which movement keys are being pressed.
        
        if (isMoving){ //The code inside this block only runs if isMoving is true. This variable is set to true if any movement keys are being pressed (W, A, S, D). If isMoving is false, which means no movement keys are being pressed, the code inside this block will be skipped.
            if (keyToAnimationMap.TryGetValue(key, out var value)){ //Here we are trying to get a value from the keyToAnimationMap dictionary using key as the lookup key. TryGetValue is a method provided by the Dictionary class that attempts to get the value associated with the specified key. If the key is found in the dictionary, TryGetValue returns true and the value associated with the key is output in the variable value. If the key is not found in the dictionary, TryGetValue returns false and value is assigned the default value of its type.
                Vector3 moveDir = new Vector3(moveX, moveY).normalized;
                
                if (lastKeyValue != keyValue){ //This line checks if the previous frame's keyValue is different from the current frame's keyValue. If they are different, it means the player has changed their movement direction.
                    lastKeyValue = keyValue; //This line is updating lastKeyValue to the current keyValue. This is done to keep track of the previous frame's movement direction, which allows us to detect when the player changes their movement direction.
                    OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //This line invokes the OnKeyPress event, passing a new instance of OnKeyPressEventArgs with the current keyValue to any subscribed event handlers. The ? before Invoke is a null-conditional operator, which means Invoke will only be called if OnKeyPress is not null (i.e., if there are any subscribers to the OnKeyPress event).
                }

                Vector3 targetMovePosition = transform.position + moveDir * speed * Time.deltaTime;
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDir, speed * Time.deltaTime);
                if (raycastHit.collider == null){
                    //No Collision with objects
                    transform.position = targetMovePosition;
                    PlayAnimation(value.animationType); //This line is calling the PlayAnimation method with the animationType obtained from the value tuple. This will start playing the animation corresponding to the current movement direction.
                    keyValue = value.keyValue; //This line is setting the keyValue variable to the keyValue obtained from the value tuple. keyValue represents the keys that are currently being pressed (for example, "W" for up, "A" for left, "S" for down, "D" for right, etc).
                }
                else{
                    //Collision with objects, cannot move DIAGONALLY
                    //Test just moving horizontal direction
                    Vector3 altMoveDir = new Vector3(moveDir.x, 0f).normalized;
                    //var tupleDir = (altMoveDir.x, altMoveDir.y);
                    targetMovePosition = transform.position + altMoveDir * speed * Time.deltaTime;
                    raycastHit = Physics2D.Raycast(transform.position, altMoveDir, speed * Time.deltaTime);

                    if (altMoveDir.x != 0f && raycastHit.collider == null){
                        //Can move horizontally

                        transform.position = targetMovePosition;
                        PlayAnimation(value.animationType); 
                        keyValue = value.keyValue;
                    }
                    else{
                        //Cannot move horizontally
                        //Test just moving horizontal direction
                        altMoveDir = new Vector3(0f, moveDir.y).normalized;
                        //tupleDir = (altMoveDir.x, altMoveDir.y);
                        targetMovePosition = transform.position + altMoveDir * speed * Time.deltaTime;
                        raycastHit = Physics2D.Raycast(transform.position, altMoveDir, speed * Time.deltaTime);
                        if (altMoveDir.y != 0f && raycastHit.collider == null){ //CHECK THIS TOMORROW//CHECK THIS TOMORROW
                            //Can move Vertically

                            transform.position = targetMovePosition;
                            PlayAnimation(value.animationType); //CHECK THIS TOMORROW//CHECK THIS TOMORROW//CHECK THIS TOMORROW
                            keyValue = value.keyValue;
                        }
                        else{
                            //Cannot move vertically
                        }
                    }
 
                    OnCollision?.Invoke(this, new OnCollisionEventArgs { collisionVar = raycastHit });
                }
            }
        }
        else{
            if (keyToAnimationMap.TryGetValue(key, out var value)){
                PlayAnimation(value.idleAnimationType);
                keyValue = value.keyValue; //This line is setting the keyValue variable to the keyValue obtained from the value tuple. keyValue represents the keys that are currently being pressed (for example, "W" for up, "A" for left, "S" for down, "D" for right, etc).
                if (lastKeyValue != keyValue){ //This line checks if the previous frame's keyValue is different from the current frame's keyValue. If they are different, it means the player has changed their movement direction.
                    lastKeyValue = keyValue; //This line is updating lastKeyValue to the current keyValue. This is done to keep track of the previous frame's movement direction, which allows us to detect when the player changes their movement direction.
                }
            }
        }
    }

    // New method for handling player input
    private void HandlePlayerInput()
    {
        float moveX = 0, moveY = 0;
        bool isMoving = false;

        if (Input.GetKey(KeyCode.W)){
            moveY = 1;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A)){
            moveX = -1;
            isMoving = true;      
        }
        if (Input.GetKey(KeyCode.S)){
            moveY = -1;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D)){
            moveX = 1;
            isMoving = true;      
        } 

        if (isMoving) lastMoveKey = (moveX, moveY);
        
        HandleMovement(moveX, moveY, isMoving);
    }

    private void HandleAnimation()
    {
        if (!isAnimating || spriteArray == null || spriteArray.Length == 0)
            return;
        
        timer += Time.deltaTime;

        //Character Sprite animations
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % spriteArray.Length;
            if (!loop && currentFrame == 0)
            {
                StopAnimating();
            }
            else
            {
                spriteRenderer.sprite = spriteArray[currentFrame];
            }

            //THIS CODE IS FOR EVENT HANDLER TO SHOW THAT A LOOP HAPPENED
            // if (currentFrame == 0)
            // {
            //     loopCounter++;
            //     if (loopCounter == 1)
            //         if (OnAnimationLoopedFirst != null) OnAnimationLoopedFirst(this, EventArgs.Empty);
                    
            //     if (OnAnimationLooped != null) OnAnimationLooped(this, EventArgs.Empty);
            // }
        }
    }

    private void PlayAnimation(AnimationType animationType)
    {
        if (animationType != activeAnimationType){
            activeAnimationType = animationType;

            OnAnimationChange?.Invoke(this, new OnAnimationChangeEventArgs { AnimationType = animationType });

            switch (animationType){
                case AnimationType.IdleUp:
                    PlayAnimation(idleUpAnimationFrameArray, .11f);
                    break;
                case AnimationType.IdleLeft:
                    PlayAnimation(idleLeftAnimationFrameArray, .11f);
                    break;
                case AnimationType.IdleDown:
                    PlayAnimation(idleDownAnimationFrameArray, .11f);
                    break;
                case AnimationType.IdleRight:
                    PlayAnimation(idleRightAnimationFrameArray, .11f);
                    break;
                case AnimationType.IdleUpLeft:
                    PlayAnimation(idleUpLeftAnimationFrameArray, .11f);
                    break;
                case AnimationType.IdleLeftDown:
                    PlayAnimation(idleLeftDownAnimationFrameArray, .11f);
                    break;
                case AnimationType.IdleDownRight:
                    PlayAnimation(idleDownRightAnimationFrameArray, .11f);
                    break;
                case AnimationType.IdleUpRight:
                    PlayAnimation(idleUpRightAnimationFrameArray, .11f);
                    break;
                case AnimationType.RunUp:
                    PlayAnimation(runUpAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunLeft:
                    PlayAnimation(runLeftAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunDown:
                    PlayAnimation(runDownAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunRight:
                    PlayAnimation(runRightAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunUpLeft:
                    PlayAnimation(runUpLeftAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunLeftDown:
                    PlayAnimation(runLeftDownAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunDownRight:
                    PlayAnimation(runDownRightAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunUpRight:
                    PlayAnimation(runUpRightAnimationFrameArray, .1f);
                    break;
            }
        }
    }
}
