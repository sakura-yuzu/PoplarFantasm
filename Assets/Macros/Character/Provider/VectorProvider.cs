using ScenarioFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ScenarioMethod("vec.mem")]
public class VectorProvider : IReflectable
{
    //Vectorの記録
    private readonly Dictionary<string, Func<Vector3>> vectorDictionary = new Dictionary<string, Func<Vector3>>();

    [ScenarioMethod("val", "与えたVector3を記憶\nMemorize the given Vector3.")]
    public void MemorizeVectorFromValue(string vecName, Vector3 vector)
    {
        vectorDictionary[vecName] = () => vector;
    }

    [ScenarioMethod("obj", "GameObjectの座標からVector3を記憶\nMemorize the Vector3 from The position of the GameObject.")]
    public void MemorizeVectorFromObject(string vecName, string objectName)
    {
        //GameObjectを取得
        var vecObj = GameObject.Find(vecName);
        //取得できたかチェック
        if (vecObj == null)
        {
            throw new ArgumentException($"Object '{objectName}' does not exist.");
        }
        //登録
        var resultPosition = vecObj.transform.position;
        vectorDictionary[vecName] = () => resultPosition;
    }

    [ScenarioMethod("bind", "GameObjectの座標への参照を記憶\nMemorize the reference to the position of the GameObject.")]
    public void MemorizeVectorReference(string vecName, string objectName)
    {
        //GameObjectを取得
        var vecObj = GameObject.Find(vecName);
        //取得できたかチェック
        if (vecObj == null)
        {
            throw new ArgumentException($"Object '{objectName}' does not exist.");
        }
        //登録
        vectorDictionary[vecName] = () => vecObj.transform.position;
    }

    [ScenarioMethod("remove", "記憶したVector3を抹消\nRemove the memorized Vector3.")]
    public void RemoveVectorMemory(string vecName)
    {
        //存在するVector3かチェック
        if (!vectorDictionary.ContainsKey(vecName))
        {
            throw new ArgumentException($"Vector memory '{vecName}' does not exit.");
        }
        //抹消
        vectorDictionary.Remove(vecName);
    }

    [ScenarioMethod("clear", "記憶したVector3を全て抹消\nRemove all the memorized Vector3.")]
    public void ClearAllVectorMemories()
    {
        //すべてのVector3を抹消
        vectorDictionary.Clear();
    }

    [Decoder]
    public Vector2 GetVector2(string source)
    {
        return GetVector3(source);
    }

    [Decoder]
    public Vector3 GetVector3(string source)
    {
        //プラス演算子で分割
        return source.Split('+')
            //空白を除く
            .Select(s => s.Trim())
            //Vector3に変換
            .Select(s => ConvertStringWithoutPlusOptIntoVector3(s))
            //和を取る
            .Aggregate((a, b) => a + b);
    }

    //プラス演算子を取り除いた文字列を解析
    private Vector3 ConvertStringWithoutPlusOptIntoVector3(string source)
    {
        //記憶されているVectorかチェック
        if (vectorDictionary.ContainsKey(source))
        {
            return vectorDictionary[source].Invoke();
        }
        //マイナス付きの記憶されたVectorかチェック
        else if (IsMemorizedMinus(source))
        {
            return -vectorDictionary[source.Substring(1)].Invoke();
        }
        else
        {
            return ConvertNumberSetIntoVector3(source);
        }
    }

    //適切な形式の数字の組を解析
    private Vector3 ConvertNumberSetIntoVector3(string source)
    {
        //アンダースコアで分割
        var components = source.Split('_');
        //適切な形式かチェック
        if (components.Length != 2 && components.Length != 3)
        {
            throw new ArgumentException($"Invalid form '{source}'. Pass two or three elements");
        }
        //Vector3を構成する各成分
        //z成分は省略される可能性がある
        var xComponent = 0.0f;
        var yComponent = 0.0f;
        var zComponent = 0.0f;
        //各成分を解析
        foreach(var index in Enumerable.Range(0, components.Length))
        {
            //解析する文字列
            var componentString = components[index];
            //解析後の数字
            var componentFloat = 0.0f;
            //記憶されているVectorか
            var isMemorizedPlus = vectorDictionary.ContainsKey(componentString);
            //記憶されているVectorにマイナスが付いたものか
            var isMemorizedMinus = IsMemorizedMinus(componentString);
            //記憶されたVectorの場合
            if (isMemorizedMinus || isMemorizedPlus)
            {
                //記憶されたVectorを符号付きで取得
                var memorizedVector = isMemorizedPlus ?
                    vectorDictionary[componentString].Invoke() :
                    -vectorDictionary[componentString.Substring(1)].Invoke();
                //適切な成分を割り当て
                if (index == 0)
                {
                    componentFloat = memorizedVector.x;
                }
                else if (index == 1)
                {
                    componentFloat = memorizedVector.y;
                }
                else
                {
                    componentFloat = memorizedVector.z;
                }
            }
            //記憶されていないVectorの場合
            else
            {
                //Floatに変換
                componentFloat = float.TryParse(componentString, out var convertedValue) ?
                    convertedValue :
                    throw new ArgumentException($"Invalid form '{source}'. The component can not be converted into a float value.");
            }
            //適切な成分に割り当て
            if (index == 0)
            {
                xComponent = componentFloat;
            }
            else if (index == 1)
            {
                yComponent = componentFloat;
            }
            else
            {
                zComponent = componentFloat;
            }
        }
        //Vectorを生成する
        return new Vector3(xComponent, yComponent, zComponent);
    }

    //記憶されたVectorにマイナスが付いたものか
    private bool IsMemorizedMinus(string source)
    {
        return source.Length > 1 &&
            source.StartsWith('-') &&
            vectorDictionary.ContainsKey(source.Substring(1));
    }
}