using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Animation Data")]
public class CharacterAnimationData : ScriptableObject
{

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public enum ActionType
    {
        Idle,
        Run,
        Attack
    }

    [System.Serializable]
    public struct AnimationSet
    {
        public Sprite[] Idle;
        public Sprite[] Run;
        public Sprite[] Attack;
    }

    [System.Serializable]
    public class AnimationEntry
    {
        public Direction direction;
        public AnimationSet animationSet;
    }

    public Dictionary<Direction, AnimationSet> animationEntries = new Dictionary<Direction, AnimationSet>();

    public CharacterAnimationData()
    {
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            animationEntries[direction] = new AnimationSet();
        }
    }

    public Sprite[] GetAnimation(Direction direction, ActionType actionType)
    {
        Debug.Log("I have reached the GetAnimation method in Data!!!");
        Debug.Log("Direction: " + direction);
        Debug.Log("Action Type: " + actionType);
        Debug.Log("Action Type IDLE: " + animationEntries[direction].Idle);
        switch (actionType)
        {
            case ActionType.Idle:
                return animationEntries[direction].Idle;
            case ActionType.Run:
                return animationEntries[direction].Run;
            case ActionType.Attack:
                return animationEntries[direction].Attack;
            default:
                throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
        }
    }
}




/*[CreateAssetMenu(menuName = "Character Animation Data")]
public class CharacterAnimationData : ScriptableObject
{
    [System.Serializable]
    public class AnimationEntry
    {
        public Direction direction;
        public AnimationSet animationSet;
    }

    public List<AnimationEntry> animationEntries = new List<AnimationEntry>();
    
    // ... (rest of your enums and struct definitions) ...

    // You can still use the dictionary in your methods but populate it at runtime
    public Dictionary<Direction, AnimationSet> ToDictionary()
    {
        Dictionary<Direction, AnimationSet> result = new Dictionary<Direction, AnimationSet>();
        foreach(var entry in animationEntries)
        {
            result[entry.direction] = entry.animationSet;
        }
        return result;
    }
    
    // ... (rest of your methods, like GetAnimation, but use the above ToDictionary method to get the dictionary) ...
}*/






/*
[CreateAssetMenu(menuName =
    "Character Animation Data")] // This allows you to create a new asset from Unity's Assets menu
public class CharacterAnimationData : ScriptableObject
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public enum ActionType
    {
        Idle,
        Run,
        Attack
    }

    public struct AnimationSet
    {
        public Sprite[] Idle;
        public Sprite[] Run;
        public Sprite[] Attack;
    }

    public Dictionary<Direction, AnimationSet> Animations = new Dictionary<Direction, AnimationSet>();

    // Constructor (if you need to initialize it with some data)
    public CharacterAnimationData()
    {
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Animations[direction] = new AnimationSet();
        }
    }

    public Sprite[] GetAnimation(Direction direction, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Idle:
                return Animations[direction].Idle;
            case ActionType.Run:
                return Animations[direction].Run;
            case ActionType.Attack:
                return Animations[direction].Attack;
            default:
                throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
        }
    }
}*/