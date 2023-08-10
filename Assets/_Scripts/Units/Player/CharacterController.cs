using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{
    // Making the State enum public
    public enum State
    {
        Idle,
        Attacking,
    }

    // Private variable to hold the state
    private State currentState;

    // Public property to access and optionally set the state
    public State CurrentState 
    {
        get { return currentState; }
        set { currentState = value; } // This setter allows other classes to change the state. If you don't want that, just remove this setter.
    }

    private void Awake()
    {
        currentState = State.Idle;
    }
}