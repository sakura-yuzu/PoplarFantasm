using UnityEngine;
using ScenarioFlow;
using ScenarioFlow.ExcelFlow;
using UnityEngine.AddressableAssets;
class CharacterBase : MonoBehaviour
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

   private void Start()
   {
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
       IScenarioBook scenarioBook = excelScenarioPublisher.Publish(excelAsset);
       //ScenarioMethodをすべて実行
       ReadScenarioBook(scenarioBook);
   }

   //ScenarioBook中のScenarioMethodをすべて実行
   private void ReadScenarioBook(IScenarioBook scenarioBook)
   {
       foreach(IScenarioPage scenarioPage in scenarioBook.ReadAll())
       {
           foreach(IScenarioSentence scenarioSentence in scenarioPage.ReadAll())
           {
               scenarioSentence.OnRead();
           }
       }
   }
}