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
    public partial struct WheelCollisionAndDamageTriggerSystem : ISystem
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
                    // wheel detection
                    if (SystemAPI.HasComponent<WheelTag>(child.Value))
                    {
                        var wheelTransform = SystemAPI.GetAspectRW<TransformAspect>(child.Value);
                        var length = 0.35f;
                        var start = wheelTransform.LocalPosition + carAspect.Position();
                        start = math.rotate(carAspect.Rotation(), start);

                        int raysToShoot = 5;
                        float startAngle = 0.5f * math.PI;
                        float endAngle = 1.5f * math.PI;
                        float deltaAngle = (endAngle - startAngle) / (raysToShoot - 1);

                        for (int i = 0; i < raysToShoot; i++)
                        {
                            float x = math.sin(startAngle);
                            float y = math.cos(startAngle);
                            startAngle += deltaAngle;

                            var ray = new float3(x * length, y * length, 0);
                            var end = start + ray;

                            Debug.DrawLine(start, end, Color.red);

                            RaycastInput input = new()
                            {
                                Start = start,
                                End = end,
                                Filter = new CollisionFilter()
                                {
                                    // 0 -- car body
                                    // 1 -- damage trigger
                                    // 2 -- wheels
                                    // 3 -- ground
                                    // 4 -- obstacles

                                    BelongsTo = 1 << 0,
                                    CollidesWith = (1 << 3) | (1 << 4),
                                    GroupIndex = 0
                                }
                            };

                            Unity.Physics.RaycastHit hit = new();
                            bool haveHit = collisionWorld.CastRay(input, out hit);
                            Entity target = Entity.Null;

                            if (haveHit)
                            {
                                target = hit.Entity;

                                if (SystemAPI.HasComponent<GroundComponent>(target))
                                    carAspect.SetJumpTrigger(true);
                            }
                        }
                    }

                    // damage trigger detection
                    if (SystemAPI.HasComponent<DamageTriggerTag>(child.Value))
                    {
                        var damageTrigger = SystemAPI.GetAspectRW<TransformAspect>(child.Value);
                        var length = 0.35f;
                        var start = damageTrigger.LocalPosition + carAspect.Position();
                        start = math.rotate(carAspect.Rotation(), start);

                        int raysToShoot = 5;
                        float startAngle = 1.5f * math.PI;
                        float endAngle = 2.5f * math.PI;
                        float deltaAngle = (endAngle - startAngle) / (raysToShoot - 1);

                        for (int i = 0; i < raysToShoot; i++)
                        {
                            float x = math.sin(startAngle);
                            float y = math.cos(startAngle);
                            startAngle += deltaAngle;

                            var ray = new float3(x * length, y * length, 0);
                            var end = start + ray;

                            Debug.DrawLine(start, end, Color.black);

                            RaycastInput input = new()
                            {
                                Start = start,
                                End = end,
                                Filter = new CollisionFilter()
                                {
                                    // 0 -- car body
                                    // 1 -- damage trigger
                                    // 2 -- wheels
                                    // 3 -- ground
                                    // 4 -- obstacles

                                    BelongsTo = 1 << 0,
                                    CollidesWith = ~0u,
                                    GroupIndex = 0
                                }
                            };

                            Unity.Physics.RaycastHit hit = new();
                            bool haveHit = collisionWorld.CastRay(input, out hit);
                            Entity target = Entity.Null;

                            if (haveHit)
                            {
                                target = hit.Entity;

                                if (SystemAPI.HasComponent<GroundComponent>(target)
                                || SystemAPI.HasComponent<ObstacleComponent>(target))
                                    carAspect.SetDamageTrigger(true);
                            }
                        }
                    }
                }
            }
        }
    }
}
