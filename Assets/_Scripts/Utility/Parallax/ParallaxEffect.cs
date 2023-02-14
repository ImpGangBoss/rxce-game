using System.Collections.Generic;
using UnityEngine;

namespace RxceGame
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private List<ParallaxComponent> images;
        private List<Transform> _parallaxObjects = new List<Transform>();
        private List<Vector3> _startPos = new List<Vector3>();
        private List<Vector3> _spriteSize = new List<Vector3>();
        private List<Vector3> _spriteOffset = new List<Vector3>();
        private Transform _mainCameraTransform;

        void Start()
        {
            for (int i = 0; i < images.Count; ++i)
            {
                var temp = new GameObject("ParallaxImage" + i);
                temp.transform.parent = transform;
                _parallaxObjects.Add(temp.transform);
                _startPos.Add(temp.transform.position);

                var tempSprite = temp.AddComponent<SpriteRenderer>();
                tempSprite.sprite = images[i].image;
                tempSprite.sortingOrder = i;
                _spriteSize.Add(tempSprite.size);
                _spriteOffset.Add(images[i].offset);

                var tempClone = Instantiate(temp, temp.transform.position + Vector3.right * tempSprite.size.x, Quaternion.identity, transform);
                _parallaxObjects.Add(tempClone.transform);
                _startPos.Add(tempClone.transform.position);
            }

            _mainCameraTransform = CameraManager.Instance.GetMainCamera().transform;
        }

        void Update()
        {
            for (int i = 0; i < _parallaxObjects.Count; i += 2)
            {
                var clampedIndex = (int)(i * 0.5f);
                var effect = images[clampedIndex].effect;
                var offset = images[clampedIndex].offset;
                var width = _mainCameraTransform.position.x * effect;
                var height = _mainCameraTransform.position.y * effect;
                var delta = Vector3.up * height + Vector3.right * width;
                var temp = _mainCameraTransform.position * (1f - effect);

                _parallaxObjects[i].position = _startPos[i] + delta + offset;
                _parallaxObjects[i + 1].position = _startPos[i + 1] + delta + offset;

                if (temp.x > _startPos[i].x + _spriteSize[clampedIndex].x)
                    _startPos[i] += Vector3.right * _spriteSize[clampedIndex].x * 2;

                if (temp.x > _startPos[i + 1].x + _spriteSize[clampedIndex].x)
                    _startPos[i + 1] += Vector3.right * _spriteSize[clampedIndex].x * 2;
            }
        }
    }
}
