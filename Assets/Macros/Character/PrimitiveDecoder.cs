using ScenarioFlow;

public class PrimitiveDecoder : IReflectable
{
   [Decoder]
   public string StringDecoder(string source)
   {
       return source;
   }
}