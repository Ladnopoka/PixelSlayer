using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Attacking,
    }

    private State currentState;

    public State CurrentState 
    {
        get { return currentState; }
        private set 
        {
            currentState = value;

            // Notify observers if the state changes to attacking
            if (currentState == State.Attacking)
            {
                OnCharacterAttack?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Define the event for other classes to subscribe to
    public event EventHandler OnCharacterAttack;

    private void Awake()
    {
        currentState = State.Idle;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    public void Attack()
    {
        CurrentState = State.Attacking;
    }
}
