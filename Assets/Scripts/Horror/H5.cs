using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H5 : MonoBehaviour
    {
        public Transform personLong;
        public Transform personLongStartPos;
        public Animator personLongAnim;
        public GameObject h5;

        bool triggered = false;

        void Awake()
        {
            personLong.position = personLongStartPos.position - Vector3.down * 100;
        }

        void OnEnable()
        {
            triggered = false;
            personLongAnim.SetTrigger("Show");
            personLong.position = personLongStartPos.position;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!triggered && other.CompareTag("Player"))
            {
                triggered = true;
                StartCoroutine(Hide());
            }
        }

        IEnumerator Hide()
        {
            personLongAnim.SetTrigger("Hide");
            yield return new WaitForSeconds(1f);
            personLong.position = personLongStartPos.position - Vector3.down * 100;
            h5.SetActive(false);
        }
    }
}
