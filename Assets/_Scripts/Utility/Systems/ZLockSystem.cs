using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

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
            foreach (var tform in SystemAPI.Query<TransformAspect>())
            {
                var prevPos = tform.WorldPosition;
                if (prevPos.z != 0f)
                    tform.WorldPosition -= new float3(0f, 0f, prevPos.z);

                var prevRot = tform.WorldRotation;
                if (prevRot.value.x != 0f || prevRot.value.y != 0f)
                    tform.WorldRotation = new quaternion(0f, 0f, prevRot.value.z, prevRot.value.w);
            }
        }
    }
}
