using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float floatSpeed = 50f;
    public float lifeTime = 0.5f;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetText(int damage)
    {
        damageText.text = damage.ToString();
    }

    void Update()
    {
        rect.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}