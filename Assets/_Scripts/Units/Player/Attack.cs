using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class Attack : MonoBehaviour
{
    public SpriteData _spriteData;
    public CharacterMovement _characterMovement;
    private (float, float) lastMoveKey;
    
    private void Update()
    {
        
    }

    private void playAttackAnimation(SpriteData.Direction direction, (float, float) lastMoveKey)
    {
        
        Debug.Log("Playing attack animation with direction" + direction);
        _characterMovement.PlayAnimation(direction, SpriteData.ActionType.Attack);
    }
    
    private void HandlePlayerInput()
    {

    }

}
