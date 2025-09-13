using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] Image image;
    void Start()
    {
        image.fillAmount = 1;
    }
    public void ChangeSlider(float value)
    {
        image.fillAmount = value;
    }
}
