using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;
using ScenarioFlow;
using System.Threading;
using UnityEngine;

[ScenarioMethod("actor")]
public class ActorAnimator : IReflectable
{
    //キャラクターの表示/非表示にかける時間
    private float fadeDuration = 0.0f;
    //キャラクターの表示/非表示のEase
    private Ease fadeEase = Ease.Linear;

    //キャラクターのスプライト変更にかける時間
    private float replaceDuration = 0.0f;
    //キャラクターのスプライト変更のEase
    private Ease replaceEase = Ease.Linear;

    //キャラクター移動のEase
    private Ease moveEase = Ease.Linear;
    //キャラクタージャンプのEase
    private Ease jumpEase = Ease.Linear;

    [ScenarioMethod("display", "キャラクターを徐々に表示\nDisplay the actor gradually.")]
    public async UniTask DisplayActorAsync(Actor actor, CancellationToken cancellationToken)
    {
        try
        {
            await actor.Renderer.DOFade(1.0f, fadeDuration).SetEase(fadeEase).ToUniTask(cancellationToken: cancellationToken);
        }
        finally
        {
            var baseColor = actor.Renderer.color;
            actor.Renderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1.0f);
        }
    }

    [ScenarioMethod("hide", "キャラクターを徐々に非表示\nHide the actor gradually.")]
    public async UniTask HideActorAsync(Actor actor, CancellationToken cancellationToken)
    {
        try
        {
            await actor.Renderer.DOFade(0.0f, fadeDuration).SetEase(fadeEase).ToUniTask(cancellationToken: cancellationToken);
        }
        finally
        {
            var baseColor = actor.Renderer.color;
            actor.Renderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.0f);
        }
    }

    [ScenarioMethod("fade.durat", "キャラクターの表示/非表示にかける時間を設定\nSet the duration to display or hide an actor.")]
    public void SetFadeDuration(float duration)
    {
        fadeDuration = duration;
    }

    [ScenarioMethod("fade.ease", "キャラクターの表示/非表示のEaseを設定\nSet the ease to display or hide an actor.")]
    public void SetFadeEase(Ease ease)
    {
        fadeEase = ease;
    }

    [ScenarioMethod("replace", "キャラクターのスプライトを徐々に変更\nChange the actor's sprite gradually.")]
    public async UniTask ReplaceActorAsync(Actor actor, Sprite sprite, CancellationToken cancellationToken)
    {
        //z座標の調整
        var zAdjustment = -0.01f;
        //後ろにオブジェクトを複製
        var backRenderer = GameObject.Instantiate<SpriteRenderer>(actor.Renderer);
        backRenderer.gameObject.transform.position = actor.Transform.position + Vector3.forward * zAdjustment;
        //後ろのオブジェクトにスプライトをセット
        backRenderer.sprite = sprite;
        //キャラクターを追従する
        var chaseDisposable = UniTaskAsyncEnumerable.EveryValueChanged(actor, a => a.Transform.position)
            .Subscribe(position => backRenderer.transform.position = position + Vector3.forward * zAdjustment);
        try
        {
            //キャラクターを徐々に透明に
            await actor.Renderer.DOFade(0.0f, replaceDuration).SetEase(replaceEase).ToUniTask(cancellationToken: cancellationToken);
        }
        finally
        {
            //追従終了
            chaseDisposable.Dispose();
            //キャラクターのスプライトをセット
            actor.Renderer.sprite = sprite;
            //キャラクターを表示
            var baseColor = actor.Renderer.color;
            actor.Renderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1.0f);
            //後ろのオブジェクトを削除
            GameObject.Destroy(backRenderer.gameObject);
        }
    }

    [ScenarioMethod("replace.durat", "キャラクターのスプライト変更にかけるる時間を設定\nSet the duration to replace an actor's sprite.")]
    public void SetReplaceDuration(float duration)
    {
        replaceDuration = duration;
    }

    [ScenarioMethod("replace.ease", "キャラクターのスプライト変更のEaseを設定\nSet the ease to replace an actor's sprite.")]
    public void  SetReplaceEase(Ease ease)
    {
        replaceEase = ease;
    }

    [ScenarioMethod("move.rel", "相対座標にキャラクターを移動\nMove the actor to the relative position")]
    public UniTask MoveActorToRelativePositionAsync(Actor actor, Vector3 distance, float duration, CancellationToken cancellationToken)
    {
        var goalPosition = actor.Transform.position + distance;
        return MoveActorAsync(actor, goalPosition, duration, cancellationToken);
    }

    [ScenarioMethod("move.abs", "絶対座標にキャラクターを移動\nMove the actor to the absolute position.")]
    public UniTask MoveActorToAbsolutePositionAsync(Actor actor, Vector3 goalPosition, float duration, CancellationToken cancellationToken)
    {
        return MoveActorAsync(actor, goalPosition, duration, cancellationToken);
    }

    [ScenarioMethod("move.ease", "キャラクター移動のEaseを設定\nSet the ease to move an actor.")]
    public void SetMoveEase(Ease ease)
    {
        moveEase = ease;
    }

    [ScenarioMethod("jump.rel", "キャラクターを相対座標にジャンプ移動\nMake the actor jump to the relative position.")]
    public UniTask MakeActorJumpToRelativePositionAsync(Actor actor, Vector3 distance, float jumpPower, float duration, CancellationToken cancellationToken)
    {
        var goalPosition = actor.Transform.position + new Vector3(distance.x, distance.y);
        return MakeActorJumpAsync(actor, goalPosition, jumpPower, duration, cancellationToken);
    }

    [ScenarioMethod("jump.abs", "キャラクターを絶対座標にジャンプ移動\nMake the actor jump to the absolute position.")]
    public UniTask MakeActorJumpToAbsolutePositionAsync(Actor actor, Vector3 goalPosition, float jumpPower, float duration, CancellationToken cancellationToken)
    {
        return MakeActorJumpAsync(actor, goalPosition, jumpPower, duration, cancellationToken);
    }

    [ScenarioMethod("jump.ease", "キャラクターのジャンプ移動のEaseを設定\nSet the ease to make an actor jump.")]
    public void SetJumpEase(Ease ease)
    {
        jumpEase = ease;
    }

    //キャラクターを指定の座標へ移動させる
    private async UniTask MoveActorAsync(Actor actor, Vector3 goalPosition, float duration, CancellationToken cancellationToken)
    {
        try
        {
            await actor.Transform.DOMove(goalPosition, duration).SetEase(moveEase).ToUniTask(cancellationToken: cancellationToken);
        }
        finally
        {
            actor.Transform.position = goalPosition;
        }
    }

    //キャラクターを指定の座標にジャンプ移動させる
    private async UniTask MakeActorJumpAsync(Actor actor, Vector3 goalPosition, float jumpPower, float duration, CancellationToken cancellationToken)
    {
        try
        {
            await actor.Transform.DOJump(goalPosition, jumpPower, 1, duration).SetEase(jumpEase).ToUniTask(cancellationToken: cancellationToken);
        }
        finally
        {
            actor.Transform.position = goalPosition;
        }
    }
}