using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UsefulFunctions
{
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vector.z = 0f;
        return vector;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static SpriteData.Direction getDirection(float x, float y)
    {
        var tuple = (x, y);

        switch (tuple)
        {
            case (0, 1):
                return SpriteData.Direction.Up;
            case (-1, 0):
                return SpriteData.Direction.Left;
            case (0, -1):
                return SpriteData.Direction.Down;
            case (1, 0):
                return SpriteData.Direction.Right;
            case (-1, 1):
                return SpriteData.Direction.UpLeft;
            case (-1, -1):
                return SpriteData.Direction.DownLeft;
            case (1, -1):
                return SpriteData.Direction.DownRight;
            case (1, 1):
                return SpriteData.Direction.UpRight;

        }

        return SpriteData.Direction.Up;
    }

    public static SpriteData.Direction getDirectionFromCoordinates(float x, float y)
    {
        Vector2 direction = new Vector2(x, y).normalized;
        float thresholdAngle = 22.5f; // half of 45 degrees

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            // More vertical
            if (direction.y > 0)
            {
                // Up
                if (Vector2.Angle(direction, Vector2.up) < thresholdAngle) return SpriteData.Direction.Up;

                // UpRight or UpLeft
                return (direction.x > 0) ? SpriteData.Direction.UpRight : SpriteData.Direction.UpLeft;
            }
            else
            {
                // Down
                if (Vector2.Angle(direction, Vector2.down) < thresholdAngle) return SpriteData.Direction.Down;

                // DownRight or DownLeft
                return (direction.x > 0) ? SpriteData.Direction.DownRight : SpriteData.Direction.DownLeft;
            }
        }
        else
        {
            // More horizontal
            if (direction.x > 0)
            {
                // Right
                if (Vector2.Angle(direction, Vector2.right) < thresholdAngle) return SpriteData.Direction.Right;

                // UpRight or DownRight
                return (direction.y > 0) ? SpriteData.Direction.UpRight : SpriteData.Direction.DownRight;
            }
            else
            {
                // Left
                if (Vector2.Angle(direction, Vector2.left) < thresholdAngle) return SpriteData.Direction.Left;

                // UpLeft or DownLeft
                return (direction.y > 0) ? SpriteData.Direction.UpLeft : SpriteData.Direction.DownLeft;
            }
        }
    }
}