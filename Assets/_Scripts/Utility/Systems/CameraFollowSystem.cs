using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace RxceGame
{
    public partial class CameraFollower : SystemBase
    {
        CameraManager _cam;

        protected override void OnCreate()
        {
            var _cam = CameraManager.Instance;
        }

        protected override void OnUpdate()
        {
            if (_cam == null)
                return;

            //_cam.SetCameraPosition()
        }
    }
}
