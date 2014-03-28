package planetserver.network {
    import com.hurlant.math.BigInteger;
    
    import flash.utils.ByteArray;
	
    /**
     * This class is used internally by the EsObject class. It is used to store the name, data type, and value of a property on the EsObject.
     */
    public class EsObjectDataHolder {

        private var name:String; 
        private var dataType:DataType;
        
        private var rawValue:Object;
        private var intValue:Number;
        private var stringValue:String;
        private var doubleValue:Number;
        private var floatValue:Number;
        private var booleanValue:Boolean;
        
     	private var byteValue:int;
        
        
        private var charValue:String;
        private var longValue:BigInteger;
        private var shortValue:Number;
        private var intArrayValue:Array;
        private var stringArrayValue:Array;
        private var doubleArrayValue:Array;
        private var floatArrayValue:Array;
        private var booleanArrayValue:Array;
     	private var byteArrayValue:ByteArray;
        private var charArrayValue:Array;
        private var longArrayValue:Array;
        private var shortArrayValue:Array;
        private var esObjectValue:EsObject;
        private var esObjectArrayValue:Array;
        private var numberValue:Number;
        private var numberArrayValue:Array;
        /**
         * The name of the property.
         * @param    Name of the property.
         */
        public function setName(name:String):void {

            this.name = name;
        }
		
        /**
         * Gets the name of the property.
         * @return The name of the property.
         */
        public function getName():String {
            return name;
        }
		
        /**
         * Sets the data type of the property.
         * @param    The data type of the property.
         */
        public function setDataType(val:DataType):void { 

             dataType = val;
        }
		
        /**
         * Gets the data type of the property.
         * @return Returns the data type of the property.
         */
        public function getDataType():DataType { 
              return dataType;
        }
		
        /**
         * Sets the raw value of the property.
         * @param    The raw value of the property.
         */
        public function setRawValue(val:Object):void { 

             rawValue = val;
        }
		
        /**
         * Gets the raw value of the property.
         * @return The raw value of the property
         */
        public function getRawValue():Object { 
              return rawValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setIntValue(val:Number):void { 

             intValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getIntValue():Number { 
              return intValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setStringValue(val:String):void { 

             stringValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getStringValue():String { 
              return stringValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setDoubleValue(val:Number):void { 

             doubleValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getDoubleValue():Number { 
              return doubleValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setFloatValue(val:Number):void { 

             floatValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getFloatValue():Number { 
              return floatValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setBooleanValue(val:Boolean):void { 

             booleanValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getBooleanValue():Boolean { 
              return booleanValue;
        }
        
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
		public function setByteValue(val:int):void {

             byteValue = val;
        }
        
		/**
		 * Gets the value of the property.
		 * @return The value of the property.
		 */
		public function getByteValue():int {
              return byteValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setCharValue(val:String):void { 

             charValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getCharValue():String { 
              return charValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setLongValue(val:BigInteger):void { 

             longValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getLongValue():BigInteger { 
              return longValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setShortValue(val:Number):void { 

             shortValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getShortValue():Number { 
              return shortValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setIntArrayValue(val:Array):void { 

             intArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getIntArrayValue():Array { 
              return intArrayValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setStringArrayValue(val:Array):void { 

             stringArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getStringArrayValue():Array { 
              return stringArrayValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setDoubleArrayValue(val:Array):void { 

             doubleArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getDoubleArrayValue():Array { 
              return doubleArrayValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setFloatArrayValue(val:Array):void { 

             floatArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getFloatArrayValue():Array { 
              return floatArrayValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setBooleanArrayValue(val:Array):void { 

             booleanArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getBooleanArrayValue():Array { 
              return booleanArrayValue;
        }
        
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setByteArrayValue(val:ByteArray):void { 

             byteArrayValue = val;
        }
     
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getByteArrayValue():ByteArray { 
              return byteArrayValue;
        }
        
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setCharArrayValue(val:Array):void { 

             charArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getCharArrayValue():Array { 
              return charArrayValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setLongArrayValue(val:Array):void { 

             longArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getLongArrayValue():Array { 
              return longArrayValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setShortArrayValue(val:Array):void { 

             shortArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getShortArrayValue():Array { 
              return shortArrayValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setEsObjectValue(val:EsObject):void { 

             esObjectValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getEsObjectValue():EsObject { 
              return esObjectValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setEsObjectArrayValue(val:Array):void { 

             esObjectArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getEsObjectArrayValue():Array { 
              return esObjectArrayValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setNumberValue(val:Number):void { 

             numberValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getNumberValue():Number { 
              return numberValue;
        }
		
        /**
         * Sets the value of the property.
         * @param    The value of the property.
         */
        public function setNumberArrayValue(val:Array):void { 

             numberArrayValue = val;
        }
		
        /**
         * Gets the value of the property.
         * @return The value of the property.
         */
        public function getNumberArrayValue():Array { 
              return numberArrayValue;
        }
    
    }
}
