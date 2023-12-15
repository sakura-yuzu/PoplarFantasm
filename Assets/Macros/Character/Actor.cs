using UnityEngine;

public class Actor
{
    public string Name { get; }
    public GameObject Object { get; }
    public Transform Transform { get; }
    public SpriteRenderer Renderer { get; }

    public Actor(string actorName)
    {
        this.Name = actorName;
        //オブジェクト生成
        var actorObject = new GameObject(actorName);
        this.Object = actorObject;
        this.Transform = actorObject.transform;
        this.Renderer = actorObject.AddComponent<SpriteRenderer>();
    }
}