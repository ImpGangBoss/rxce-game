using Unity.Entities;
using UnityEngine;

namespace RxceGame
{
    public class ObstacleAuthoring : MonoBehaviour
    {

    }

    public class ObstacleBaker : Baker<ObstacleAuthoring>
    {
        public override void Bake(ObstacleAuthoring authoring)
        {
            AddComponent(new ObstacleComponent
            {
                isSpawned = false
            });
        }
    }
}