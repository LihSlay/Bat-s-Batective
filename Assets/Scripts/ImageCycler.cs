using UnityEngine;
using UnityEngine.UI;

public class ImageCycler : MonoBehaviour
{
    public Image targetImage;
    public Sprite[] sprites;

    void Start()
    {
        if (sprites.Length > 0)
            targetImage.sprite = sprites[0];
    }

    public void ShowImage(int index)
    {
        if (index >= 0 && index < sprites.Length)
            targetImage.sprite = sprites[index];
    }
}
