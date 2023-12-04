using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/**
*
* 
*/
class ButtonWithValue : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public string value;
	public GameObject ToggleGroup;
	// public Image backgroundImage;
	// public Sprite selectedImage;
	// public Sprite deselectedImage;
	// private ManagerContainer managerContainer;
	// private SoundManager soundManager;

	// public ButtonWithValue(ManagerContainer _managerContainer){
		// managerContainer = _managerContainer;
		// soundManager = managerContainer.soundManager;
	// }

	public void OnSelect(BaseEventData e){
		// backgroundImage = selectedImage;
		// soundManager.Play();
	}

	public void OnDeselect(BaseEventData e){
		// backgroundImage = deselectedImage;
	}

	public void OnPointerEnter(PointerEventData e){
		// backgroundImage = selectedImage;
		// soundManager.Play();
	}

	public void OnPointerExit(PointerEventData e){
		// backgroundImage = deselectedImage;
	}

	public void OnPointerClick(PointerEventData e){
		// Toggleでイベント吸っちゃってToggleGroup側で検知できなかったので力技
		// TODO: ToggleGroup用のスクリプト作ろうね
		if(ToggleGroup.GetComponent<LocaleSelector>()){
			ToggleGroup.GetComponent<LocaleSelector>().OnPointerClick(e);
		}else{
			ToggleGroup.GetComponent<ModeSelector>().OnPointerClick(e);
		}
		
	}
}