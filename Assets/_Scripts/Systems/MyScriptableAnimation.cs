using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MyScriptableAnimation : MonoBehaviour
{
    public MyScriptableObject animationSprites;
    private Sprite[] spriteArray;
    private float frameRate = .11f;
    private int currentFrame;
    private float timer;
    private bool isAnimating = true;
    private bool loop = true;
    private SpriteRenderer spriteRenderer;
    private Sprite[] currentAnimation;

    private Attack attack;
    //Sprite[] currentAnimation = animationData.GetAnimation(CharacterAnimationData.Direction.Up, CharacterAnimationData.ActionType.Attack);

    
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //PlayAnimation(animationSprites.GetAnimation(animationSprites., .11f);
        Sprite[] currentAnimation = animationSprites.GetAnimation(MyScriptableObject.Direction.Up, MyScriptableObject.ActionType.Attack);
    }

    private void Update()
    {
        HandlePlayerInput();
        HandleAnimation();
    }

    private void HandlePlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentAnimation = animationSprites.GetAnimation(MyScriptableObject.Direction.Down, MyScriptableObject.ActionType.Attack);
            PlayAnimation(currentAnimation, .1f);
        }
    }

    public void PlayAnimation(Sprite[] spriteArray, float frameRate)
    {
        this.spriteArray = spriteArray;
        this.frameRate = frameRate;
        currentFrame = 0;
        timer = 0;
        Debug.Log("Currently playing animation: " + spriteRenderer.sprite);
        spriteRenderer.sprite = spriteArray[currentFrame];
    }
    private void HandleAnimation()
    {
        if (!isAnimating || spriteArray == null || spriteArray.Length == 0)
            return;
        
        Debug.Log("Current Frame: " + currentFrame);
    
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
    private void StopAnimating(){
        isAnimating = false;
    }
}