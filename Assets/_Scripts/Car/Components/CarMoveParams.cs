using Unity.Entities;

namespace RxceGame
{
    public struct CarMoveParams : IComponentData
    {
        public bool initialized;
        public float mass;
        public float acceleration;
        public float maxSpeed;
        public float jumpImpulse;
        public float rotationSpeed;
        public float brakeSpeed;
        public bool JumpTrigger { get; set; }
    }
}
