using Unity.Entities;

namespace RxceGame
{
    public struct CarMoveParams : IComponentData
    {
        public Entity entity;
        public bool initialized;
        public float hp;
        public float mass;
        public float acceleration;
        public float maxSpeed;
        public float jumpImpulse;
        public float rotationSpeed;
        public float brakeSpeed;
        public bool JumpTrigger { get; set; }
        public bool DamageTrigger { get; set; }
    }
}
