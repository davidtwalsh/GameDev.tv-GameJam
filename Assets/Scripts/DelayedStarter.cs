using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedStarter : MonoBehaviour
{
    [SerializeField]
    private List<DelayedStartInfo> delayedStarts = new List<DelayedStartInfo>();

    private void Start()
    {
        foreach (DelayedStartInfo delayedStart in delayedStarts)
        {
            StartCoroutine(DelayStartCoroutine(delayedStart));
        }
    }

    IEnumerator DelayStartCoroutine(DelayedStartInfo delayedStartInfo)
    {

        yield return new WaitForSeconds(delayedStartInfo.delayTime);

        delayedStartInfo.obj.SetActive(true);
    }
}

[System.Serializable]
public class DelayedStartInfo
{
    public GameObject obj;
    public float delayTime;
}
