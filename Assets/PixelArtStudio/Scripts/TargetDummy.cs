
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*public class TargetDummy : MonoBehaviour{
    //[SerializeField] private Sprite[] idleLeftDownAnimationFrameArray;
    CharacterMovement characterMovement;

    void Start() {
        characterMovement.PlayAnimation(characterMovement., .11f);
    }
}*/

public class TargetDummy : MonoBehaviour{

    [SerializeField] private Sprite[] frameArray;
    private int currFrame;
    private float timer;
    private float frameRate = .11f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currFrame = (currFrame+1) % frameArray.Length;
            gameObject.GetComponent<SpriteRenderer>().sprite = frameArray[currFrame];
        }

    }
}
