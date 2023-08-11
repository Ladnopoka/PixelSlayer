using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkeletonData", menuName = "Skeleton Data")]
public class SkeletonScriptableObject : ScriptableObject
{
    public string testString;
    public Sprite[] spriteArray;

    public void SkeletonScriptableObjectTrigger()
    {
        Debug.Log("spriteArray Length: " + spriteArray.Length);
        Debug.Log("spriteArray Length: " + spriteArray[0]);
    }
}
