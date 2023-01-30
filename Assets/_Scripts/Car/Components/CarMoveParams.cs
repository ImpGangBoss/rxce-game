using Unity.Entities;

namespace RxceGame
{
    public struct CarMoveParams : IComponentData
    {
        public float mass;
        public float acceleration;
        public float maxSpeed;
        public float jumpImpulse;
        public bool JumpTrigger { get; set; }
        public float lastPhysicsUpdateTime;
    }
}
