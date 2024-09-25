using UnityEngine;
using UnityEngine.UI;

public class SkillIconLoader : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void LoadIcon(Sprite icon)
    {
        image.sprite = icon;
    }
}
