package planetserver.network {
    import com.hurlant.math.BigInteger;
	import planetserver.util.Base64Decoder;
	import planetserver.util.Base64Encoder;
    
    import flash.utils.ByteArray;
    import flash.utils.Dictionary;
    import flash.xml.XMLNode;
    /**
     * This class is used to represent an EsObject. EsObject is used all throughout the API on both the client and server side of ElectroServer. It is a way to create an object with strictly typed properties. This object can be of unlimited depth and is understood by ActionScript 3, Java and any other language that has an ElectroServer API. With EsObject the client and server, or ActionScript and Java, can exchange custom deep data representations. EsObject is serialized with the smallest footprint possible. It is significantly smaller than other object serializers such as JSON.
     * <br/><br/>EsObject supports the following properties. 
     * <br/><ul>
     * <li>String and string array</li>
     * <li>Boolean and boolean array</li>
     * <li>EsObject and EsObject array</li>
     * <li>Integer and integer array</li>
     * <li>Double and double array</li>
     * <li>Float and float array</li>
     * <li>Number and number array</li>
     * <li>Long and long array</li>
     * <li>Short and short array</li>
     * <li>Byte and byte array</li>
     * <li>Character and character array</li>
     * </ul>
     * @example
     * This example shows how to create a new EsObject and populate it with data.
     * <listing>
    import com.electrotank.electroserver5.esobject.EsObject;
    var esob:EsObject = new EsObject();
    esob.setString("MyName", "Jobe");
    esob.setInteger("MyAge", 32);
    //create 2nd ob to place in first
    var esob2:EsObject = new EsObject();
    esob2.setString("TestVar", "test value");
    esob2.setBoolean("IsProgrammer", true);
    //place 2nd ob in first
    esob.setEsObject("MySecondOb", esob2);
    
     * </listing>
     */
    
    public class EsObject implements EsObjectRO {
    	private static var dataTypeToCompatibleName:Dictionary = new Dictionary();
    	private static var compatibleNameToDataType:Dictionary = new Dictionary();
    	
    	addDataTypeCompatableName(DataType.Integer, "integer");
    	addDataTypeCompatableName(DataType.EsString, "string");
    	addDataTypeCompatableName(DataType.Double, "double");
    	addDataTypeCompatableName(DataType.Float, "float");
    	addDataTypeCompatableName(DataType.Boolean, "boolean");
    	addDataTypeCompatableName(DataType.Byte, "byte");
    	addDataTypeCompatableName(DataType.Character, "character");
    	addDataTypeCompatableName(DataType.Long, "long");
    	addDataTypeCompatableName(DataType.Short, "short");
    	addDataTypeCompatableName(DataType.EsObject, "esobject");
    	addDataTypeCompatableName(DataType.EsObjectArray, "esobject_array");
    	addDataTypeCompatableName(DataType.IntegerArray, "integer_array");
    	addDataTypeCompatableName(DataType.StringArray, "string_array");
    	addDataTypeCompatableName(DataType.DoubleArray, "double_array");
    	addDataTypeCompatableName(DataType.FloatArray, "float_array");
    	addDataTypeCompatableName(DataType.BooleanArray, "boolean_array");
    	addDataTypeCompatableName(DataType.ByteArray, "byte_array");
    	addDataTypeCompatableName(DataType.CharacterArray, "character_array");
    	addDataTypeCompatableName(DataType.LongArray, "long_array");
    	addDataTypeCompatableName(DataType.ShortArray, "short_array");
    	addDataTypeCompatableName(DataType.EsNumber, "number");
    	addDataTypeCompatableName(DataType.NumberArray, "number_array");
    	
    	private static function addDataTypeCompatableName(type:DataType, name:String):void {
    		dataTypeToCompatibleName[type] = name;
    		compatibleNameToDataType[name] = type;
    	}
    	
        private var data:Object;
        private var list:Array;
		
        /**
         * Creates a new instance of the EsObject class.
         */
        public function EsObject() {
            data = new Object();
            list = new Array();
        }
        
		/**
		 * This method is used to generate a string representation of the contents of the EsObject. It is useful for debugging.
		 * @param	Optional character to used in place of each tab.
		 * @return A string representing the contents of the EsObject
		 */
	     public function toString(tabs:String=null):String {
            var esobStr:String = "{EsObject:\n";
            if (tabs == null) {
                tabs = "";
            }
            tabs += "\t";
            for (var i:Number=0;i<getEntries().length;++i) {
                var dh:EsObjectDataHolder = getEntries()[i];
                var name:String = dh.getName();
                esobStr += tabs+name+":"+dh.getDataType().name+" = ";
                var tb:String = tabs + "\t";
     			var arr:Array;
                switch(dh.getDataType()) {
                    case DataType.EsObject:
                        esobStr += dh.getRawValue().toString(tabs);
                        break;
                    case DataType.Byte:
                    case DataType.Character:
                    case DataType.Double:
                    case DataType.Boolean:
                    case DataType.EsNumber:
                    case DataType.EsString:
                    case DataType.Float:
                    case DataType.Integer:
                    case DataType.Short:
                        esobStr += dh.getRawValue().toString();
                        break;
                    case DataType.Long:
                    	esobStr += dh.getLongValue().toString(10);
                    	break;
                    case DataType.BooleanArray:
                    case DataType.CharacterArray:
                    case DataType.DoubleArray:
                    case DataType.FloatArray:
                    case DataType.IntegerArray:
                    case DataType.NumberArray:
                    case DataType.ShortArray:
                    case DataType.StringArray:
                        esobStr += "\n" + tb +"[\n"
						arr = dh.getRawValue() as Array;
                        for (var j1:Number = 0; j1 < arr.length;++j1) {
                            esobStr += tb+arr[j1];
                            if (j1 != arr.length -1) {
                                esobStr += ",\n";
                            }
                        }
                        esobStr += "\n"+tb+"]";
                        break;
                    case DataType.LongArray:
                        esobStr += "\n" + tb +"[\n"
						arr = dh.getRawValue() as Array;
                        for (var j:Number = 0; j < arr.length;++j) {
                            esobStr += tb + (arr[j] as BigInteger).toString(10);
                            if (j != arr.length -1) {
                                esobStr += ",\n";
                            }
                        }
                        esobStr += "\n"+tb+"]";
                        break;
                    case DataType.ByteArray:
						var ba:ByteArray = dh.getRawValue() as ByteArray;
						esobStr += "\n" + tb +"[\n";
						for (var jk:Number = 0; jk < ba.length; jk++) {
							esobStr += ba[jk] + "\t";
						}

                        esobStr += "\n"+tb+"]";
                        break;
                    case DataType.EsObjectArray:
                        esobStr += "\n"+tb +"[\n"
						var obArr:Array = dh.getRawValue() as Array;
                        for (var k:Number = 0; k < obArr.length;++k) {
                            var eob:EsObject = obArr[k];
                            esobStr += tb+eob.toString(tabs);
                            if (k != obArr.length -1) {
                                esobStr += ",\n";
                            }
                        }
                        esobStr += "\n"+tb+"]";
                        break;
                    default:
                        trace("EsObject.toString() data type not supported: "+dh.getDataType().name);
                        break;
                }
                if (i != getEntries().length -1) {
                    esobStr += "\n";
                }
            }
            esobStr += "\n"+tabs+"}";
            return esobStr;
        }
		
		/**
		 * Serializes the EsObject to an XML formatted string. You can use this with EsObject.fromXML to convert from XML back into an EsObject.
		 * @param	Optional character to use along with the tabs when formatting the XML.
		 * @return An XML formatted string representing the EsObject.
		 */
		public function toXML(tabs:String=null):String {
			var base64:Base64Encoder;
			
            if (tabs == null) {
                tabs = "";
            }
            tabs += "\t";
            var esobStr:String = "";
            if (tabs == "\t") {
                esobStr += "<Variables>";
            }
            esobStr += "\n";
            for (var i:Number=0;i<getEntries().length;++i) {
                var dh:EsObjectDataHolder = getEntries()[i];
                var name:String = dh.getName();
                esobStr += tabs + "<Variable name='" + name + "' type='" + dataTypeToCompatibleName[dh.getDataType()].replace("_array", "") + "' >";
                var tb:String = tabs+"\t";
 				var arr:Array;
                switch(dh.getDataType()) {
                    case DataType.EsObject:
                        esobStr += dh.getEsObjectValue().toXML(tabs);
                        esobStr += "\n"+tabs;
                        break;
                    //case DataType.Byte:
                    case DataType.Character:
                    case DataType.Double:
                    case DataType.Boolean:
                    case DataType.EsNumber:
                    case DataType.EsString:
                    case DataType.Float:
                    case DataType.Integer:
                    case DataType.Short:
                        esobStr += dh.getRawValue().toString();
                        break;
                    case DataType.Long:
                    	esobStr += dh.getLongValue().toString(10);
                    	break;
                    case DataType.Byte:
						base64 = new Base64Encoder();
						var ba:ByteArray = new ByteArray();
						ba.writeByte(dh.getByteValue());
						base64.encodeBytes(ba);
						esobStr += base64.drain();
                        break;
                    case DataType.ByteArray:
						base64 = new Base64Encoder();
						base64.encodeBytes(dh.getByteArrayValue());
						esobStr += base64.drain();
						break;
                    case DataType.BooleanArray:
                    case DataType.CharacterArray:
                    case DataType.DoubleArray:
                    case DataType.FloatArray:
                    case DataType.IntegerArray:
                    case DataType.NumberArray:
                    case DataType.ShortArray:
                    case DataType.StringArray:
                        //esobStr += "\n"+tb +"<Entry>\n"
     					arr = dh.getRawValue() as Array;
                        for (var j1:Number = 0; j1 < arr.length;++j1) {
                            esobStr += "\n"+tb+"<Entry>"+arr[j1].toString()+"</Entry>";
                        }
                        esobStr += "\n"+tabs;
                        break;
                    case DataType.LongArray:
     					arr = dh.getRawValue() as Array;
                        for (var j:Number = 0; j < arr.length;++j) {
                            esobStr += "\n"+tb+"<Entry>"+(arr[j] as BigInteger).toString(10)+"</Entry>";
                        }
                        esobStr += "\n"+tabs;
                        break;
                    case DataType.EsObjectArray:
     					var obArr:Array = dh.getRawValue() as Array;
                        for (var k:Number = 0; k < obArr.length;++k) {
                            var eob:EsObject = obArr[k];
                            esobStr += "\n"+tb+"<Entry>"+eob.toXML(tabs+"\t")+"\n"+tb+"</Entry>";
                        }
                        esobStr += "\n"+tabs;
                        break;
                    default:
                        trace("EsObject.toString() data type not supported: "+dh.getDataType().name);
                        break;
                }
                esobStr += "</Variable>";
                if (i != getEntries().length -1) {
                    esobStr += "\n";
                }
            }
            if (tabs == "\t") {
                esobStr += "\n</Variables>";
            }
            //esobStr += "\n";
            return esobStr;
        }
		
		
		private static function containsEntryNodes(node:XMLNode):Boolean {
			for each (var child:XMLNode in node.childNodes) {
				if (child && child.nodeName && child.nodeName.toLowerCase() == "entry") {
					return true;
				}
			}
			
			return false;
		}
		
		/**
		 * Uses the contents of an XMLNode to popuplate the properties of this EsObject.
		 * 
		 * Example usage:
		 * var xmlDocument:XMLDocument = new XMLDocument();
		 * xmlDocument.ignoreWhite = true;
		 * xmlDocument.parseXML(xmlString);
		 * var xmlNode:XMLNode = xmlDocument.firstChild;
		 * var esob:EsObject = new EsObject();
		 * esob.fromXML(xmlNode);
		 * 
		 * @param	The XMLNode to parse.
		 */
        public function fromXML(info:XMLNode):void {

            var children:Array /* of XMLNode */ = info.childNodes;
            for (var i:Number = 0; i < children.length;++i) {
                var child:XMLNode = children[i];
                var atts:Object = child.attributes;
                var name:String = atts.name;
                var dataType:DataType = compatibleNameToDataType[atts.type + (containsEntryNodes(child) ? "_array" : "")];
                var value:String;
				var base64:Base64Decoder;
                var arr:Array = new Array();
                var kids:Array;
                var kid:XMLNode;
                var j:Number;
                var esob:EsObject;
				
				switch(dataType) {
					
                    case DataType.EsObject:
                        esob = new EsObject();
                        esob.fromXML(child);
                        setEsObject(name, esob);
                        break;
                    case DataType.Byte:
					case DataType.ByteArray:
						base64 = new Base64Decoder();
						value = child.firstChild.nodeValue;
						base64.decode(value);
						var ba:ByteArray = base64.toByteArray();
						if (ba.bytesAvailable > 1) {
							// We're dealing with a ByteArray
							setByteArray(name, ba);
						} else {
							setByte(name, ba.readByte());
						}
						break;
                    case DataType.Character:
                        value = child.firstChild.nodeValue;
                        setChar(name, value);
                        break;
                    case DataType.Double:
                        value = child.firstChild.nodeValue;
                        setDouble(name, Number(value));
                        break;
                    case DataType.Boolean:
                        value = child.firstChild.nodeValue;
                        setBoolean(name, value.toLowerCase() == "true" ? true : false);
                        break;
                    case DataType.EsNumber:
                        value = child.firstChild.nodeValue;
                        setNumber(name, Number(value));
                        break;
                    case DataType.EsString:
                        value = child.firstChild.nodeValue;
                        setString(name, value);
                        break;
                    case DataType.Float:
                        value = child.firstChild.nodeValue;
                        setFloat(name, Number(value));
                        break;
                    case DataType.Integer:
                        value = child.firstChild.nodeValue;
                        setInteger(name, Number(value));
                        break;
                    case DataType.Long:
                        value = child.firstChild.nodeValue;
                        setLong(name, new BigInteger((value as String), 10));
                        break;
                    case DataType.Short:
                        value = child.firstChild.nodeValue;
                        setShort(name, Number(value));
                        break;
                    case DataType.StringArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            arr[j] = kid.firstChild.nodeValue;
                        }
                        setStringArray(name, arr);
                        break;
                    case DataType.CharacterArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            arr[j] = kid.firstChild.nodeValue;
                        }
                        setCharArray(name, arr);
                        break;
                    case DataType.LongArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            arr[j] = new BigInteger(kid.firstChild.nodeValue, 10);
                        }
                        setLongArray(name, arr);
                        break;
                    case DataType.DoubleArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            arr[j] = Number(kid.firstChild.nodeValue);
                        }
                        setDoubleArray(name, arr);
                        break;
                    case DataType.FloatArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            arr[j] = Number(kid.firstChild.nodeValue);
                        }
                        setFloatArray(name, arr);
                        break;
                    case DataType.IntegerArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
							arr[j] = int(kid.firstChild.nodeValue);
                        }
                        setIntegerArray(name, arr);
                        break;
                    case DataType.NumberArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            arr[j] = Number(kid.firstChild.nodeValue);
                        }
                        setNumberArray(name, arr);
                        break;
                    case DataType.ShortArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            arr[j] = Number(kid.firstChild.nodeValue);
                        }
                        setShortArray(name, arr);
                        break;
                    case DataType.BooleanArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            arr[j] = kid.firstChild.nodeValue.toLowerCase() == "true" ? true : false;
                        }
                        setBooleanArray(name, arr);
                        break;
                    case DataType.EsObjectArray:
                        kids = child.childNodes;
                        for (j = 0; j < kids.length;++j) {
                            kid = kids[j];
                            esob = new EsObject();
                            esob.fromXML(kid);
                            arr[j] = esob;
                        }
                        setEsObjectArray(name, arr);
                        break;
                    default:
                        trace("EsObject.fromXML() data type not supported: "+atts.type);
                        break;
                }
            }
        }
		
        /**
         * Checks to see if a property exists on the EsObject. If it does then true is returned. Otherwise, false is returned.
         * @param    The name of the property to check for.
         * @return True or false.
         */
        public function doesPropertyExist(name:String):Boolean {
            return data[name] != null;
        }
		
        /**
         * Returns the number of properties on the object.
         * @return The number of properties on the object.
         */
        public function getSize():Number {
            return list.length;
        }
		
        /**
         * Returns the list of entries on the object. The value of each entry is of type EsObjectDataHolder.
         * @return The list of entries on the object.
         */
        public function getEntries():Array {
            return list;
        }
		
        /**
         * Used internally. Puts a value on the EsObject.
         * @param    The name of the property.
         * @param    The value of the property.
		 * @private
         */
        private function put(name:String, value:EsObjectDataHolder):void {

            value.setName(name);
            if (data[name] != null) {
                var oldValue:EsObjectDataHolder = data[name];
                for (var i:Number = 0; i < list.length;++i) {
                    if (list[i] == oldValue) {
                        list.splice(i, 1);
                        break;
                    }
                }
                data[name] = null;
            }
            data[name] = value;
            list.push(value);
        }
		
        /**
         * Returns the DataType of a property based on its name.
         * @param    The name of the property.
         * @return The DataType of property.
         */
        public function getDataType(name:String):DataType {
            return getHolderForName(name).getDataType();
        }
		
        /**
         * Sets an integer ont onto the EsObject.
         * @param    The name of the integer.
         * @param    The value of the integer.
         */
        public function setInteger(name:String, value:Number):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.Integer);
            esh.setIntValue(value);
            put(name, esh);
        }
		
        /**
         * Sets a string onto the EsObject.
         * @param    Name of the string.
         * @param    Value of the string.
         */
        public function setString(name:String, value:String):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.EsString);
            esh.setStringValue(value);
            put(name, esh);
        }
		
        /**
         * Sets a double onto the EsObject.
         * @param    Name of the EsObject.
         * @param    Value of the EsObject.
         */
        public function setDouble(name:String, value:Number):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.Double);
            esh.setDoubleValue(value);
            put(name, esh);
        }
		
        /**
         * Sets a float onto the EsObject.
         * @param    Name of the float.
         * @param    Value of the float.
         */
        public function setFloat(name:String, value:Number):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.Float);
            esh.setFloatValue(value);
            put(name, esh);
        }
		
        /**
         * Sets a boolean onto the EsObject.
         * @param    Name of the boolean.
         * @param    Value of the boolean.
         */
        public function setBoolean(name:String, value:Boolean):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.Boolean);
            esh.setBooleanValue(value);
            put(name, esh);
        }
        
        /**
         * Sets a byte onto the EsObject.
         * @param    Name of the byte.
         * @param    Value of the byte.
         */
		public function setByte(name:String, value:int):void {
            
            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.Byte);
            esh.setByteValue(value);
            put(name, esh);
        }
        
        /**
         * Sets a character onto the EsObject.
         * @param    Name of the character.
         * @param    Value of the character.
         */
        public function setChar(name:String, value:String):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.Character);
            esh.setCharValue(value);
            put(name, esh);
        }
		
        /**
         * Sets a long onto the EsObject.
         * @param    Name of the long.
         * @param    Value of the long.
         */
        public function setLong(name:String, value:BigInteger):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.Long);
            esh.setLongValue(value);
            put(name, esh);
        }
		
        /**
         * Sets a short onto the EsObject.
         * @param    Name of the short.
         * @param    Value of the short.
         */
        public function setShort(name:String, value:Number):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.Short);
            esh.setShortValue(value);
            put(name, esh);
        }
		
        /**
         * Sets an EsObject onto the EsObject.
         * @param    Name of the EsObject.
         * @param    Value of the EsObject.
         */
        public function setEsObject(name:String, value:EsObject):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.EsObject);
            esh.setEsObjectValue(value);
            put(name, esh);
        }
		
        /**
         * Sets a number onto the EsObject.
         * @param    Name of the number.
         * @param    Value of the number.
         */
        public function setNumber(name:String, value:Number):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.EsNumber);
            esh.setNumberValue(value);
            put(name, esh);
        }
		
        /**
         * Sets an array of integers onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setIntegerArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.IntegerArray);
            esh.setIntArrayValue(value);
            put(name, esh);
        }
        
        /**
         * Sets an array of strings onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setStringArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.StringArray);
            esh.setStringArrayValue(value);
            put(name, esh);
        }
        
        /**
         * Sets an array of doubles onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setDoubleArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.DoubleArray);
            esh.setDoubleArrayValue(value);
            put(name, esh);
        }
        
        /**
         * Sets an array of floats onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setFloatArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.FloatArray);
            esh.setFloatArrayValue(value);
            put(name, esh);
        }
		
        /**
         * Sets an array of booleans onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setBooleanArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.BooleanArray);
            esh.setBooleanArrayValue(value);
            put(name, esh);
        }
        
        /**
         * Sets an array of bytes onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setByteArray(name:String, value:ByteArray):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.ByteArray);
            esh.setByteArrayValue(value);
            put(name, esh);
        }
     
        
        /**
         * Sets an array of characters onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setCharArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.CharacterArray);
            esh.setCharArrayValue(value);
            put(name, esh);
        }
        
        /**
         * Sets an array of longs onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setLongArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.LongArray);
            esh.setLongArrayValue(value);
            put(name, esh);
        }
        
        /**
         * Sets an array of shorts onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setShortArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.ShortArray);
            esh.setShortArrayValue(value);
            put(name, esh);
        }
        
        /**
         * Sets an array of EsObjects onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setEsObjectArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.EsObjectArray);
            esh.setEsObjectArrayValue(value);
            put(name, esh);
        }
        
        /**
         * Sets an array of numbers onto the EsObject.
         * @param    Name of the array.
         * @param    The array.
         */
        public function setNumberArray(name:String, value:Array):void {

            var esh:EsObjectDataHolder = new EsObjectDataHolder();
            esh.setRawValue(value);
            esh.setDataType(DataType.NumberArray);
            esh.setNumberArrayValue(value);
            put(name, esh);
        }
        
		/**
		 * Gets the EsObjectDataHolder instance for the property named here
		 * @param	Name of the property
		 * @return EsObjectDataHolder for the property named
		 * @private
		 */
        private function getHolderForName(name:String):EsObjectDataHolder {
            var holder:EsObjectDataHolder = data[name];
            if(holder == null) {
               throw new Error("Unable to locate variable named '" + name + "' on EsObject");
            }
            return holder;
        }
		
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getInteger(name:String):Number {
            return getHolderForName(name).getIntValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getString(name:String):String {
            return getHolderForName(name).getStringValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getDouble(name:String):Number {
            return getHolderForName(name).getDoubleValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getFloat(name:String):Number {
            return getHolderForName(name).getFloatValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getBoolean(name:String):Boolean {
            return getHolderForName(name).getBooleanValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
		public function getByte(name:String):int {
            return getHolderForName(name).getByteValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getChar(name:String):String {
            return getHolderForName(name).getCharValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getLong(name:String):BigInteger {
            return getHolderForName(name).getLongValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getShort(name:String):Number {
            return getHolderForName(name).getShortValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getEsObject(name:String):EsObject {
            return getHolderForName(name).getEsObjectValue();
        }
            
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getNumber(name:String):Number {
            return getHolderForName(name).getNumberValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getIntegerArray(name:String):Array {
            return getHolderForName(name).getIntArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getStringArray(name:String):Array {
            return getHolderForName(name).getStringArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getDoubleArray(name:String):Array {
            return getHolderForName(name).getDoubleArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getFloatArray(name:String):Array {
            return getHolderForName(name).getFloatArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getBooleanArray(name:String):Array {
            return getHolderForName(name).getBooleanArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getByteArray(name:String):ByteArray {
            return getHolderForName(name).getByteArrayValue();
        }
    
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getCharArray(name:String):Array {
            return getHolderForName(name).getCharArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getLongArray(name:String):Array {
            return getHolderForName(name).getLongArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getShortArray(name:String):Array {
            return getHolderForName(name).getShortArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getEsObjectArray(name:String):Array {
            return getHolderForName(name).getEsObjectArrayValue();
        }
        
        /**
         * Gets the value of the property based on a name..
         * @param    Name of the property.
         * @return The value of the property.
         */
        public function getNumberArray(name:String):Array {
            return getHolderForName(name).getNumberArrayValue();
        }
        
        /**
         * Remove a variable from the EsObject that has the name passed in.
         * @param    Name of the variable to remove.
         */
        public function removeVariable(name:String):void {

            delete data[name];
            for (var i:Number=0;i<list.length;++i) {
                var dh:EsObjectDataHolder = list[i];
                if (dh.getName() == name) {
                    list.splice(i, 1);
                    break;
                }
            }
        }
        
        /**
         * Completely clears out the EsObject.
         */
        public function removeAll():void {

            data = new Object();
            list = new Array();
        }
		
        /**
         * Gets the value of a variable before it has been cast to any data type.
         * @param    Name of the variable to get.
         * @return The raw value of the variable.
         */
        public function getRawVariable(name:String):Object {
            return getHolderForName(name).getRawValue();
        }
	  
		/**
		* Adds the contents of the argument.
		* Useful for combining two EsObjects.
		* 
		* @param esObjectRO the read-only EsObject to be added.
		* @see EsObjectRO EsObjectRO
		*/
		public function addAll(esObjectRO:EsObjectRO):void {
			var esObject:EsObject = esObjectRO as EsObject;
			var list:Array = esObject.getEntries();
			for (var ii:int = 0; ii < list.length; ii++) {
				var dh:EsObjectDataHolder = list[ii];
				var dataType:DataType = dh.getDataType();
				var name:String = dh.getName();
				
                switch(dataType) {
                    case DataType.EsObject:
                        setEsObject(name, dh.getEsObjectValue());
                        break;
                    case DataType.Byte:
						setByte(name, dh.getByteValue());
                        break;
                    case DataType.Character:
                        setChar(name, dh.getCharValue());
                        break;
                    case DataType.Double:
                        setDouble(name, dh.getDoubleValue());
                        break;
                    case DataType.Boolean:
                        setBoolean(name, dh.getBooleanValue());
                        break;
                    case DataType.EsNumber:
                        setNumber(name, dh.getNumberValue());
                        break;
                    case DataType.EsString:
                        setString(name, dh.getStringValue());
                        break;
                    case DataType.Float:
                        setFloat(name, dh.getFloatValue());
                        break;
                    case DataType.Integer:
                        setInteger(name, dh.getIntValue());
                        break;
                    case DataType.Long:
                        setLong(name, dh.getLongValue());
                        break;
                    case DataType.Short:
                        setShort(name, dh.getShortValue());
                        break;
                    case DataType.StringArray:
                        setStringArray(name, dh.getStringArrayValue());
                        break;
                    case DataType.CharacterArray:
                        setCharArray(name, dh.getCharArrayValue());
                        break;
                    case DataType.LongArray:
                        setLongArray(name, dh.getLongArrayValue());
                        break;
                    case DataType.DoubleArray:
                        setDoubleArray(name, dh.getDoubleArrayValue());
                        break;
                    case DataType.FloatArray:
                        setFloatArray(name, dh.getFloatArrayValue());
                        break;
                    case DataType.IntegerArray:
                        setIntegerArray(name, dh.getIntArrayValue());
                        break;
                    case DataType.NumberArray:
                        setNumberArray(name, dh.getNumberArrayValue());
                        break;
                    case DataType.ShortArray:
                        setShortArray(name, dh.getShortArrayValue());
                        break;
                    case DataType.BooleanArray:
                        setBooleanArray(name, dh.getBooleanArrayValue());
                        break;
                    case DataType.ByteArray:
                        setByteArray(name, dh.getByteArrayValue());
                        break;
                    case DataType.EsObjectArray:
                        setEsObjectArray(name, dh.getEsObjectArrayValue());
                        break;
                    default:
                        trace("EsObject.addAll() data type not supported: " + dataType.name);
                        break;
                }
				
			}
		}

        /**
		 * Creates a shallow clone EsObject.
		 * Shallow cloning is faster than deep cloning
		 * but does not treat non-primitive data types correctly.
		 * 
		 * @return shallow clone of the argument
         */
        public function shallowClone():EsObject {
            var clone:EsObject = new EsObject();
			clone.addAll(this);
			return clone;
        }
		
        /**
		 * Creates a deep clone EsObject.
		 * Shallow cloning is faster than deep cloning
		 * but does not treat non-primitive data types correctly.
		 * 
		 * @return deep clone of the argument
         */
        public function deepClone():EsObject {
            var clone:EsObject = shallowClone();
			// now look for EsObjects and arrays and fix those
			var list:Array = this.getEntries();
			for (var ii:int = 0; ii < list.length; ii++) {
				var dh:EsObjectDataHolder = list[ii];
				var dataType:DataType = dh.getDataType();
				var name:String = dh.getName();
				var arrayClone:Array;
				
                switch(dataType) {
                    case DataType.EsObject:
						var child:EsObject = dh.getEsObjectValue();
						clone.setEsObject(name, child.deepClone());
                        break;
                    case DataType.Byte:
                    case DataType.Character:
                    case DataType.Double:
                    case DataType.Boolean:
                    case DataType.Float:
                    case DataType.Integer:
                    case DataType.Long:
                    case DataType.Short:
						// already cloned correctly
						break;
                    case DataType.EsNumber:
						var num:Number = dh.getNumberValue();
						clone.setNumber(name, num.valueOf());
                        break;
                    case DataType.EsString:
                        clone.setString(name, "" + dh.getStringValue());
                        break;
                    case DataType.StringArray:
						arrayClone = cloneArray(dh.getStringArrayValue());
                        clone.setStringArray(name, arrayClone);
                        break;
                    case DataType.CharacterArray:
						arrayClone = cloneArray(dh.getCharArrayValue());
                        clone.setCharArray(name, arrayClone);
                        break;
                    case DataType.LongArray:
						arrayClone = cloneArray(dh.getLongArrayValue());
                        clone.setLongArray(name, arrayClone);
                        break;
                    case DataType.DoubleArray:
						arrayClone = cloneArray(dh.getDoubleArrayValue());
                        clone.setDoubleArray(name, arrayClone);
                        break;
                    case DataType.FloatArray:
						arrayClone = cloneArray(dh.getFloatArrayValue());
                        clone.setFloatArray(name, arrayClone);
                        break;
                    case DataType.IntegerArray:
						arrayClone = cloneArray(dh.getIntArrayValue());
                        clone.setIntegerArray(name, arrayClone);
                        break;
                    case DataType.NumberArray:
						arrayClone = cloneArray(dh.getNumberArrayValue());
                        clone.setNumberArray(name, arrayClone);
                        break;
                    case DataType.ShortArray:
						arrayClone = cloneArray(dh.getShortArrayValue());
                        clone.setShortArray(name, arrayClone);
                        break;
                    case DataType.BooleanArray:
						arrayClone = cloneArray(dh.getBooleanArrayValue());
                        clone.setBooleanArray(name, arrayClone);
                        break;
                    case DataType.ByteArray:
						var ba:ByteArray = cloneArray(dh.getByteArrayValue());
                        clone.setByteArray(name, ba);
                        break;
                    case DataType.EsObjectArray:
						var arr:Array = dh.getEsObjectArrayValue();
						var arr2:Array = new Array();
                        for (var j:int = 0; j < arr.length; j++) {
							var obj:EsObject = arr[j];
							arr2.push(obj.deepClone());
                        }
                        clone.setEsObjectArray(name, arr2);
						
                        break;
                    default:
                        trace("EsObject.addAll() data type not supported: " + dataType.name);
                        break;
                }
				
			}
			
			return clone;
        }
		
		private function cloneArray(source:Object):* {
			var myBA:ByteArray = new ByteArray();
			myBA.writeObject(source);
			myBA.position = 0;
			return(myBA.readObject());
		}		
		
      }
	  
}
