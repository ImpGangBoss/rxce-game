using System;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using System.Runtime.InteropServices;
using Unity.Transforms;
using Unity.Collections;

namespace RxceGame
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [BurstCompile]
    [StructLayout(LayoutKind.Auto)]
    public partial struct WheelCollisionTriggerSystem : ISystem
    {
        ComponentLookup<GroundComponent> groundLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            groundLookup = SystemAPI.GetComponentLookup<GroundComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbBS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();

            groundLookup.Update(ref state);

            foreach (var carAspect in SystemAPI.Query<CarAspect>())
            {
                //TODO: fix problem with wheels and physic body
                // var wheelsBuffer = SystemAPI.GetBuffer<LinkedEntityGroup>(carAspect.Entity());
                // NativeList<Entity> wheelsEntities = new NativeList<Entity>(Allocator.TempJob);
                // foreach (var wheel in wheelsBuffer)
                //     if (SystemAPI.HasComponent<WheelTag>(wheel.Value))
                //         wheelsEntities.Add(wheel.Value);

                state.Dependency = new WheelHitJob()
                {
                    car = carAspect.GetMoveParams(),
                    //wheels = wheelsEntities,
                    allGround = groundLookup,
                    ecb = ecbBS
                }.Schedule(simulation, state.Dependency);
            }
        }
    }

    public struct WheelHitJob : ICollisionEventsJob
    {
        public CarMoveParams car;
        //public NativeList<Entity> wheels;
        public ComponentLookup<GroundComponent> allGround;
        public EntityCommandBuffer ecb;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity ground = Entity.Null;
            //Entity wheel = Entity.Null;

            var EntityA = collisionEvent.EntityA;
            var EntityB = collisionEvent.EntityB;

            if (allGround.HasComponent(EntityA))
                ground = EntityA;

            if (allGround.HasComponent(EntityB))
                ground = EntityB;

            // Debug.Log("-----------");
            // Debug.Log(EntityA.Index);
            // Debug.Log(EntityB.Index);
            // Debug.Log(wheels[0].Index);
            // Debug.Log(wheels[1].Index);
            // Debug.Log(car.entity.Index);
            // Debug.Log(ground.Index);
            // Debug.Log("-----------");

            // foreach (var w in wheels)
            // {
            //     if (w.Equals(EntityA))
            //         wheel = EntityA;

            //     if (w.Equals(EntityB))
            //         wheel = EntityB;
            // }


            if (Entity.Null.Equals(ground) || Entity.Null.Equals(car))
            {
                //Debug.Log(wheels.Length);
                Debug.Log("Ground: " + Entity.Null.Equals(ground));
                //Debug.Log("Wheel: " + Entity.Null.Equals(wheel));
                Debug.Log("Car: " + Entity.Null.Equals(car));
                return;
            }

            car.JumpTrigger = true;
            ecb.SetComponent(car.entity, car);
        }
    }
}
