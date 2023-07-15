using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementTest : MonoBehaviour
{
    public event EventHandler OnAnimationLooped;
    public event EventHandler OnAnimationLoopedFirst;

    [SerializeField] private Sprite[] spriteArray; 
    private int currentFrame;
    private float timer;
    private float frameRate = .11f;
    private SpriteRenderer spriteRenderer;
    private bool loop = true;
    private bool isAnimating = true;
    private int loopCounter = 0;
    private float speed = 10f;

    private void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I have reached the Debug.Log: ");
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
        float moveX = 0, moveY = 0;

        if (Input.GetKey(KeyCode.W)){
            moveY = 1;
        }
        if (Input.GetKey(KeyCode.A)){
            moveX = -1;
        }
        if (Input.GetKey(KeyCode.S)){
            moveY = -1;
        }
        if (Input.GetKey(KeyCode.D)){
            moveX = 1;
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        transform.position += moveDir * speed * Time.deltaTime;
    }

    private void HandleAnimation()
    {
        if (!isAnimating)
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

            if (currentFrame == 0)
            {
                loopCounter++;
                if (loopCounter == 1)
                    if (OnAnimationLoopedFirst != null) OnAnimationLoopedFirst(this, EventArgs.Empty);
                    
                if (OnAnimationLooped != null) OnAnimationLooped(this, EventArgs.Empty);
            }
        }
    }
}
