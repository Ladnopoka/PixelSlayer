using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] CharacterMovement charMovement;
    
    private AnimationType activeAnimationType;
    
    [SerializeField] private Sprite[] idleUpAnimationFrameArray;
    [SerializeField] private Sprite[] idleLeftAnimationFrameArray;
    [SerializeField] private Sprite[] idleDownAnimationFrameArray;
    [SerializeField] private Sprite[] idleRightAnimationFrameArray;
    [SerializeField] private Sprite[] idleUpLeftAnimationFrameArray;
    [SerializeField] private Sprite[] idleLeftDownAnimationFrameArray;
    [SerializeField] private Sprite[] idleDownRightAnimationFrameArray;
    [SerializeField] private Sprite[] idleUpRightAnimationFrameArray;
    [SerializeField] private Sprite[] attackUpAnimationFrameArray;
    [SerializeField] private Sprite[] attackLeftAnimationFrameArray;
    [SerializeField] private Sprite[] attackDownAnimationFrameArray;
    [SerializeField] private Sprite[] attackRightAnimationFrameArray;
    [SerializeField] private Sprite[] attackUpLeftAnimationFrameArray;
    [SerializeField] private Sprite[] attackLeftDownAnimationFrameArray;
    [SerializeField] private Sprite[] attackDownRightAnimationFrameArray;
    [SerializeField] private Sprite[] attackUpRightAnimationFrameArray;
    
    [SerializeField] public Sprite[] attackArray;

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
        attackUp,
        attackLeft,
        attackDown,
        attackRight,
        attackUpLeft,
        attackLeftDown,
        attackDownRight,
        attackUpRight,
        attack
    }
    
    
    
    
    
    [SerializeField] private Sprite[] spriteArray;
    private float frameRate = .11f;
    private int currentFrame;
    private float timer;
    private SpriteRenderer spriteRenderer;
    private bool isAnimating = true;
    private bool loop = true;
    
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
    
    public void PlayAnimation(Sprite[] spriteArray, float frameRate)
    {
        this.spriteArray = spriteArray;
        this.frameRate = frameRate;
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = spriteArray[currentFrame];
    }
    
    private void PlayAnimation(AnimationType animationType)
    {
        if (animationType != activeAnimationType){
            activeAnimationType = animationType;

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
                case AnimationType.attackUp:
                    PlayAnimation(attackUpAnimationFrameArray, .1f);
                    break;
                case AnimationType.attackLeft:
                    PlayAnimation(attackLeftAnimationFrameArray, .1f);
                    break;
                case AnimationType.attackDown:
                    PlayAnimation(attackDownAnimationFrameArray, .1f);
                    break;
                case AnimationType.attackRight:
                    PlayAnimation(attackRightAnimationFrameArray, .1f);
                    break;
                case AnimationType.attackUpLeft:
                    PlayAnimation(attackUpLeftAnimationFrameArray, .1f);
                    break;
                case AnimationType.attackLeftDown:
                    PlayAnimation(attackLeftDownAnimationFrameArray, .1f);
                    break;
                case AnimationType.attackDownRight:
                    PlayAnimation(attackDownRightAnimationFrameArray, .1f);
                    break;
                case AnimationType.attackUpRight:
                    PlayAnimation(attackUpRightAnimationFrameArray, .1f);
                    break;
                case AnimationType.attack:
                    PlayAnimation(attackArray, .1f);
                    break;
            }
        }
    }
    private void StopAnimating(){
        isAnimating = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
