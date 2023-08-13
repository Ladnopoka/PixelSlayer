using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    public SpriteData spriteData;

    private enum State
    {
        Idle,
        Attacking
    }

    //Generic version of EventHandler, pass in our specific EventArgs as in generic parameter
    public event EventHandler<OnKeyPressEventArgs> OnKeyPress;

    public class OnKeyPressEventArgs : EventArgs
    {
        public string keyValue;
    }

    // public event EventHandler<OnMousePressEventArgs> OnMousePress;
    //
    // public class OnMousePressEventArgs : EventArgs
    // {
    //     public bool mousePress;
    // }

    public event EventHandler<OnCollisionEventArgs> OnCollision;

    public class OnCollisionEventArgs : EventArgs
    {
        public RaycastHit2D collisionVar;
    }
    
    public event EventHandler OnCharacterAttack;

    private Sprite[] spriteArray;
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
    private (float, float) lastMoveKey;
    private Sprite[] currentAnimation;
    private State state;
    private Action animationEndCallback;

    private SpriteData.Direction currentDirection;
    private SpriteData.ActionType currentActionType;
    private SpriteData.DirectionAnimation currentDirectionalAnimation;

    private Attack attack;
    private SpriteData.Direction direction;

    private UnityEngine.CharacterController _characterController;
    //private UsefulFunctions _usefulFunctions;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        state = State.Idle;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentAnimation = spriteData.GetAnimation(SpriteData.Direction.Down, SpriteData.ActionType.Idle);
        PlayAnimation(currentAnimation, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerInput();
        HandleAnimation();
    }

    private void StopAnimating()
    {
        isAnimating = false;
    }

    private Dictionary<(float moveX, float moveY), (SpriteData.Direction direction, SpriteData.ActionType actionType, string keyValue)> keyToAnimationMap =
        new Dictionary<(float, float), (SpriteData.Direction, SpriteData.ActionType, string)>
        {
            { (0, 1), (SpriteData.Direction.Up, SpriteData.ActionType.Run, "W") },
            { (-1, 0), (SpriteData.Direction.Left, SpriteData.ActionType.Run, "A") },
            { (0, -1), (SpriteData.Direction.Down, SpriteData.ActionType.Run, "S") },
            { (1, 0), (SpriteData.Direction.Right, SpriteData.ActionType.Run, "D") },
            { (-1, 1), (SpriteData.Direction.UpLeft, SpriteData.ActionType.Run, "WA") },
            { (-1, -1), (SpriteData.Direction.DownLeft, SpriteData.ActionType.Run, "AS") },
            { (1, -1), (SpriteData.Direction.DownRight, SpriteData.ActionType.Run, "SD") },
            { (1, 1), (SpriteData.Direction.UpRight, SpriteData.ActionType.Run, "WD") }
        };

    private void HandleMovement(float moveX, float moveY, bool isMoving)
    {
        var key = isMoving? (moveX, moveY) : lastMoveKey; //This line is creating a tuple that stores two integer values, moveX and moveY. moveX and moveY are integers that are set based on which movement keys are being pressed.

        if (isMoving)
        {
            //The code inside this block only runs if isMoving is true. This variable is set to true if any movement keys are being pressed (W, A, S, D). If isMoving is false, which means no movement keys are being pressed, the code inside this block will be skipped.
            if (keyToAnimationMap.TryGetValue(key, out var value))
            {
                //Here we are trying to get a value from the keyToAnimationMap dictionary using key as the lookup key. TryGetValue is a method provided by the Dictionary class that attempts to get the value associated with the specified key. If the key is found in the dictionary, TryGetValue returns true and the value associated with the key is output in the variable value. If the key is not found in the dictionary, TryGetValue returns false and value is assigned the default value of its type.
                Vector3 moveDir = new Vector3(moveX, moveY).normalized;

                if (lastKeyValue != keyValue)
                {
                    //This line checks if the previous frame's keyValue is different from the current frame's keyValue. If they are different, it means the player has changed their movement direction.
                    lastKeyValue =
                        keyValue; //This line is updating lastKeyValue to the current keyValue. This is done to keep track of the previous frame's movement direction, which allows us to detect when the player changes their movement direction.
                    OnKeyPress?.Invoke(this, new OnKeyPressEventArgs{ keyValue = keyValue }); //This line invokes the OnKeyPress event, passing a new instance of OnKeyPressEventArgs with the current keyValue to any subscribed event handlers. The ? before Invoke is a null-conditional operator, which means Invoke will only be called if OnKeyPress is not null (i.e., if there are any subscribers to the OnKeyPress event).
                }

                Vector3 targetMovePosition = transform.position + moveDir * (speed * Time.deltaTime);
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDir, speed * Time.deltaTime);
                if (raycastHit.collider == null)
                {
                    //No Collision with objects
                    transform.position = targetMovePosition;
                    PlayAnimation(value.direction, value.actionType); //This line is calling the PlayAnimation method with the animationType obtained from the value tuple. This will start playing the animation corresponding to the current movement direction.
                    keyValue = value.keyValue; //This line is setting the keyValue variable to the keyValue obtained from the value tuple. keyValue represents the keys that are currently being pressed (for example, "W" for up, "A" for left, "S" for down, "D" for right, etc).
                }
                else
                {
                    //Collision with objects, cannot move DIAGONALLY
                    //Test just moving horizontal direction
                    Vector3 altMoveDir = new Vector3(moveDir.x, 0f).normalized;
                    //var tupleDir = (altMoveDir.x, altMoveDir.y);
                    targetMovePosition = transform.position + altMoveDir * (speed * Time.deltaTime);
                    raycastHit = Physics2D.Raycast(transform.position, altMoveDir, speed * Time.deltaTime);

                    if (altMoveDir.x != 0f && raycastHit.collider == null)
                    {
                        //Can move horizontally

                        transform.position = targetMovePosition;
                        PlayAnimation(value.direction, value.actionType);
                        keyValue = value.keyValue;
                    }
                    else
                    {
                        //Cannot move horizontally
                        //Test just moving horizontal direction
                        altMoveDir = new Vector3(0f, moveDir.y).normalized;
                        //tupleDir = (altMoveDir.x, altMoveDir.y);
                        targetMovePosition = transform.position + altMoveDir * (speed * Time.deltaTime);
                        raycastHit = Physics2D.Raycast(transform.position, altMoveDir, speed * Time.deltaTime);
                        if (altMoveDir.y != 0f && raycastHit.collider == null)
                        {
                            //Can move Horizontally

                            transform.position = targetMovePosition;
                            PlayAnimation(value.direction, value.actionType);
                            keyValue = value.keyValue;
                        }
                        else
                        {
                            // Cannot move vertically
                            // Try moving diagonally
                            altMoveDir = new Vector3(moveDir.x, moveDir.y).normalized;
                            raycastHit = Physics2D.Raycast(transform.position, altMoveDir, speed * Time.deltaTime);
                            if (altMoveDir.x != 0f && altMoveDir.y != 0f && raycastHit.collider == null)
                            {
                                // Can move diagonally
                                transform.position = targetMovePosition;
                                PlayAnimation(value.direction, value.actionType);
                                keyValue = value.keyValue;
                                OnCollision?.Invoke(this, new OnCollisionEventArgs { collisionVar = raycastHit });
                            }
                            else
                            {
                                // Cannot move diagonally either
                                raycastHit = Physics2D.Raycast(transform.position, moveDir, speed * Time.deltaTime);
                                OnCollision?.Invoke(this, new OnCollisionEventArgs { collisionVar = raycastHit });
                            }
                            OnCollision?.Invoke(this, new OnCollisionEventArgs { collisionVar = raycastHit });
                        }
                    }
                }
            }
        }
        else
        {
            PlayAnimation(currentDirection, SpriteData.ActionType.Idle);
            // keyValue = value.keyValue; //This line is setting the keyValue variable to the keyValue obtained from the value tuple. keyValue represents the keys that are currently being pressed (for example, "W" for up, "A" for left, "S" for down, "D" for right, etc).
            // if (lastKeyValue != keyValue)
            // {
            //     lastKeyValue =
            //         keyValue; //This line is updating lastKeyValue to the current keyValue. This is done to keep track of the previous frame's movement direction, which allows us to detect when the player changes their movement direction.
            // }
        }
    }

    // New method for handling player input
    private void HandlePlayerInput()
    {
        Debug.Log("State at start of HandlePlayerInput: " + state);

        switch (state)
        {
            case State.Idle:
                bool isMoving = false;
                float moveX = 0, moveY = 0;

                if (Input.GetKey(KeyCode.W))
                {
                    moveY = 1;
                    isMoving = true;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    moveX = -1;
                    isMoving = true;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    moveY = -1;
                    isMoving = true;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    moveX = 1;
                    isMoving = true;
                }

                if (Input.GetKey(KeyCode.Space))
                {

                }

                if (Input.GetMouseButtonDown(0))
                {
                    OnCharacterAttack?.Invoke(this, EventArgs.Empty);
                    Vector3 mousePosition = UsefulFunctions.GetMouseWorldPosition();
                    Vector3 attackDirection = (mousePosition - transform.position).normalized;
                    direction = UsefulFunctions.getDirectionFromCoordinates(attackDirection.x, attackDirection.y);
                    Debug.Log("Direction X: " + attackDirection.x);
                    Debug.Log("Direction Y: " + attackDirection.y);
                    Debug.Log("Direction: " + direction);
                    state = State.Attacking;
                    PlayAnimation(direction, SpriteData.ActionType.Attack, () => state = State.Idle);
                }
                
                if (isMoving) lastMoveKey = (moveX, moveY);
                HandleMovement(moveX, moveY, isMoving);
                break;
            case State.Attacking:
                break;
        }
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
                
                animationEndCallback?.Invoke();
                animationEndCallback = null; // Clear the callback to prevent re-invoking it
                state = State.Idle;
                PlayAnimation(currentDirection, SpriteData.ActionType.Idle);
            }
            else
            {
                spriteRenderer.sprite = spriteArray[currentFrame];
            }
        }
    }
    public void PlayAnimation(Sprite[] spriteArray, float frameRate, Action callback = null)
    {
        this.spriteArray = spriteArray;
        this.frameRate = frameRate;
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = spriteArray[currentFrame];
        
        // This is the change: Store the callback for later use
        this.animationEndCallback = callback;
        animationEndCallback?.Invoke();
        
        Debug.Log("animationEndCallback: " + animationEndCallback);

        animationEndCallback = null; 
    }

    public void PlayAnimation(SpriteData.Direction direction, SpriteData.ActionType actionType, Action callback = null)
    {
        if (direction != currentDirection || actionType != currentActionType)
        {
            currentDirection = direction;
            currentActionType = actionType;
            Debug.Log("State: " + state);
            switch (actionType)
            {
                case SpriteData.ActionType.Idle:
                    actionType = SpriteData.ActionType.Idle;
                    break;
                case SpriteData.ActionType.Run:
                    actionType = SpriteData.ActionType.Run;
                    break;
                case SpriteData.ActionType.Attack:
                    actionType = SpriteData.ActionType.Attack;
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException("Unknown ActionType:  " + actionType);
            }

            switch (direction)
            {
                case SpriteData.Direction.Up:
                    PlayAnimation(spriteData.GetAnimation(direction, actionType), 0.1f, callback); ;
                    break;
                case SpriteData.Direction.Left:
                    PlayAnimation(spriteData.GetAnimation(direction, actionType), 0.1f, callback); ;
                    break;
                case SpriteData.Direction.Down:
                    PlayAnimation(spriteData.GetAnimation(direction, actionType), 0.1f, callback); ;
                    break;
                case SpriteData.Direction.Right:
                    PlayAnimation(spriteData.GetAnimation(direction, actionType), 0.1f, callback); ;
                    break;
                case SpriteData.Direction.UpLeft:
                    PlayAnimation(spriteData.GetAnimation(direction, actionType), 0.1f, callback); ;
                    break;
                case SpriteData.Direction.DownLeft:
                    PlayAnimation(spriteData.GetAnimation(direction, actionType), 0.1f, callback); ;
                    break;
                case SpriteData.Direction.DownRight:
                    PlayAnimation(spriteData.GetAnimation(direction, actionType), 0.1f, callback); ;
                    break;
                case SpriteData.Direction.UpRight:
                    PlayAnimation(spriteData.GetAnimation(direction, actionType), 0.1f, callback); ;
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
