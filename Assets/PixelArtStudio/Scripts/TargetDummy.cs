
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetDummy : MonoBehaviour{

    [SerializeField] private Sprite[] frameArray;
    private int currFrame;
    private float timer;
    private float _frameRate = .11f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= _frameRate)
        {
            timer -= _frameRate;
            currFrame = (currFrame+1) % frameArray.Length;
            gameObject.GetComponent<SpriteRenderer>().sprite = frameArray[currFrame];
        }

    }
}
