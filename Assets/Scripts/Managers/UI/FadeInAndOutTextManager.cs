using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(TMP_Text))]
    public class FadeInAndOutTextManager : MonoBehaviour
    {
        TMP_Text text;

        void Awake()
        {
            text = GetComponent<TMP_Text>();
        }
        void OnBecameVisible()
        {
            StartCoroutine(FadeInAndOut());
        }

        

        

        IEnumerator FadeInAndOut()
        {
            Color originalColor = text.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = text.color;
            while (text.color.a >= 0f)
            {
                temp_a -= 0.007f;
                temp.a = temp_a;
                text.color = temp;
                yield return null;
            }
            temp_a = 0f;
            temp.a = temp_a;
           text.color = temp;
            yield return new WaitForSeconds(1f);
            while (text.color.a <= 1f)
            {
                temp_a += 0.007f;
                temp.a = temp_a;
               text.color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            text.color = temp;
            yield return new WaitForSeconds(1f);
            StartCoroutine(FadeInAndOut());


        }
    }

}