using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameHandler : MonoBehaviour
{
    [SerializeField] CharacterMovement charMovement;

    private void Awake() {
        if(charMovement != null) {
            charMovement.OnKeyPress += MoveCharacter; //subscribe
            charMovement.OnAnimationChange += AnimationChange;
        }
        else {
            Debug.LogError("CharacterMovement component not found on this GameObject.");
        }
    }

    private void Start() {

    }

    private void Update() {

    }
    
    private void MoveCharacter(object sender, CharacterMovement.OnKeyPressEventArgs e){
        string key = e.keyValue;

        Debug.Log(key);
    }
    private void AnimationChange(object sender, CharacterMovement.OnAnimationChangeEventArgs e){
        Debug.Log("Animation: " + e.AnimationType);
    }

    void OnDestroy()
    {
        if(charMovement != null){
            charMovement.OnKeyPress -= MoveCharacter;
            charMovement.OnAnimationChange -= AnimationChange; // unsubscribe to the new event
        }
    }
}
