using ScenarioFlow;
using System;
using System.Collections.Generic;
using UnityEngine;

[ScenarioMethod("actor")]
public class ActorProvider : IReflectable
{
    private readonly GameObject actorParent = new GameObject("ActorParent");

    private readonly Dictionary<string, Actor> actorDictionary = new Dictionary<string, Actor>();

    [ScenarioMethod("add", "キャラクターを追加する\nAdd the actor.")]
    public void AddActor(string actorName)
    {
        //すでに追加済みでないかチェック
        if (actorDictionary.ContainsKey(actorName))
        {
            throw new ArgumentException($"The actor '{actorName}' exists already.");
        }
        //オブジェクト生成
        var actor = new Actor(actorName);
        //透明にする
        actor.Renderer.color -= Color.black;
        //親オブジェクトを設定
        actor.Renderer.transform.SetParent(actorParent.transform);
        //登録
        actorDictionary.Add(actorName, actor);
    }

    [ScenarioMethod("remove", "キャラクターを削除する\nRemove the actor.")]
    public void RemoveActor(string actorName)
    {
        //存在するキャラクターかチェック
        if (!actorDictionary.ContainsKey(actorName))
        {
            throw new ArgumentException($"The actor '{actorName}' does not exist.");
        }
        //オブジェクト削除
        GameObject.Destroy(actorDictionary[actorName].Object);
        //抹消
        actorDictionary.Remove(actorName);
    }

    [ScenarioMethod("remove.all", "キャラクターをすべて削除する\nRemove all the actors.")]
    public void RemoveAllActors()
    {
        //オブジェクト削除
        foreach(var actor in actorDictionary.Values)
        {
            GameObject.Destroy(actor.Object);
        }
        //全て抹消
        actorDictionary.Clear();
    }

    [Decoder]
    public Actor GetActor(string actorName)
    {
        //存在するキャラクターかチェック
        if (!actorDictionary.ContainsKey(actorName))
        {
            throw new ArgumentException($"The actor '{actorName}' does not exist.");
        }

        return actorDictionary[actorName];
    }
}