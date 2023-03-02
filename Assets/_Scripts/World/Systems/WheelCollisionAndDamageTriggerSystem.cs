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
                        var wheelTransform = SystemAPI.GetComponent<LocalToWorld>(child.Value);
                        var length = 0.35f;
                        var start = wheelTransform.Position;

                        int raysToShoot = 10;
                        float qw = wheelTransform.Rotation.value.w;
                        float qz = wheelTransform.Rotation.value.z;
                        float direction = -math.clamp(qz / math.sqrt(1 - qw * qw), -1f, 1f);
                        float angleOffset = 2 * math.acos(qw) * direction;
                        float startAngle = 0 + angleOffset;
                        float endAngle = 2 * math.PI + angleOffset;
                        float deltaAngle = (endAngle - startAngle) / raysToShoot;

                        for (int i = 0; i < raysToShoot; i++)
                        {
                            float x = math.sin(startAngle);
                            float y = math.cos(startAngle);
                            startAngle += deltaAngle;

                            var ray = new float3(x * length, y * length, 0f);
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
                        var damageTrigger = SystemAPI.GetComponent<LocalToWorld>(child.Value);
                        var length = 1.5f;
                        var start = damageTrigger.Position;

                        var qw = damageTrigger.Rotation.value.w;
                        var qz = damageTrigger.Rotation.value.z;
                        float direction = -math.clamp(qz / math.sqrt(1 - qw * qw), -1f, 1f);
                        float angleOffset = 2 * math.acos(qw) * direction;
                        float startAngle = angleOffset;
                        float endAngle = angleOffset;

                        float x = math.sin(startAngle);
                        float y = math.cos(startAngle);

                        var ray = new float3(x * length, y, 0f);
                        start += new float3(-length * 0.5f, 0f, 0f);
                        var end = start + ray + new float3(length * 0.5f, 0f, 0f);

                        Debug.DrawLine(start, end, Color.black);

                        RaycastInput input = new()
                        {
                            Start = start,
                            End = end,
                            Filter = new CollisionFilter()
                            {
                                BelongsTo = ~0u,
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
