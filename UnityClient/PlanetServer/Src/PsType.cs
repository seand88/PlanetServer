internal class Constants
{
    /// <summary>
    /// Types of values.
    /// </summary>
    public enum PsType
    {
        /// <summary>
        /// Boolean.
        /// </summary>
        Boolean = 1,
        /// <summary>
        /// String.
        /// </summary>
        String,
        /// <summary>
        /// Integer.
        /// </summary>
        Integer,
        /// <summary>
        /// Long.
        /// </summary>
        Long,
        /// <summary>
        /// Float.
        /// </summary>
        Float,
        /// <summary>
        /// Double.
        /// </summary>
        Double,
        /// <summary>
        /// PsObject.
        /// </summary>
        PSObject,
        /// <summary>
        /// PsArray.
        /// </summary>
        PSArray,
        /// <summary>
        /// (Flash)Number.
        /// </summary>
        Number
    }

    /// <summary>
    /// Type constant.
    /// </summary>
    public static string TYPE_FLAG = "t";
    /// <summary>
    /// Value constant.
    /// </summary>
	public static string VALUE_FLAG = "v";
}
