using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private MovementTest movementTest; 
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
    private AnimationType activeAnimationType;

    private void Start() {
        PlayAnimation(AnimationType.Idle);  
    }

    private void Update() {
        MoveCharacter();
    }

    private void PlayAnimation(AnimationType animationType)
    {
        if (animationType != activeAnimationType){
            activeAnimationType = animationType;

            switch (animationType){
                case AnimationType.Idle:
                    movementTest.PlayAnimation(idleAnimationFrameArray, .11f);
                    break;
                case AnimationType.RunUp:
                    movementTest.PlayAnimation(runUpAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunLeft:
                    movementTest.PlayAnimation(runLeftAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunDown:
                    movementTest.PlayAnimation(runDownAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunRight:
                    movementTest.PlayAnimation(runRightAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunUpLeft:
                    movementTest.PlayAnimation(runUpLeftAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunLeftDown:
                    movementTest.PlayAnimation(runLeftDownAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunDownRight:
                    movementTest.PlayAnimation(runDownRightAnimationFrameArray, .1f);
                    break;
                case AnimationType.RunUpRight:
                    movementTest.PlayAnimation(runUpRightAnimationFrameArray, .1f);
                    break;
            }
        }
    }
    
    private void MoveCharacter(){
        bool isMoving = false;
        string key = " ";

        if (Input.GetKey(KeyCode.W)){
            isMoving = true;
            key = "W";
        }
        if (Input.GetKey(KeyCode.A)){
            isMoving = true;
            key = "A";
        }
        if (Input.GetKey(KeyCode.S)){
            isMoving = true;
            key = "S";
        }
        if (Input.GetKey(KeyCode.D)){
            isMoving = true;
            key = "D";
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            isMoving = true;
            key = "WA";
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {
            isMoving = true;
            key = "AS";
        }        
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            isMoving = true;
            key = "SD";
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            isMoving = true;
            key = "WD";
        }        

        if (isMoving){
            if (key == "W")
                PlayAnimation(AnimationType.RunUp); 
            if (key == "A")
                PlayAnimation(AnimationType.RunLeft); 
            if (key == "S")
                PlayAnimation(AnimationType.RunDown); 
            if (key == "D")
                PlayAnimation(AnimationType.RunRight);
            if (key == "WA")
                PlayAnimation(AnimationType.RunUpLeft); 
            if (key == "AS")
                PlayAnimation(AnimationType.RunLeftDown); 
            if (key == "SD")
                PlayAnimation(AnimationType.RunDownRight); 
            if (key == "WD")
                PlayAnimation(AnimationType.RunUpRight);
        }
        else{
            PlayAnimation(AnimationType.Idle);
        }
    }
}
