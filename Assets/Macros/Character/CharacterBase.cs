using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;

using ScenarioFlow;
using ScenarioFlow.ExcelFlow;
class CharacterBase : FieldObjectBase
{
	// 立ち絵　無くてもいい
	public GameObject characterSpeakArea;

	void Awake(){
		Prepare();
	}

	async void Prepare(){
		// characterSpeakArea = await Addressables.LoadAssetAsync<GameObject>("CharacterSpeakArea").Task;
	}

   //読み込むシナリオのソースファイル
   [SerializeField]
   private ExcelAsset excelAsset;

   private IScenarioBook scenarioBook;

   private void Start()
   {
        // Debug.Log("目印Start");
       //ScenarioMethodをExcelScenarioPublisherに提供する
       IScenarioMethodSearcher scenarioMethodSearcher =
           new ScenarioMethodSearcher(
               new IReflectable[]
               {
                   //使用したいScenarioMethodとDecoderを宣言している
                   //クラスのインスタンス
                   new Communicator(characterSpeakArea),
                   new PrimitiveDecoder(),
               });
       //ExcelAssetをScenarioBookに変換する
       IScenarioPublisher<ExcelAsset> excelScenarioPublisher = 
           new ExcelScenarioPublisher(scenarioMethodSearcher);
       //ExcelAsset -> ScenarioBook
        scenarioBook = excelScenarioPublisher.Publish(excelAsset);
        // Debug.Log("目印StartEnd");
   }

   //ScenarioBook中のScenarioMethodをすべて実行
   private void ReadScenarioBook()
   {
        // Debug.Log("目印ReadScenarioBook");
       foreach(IScenarioPage scenarioPage in scenarioBook.ReadAll())
       {
           foreach(IScenarioSentence scenarioSentence in scenarioPage.ReadAll())
           {
               scenarioSentence.OnRead();
           }
       }
   }

       // 親クラスから呼ばれるコールバックメソッド (接触 & ボタン押したときに実行)
    protected override void OnAction() {
        // Debug.Log("目印OnAction");
        // 会話をwindowのtextフィールドに表示
        // ScenarioMethodをすべて実行
        ReadScenarioBook();
    }
}