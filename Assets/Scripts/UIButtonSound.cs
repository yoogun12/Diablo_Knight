using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound; // ��ư���� �ٸ� ȿ���� ����
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