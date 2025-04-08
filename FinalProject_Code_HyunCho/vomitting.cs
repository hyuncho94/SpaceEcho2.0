using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MText
{
    public class vomitting : MonoBehaviour
    {
        public int isVomit;
        public Modular3DText modular3DText;
        public Transform spawnPoint;
        public GameObject letterPrefab;

        public void spawnLetter(string character)
        {
            if (letterPrefab == null || spawnPoint == null) return;

            GameObject go = Instantiate(letterPrefab, spawnPoint.position, spawnPoint.rotation);
            Vomit letter = go.GetComponent<Vomit>();
            if (letter != null)
            {
                letter.newTypeOneChar(character);
            }
        }

        public void VomitText(string text)
        {
            StartCoroutine(VomitRoutine(text));
        }

        private IEnumerator VomitRoutine(string text)
        {
            isVomit = 100;

            foreach (char c in text)
            {
                spawnLetter(c.ToString());
                yield return new WaitForSeconds(0.1f);
            }

            isVomit = 0;
        }
    }
}
