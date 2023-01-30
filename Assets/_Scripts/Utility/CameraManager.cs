using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace RxceGame
{
    public class CameraManager : SingletonComponent<CameraManager>
    {
        private Camera _mainCamera;
        private Transform _tform;

        void Awake()
        {
            _mainCamera = Camera.main;
            _tform = transform;
        }

        public void SetCameraPosition(float3 targetPos) => _tform.position = new float3(targetPos.x, targetPos.y, _tform.position.z);
    }
}
