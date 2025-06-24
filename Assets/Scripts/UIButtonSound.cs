using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound; // 버튼마다 다른 효과음 가능
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (clickSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(clickSound);
        }
    }
}