using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace RxceGame
{
    public class PlayerCarAuthoring : MonoBehaviour
    {

    }

    public class PlayerCarAuthoringBaker : Baker<PlayerCarAuthoring>
    {
        public override void Bake(PlayerCarAuthoring authoring)
        {
            AddComponent(new PlayerTag { });
        }
    }

}