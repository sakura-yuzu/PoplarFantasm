using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BranchButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI choiceText;

    [SerializeField]
    private Image choiceImage;

    [SerializeField]
    private Button choiceButton;

    public TextMeshProUGUI ChoiceText => choiceText;
    public Image ChoiceImage => choiceImage;
    public Button ChoiceButton => choiceButton;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        //分岐ボタン透明化
        _ = ChoiceImage.DOFade(0.0f, 0.0f);
        //分岐テキスト透明化
        _ = ChoiceText.DOFade(0.0f, 0.0f);
        //分岐ボタン無効化
        ChoiceButton.interactable = false;
        //分岐ボタンオブジェクト非アクティブ
        gameObject.SetActive(false);
    }
}