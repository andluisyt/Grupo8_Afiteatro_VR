using UnityEngine;
using UnityEngine.UI;

public class EraseButtonColor : MonoBehaviour
{
    public Image buttonImage;

    private readonly Color32 colorOn  = new Color32(255, 255, 255, 255);
    private readonly Color32 colorOff = new Color32(180, 180, 180, 255);

    private bool activo = false;

    void Start()
    {
        buttonImage.color = colorOff;
    }

    public void OnPress()
    {
        if (activo)
        {
            activo = false;
            buttonImage.color = colorOff;
        }
        else
        {
            activo = true;
            buttonImage.color = colorOn;
        }
    }
}


