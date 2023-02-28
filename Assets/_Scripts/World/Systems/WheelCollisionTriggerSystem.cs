using System;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

namespace RxceGame
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [BurstCompile]
    public partial struct WheelCollisionTriggerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var collisionWorld = physicsWorldSingleton.CollisionWorld;

            foreach (var carAspect in SystemAPI.Query<CarAspect>())
            {
                var childrenBuffer = SystemAPI.GetBuffer<LinkedEntityGroup>(carAspect.Entity());
                foreach (var child in childrenBuffer)
                {
                    if (SystemAPI.HasComponent<WheelTag>(child.Value))
                    {
                        var wheelBody = SystemAPI.GetComponent<WorldTransform>(child.Value);

                        // 0 -- car body
                        // 1 -- damage trigger
                        // 2 -- wheels
                        // 3 -- ground
                        // 4 -- obstacles

                        RaycastInput input = new()
                        {
                            Start = wheelBody.Position,
                            End = wheelBody.Position + new float3(0f, -0.31f, 0f), // magic float3 of wheel radius
                            Filter = new CollisionFilter()
                            {
                                BelongsTo = 1 << 0,
                                CollidesWith = (1 << 3) | (1 << 4),
                                GroupIndex = 0
                            }
                        };

                        //TODO: BLACK MAGIC WITH RAYS. NEED TO FIX

                        Debug.DrawLine(wheelBody.Position, wheelBody.Position + new float3(0f, -0.31f, 0f), Color.red);

                        Unity.Physics.RaycastHit hit = new();
                        bool haveHit = collisionWorld.CastRay(input, out hit);
                        Entity target = Entity.Null;

                        if (haveHit)
                        {
                            target = hit.Entity;

                            if (SystemAPI.HasComponent<WheelTag>(target))
                                Debug.Log("Wheel");

                            if (SystemAPI.HasComponent<GroundComponent>(target))
                            {
                                Debug.Log("Ground");
                                carAspect.SetJumpTrigger(true);
                            }
                        }
                    }
                }
            }
        }
    }
}
