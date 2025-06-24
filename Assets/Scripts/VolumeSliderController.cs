using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        if (volumeSlider == null)
            volumeSlider = GetComponent<Slider>();

        // �����̴� �ʱⰪ ����
        volumeSlider.value = SoundManager.Instance.GetVolume();

        // �����̴� �� ���� �̺�Ʈ ���
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }
}