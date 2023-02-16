using UnityEngine;
using UnityEngine.UI;

namespace RxceGame
{
    public class LoadingProgressBar : MonoBehaviour
    {
        private Image _image;

        void Awake()
        {
            _image = GetComponent<Image>();
        }

        void LateUpdate()
        {
            _image.fillAmount = SceneLoader.Instance.GetLoadingProgress();
        }
    }

}