using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Sprite;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class Item : ScriptableObject
{
    public int id;                                      //ID
    public string itemName;
    public string description;
    public int quantity;
    public Sprite image;
    public int buyPrice;
    public int sellPrice;
}