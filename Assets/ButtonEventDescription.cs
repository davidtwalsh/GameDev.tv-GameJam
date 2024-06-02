using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEventDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    TextMeshProUGUI descriptionTextObject;

    [SerializeField]
    private string descriptionText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionTextObject.text = descriptionText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionTextObject.text = "";
    }
}
