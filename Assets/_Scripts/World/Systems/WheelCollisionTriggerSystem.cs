using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using System.Runtime.InteropServices;

namespace RxceGame
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [BurstCompile]
    [StructLayout(LayoutKind.Auto)]
    public partial struct WheelCollisionTrigger : ISystem
    {
        ComponentLookup<CarMoveParams> carParamsLookup;
        ComponentLookup<GroundTag> groundLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            carParamsLookup = SystemAPI.GetComponentLookup<CarMoveParams>();
            groundLookup = SystemAPI.GetComponentLookup<GroundTag>();
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

            carParamsLookup.Update(ref state);
            groundLookup.Update(ref state);

            state.Dependency = new WheelHitJob()
            {
                allCars = carParamsLookup,
                allGround = groundLookup,
                ecb = ecbBS
            }.Schedule(simulation, state.Dependency);
        }

    }

    public struct WheelHitJob : ICollisionEventsJob
    {
        public ComponentLookup<CarMoveParams> allCars;
        public ComponentLookup<GroundTag> allGround;
        public EntityCommandBuffer ecb;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity car = Entity.Null;
            Entity ground = Entity.Null;

            if (allGround.HasComponent(collisionEvent.EntityA))
                ground = collisionEvent.EntityA;
            if (allGround.HasComponent(collisionEvent.EntityB))
                ground = collisionEvent.EntityB;
            if (allCars.HasComponent(collisionEvent.EntityA))
                car = collisionEvent.EntityA;
            if (allCars.HasComponent(collisionEvent.EntityB))
                car = collisionEvent.EntityB;

            // if its a pair of entity we don't want to process, exit
            if (Entity.Null.Equals(ground) || Entity.Null.Equals(car))
            {
                Debug.Log("Something else");
                return;
            }

            var currCarParam = allCars[car];
            currCarParam.JumpTrigger = true;
            ecb.SetComponent(car, currCarParam);
        }
    }
}
