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
            charMovement.OnCollision += OnCollision;
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
        Debug.Log(e.keyValue);
    }
    private void AnimationChange(object sender, CharacterMovement.OnAnimationChangeEventArgs e){
        Debug.Log("Animation: " + e.AnimationType);
    }
    private void OnCollision(object sender, CharacterMovement.OnCollisionEventArgs e){
        Debug.Log("Collision detected with: " + e.collisionVar.collider.gameObject.name);
        Debug.Log("And it happened at : " + e.collisionVar.point);
    }

    void OnDestroy()
    {
        if(charMovement != null){
            charMovement.OnKeyPress -= MoveCharacter;
            charMovement.OnAnimationChange -= AnimationChange; // unsubscribe to the new event
        }
    }
}
