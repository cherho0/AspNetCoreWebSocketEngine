using System;

namespace TestServices
{
    public interface ICommonService
    {
        string GetWorld();
    }

    public class CommonService : ICommonService
    {
        public string GetWorld()
        {
            return $"Hello world!";
        }
    }
}
