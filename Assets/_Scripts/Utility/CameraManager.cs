using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RxceGame
{
    public class CameraManager : SingletonComponent<CameraManager>
    {
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private Vector3 offset;
        private Camera _mainCamera;
        private Transform _tform;
        private Vector3 _targetPos;
        private Vector3 _velocity = Vector3.zero;

        void Awake()
        {
            Application.targetFrameRate = 60;

            _mainCamera = Camera.main;
            _tform = transform;
            _targetPos = _tform.position;
        }

        void LateUpdate()
        {
            _tform.position = Vector3.SmoothDamp(_tform.position, _targetPos + offset, ref _velocity, smoothTime);
        }

        Vector3 CutOffZ(Vector3 v) => new Vector3(v.x, v.y, _tform.position.z);

        public void SetTargetPosition(Vector3 target) => _targetPos = CutOffZ(target);
    }
}
