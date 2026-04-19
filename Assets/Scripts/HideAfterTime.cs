using UnityEngine;

public class HideAfterTime : MonoBehaviour
{
    public float delay = 2f;

    void Start()
    {
        Invoke("Hide", delay);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}