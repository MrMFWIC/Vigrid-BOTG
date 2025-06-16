using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Leader", menuName = "Leaders")]
public class LeaderSO : ScriptableObject
{
    public string leaderID;

    [Header("Info")]
    public string leaderName;
    public string leaderLore;
    public string leaderEffect;

    public Sprite leaderImage;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(leaderID))
        {
            leaderID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
#endif


}
