using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class BoneVisual : MonoBehaviour
{

    public int currentBones;

    private int totalBones;
    public int TotalBones
    {
        get { return totalBones; }

        set
        {
            totalBones = value;
            ProgressText.text = string.Format("{0}", TotalBones.ToString());
        }
    }

    public TMP_Text ProgressText;

    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            TotalBones = currentBones;
        }
    }

}
