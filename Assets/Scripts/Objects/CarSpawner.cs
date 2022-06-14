using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class CarSpawner : MonoBehaviour
    {
        public Transform[] cars;
        public float minTime;
        public float maxTime;
        public float metersPerSecond;

        public Transform startPosL;
        public Transform startPosR;
        public Transform center;

        private IEnumerator Start()
        {
            while (isActiveAndEnabled)
            {
                //wait
                yield return new WaitForSeconds(Random.Range(minTime, maxTime));

                //get car
                bool left = Random.value > 0.5f;
                Transform startPos = left ? startPosL : startPosR;
                Transform car = cars[Random.Range(0, cars.Length)];

                car.position = startPos.position;
                car.rotation = startPos.rotation;
                car.gameObject.SetActive(true);

                //move car to center
                if (left)
                    while (car.position.x - 4f < center.position.x)
                    {
                        car.position += Vector3.right * Time.deltaTime * metersPerSecond;
                        yield return null;
                    }
                else
                    while (car.position.x + 4f > center.position.x)
                    {
                        car.position -= Vector3.right * Time.deltaTime * metersPerSecond;
                        yield return null;
                    }

                //sometimes randomly stop in the middle
                if (Random.value > 0.2f)
                    yield return new WaitForSeconds(Random.Range(5f, 20f));

                //move car to end
                if (left)
                    while (car.position.x - 4f < startPosR.position.x)
                    {
                        car.position += Vector3.right * Time.deltaTime * metersPerSecond;
                        yield return null;
                    }
                else
                    while (car.position.x + 4f > startPosL.position.x)
                    {
                        car.position -= Vector3.right * Time.deltaTime * metersPerSecond;
                        yield return null;
                    }

                //hide car
                car.gameObject.SetActive(false);
            }
        }
    }
}
