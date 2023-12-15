using Cysharp.Threading.Tasks;
using ScenarioFlow;
using ScenarioFlow.ExcelFlow;
using ScenarioFlow.TaskFlow;
using System;
using UnityEngine;
class CharacterBase : FieldObjectBase
{
	// 立ち絵　無くてもいい
	public GameObject characterSpeakArea;

   [SerializeField]
   private Communicator.Settings communicatorSettings;

   [SerializeField]
   private KeyProgressor.Settings keyProgressorSettings;

//    [SerializeField]
//    private ButtonProgressor.Settings buttonProgressorSettings;
//    [SerializeField]
//    private BackgroundAnimator.Settings backgroundAnimatorSettings;
   //読み込むシナリオのソースファイル
   [SerializeField]
   private ExcelAsset excelAsset;

   private IScenarioBook scenarioBook;
   //Disposable
   private IDisposable[] disposables;
   [SerializeField]
   private ButtonBranchSelector.Settings buttonBranchSelectorSettings;

   private void Start()
   {
       //CancellationTokenを取得
       var cancellationToken = this.GetCancellationTokenOnDestroy();

       //------Progressorの準備
       var keyProgressor = new KeyProgressor(keyProgressorSettings);
    //    var buttonProgressor = new ButtonProgressor(buttonProgressorSettings);
       //使用するNextProgressorをここに
       INextProgressor nextProgressor = new CompositeAnyNextProgressor(
           new INextProgressor[]
           {
               keyProgressor,
            //    buttonProgressor,
           });
       //使用するCancellationProgressorをここに
       ICancellationProgressor cancellationProgressor = new CompositeAnyCancellationProgressor(
           new ICancellationProgressor[]
           {
            //    keyProgressor,
            //    buttonProgressor,
           });
       //------

       //------ScenarioBookReaderの準備
       var tokenCodeHolder = new TokenCodeHolder();

       var scenarioTaskExecuter = new ScenarioTaskExecuter(tokenCodeHolder);

       var scenarioBookReader = new ScenarioBookReader(
           new ScenarioTaskExecuterTokenCodeDecorator(
               scenarioTaskExecuter,
               nextProgressor,
               tokenCodeHolder));
       //------
       //------CancellationToken用のDecoderの準備
       var cancellationTokenDecoder = new CancellationTokenDecoder(tokenCodeHolder);

       var cancellationTokenDecoderTokenCodeDecorator = new CancellationTokenDecoderTokenCodeDecorator(
           cancellationTokenDecoder,
           new CancellationProgressorTokenCodeDecorator(cancellationProgressor, tokenCodeHolder),
           tokenCodeHolder);
       //-----
        // Debug.Log("目印Start");
       var resourcesAssetLoader = new AddressablesAssetLoader();
       var channelMediator = new ChannelMediator();
       //ScenarioMethodをExcelScenarioPublisherに提供する
       IScenarioMethodSearcher scenarioMethodSearcher =
           new ScenarioMethodSearcher(
               new IReflectable[]
               {
                   //使用したいScenarioMethodとDecoderを宣言している
                   //クラスのインスタンス
                   new Communicator(communicatorSettings),
                   new PrimitiveDecoder(),
                   new AsyncDecoder(cancellationTokenDecoderTokenCodeDecorator),
                   new TaskDecoder(cancellationTokenDecoderTokenCodeDecorator),
                //    new LineWriter(lineWriterSettings),
                //    new BackgroundAnimator(backgroundAnimatorSettings),
                   new ActorAnimator(),
                   new ActorConfigurator(),
                   new DelayGenerator(),
                   new ScenarioTaskDealer(scenarioTaskExecuter, cancellationTokenDecoder),
                   new BranchRecorder(channelMediator, new ButtonBranchSelector(buttonBranchSelectorSettings)),
                   new BranchMaker(channelMediator, scenarioBookReader),
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
        characterSpeakArea.GetComponent<Canvas>().enabled = true;
        ReadScenarioBook();
    }
}