using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private Sprite[] attackUpArray;
    [SerializeField] private Sprite[] attackLeftArray;
    [SerializeField] private Sprite[] attackDownArray;
    [SerializeField] private Sprite[] attackRightArray;
    [SerializeField] private Sprite[] attackUpLeftArray;
    [SerializeField] private Sprite[] attackUpRightArray;
    [SerializeField] private Sprite[] attackLeftDownArray;
    [SerializeField] private Sprite[] attackDownRightArray;

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
        RunUpRight,
        AttackUp,
        AttackLeft,
        AttackDown,
        AttackRight,
        AttackUpLeft,
        AttackLeftDown,
        AttackDownRight,
        AttackUpRight
    }

    //Generic version of EventHandler, pass in our specific EventArgs as in generic parameter
    public event EventHandler<OnKeyPressEventArgs> OnKeyPress;
    public class OnKeyPressEventArgs : EventArgs{
        public string keyValue;
    }
    
    public event EventHandler<OnMousePressEventArgs> OnMousePress;
    public class OnMousePressEventArgs : EventArgs{
        public bool mousePress;
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
    private float speed = 10f;
    private int keyCount;
    private string keyValue;
    private string lastKeyValue;
    private AnimationType activeAnimationType;
    private (float, float) lastMoveKey;

    private CharacterAttack _characterAttack;
    private CharacterController _characterController;
    //private UsefulFunctions _usefulFunctions;

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
    
    // Dictionary for mapping key combinations to animation types and keyValues
    private Dictionary<(float moveX, float moveY), (AnimationType animationType, string keyValue)> keyToAttackMap = new Dictionary<(float, float), (AnimationType, string)>
    {
        {(0, 1), (AnimationType.AttackUp, "W")},
        {(-1, 0), (AnimationType.AttackLeft, "A")},
        {(0, -1), (AnimationType.AttackDown, "S")},
        {(1, 0), (AnimationType.AttackRight, "D")},
        {(-1, 1), (AnimationType.AttackUpLeft, "WA")},
        {(-1, -1), (AnimationType.AttackLeftDown, "AS")},
        {(1, -1), (AnimationType.AttackDownRight, "SD")},
        {(1, 1), (AnimationType.AttackUpRight, "WD")}
    };

    private void HandleAttack(float moveX, float moveY)
    {
        var key = (moveX, moveY);

        if (keyToAttackMap.TryGetValue(key, out var value))
        {
            Debug.Log("What is my key on mouse click?: " + value);
            Debug.Log("Value.animationType = " + value.animationType);
        }

        PlayAnimation(value.animationType);
        Debug.Log("Clicking on " + value.animationType);
    }

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

                Vector3 targetMovePosition = transform.position + moveDir * (speed * Time.deltaTime);
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
                    targetMovePosition = transform.position + altMoveDir * (speed * Time.deltaTime);
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
                        targetMovePosition = transform.position + altMoveDir * (speed * Time.deltaTime);
                        raycastHit = Physics2D.Raycast(transform.position, altMoveDir, speed * Time.deltaTime);
                        if (altMoveDir.y != 0f && raycastHit.collider == null){
                            //Can move Horizontally

                            transform.position = targetMovePosition;
                            PlayAnimation(value.animationType);
                            keyValue = value.keyValue;
                        }
                        else{
                            // Cannot move vertically
                            // Try moving diagonally
                            altMoveDir = new Vector3(moveDir.x, moveDir.y).normalized;
                            raycastHit = Physics2D.Raycast(transform.position, altMoveDir, speed * Time.deltaTime);
                            if (altMoveDir.x != 0f && altMoveDir.y != 0f && raycastHit.collider == null) {
                                // Can move diagonally
                                transform.position = targetMovePosition;
                                PlayAnimation(value.animationType);
                                keyValue = value.keyValue;
                                //OnCollision?.Invoke(this, new OnCollisionEventArgs { collisionVar = raycastHit });
                            } else {
                                // Cannot move diagonally either
                                raycastHit = Physics2D.Raycast(transform.position, moveDir, speed * Time.deltaTime);
                                OnCollision?.Invoke(this, new OnCollisionEventArgs { collisionVar = raycastHit });
                            }
                            //OnCollision?.Invoke(this, new OnCollisionEventArgs { collisionVar = raycastHit });
                        }
                    }
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
            if (Input.GetMouseButtonDown(0))
            {
                //HandleAttack(moveX, moveY);
                Vector3 mousePosition = UsefulFunctions.GetMouseWorldPosition();
                Vector3 attackDirection = (mousePosition - transform.position).normalized;
                Debug.Log("GetMouseWorldPosition(): " + UsefulFunctions.GetMouseWorldPosition());
                Debug.Log("AttackDirection: " + attackDirection);
                PlayAnimation(attackDownArray, .1f);
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
                case AnimationType.AttackUp:
                    PlayAnimation(attackUpArray, .1f);
                    break;
                case AnimationType.AttackLeft:
                    PlayAnimation(attackLeftArray, .1f);
                    break;
                case AnimationType.AttackDown:
                    PlayAnimation(attackDownArray, .1f);
                    break;
                case AnimationType.AttackRight:
                    PlayAnimation(attackRightArray, .1f);
                    break;
                case AnimationType.AttackUpLeft:
                    PlayAnimation(attackUpLeftArray, .1f);
                    break;
                case AnimationType.AttackLeftDown:
                    PlayAnimation(attackLeftDownArray, .1f);
                    break;
                case AnimationType.AttackDownRight:
                    PlayAnimation(attackDownRightArray, .1f);
                    break;
                case AnimationType.AttackUpRight:
                    PlayAnimation(attackUpRightArray, .1f);
                    break;
            }
        }
    }
}

/*
Here's an example of how the movement and speed and collision of character is calculated:

Let's say your game object is at position (1, 2, 3), you want it to move to the right at a speed of 4 units per second, and the last frame took 0.5 seconds to complete.
Then, transform.position will be (1, 2, 3), moveDir will be (1, 0, 0) (to the right), speed will be 4, and Time.deltaTime will be 0.5.
First, moveDir * speed * Time.deltaTime gives (1, 0, 0) * 4 * 0.5 = (2, 0, 0). This means you want to move the game object 2 units to the right in the current frame.
Then, transform.position + moveDir * speed * Time.deltaTime gives (1, 2, 3) + (2, 0, 0) = (3, 2, 3). This is the target position for the game object in the current frame.
Next, Physics2D.Raycast(transform.position, moveDir, speed * Time.deltaTime) will shoot a ray from (1, 2, 3) to the right, and the maximum distance of the ray will be 2.
If there's an object at position (2, 2, 3), the ray will hit this object and raycastHit.collider will not be null. The raycastHit object will contain information about this collision. If there's no object within 2 units to the right, raycastHit.collider will be null, and the game object will move to the target position (3, 2, 3).
*/
