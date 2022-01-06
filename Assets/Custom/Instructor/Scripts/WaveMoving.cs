using UnityEngine;

namespace Custom.Instructor
{
    public class WaveMoving : MonoBehaviour
    {
        [SerializeField] private float speed = 15;
        [SerializeField] private float frequency = 12;
        [SerializeField] private float amplitude = 6;

        //moving the object with the defined speed
        private void Update()
        {
            var horizontalFactor = Mathf.Sin(Time.time * frequency) * amplitude;
            var movement = (Vector3.up * speed + Vector3.right * horizontalFactor) * Time.deltaTime;
            transform.Translate(movement);
        }
    }
}