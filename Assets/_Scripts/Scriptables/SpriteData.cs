using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Animation Data NEW")]
public class SpriteData : ScriptableObject
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
    public class DirectionAnimation
    {
        public Sprite[] Up;
        public Sprite[] Down;
        public Sprite[] Left;
        public Sprite[] Right;
        public Sprite[] UpLeft;
        public Sprite[] UpRight;
        public Sprite[] DownLeft;
        public Sprite[] DownRight;
    }

    [System.Serializable]
    public class AnimationSet
    {
        public DirectionAnimation Idle;
        public DirectionAnimation Run;
        public DirectionAnimation Attack;
    }

    public AnimationSet animations;

    public Sprite[] GetAnimation(Direction direction, ActionType actionType)
    {
        DirectionAnimation directionAnimation;

        switch (actionType)
        {
            case ActionType.Idle:
                directionAnimation = animations.Idle;
                break;
            case ActionType.Run:
                directionAnimation = animations.Run;
                break;
            case ActionType.Attack:
                directionAnimation = animations.Attack;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
        }

        return GetSpritesByDirection(direction, directionAnimation);
    }

    private Sprite[] GetSpritesByDirection(Direction direction, DirectionAnimation directionAnimation)
    {
        switch (direction)
        {
            case Direction.Up:
                return directionAnimation.Up;
            case Direction.Down:
                return directionAnimation.Down;
            case Direction.Left:
                return directionAnimation.Left;
            case Direction.Right:
                return directionAnimation.Right;
            case Direction.UpLeft:
                return directionAnimation.UpLeft;
            case Direction.UpRight:
                return directionAnimation.UpRight;
            case Direction.DownLeft:
                return directionAnimation.DownLeft;
            case Direction.DownRight:
                return directionAnimation.DownRight;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
    
    /*public Sprite[] GetAnimation(int num)
    {
        if (num == 0)
            return idle;
        if (num == 1)
            return attack;

        return null;
        /*Debug.Log("Returned sprite array length for direction " + direction + " and action " + actionType + ": " + animationEntries[direction].Attack.Length);
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
        }#1#
    }*/
}