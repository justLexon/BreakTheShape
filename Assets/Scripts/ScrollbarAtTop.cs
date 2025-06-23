using UnityEngine;
using UnityEngine.UI;

public class ScrollbarAtTop : MonoBehaviour
{
    void Start()
    {
        Scrollbar sb = GetComponent<Scrollbar>();
        if (sb != null)
        {
            sb.value = 1f; // Scroll to top
        }
    }
}
