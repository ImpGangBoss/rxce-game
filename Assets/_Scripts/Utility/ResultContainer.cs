using UnityEngine;
using TMPro;

namespace RxceGame
{
    public class ResultContainer : SingletonComponent<ResultContainer>
    {
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI distance;
        [SerializeField] private TextMeshProUGUI position;

        private bool _showPosition;

        public void ShowResults()
        {
            resultPanel.SetActive(true);
        }

        public void RestartGame() => SceneLoader.Instance.LoadGame();
        public void LoadGarage() => SceneLoader.Instance.LoadGarage();
    }
}
