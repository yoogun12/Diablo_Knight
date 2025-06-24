using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        if (volumeSlider == null)
            volumeSlider = GetComponent<Slider>();

        // 슬라이더 초기값 설정
        volumeSlider.value = SoundManager.Instance.GetVolume();

        // 슬라이더 값 변경 이벤트 등록
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }
}