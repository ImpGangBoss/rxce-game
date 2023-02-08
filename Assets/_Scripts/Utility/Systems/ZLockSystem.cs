using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics.Aspects;

namespace RxceGame
{
    public partial struct ZLockSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }

        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var rbody in SystemAPI.Query<RigidBodyAspect>())
            {
                var prevPos = rbody.Position;
                if (prevPos.z != 0f)
                    rbody.Position -= new float3(0f, 0f, prevPos.z);

                var prevRot = rbody.Rotation;
                if (prevRot.value.x != 0f || prevRot.value.y != 0f)
                    rbody.Rotation = new quaternion(0f, 0f, prevRot.value.z, prevRot.value.w);
            }
        }
    }
}
