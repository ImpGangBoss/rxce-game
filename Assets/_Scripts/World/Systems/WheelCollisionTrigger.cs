using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;


namespace RxceGame
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [BurstCompile]
    public partial struct WheelCollisionTrigger : ISystem
    {
        ComponentLookup<PlayerTag> playerLookup;
        ComponentLookup<CarMoveParams> carParamsLookup;
        ComponentLookup<GroundTag> groundLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            playerLookup = SystemAPI.GetComponentLookup<PlayerTag>();
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
            var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();

            playerLookup.Update(ref state);
            carParamsLookup.Update(ref state);
            groundLookup.Update(ref state);

            state.Dependency = new WheelHitJob()
            {
                allPlayers = playerLookup,
                allCarParams = carParamsLookup,
                allGround = groundLookup,
                ECB = ecbBOS
            }.Schedule(simulation, state.Dependency);
        }

    }

    //[BurstCompile]
    public struct WheelHitJob : ICollisionEventsJob
    {
        public ComponentLookup<PlayerTag> allPlayers;
        public ComponentLookup<CarMoveParams> allCarParams;
        public ComponentLookup<GroundTag> allGround;
        public EntityCommandBuffer ECB;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity wheel = Entity.Null;
            Entity ground = Entity.Null;
            Entity player = Entity.Null;

            if (allGround.HasComponent(collisionEvent.EntityA))
                ground = collisionEvent.EntityA;
            if (allGround.HasComponent(collisionEvent.EntityB))
                ground = collisionEvent.EntityB;
            if (allPlayers.HasComponent(collisionEvent.EntityA))
                player = collisionEvent.EntityA;
            if (allPlayers.HasComponent(collisionEvent.EntityB))
                player = collisionEvent.EntityB;

            // if its a pair of entity we don't want to process, exit
            if (Entity.Null.Equals(ground) || Entity.Null.Equals(player))
            {
                Debug.Log("Something else");
                return;
            }

            var playerCarParam = allCarParams[player];
            playerCarParam.JumpTrigger = true;
            ECB.SetComponent(player, playerCarParam);
        }
    }
}
