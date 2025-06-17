using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

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
            leaderID = $"{Regex.Replace(leaderName.ToLower(), "[^a-z0-9]", "")}_001";
        }
    }
#endif
}
