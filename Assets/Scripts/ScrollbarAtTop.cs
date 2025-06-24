using UnityEngine;
using UnityEngine.UI;

public class ScrollbarAtTop : MonoBehaviour
{
    public GameObject scrollbar; // Drag your VerticalScrollbar GameObject here

    void Start()
    {
        Scrollbar sb = scrollbar.GetComponent<Scrollbar>();
        if (sb != null)
        {
            sb.value = 1.1f; // Scroll to top
        }
    }
}
