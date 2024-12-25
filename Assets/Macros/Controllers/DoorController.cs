using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class DoorController : FieldObjectBase
{
    public Animator animator;
    // 親クラスから呼ばれるコールバックメソッド (接触 & ボタン押したときに実行)
    protected override void OnAction() {
      Debug.Log("ここ来てない？");
			animator.SetBool("maindoor_open", !animator.GetBool("maindoor_open"));
			animator.SetBool("individualdoor_open", !animator.GetBool("individualdoor_open"));
      return;
    }
}