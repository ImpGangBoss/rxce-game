using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics.Aspects;

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
            var playerCarBody = SystemAPI.GetAspectRW<RigidBodyAspect>(SystemAPI.GetSingletonEntity<PlayerCar>());

            if (cam == null)
                return;

            cam.SetTargetPosition(playerCarBody.Position);
        }
    }
}
