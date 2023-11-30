using ScenarioFlow;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Communicator : IReflectable
{
    private GameObject characterSpeakArea;
    public Communicator(GameObject _characterSpeakArea){
        characterSpeakArea = _characterSpeakArea;
    }

    [ScenarioMethod("talk")]
    public void Talk(string message)
    {
        Debug.Log(characterSpeakArea == null ? "null": "not null");
        var transform = characterSpeakArea.transform.GetChild(0);
        var tmptext = transform.Find("Text (TMP)");
        var cmptext = tmptext.GetComponent<TextMeshProUGUI>();
        cmptext.text = message;
        Debug.Log(message);
    }
    [ScenarioMethod("shout")]
    public void Shout(string message)
    {
        Debug.Log(message);
    }
}