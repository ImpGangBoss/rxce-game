using Unity.Entities;
using Unity.Physics.Aspects;
using Unity.Burst;

namespace RxceGame
{
    [BurstCompile]
    public partial struct CameraFollowerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        public void OnUpdate(ref SystemState state)
        {
            var cam = CameraManager.Instance;
            var playerCarBody = SystemAPI.GetAspectRW<RigidBodyAspect>(SystemAPI.GetSingletonEntity<PlayerTag>());

            if (cam == null)
                return;

            cam.SetTargetPosition(playerCarBody.Position);
        }
    }
}
