using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RxceGame
{
    public class CarSelector : MonoBehaviour
    {
        [SerializeField] private List<CarSelectorInfoHolder> cars;
        [SerializeField] private TextMeshProUGUI carName;

        [Header("Move Params")]
        [SerializeField] private TextMeshProUGUI mass;
        [SerializeField] private TextMeshProUGUI acceleration;
        [SerializeField] private TextMeshProUGUI maxSpeed;
        [SerializeField] private TextMeshProUGUI jump;
        [SerializeField] private TextMeshProUGUI rotation;
        [SerializeField] private TextMeshProUGUI brake;

        private int currentCarIndex = 0;

        private void Start()
        {
            SetTextValues(cars[currentCarIndex].data, cars[currentCarIndex].name);
        }

        private void SetTextValues(MoveParamsData data, string name)
        {
            mass.text = data.Mass.ToString();
            acceleration.text = data.Acceleration.ToString();
            maxSpeed.text = data.MaxSpeed.ToString();
            jump.text = data.JumpImpulse.ToString();
            rotation.text = data.RotationSpeed.ToString();
            brake.text = data.BrakeSpeed.ToString();

            carName.text = name;
        }

        public void NextCar()
        {
            cars[currentCarIndex].prefab.SetActive(false);
            currentCarIndex++;
            currentCarIndex = currentCarIndex % cars.Count;

            var currentCar = cars[currentCarIndex];

            currentCar.prefab.SetActive(true);
            SetTextValues(currentCar.data, currentCar.name);
        }

        public void PrevCar()
        {
            cars[currentCarIndex].prefab.SetActive(false);
            currentCarIndex--;

            if (currentCarIndex < 0)
                currentCarIndex = cars.Count - 1;

            var currentCar = cars[currentCarIndex];

            currentCar.prefab.SetActive(true);
            SetTextValues(currentCar.data, currentCar.name);
        }
    }
}
