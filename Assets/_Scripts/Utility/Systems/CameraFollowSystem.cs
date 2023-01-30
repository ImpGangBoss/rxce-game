using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace RxceGame
{
    public partial struct CameraFollowerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerCar>();
        }

        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            var cam = CameraManager.Instance;
            var playerTransform = SystemAPI.GetAspectRO<TransformAspect>(SystemAPI.GetSingletonEntity<PlayerCar>());

            if (cam == null)
                return;

            cam.SetTargetPosition(playerTransform.WorldPosition);
        }
    }
}
