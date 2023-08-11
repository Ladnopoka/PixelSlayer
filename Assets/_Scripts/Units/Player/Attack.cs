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
    // Define the event for other classes to subscribe to
    public event EventHandler OnCharacterAttack;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnCharacterAttack?.Invoke(this, EventArgs.Empty);
        }
    }
}
