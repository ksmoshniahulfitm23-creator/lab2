using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI attemptsText;

    private static int attempts = 0;

    void Start()
    {
        attempts++;
        attemptsText.text = "ATTEMPT " + attempts;
    }
}