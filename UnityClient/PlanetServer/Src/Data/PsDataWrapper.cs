namespace PS.Data
{
    /// <summary>
    /// Generic container for holding different data types.
    /// </summary>
    public class PsDataWrapper
    {
        /// <summary>
        /// Value to hold.
        /// </summary>
        public object v { get; private set; }
        /// <summary>
        /// Type of value.
        /// </summary>
        public int t { get; private set; }

        /// <summary>
        /// Initializes a new instance of the PsDataWrapper class.
        /// </summary>
        public PsDataWrapper()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the PsDataWrapper class.
        /// </summary>
        /// <param name="type">Type of data</param>
        /// <param name="value">Value of data</param>
        public PsDataWrapper(int type, object value)
        {
            t = type;
            v = value;
        }
    }
}
