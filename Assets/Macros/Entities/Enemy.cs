using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Sprite;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class Enemy : ScriptableObject
{
    public int id;                                      //ID
    public string enemyName;
    public Sprite image;
    public int hp;
    public int mp;
    public int attackPower;
    public int defensePower;
    public int exp;
    public List<Item> dropItem;


}