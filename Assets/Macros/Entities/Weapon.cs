using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class Weapon : ScriptableObject
{
    public int id;                                      //ID
    public string weaponName;
    public string description;
    public int target;                                  // 効果対象　自分、仲間一人、仲間全体、敵一人、敵全体
    public bool has;                                    // 取得済み
    public bool qualified;                              // 使える
    public int weaponType;                              // このスキルを利用できる武器　あとでenum書く
    public int attributeType;                           // 属性
    public Sprite image;
    public int attackPower;

    public enum WeaponEnum
    {
        Rod,                                             // 杖
        Sword,                                           // 剣
        Bow,                                             // 弓
        Spear,                                           // 槍
    }

    public enum AttributeType
    {
        None,                                            // なし
        Fire,                                            // 炎
        Thunder,                                         // 雷
        Ice,                                             // 氷
    }
}