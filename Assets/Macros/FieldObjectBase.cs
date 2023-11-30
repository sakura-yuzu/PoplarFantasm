using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 * フィールドオブジェクトの基本処理
 */
public abstract class FieldObjectBase : MonoBehaviour
{
    // 接触判定
    private bool isContacted = false;

    private void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag.Equals("Player") == true){
            isContacted = true;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if(collider.gameObject.tag.Equals("Player")){
            isContacted = false;
        }
    }
    private void Update() {
        if (isContacted && Input.GetKeyDown(KeyCode.Return)) {
            // Debug.Log("目印GetKeyDown");
            OnAction();
        }
    }

    protected abstract void OnAction();
}