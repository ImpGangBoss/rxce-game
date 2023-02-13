using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace RxceGame
{
    public struct RandomComponent : IComponentData
    {
        public Random random;
    }
}
