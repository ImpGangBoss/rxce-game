using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using System.Runtime.InteropServices;
using Unity.Transforms;

namespace RxceGame
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [BurstCompile]
    [StructLayout(LayoutKind.Auto)]
    public partial struct WheelCollisionTriggerSystem : ISystem
    {
        ComponentLookup<WheelTag> wheelsLookup;
        ComponentLookup<GroundComponent> groundLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            wheelsLookup = SystemAPI.GetComponentLookup<WheelTag>();
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

            wheelsLookup.Update(ref state);
            groundLookup.Update(ref state);

            state.Dependency = new WheelHitJob()
            {
                allWheels = wheelsLookup,
                allGround = groundLookup,
                ecb = ecbBS
            }.Schedule(simulation, state.Dependency);
        }
    }

    public struct WheelHitJob : ICollisionEventsJob
    {
        public ComponentLookup<WheelTag> allWheels;
        public ComponentLookup<GroundComponent> allGround;
        public EntityCommandBuffer ecb;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity wheel = Entity.Null;
            Entity ground = Entity.Null;

            if (allGround.HasComponent(collisionEvent.EntityA))
                ground = collisionEvent.EntityA;
            if (allGround.HasComponent(collisionEvent.EntityB))
                ground = collisionEvent.EntityB;

            if (allWheels.HasComponent(collisionEvent.EntityA))
                wheel = collisionEvent.EntityA;
            if (allWheels.HasComponent(collisionEvent.EntityB))
                wheel = collisionEvent.EntityB;

            if (Entity.Null.Equals(ground) || Entity.Null.Equals(wheel))
            {
                Debug.Log("Something else");
                Debug.Log("Ground: " + Entity.Null.Equals(ground));
                Debug.Log("Wheel: " + Entity.Null.Equals(wheel));
                return;
            }

            var car = SystemAPI.GetComponent<PreviousParent>(wheel);
            var carAspect = SystemAPI.GetAspectRW<CarAspect>(car.Value);
            carAspect.SetJumpTrigger(true);
        }
    }
}
