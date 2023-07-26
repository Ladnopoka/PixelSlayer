using System;
using UnityEngine;

namespace PixelArtStudio.Scripts
{
    public class CameraMovement : MonoBehaviour{
        
        private Func<Vector3> GetCameraFollowPositionFunc;

        public void Setup(Func<Vector3> GetCameraFollowPositionFunc)
        {
            this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
        }

        private void Update() {
            Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
            cameraFollowPosition.z = transform.position.z;
            transform.position = cameraFollowPosition;
        }
    }
}