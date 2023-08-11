using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Observer : MonoBehaviour
{
    public Attack _attack;
    private CharacterMovement _characterMovement;
    private MyScriptableAnimation _characterAnimation;
    public MyScriptableObject _myScriptableObject;
    
    private void Awake()
    {

    }

    private void Start()
    {
        if (_attack == null)
        {
            Debug.LogError("CharacterController not found on GameObject: " + gameObject.name);
            return;
        }
        
        // Subscribe to the attack event
        _attack.OnCharacterAttack += HandleCharacterAttack;
    }
    
    private void OnDestroy()
    {
        if (_attack != null)
        {    // Unsubscribe when this object is destroyed to prevent memory leaks
            _attack.OnCharacterAttack -= HandleCharacterAttack;
        }
    }

    private void HandleCharacterAttack(object sender, EventArgs e)
    {
        Debug.Log("Obeserver is working with attack class!!!");
        //_characterAnimation.PlayAnimation(_myScriptableObject.GetAnimation(), .11f);
    }
}
