using System;
using System.Linq;
using ScenarioFlow;

public class PrimitiveDecoder : IReflectable
{
    [Decoder]
    public int IntDecoder(string source)
    {
        return int.TryParse(source, out int result) ?
            result :
            throw new ArgumentException(DecodeErrorMessage<int>(source));
    }

    [Decoder]
    public int[] IntArrayDecoder(string[] sources)
    {
        return sources.Select(s => IntDecoder(s)).ToArray();
    }

    [Decoder]
    public float FloatDecoder(string source)
    {
        return float.TryParse(source, out float result) ?
            result :
            throw new ArgumentException(DecodeErrorMessage<float>(source));
    }

    [Decoder]
    public float[] FloatArrayDecoder(string[] sources)
    {
        return sources.Select(s => FloatDecoder(s)).ToArray();
    }

    [Decoder]
    public bool BoolDecoder(string source)
    {
        return bool.TryParse(source, out bool result) ?
            result :
            throw new ArgumentException(DecodeErrorMessage<bool>(source));
    }

    [Decoder]
    public bool[] BoolArrayDecoder(string[] sources)
    {
        return sources.Select(s => BoolDecoder(s)).ToArray();
    }

    [Decoder]
    public string StringDecoder(string source)
    {
        return source;
    }

    [Decoder]
    public string[] StringArrayDecoder(string[] sources)
    {
        return sources.Select(s => StringDecoder(s)).ToArray();
    }

    private string DecodeErrorMessage<T>(string source)
    {
        return $"'{source}' can't be converted into '{typeof(T).Name}'";
    }
}