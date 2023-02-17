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
    public partial struct DamageTriggerSystem : ISystem
    {
        ComponentLookup<DamageTriggerTag> damageTriggerLookup;
        ComponentLookup<CarMoveParams> carsLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            damageTriggerLookup = SystemAPI.GetComponentLookup<DamageTriggerTag>();
            carsLookup = SystemAPI.GetComponentLookup<CarMoveParams>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbBS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();

            damageTriggerLookup.Update(ref state);
            carsLookup.Update(ref state);

            state.Dependency = new DamageTriggerHitJob()
            {
                allCars = carsLookup,
                allTriggers = damageTriggerLookup,
                ecb = ecbBS
            }.Schedule(simulation, state.Dependency);
        }
    }

    public struct DamageTriggerHitJob : ITriggerEventsJob
    {
        public ComponentLookup<DamageTriggerTag> allTriggers;
        public ComponentLookup<CarMoveParams> allCars;
        public EntityCommandBuffer ecb;
        public void Execute(TriggerEvent triggerEvent)
        {
            Debug.Log(triggerEvent.ColliderKeyA.Value);
            Debug.Log(triggerEvent.ColliderKeyB.Value);

            Entity trigger = Entity.Null;

            if (allTriggers.HasComponent(triggerEvent.EntityA))
                trigger = triggerEvent.EntityA;

            if (allTriggers.HasComponent(triggerEvent.EntityB))
                trigger = triggerEvent.EntityB;

            if (Entity.Null.Equals(trigger))
            {
                Debug.Log("Wrong trigger");
                return;
            }

            var car = SystemAPI.GetComponent<PreviousParent>(trigger);
            var carAspect = SystemAPI.GetAspectRW<CarAspect>(car.Value);
            carAspect.TakeDamage(Time.deltaTime);
        }
    }
}
