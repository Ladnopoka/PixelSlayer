using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Observer : MonoBehaviour
{
    public Attack _attack;
    private CharacterMovement _characterMovement;
    private void Awake()
    {

    }

    private void Start()
    {
        // Subscribe to the attack event
        _characterMovement.OnCharacterAttack += HandleCharacterAttack;
    }
    
    private void OnDestroy()
    {
        if (_attack != null)
        {    // Unsubscribe when this object is destroyed to prevent memory leaks
            _characterMovement.OnCharacterAttack -= HandleCharacterAttack;
        }
    }

    private void HandleCharacterAttack(object sender, EventArgs e)
    {
        Debug.Log("Obeserver is working with attack class!!!");
        // _characterAnimation.PlayAnimation(_SpriteData.GetAnimation(), .11f);
    }
}
