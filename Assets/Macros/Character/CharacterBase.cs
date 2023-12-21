using Cysharp.Threading.Tasks;
using ScenarioFlow;
using ScenarioFlow.ExcelFlow;
using ScenarioFlow.TaskFlow;
using System;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
class CharacterBase : FieldObjectBase
{
    public GameObject characterSpeakArea;

    [SerializeField]
    private Communicator.Settings communicatorSettings;

    [SerializeField]
    private MissionDatabase missionDatabase;
    private MissionManager missionManager;
    //読み込むシナリオのソースファイル
    [SerializeField]
    private ExcelAsset excelAsset;

    private IScenarioBook scenarioBook;

    private ScenarioBookReader scenarioBookReader;
    //Disposable
    private IDisposable[] disposables;
    [SerializeField]
    private ButtonBranchSelector.Settings buttonBranchSelectorSettings;
    [SerializeField]
    private WindowCloser.Settings windowCloserSettings;
    private CancellationToken cancellationToken;
    private void Start()
    {
        //CancellationTokenを取得
        cancellationToken = this.GetCancellationTokenOnDestroy();
        //------Progressorの準備

        KeyProgressor.Settings enterKeyProgressorSettings = new KeyProgressor.Settings();
        enterKeyProgressorSettings.KeyCodes = new KeyCode[]{KeyCode.Return};

        KeyProgressor.Settings escapeKeyProgressorSettings = new KeyProgressor.Settings();
        escapeKeyProgressorSettings.KeyCodes = new KeyCode[]{KeyCode.Escape};

        var enterKeyProgressor = new KeyProgressor(enterKeyProgressorSettings);
        var escapeKeyProgressor = new KeyProgressor(escapeKeyProgressorSettings);

        INextProgressor nextProgressor = new CompositeAnyNextProgressor(
            new INextProgressor[]
            {
                        enterKeyProgressor,
            });
        ICancellationProgressor cancellationProgressor = new CompositeAnyCancellationProgressor(
            new ICancellationProgressor[]
            {
                     escapeKeyProgressor,
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

        missionManager = new MissionManager(missionDatabase);

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
                                     new MissionReceiver(missionManager),
                                     new DelayGenerator(),
                                     new ScenarioTaskDealer(scenarioTaskExecuter, cancellationTokenDecoder),
                                     new BranchRecorder(channelMediator, new ButtonBranchSelector(buttonBranchSelectorSettings)),
                                     new BranchMaker(channelMediator, scenarioBookReader),
                                     new WindowCloser(windowCloserSettings)
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
        if (characterSpeakArea.GetComponent<Canvas>().enabled == false)
        {
            characterSpeakArea.GetComponent<Canvas>().enabled = true;
            await ReadScenarioBook();
        }
    }

    [ContextMenu("AttachObjects")]
    private void AttachObjects(){
        characterSpeakArea = GameObject.Find("CharacterSpeakArea");
        communicatorSettings.characterNameArea = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        communicatorSettings.characterSpeakArea = GameObject.Find("SpeakArea").GetComponent<TextMeshProUGUI>();
        windowCloserSettings.characterSpeakArea = GameObject.Find("CharacterSpeakArea");
        buttonBranchSelectorSettings.BranchButtons = new BranchButton[]{
            GameObject.Find("BranchButton").GetComponent<BranchButton>(),
            GameObject.Find("BranchButton (1)").GetComponent<BranchButton>(),
        };
    }
}