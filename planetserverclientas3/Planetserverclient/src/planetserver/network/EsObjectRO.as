package planetserver.network {
    import com.hurlant.math.BigInteger;
    
    import flash.utils.ByteArray;
	
    /**
     * This interface is used to define what is readable off of an EsObject.
     */
    public interface EsObjectRO {

        /**
         * Gets the number of properties stored on the EsObject.
         * @return
         */
        function getSize():Number;
		
        /**
         * Gets the data type of a given property.
         * @param    Name of the data type.
         * @return The data type.
         */
        function getDataType(name:String):DataType;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getInteger(name:String):Number;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getString(name:String):String;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getDouble(name:String):Number;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getFloat(name:String):Number;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getBoolean(name:String):Boolean;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getNumber(name:String):Number;
        
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
     	function getByte(name:String):int;
        
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getChar(name:String):String;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getLong(name:String):BigInteger;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getShort(name:String):Number;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getEsObject(name:String):EsObject;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getIntegerArray(name:String):Array;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getStringArray(name:String):Array;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getDoubleArray(name:String):Array;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getFloatArray(name:String):Array;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getBooleanArray(name:String):Array;
        
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
     	function getByteArray(name:String):ByteArray;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getCharArray(name:String):Array;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getLongArray(name:String):Array;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getShortArray(name:String):Array;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getEsObjectArray(name:String):Array;
		
        /**
         * Gets the value of the property based on the name provided.
         * @param    Name of the property.
         * @return The value of the property.
         */
        function getNumberArray(name:String):Array;
        
		/**
		 * Gets the list of all properties. Each element is an EsObjectDataHolder instance.
		 * @return Array of all properties, where each is contained within an EsObjectDataHolder instance.
		 */
        function getEntries():Array;
        
    }
}
