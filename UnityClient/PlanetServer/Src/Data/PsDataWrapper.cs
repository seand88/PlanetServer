using Newtonsoft.Json;

namespace PlanetServer.Data
{
    public class PsDataWrapper
    {
        public object v { get; private set; }
        public int t { get; private set; }

        public PsDataWrapper()
        {
            
        }

        public PsDataWrapper(int type, object value)
        {
            t = type;
            v = value;
        }
    }
}
