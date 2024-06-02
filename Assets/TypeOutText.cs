using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TypeOutText : MonoBehaviour
{
    [SerializeField]
    [TextArea(0, 10)]
    private string text;
    [SerializeField]
    private float timeBetweenLetters;

    [SerializeField]
    private UnityEvent finishedTyping;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    void Start()
    {
        TypeText();
    }


    public void TypeText()
    {
        StartCoroutine(TypeTextRoutine());
    }

    private IEnumerator TypeTextRoutine()
    {
        string message = "";
        for (int i = 0; i < text.Length; i++)
        {
            message += text[i];
            textMeshProUGUI.text = message;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        finishedTyping.Invoke();
    }
}
