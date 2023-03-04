using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RxceGame
{
    public class ResultContainer : SingletonComponent<ResultContainer>
    {
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private GameObject positionObject;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI distance;
        [SerializeField] private TextMeshProUGUI position;

        [Header("HP")]
        [SerializeField] private Image hpBar;
        [SerializeField] private Color baseHPColor;
        [SerializeField] private Color damagedHPColor;
        [Range(0f, 1f)][SerializeField] private float damageLevel;

        private float _startHP = 100f;
        private bool _damaged;
        private bool _showPosition;
        private float _timer = 0f;
        private bool _recordTime;
        private bool _showingResult;
        void Start()
        {
            _recordTime = true;
            hpBar.color = baseHPColor;
        }

        void Update()
        {
            if (_recordTime)
                _timer += Time.unscaledDeltaTime;
        }

        public void ShowResults(int playerPos, float playerDis)
        {
            if (_showingResult)
                return;

            _showingResult = true;
            _recordTime = false;

            resultPanel.SetActive(true);

            time.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(_timer / 60f), Mathf.FloorToInt(_timer % 60));
            distance.text = string.Format("{0:00}m", playerDis);

            if (playerPos > 0)
            {
                positionObject.SetActive(true);
                position.text = playerPos.ToString();
            }
        }

        public void FillHP(float hp)
        {
            var fill = hp / _startHP;
            if (fill < damageLevel && !_damaged)
            {
                _damaged = true;
                hpBar.color = damagedHPColor;
                //Object.FindObjectOfType<PlayerCarAuthoring>()?.gameObject?.GetComponent<Renderer>()?.material?.SetInt("Dirt", 1);
            }
            hpBar.fillAmount = fill;
        }

        public bool IsDamaged() => _damaged;

        public void SetStartHP(float hp) => _startHP = hp;

        public void RestartGame() => SceneLoader.Instance.LoadGame();

        public void LoadGarage() => SceneLoader.Instance.LoadGarage();
    }
}
