using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarStartAtTop : MonoBehaviour
{
   Scrollbar bar;

    public IEnumerator Start()
    {
        yield return null; // Waiting just one frame is probably good enough, yield return null does that
        bar = GetComponentInChildren<Scrollbar>();
        bar.value = 1;

}
}
