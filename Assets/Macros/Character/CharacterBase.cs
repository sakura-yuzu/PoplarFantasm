using Cysharp.Threading.Tasks;
using ScenarioFlow;
using ScenarioFlow.ExcelFlow;
using ScenarioFlow.TaskFlow;
using System;
using System.Threading;
using UnityEngine;
class CharacterBase : FieldObjectBase
{
    public GameObject characterSpeakArea;

    [SerializeField]
    private Communicator.Settings communicatorSettings;

    [SerializeField]
    private KeyProgressor.Settings keyProgressorSettings;

    [SerializeField]
    private ButtonProgressor.Settings buttonProgressorSettings;
    [SerializeField]
    private BackgroundAnimator.Settings backgroundAnimatorSettings;
    //読み込むシナリオのソースファイル
    [SerializeField]
    private ExcelAsset excelAsset;

    private IScenarioBook scenarioBook;

    private ScenarioBookReader scenarioBookReader;
    //Disposable
    private IDisposable[] disposables;
    [SerializeField]
    private ButtonBranchSelector.Settings buttonBranchSelectorSettings;
    private CancellationToken cancellationToken;
    private void Start()
    {
       //CancellationTokenを取得
       cancellationToken = this.GetCancellationTokenOnDestroy();
        //------Progressorの準備
        var keyProgressor = new KeyProgressor(keyProgressorSettings);
        var buttonProgressor = new ButtonProgressor(buttonProgressorSettings);

        INextProgressor nextProgressor = new CompositeAnyNextProgressor(
            new INextProgressor[]
            {
                        keyProgressor,
                        buttonProgressor,
            });
        ICancellationProgressor cancellationProgressor = new CompositeAnyCancellationProgressor(
            new ICancellationProgressor[]
            {
                     keyProgressor,
                     buttonProgressor,
            });

        //------ScenarioBookReaderの準備
        var tokenCodeHolder = new TokenCodeHolder();

        var scenarioTaskExecuter = new ScenarioTaskExecuter(tokenCodeHolder);

        scenarioBookReader = new ScenarioBookReader(
                new ScenarioTaskExecuterTokenCodeDecorator(
                        scenarioTaskExecuter,
                        nextProgressor,
                        tokenCodeHolder));

        //------CancellationToken用のDecoderの準備
        var cancellationTokenDecoder = new CancellationTokenDecoder(tokenCodeHolder);

        var cancellationTokenDecoderTokenCodeDecorator = new CancellationTokenDecoderTokenCodeDecorator(
                cancellationTokenDecoder,
                new CancellationProgressorTokenCodeDecorator(cancellationProgressor, tokenCodeHolder),
                tokenCodeHolder
         );

        var resourcesAssetLoader = new AddressablesAssetLoader();
        var channelMediator = new ChannelMediator();

        //Disposableの登録
        disposables = new IDisposable[]
        {
           cancellationTokenDecoder,
           cancellationTokenDecoderTokenCodeDecorator,
        };

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
                  //  new ActorAnimator(),
                  //  new ActorConfigurator(),
                  //  new DelayGenerator(),
                   new ScenarioTaskDealer(scenarioTaskExecuter, cancellationTokenDecoder),
                                     new BranchRecorder(channelMediator, new ButtonBranchSelector(buttonBranchSelectorSettings)),
                                     new BranchMaker(channelMediator, scenarioBookReader),
                        }
                 );
        IScenarioPublisher<ExcelAsset> excelScenarioPublisher =
            new ExcelScenarioPublisher(scenarioMethodSearcher);
        //ExcelAsset -> ScenarioBook
        scenarioBook = excelScenarioPublisher.Publish(excelAsset);
    }

    private async UniTask ReadScenarioBook()
    {
         await scenarioBookReader.ReadScenarioBookAsync(scenarioBook, cancellationToken);
    }

    // 親クラスから呼ばれるコールバックメソッド (接触 & ボタン押したときに実行)
    protected override async void OnAction()
    {
        // 会話をwindowのtextフィールドに表示
        // ScenarioMethodをすべて実行
        characterSpeakArea.GetComponent<Canvas>().enabled = true;
        await ReadScenarioBook();
    }
}