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
        HandleMovement();
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

    private void HandleMovement()
    {
        bool isMoving = false;
        float moveX = 0, moveY = 0;

        if (Input.GetKey(KeyCode.W)){
            moveY = 1;
            keyCount++;
            keyValue = "W";
            isMoving = true;
            OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //Invoke key press if not null
        }
        if (Input.GetKey(KeyCode.A)){
            moveX = -1;
            keyCount++;
            keyValue = "A";
            isMoving = true;
            OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //Invoke key press if not null
        }
        if (Input.GetKey(KeyCode.S)){
            moveY = -1;
            keyCount++;
            keyValue = "S";
            isMoving = true;
            OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //Invoke key press if not null
        }
        if (Input.GetKey(KeyCode.D)){
            moveX = 1;
            keyCount++;
            keyValue = "D";
            isMoving = true;
            OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //Invoke key press if not null
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)){
            keyValue = "WA";
            isMoving = true;
            OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //Invoke key press if not null
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S)){

            keyValue = "AS";
            isMoving = true;
            OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //Invoke key press if not null
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)){

            keyValue = "SD";
            isMoving = true;
            OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //Invoke key press if not null
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)){

            keyValue = "WD";
            isMoving = true;
            OnKeyPress?.Invoke(this, new OnKeyPressEventArgs { keyValue = keyValue }); //Invoke key press if not null
        }

        if (isMoving){
            if (keyValue == "W")
                PlayAnimation(AnimationType.RunUp); 
            if (keyValue == "A")
                PlayAnimation(AnimationType.RunLeft); 
            if (keyValue == "S")
                PlayAnimation(AnimationType.RunDown); 
            if (keyValue == "D")
                PlayAnimation(AnimationType.RunRight);
            if (keyValue == "WA")
                PlayAnimation(AnimationType.RunUpLeft); 
            if (keyValue == "AS")
                PlayAnimation(AnimationType.RunLeftDown); 
            if (keyValue == "SD")
                PlayAnimation(AnimationType.RunDownRight); 
            if (keyValue == "WD")
                PlayAnimation(AnimationType.RunUpRight);
        }
        else{
            PlayAnimation(AnimationType.Idle);
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        transform.position += moveDir * speed * Time.deltaTime;
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
