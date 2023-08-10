using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Observer : MonoBehaviour
{
    public CharacterController _characterController;
    private CharacterMovement _characterMovement;
    public CharacterAnimation _characterAnimation;
    
    private void Awake()
    {
        _characterAnimation = GetComponent<CharacterAnimation>();
        if (_characterAnimation == null)
        {
            Debug.LogError("CharacterAnimation not found during Awake on GameObject: " + gameObject.name);
        }
    }

    private void Start()
    {
        if (_characterController == null)
        {
            Debug.LogError("CharacterController not found on GameObject: " + gameObject.name);
            return;
        }
        
        // Subscribe to the attack event
        _characterController.OnCharacterAttack += HandleCharacterAttack;
    }
    
    private void OnDestroy()
    {
        if (_characterController != null)
        {    // Unsubscribe when this object is destroyed to prevent memory leaks
            _characterController.OnCharacterAttack -= HandleCharacterAttack;
        }
    }

    private void HandleCharacterAttack(object sender, EventArgs e)
    {
        Debug.Log("The character attacked!");

        if (_characterAnimation != null)
        {
            _characterAnimation.TriggerAttack();
        }
        else
        {
            Debug.LogError("_characterAnimation is null.");
        }
    }
}
