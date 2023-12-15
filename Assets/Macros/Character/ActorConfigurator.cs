using ScenarioFlow;
using UnityEngine;

[ScenarioMethod("actor")]
public class ActorConfigurator : IReflectable
{
    [ScenarioMethod("spr", "キャラクターのスプライトを設定\nSet the actor's sprite.")]
    public void SetSprite(Actor actor, Sprite sprite)
    {
        actor.Renderer.sprite = sprite;
    }

    [ScenarioMethod("pos", "キャラクターのポジションを設定\nSet the actor's position.")]
    public void SetPosition(Actor actor, Vector3 position)
    {
        actor.Transform.position = position;
    }

    [ScenarioMethod("alpha", "キャラクターの透明度を設定\nSet the actor's alpha.")]
    public void SetAlpha(Actor actor, float alpha)
    {
        var baseColor = actor.Renderer.color;
        actor.Renderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
    }

    [ScenarioMethod("scale", "キャラクターの大きさを設定\nSet the actor's scale.")]
    public void SetScale(Actor actor, float scale)
    {
        actor.Transform.localScale = Vector3.one * scale;
    }

    [ScenarioMethod("rotate", "キャラクターの角度を設定\nSet the actor's rotate.")]
    public void SetRotation(Actor actor, float rotate)
    {
        actor.Transform.rotation = Quaternion.Euler(0, 0, rotate);
    }

    [ScenarioMethod("layer", "キャラクターのOrder in Layerを設定\nSet the actor's Order in Layer.")]
    public void SetOrderInLayer(Actor actor, int layer)
    {
        actor.Renderer.sortingOrder = layer;
    }
}