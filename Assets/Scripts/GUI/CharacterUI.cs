using System.Collections;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Sets the images to a certain character for use in the UI
/// </summary>
public class CharacterUI : MonoBehaviour {

    public Image[] images;
    private float pixelSize = 0.04f;

    public void setSprites(Sprite[] sprites)
    {
        //Get height of head in pixels
        int headPixelHeight = (int) sprites[0].rect.height;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = sprites[i];

            //To rescale image if heads are different sizes
            if(i == 0)
                images[i].rectTransform.sizeDelta = new Vector2(images[i].rectTransform.sizeDelta.x, pixelSize * headPixelHeight);
        }
    }
}
