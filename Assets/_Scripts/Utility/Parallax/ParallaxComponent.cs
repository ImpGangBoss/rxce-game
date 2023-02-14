using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RxceGame
{
    [System.Serializable]
    public class ParallaxComponent
    {
        public Sprite image;
        [Range(0f, 1f)] public float effect;
        public Vector3 offset;
    }
}
