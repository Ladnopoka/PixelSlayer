using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterAnimation : MonoBehaviour
{
    public CharacterAnimationData animationSprites;
    private AnimationType activeAnimationType;
    private Sprite[] spriteArray;
    private float frameRate = .11f;
    private int currentFrame;
    private float timer;
    private bool isAnimating;
    private bool loop = true;
    
    
    
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
    
    private Sprite[] GetIdleUpSprites()
    {
        return animationSprites.GetAnimation(CharacterAnimationData.Direction.Up, CharacterAnimationData.ActionType.Idle);
    }

    private Sprite[] GetRunUpSprites()
    {
        return animationSprites.GetAnimation(CharacterAnimationData.Direction.Up, CharacterAnimationData.ActionType.Run);
    }

    private Sprite[] GetAttackUpSprites()
    {
        return animationSprites.GetAnimation(CharacterAnimationData.Direction.Up, CharacterAnimationData.ActionType.Attack);
    }

    public event EventHandler<OnAnimationChangeEventArgs> OnAnimationChange;
    
    public class OnAnimationChangeEventArgs : EventArgs 
    {
        public AnimationType AnimationType;
    }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetAnimationData(CharacterAnimationData data)
    {
        animationSprites = data;
    }

    // private Sprite[] GetIdleUpSprites()
    // {
    //     return animationSprites.GetAnimation(CharacterAnimationData.Direction.Up, CharacterAnimationData.ActionType.Idle);
    // }

    // ... Other methods to get sprites for other animations
    
    
    
    
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
    private void PlayAnimation(Sprite[] animationSprites, float frameRate)
    {
        Debug.Log("AnimationSprites number: " + animationSprites.Length);
        // Actual logic to play the animation with the provided sprites and duration
        this.spriteArray = animationSprites;
        this.frameRate = frameRate;
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = spriteArray[currentFrame];
    }
    
    private void PlayAnimation(AnimationType animationType)
    {
        if (animationType != activeAnimationType)
        {
            activeAnimationType = animationType;
            OnAnimationChange?.Invoke(this, new OnAnimationChangeEventArgs { AnimationType = animationType });

            switch (animationType)
            {
                case AnimationType.IdleUp:
                    PlayAnimation(GetIdleUpSprites(), .11f);
                    break;
                case AnimationType.RunUp:
                    PlayAnimation(GetRunUpSprites(), .11f);
                    break;
                case AnimationType.AttackUp:
                    PlayAnimation(GetAttackUpSprites(), .11f);
                    break;
                //... Other cases for other animation types
            }
        }
    }  
    
    private void StopAnimating(){
        isAnimating = false;
    }
    
    public void TriggerAttack()
    {
        PlayAttackAnimation(); // This is a private method that handles the actual animation
    }

    private void PlayAttackAnimation()
    {
        Debug.Log("Playing Attack Animation Lulz");
        Debug.Log("AnimationType: " + AnimationType.AttackUp);
        PlayAnimation(AnimationType.AttackUp);
    }

    private void Update()
    {
        HandleAnimation();
    }
}




/*public class CharacterAnimation : MonoBehaviour
{
    // ... All the sprite arrays, AnimationType enum, and related events.
    
    private CharacterAnimationData animationData;
    private AnimationType activeAnimationType;
    
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

    public event EventHandler<OnAnimationChangeEventArgs> OnAnimationChange;
    public class OnAnimationChangeEventArgs : EventArgs {
        public AnimationType AnimationType;
    }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // public void PlayAnimation(AnimationType animationType)
    // {
    //     // ... Logic to play animations based on the AnimationType passed.
    // }
    
    
    
    
    
// Example Usage:


    public CharacterAnimation(CharacterAnimationData data)
    {
       animationData = data;
    }

    public void CreateIdleUpAnimation()
    {
        Sprite[] sprites = animationData.Animations[CharacterAnimationData.Direction.Up].Idle;
        // Create animation using sprites...
    }

    // ... Other animation creation methods
    
    Sprite[] idleUpSprites = animationData.GetAnimation(CharacterAnimationData.Direction.Up, CharacterAnimationData.ActionType.Idle);
        
        
        
        
    private void PlayAnimation(AnimationType animationType)
    {
        if (animationType != activeAnimationType){
            activeAnimationType = animationType;

            OnAnimationChange?.Invoke(this, new OnAnimationChangeEventArgs { AnimationType = animationType });

            switch (animationType){
                case AnimationType.IdleUp:
                    PlayAnimation(animationData.idleUpAnimationFrameArray, .11f);
                    break;
                /*case AnimationType.IdleLeft:
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
                    break;#1#
            }
        }
    }
}*/
