package planetserver.network 
{
	/**
	 * @author sean
	 * 
	 * Wrapper for networked arrays
	 */
	public class PsArray 
	{
		private var _content:Vector.<PsDataWrapper>;
		
		public function PsArray() 
		{
			_content = new Vector.<PsDataWrapper>();
		}
		
		public function getInt(index:int):int
		{
			return int(_content[index].getValue());
		}
		
		public function getNumber(index:int):Number
		{
			return Number(_content[index].getValue());
		}
		
		public function getString(index:int):String
		{
			return String(_content[index].getValue());
		}
		
		public function getBoolean(index:int):Boolean
		{
			return Boolean(_content[index].getValue());
		}
		
		public function getPsObject(index:int):PsObject
		{
			return PsObject(_content[index].getValue());
		}
		
		public function getPsArray(index:int):PsArray
		{
			return PsArray(_content[index].getValue());
		}
		
		public function addInt(value:int):void
		{
			_content.push(new PsDataWrapper(value, PsType.TYPE_INTEGER));
		}
		
		public function addNumber(value:Number):void
		{
			_content.push(new PsDataWrapper(value, PsType.TYPE_NUMBER));
		}
		
		public function addString(value:String):void
		{
			_content.push(new PsDataWrapper(value, PsType.TYPE_STRING));
		}
		
		public function addBoolean(value:Boolean):void
		{
			_content.push(new PsDataWrapper(value, PsType.TYPE_BOOLEAN));
		}
		
		public function addPsArray(value:PsArray):void
		{
			_content.push(new PsDataWrapper(value, PsType.TYPE_PSARRAY));
		}
		
		public function addPsObject(value:PsObject):void
		{
			_content.push(new PsDataWrapper(value, PsType.TYPE_PSOBJECT));
		}
		
		public function toArrayObject():Array
		{
			var out:Array = new Array(); //represents this object
			for each (var obj:PsDataWrapper in _content) 
			{
				if (obj.getValue() is PsObject)
				{
					var psObject:Object = new Object();
					psObject[PsType.TYPE_FLAG] = PsType.TYPE_PSOBJECT;
					psObject[PsType.VALUE_FLAG] = PsObject(obj.getValue()).toObject();
					out.push(psObject);
				}
				else if (obj.getValue is PsArray)
				{	
					var psArray:Object = new Object();
					psObject[PsType.TYPE_FLAG] = PsType.TYPE_PSARRAY;
					psObject[PsType.VALUE_FLAG] = PsArray(obj.getValue()).toArrayObject();
					out.push(psObject);
				}
				else
				{
					out.push(obj);
				}
			}
			return out;			
		}
		
		
		/**
		 * Converts to a psObject from a parsed json object
		 * @return
		 */
		public function fromObject(jsonObject:Object):void
		{
			for (var key:String in jsonObject) 
			{
				var type:int = jsonObject[key].t
				var value:Object = jsonObject[key].v			
				
				switch(type)
				{
					case PsType.TYPE_BOOLEAN:
						addBoolean(Boolean(value));
						break;
						
					case PsType.TYPE_STRING:
						addString(String(value));
						break;
						
					case PsType.TYPE_INTEGER:
						addInt(int(value));
						break;
						
					case PsType.TYPE_LONG:
						addNumber(Number(value));
						break;
						
					case PsType.TYPE_FLOAT:
						addNumber(Number(value));
						break;
						
					case PsType.TYPE_DOUBLE:
						addNumber(Number(value));
						break;
						
					case PsType.TYPE_PSOBJECT:
						var newObj:PsObject = new PsObject();
						newObj.fromObject(value);
						addPsObject(newObj);
						break;
						
					case PsType.TYPE_PSARRAY:
						var newArray:PsArray = new PsArray();
						newArray.fromObject(value);
						addPsArray(newArray);
						break;
					
					case PsType.TYPE_NUMBER:
						addNumber(Number(value));
						break;
						
					default:
						throw new Error("TYPE NOT SUPPORTED!!!");
						break;
				}
			}
		}
		
		/**
		 * private function for converting from psarray to an as3 array
		 * @return
		 */
		public function toArray():Array
		{
			var outArray:Array = new Array();
			for each (var item:PsDataWrapper in _content) 
			{
				outArray.push(item.getValue());
			}
			return outArray
		}
		
	}
}