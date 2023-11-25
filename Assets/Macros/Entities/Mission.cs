using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class Mission : ScriptableObject
{
    public int id;                                      //ID
    public string title;
    public string description;
    public int rewardAmount;    // 報酬額
    public Item rewardItem;     // 報酬アイテム
    public int requester;       // 依頼者
    // あとなんか座標とか
    public bool cleared;
    public Status status;

    public enum Status
    {
        Unvisible,  // 不可視
        Visible,    // 発生
        Received,   // 受注
        Cleared,    // クリア済み
    }
}