using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class CharacterAnimationData
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
}




/*[System.Serializable]
public class CharacterAnimationData
{
    // public Sprite[] IdleUp;
    // public Sprite[] IdleLeft;
    // ... Other sprite arrays.
    public struct AnimationFrames
    {
        public Sprite[] idleUpAnimationFrameArray;
        public Sprite[] idleLeftAnimationFrameArray;
        public Sprite[] idleDownAnimationFrameArray;
        public Sprite[] idleRightAnimationFrameArray;
        public Sprite[] idleUpLeftAnimationFrameArray;
        public Sprite[] idleLeftDownAnimationFrameArray;
        public Sprite[] idleDownRightAnimationFrameArray;
        public Sprite[] idleUpRightAnimationFrameArray;
        public Sprite[] runUpAnimationFrameArray;
        public Sprite[] runLeftAnimationFrameArray;
        public Sprite[] runDownAnimationFrameArray;
        public Sprite[] runRightAnimationFrameArray;
        public Sprite[] runUpLeftAnimationFrameArray;
        public Sprite[] runLeftDownAnimationFrameArray;
        public Sprite[] runDownRightAnimationFrameArray;
        public Sprite[] runUpRightAnimationFrameArray;
        public Sprite[] attackUpArray;
        public Sprite[] attackLeftArray;
        public Sprite[] attackDownArray;
        public Sprite[] attackRightArray;
        public Sprite[] attackUpLeftArray;
        public Sprite[] attackUpRightArray;
        public Sprite[] attackLeftDownArray;
        public Sprite[] attackDownRightArray;
    }
    
    public AnimationFrames Up { get; set; }
    public AnimationFrames Down { get; set; }
    public AnimationFrames Left { get; set; }
    public AnimationFrames Right { get; set; }
    public AnimationFrames UpLeft { get; set; }
    public AnimationFrames UpRight { get; set; }
    public AnimationFrames DownLeft { get; set; }
    public AnimationFrames DownRight { get; set; }
    
    // Constructor (if you need to initialize it with some data)
    public CharacterAnimationData()
    {
        Up = new AnimationFrames();
        Down = new AnimationFrames();
        Left = new AnimationFrames();
        Right = new AnimationFrames();
        UpLeft = new AnimationFrames();
        UpRight = new AnimationFrames();
        DownLeft = new AnimationFrames();
        DownRight = new AnimationFrames();
        // ... Initialize others as needed
    }
}*/
