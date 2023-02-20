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

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            damageTriggerLookup = SystemAPI.GetComponentLookup<DamageTriggerTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbBS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();

            damageTriggerLookup.Update(ref state);

            foreach (var carAspect in SystemAPI.Query<CarAspect>())
            {
                state.Dependency = new DamageTriggerHitJob()
                {
                    // allTriggers = damageTriggerLookup,
                    // car = carAspect.GetMoveParams(),
                    // ecb = ecbBS
                }.Schedule(simulation, state.Dependency);
            }
        }
    }

    public struct DamageTriggerHitJob : ITriggerEventsJob
    {
        //TODO: Write something that works
        // public ComponentLookup<DamageTriggerTag> allTriggers;
        // public CarMoveParams car;
        // public EntityCommandBuffer ecb;
        public void Execute(TriggerEvent triggerEvent)
        {
            //     Entity trigger = Entity.Null;

            //     if (allTriggers.HasComponent(triggerEvent.EntityA))
            //         trigger = triggerEvent.EntityA;

            //     if (allTriggers.HasComponent(triggerEvent.EntityB))
            //         trigger = triggerEvent.EntityB;

            //     if (Entity.Null.Equals(trigger))
            //     {
            //         Debug.Log("Trigger: " + Entity.Null.Equals(trigger));
            //         return;
            //     }

            //     Debug.Log("Triggered");

            //     car.DamageTrigger = true;
            //     ecb.SetComponent(car.entity, car);

        }
    }
}
