using UnityEngine;
[RequireComponent(typeof(UnityEngine.UI.Button))]

public class ActiveButton : MonoBehaviour
{
    private  UnityEngine.UI.Button button;
    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        button.Select();
    }
}
