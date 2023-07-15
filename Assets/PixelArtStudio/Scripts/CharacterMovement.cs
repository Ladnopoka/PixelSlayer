using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Sprite[] idleAnimationFrameArray;
    [SerializeField] private Sprite[] runUpAnimationFrameArray;
    [SerializeField] private Sprite[] runLeftAnimationFrameArray;
    [SerializeField] private Sprite[] runDownAnimationFrameArray;
    [SerializeField] private Sprite[] runRightAnimationFrameArray;
    [SerializeField] private Sprite[] runUpLeftAnimationFrameArray;
    [SerializeField] private Sprite[] runLeftDownAnimationFrameArray;
    [SerializeField] private Sprite[] runDownRightAnimationFrameArray;
    [SerializeField] private Sprite[] runUpRightAnimationFrameArray;

    private enum AnimationType
    {
        Idle,
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
        public int keyCount;    //field
        public string keyValue;
        AnimationType AnimationType;
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

    private void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I have reached the Debug.Log: ");
        PlayAnimation(AnimationType.Idle);
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
    private Dictionary<(float moveX, float moveY), (AnimationType animationType, string keyValue)> keyToAnimationMap = new Dictionary<(float, float), (AnimationType, string)>
    {
        {(0, 1), (AnimationType.RunUp, "W")},
        {(-1, 0), (AnimationType.RunLeft, "A")},
        {(0, -1), (AnimationType.RunDown, "S")},
        {(1, 0), (AnimationType.RunRight, "D")},
        {(-1, 1), (AnimationType.RunUpLeft, "WA")},
        {(-1, -1), (AnimationType.RunLeftDown, "AS")},
        {(1, -1), (AnimationType.RunDownRight, "SD")},
        {(1, 1), (AnimationType.RunUpRight, "WD")}
    };

    private void HandleMovement(float moveX, float moveY, bool isMoving)
    {
        if (isMoving){ //The code inside this block only runs if isMoving is true. This variable is set to true if any movement keys are being pressed (W, A, S, D). If isMoving is false, which means no movement keys are being pressed, the code inside this block will be skipped.
            var key = (moveX, moveY); //This line is creating a tuple that stores two integer values, moveX and moveY. moveX and moveY are integers that are set based on which movement keys are being pressed.
            if (keyToAnimationMap.TryGetValue(key, out var value)){ //Here we are trying to get a value from the keyToAnimationMap dictionary using key as the lookup key. TryGetValue is a method provided by the Dictionary class that attempts to get the value associated with the specified key. If the key is found in the dictionary, TryGetValue returns true and the value associated with the key is output in the variable value. If the key is not found in the dictionary, TryGetValue returns false and value is assigned the default value of its type.
                PlayAnimation(value.animationType); //This line is calling the PlayAnimation method with the animationType obtained from the value tuple. This will start playing the animation corresponding to the current movement direction.
                keyValue = value.keyValue; //This line is setting the keyValue variable to the keyValue obtained from the value tuple. keyValue represents the keys that are currently being pressed (for example, "W" for up, "A" for left, "S" for down, "D" for right, etc).
                if (lastKeyValue != keyValue){ //This line checks if the previous frame's keyValue is different from the current frame's keyValue. If they are different, it means the player has changed their movement direction.
                    lastKeyValue = keyValue; //This line is updating lastKeyValue to the current keyValue. This is done to keep track of the previous frame's movement direction, which allows us to detect when the player changes their movement direction.
                    OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //This line invokes the OnKeyPress event, passing a new instance of OnKeyPressEventArgs with the current keyValue to any subscribed event handlers. The ? before Invoke is a null-conditional operator, which means Invoke will only be called if OnKeyPress is not null (i.e., if there are any subscribers to the OnKeyPress event).
                }
            }
        }
        else{
            PlayAnimation(AnimationType.Idle);
            keyValue = "IDLE";
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        transform.position += moveDir * speed * Time.deltaTime;
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

            switch (animationType){
                case AnimationType.Idle:
                    PlayAnimation(idleAnimationFrameArray, .11f);
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
