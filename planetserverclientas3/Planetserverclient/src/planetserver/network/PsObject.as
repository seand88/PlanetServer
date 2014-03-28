package planetserver.network 
{
	import flash.utils.Dictionary;
	import it.sephiroth.utils.collections.iterators.Iterator;
	import it.sephiroth.utils.HashMap;
	import it.sephiroth.utils.KeySet;
	/**
	 * ...
	 * @author sean
	 */
	public class PsObject 
	{
		private var _content:HashMap;
		
		public function PsObject() 
		{
			_content = new HashMap();
		}
		
		public function hasKey(key:String):Boolean
		{
			return _content.containsKey(key);
		}
		
		public function getInt(key:String):int
		{
			return int(PsDataWrapper(_content.getValue(key)).getValue());
		}
		
		public function getNumber(key:String):Number
		{
			return Number(PsDataWrapper(_content.getValue(key)).getValue());
		}
		
		public function getString(key:String):String
		{
			return String(PsDataWrapper(_content.getValue(key)).getValue());
		}
		
		public function getBoolean(key:String):Boolean
		{
			return Boolean(PsDataWrapper(_content.getValue(key)).getValue());
		}
		
		public function getPsObject(key:String):PsObject
		{
			return PsObject(PsDataWrapper(_content.getValue(key)).getValue());
		}
		
		public function getPsArray(key:String):PsArray
		{
			return PsArray(PsDataWrapper(_content.getValue(key)).getValue());
		}	
		
		public function getArray(key:String):Array
		{
			var psArray:PsArray = PsArray(PsDataWrapper(_content.getValue(key)).getValue());
			return psArray.toArray();
		}
		
		public function setInt(key:String, value:int):void
		{
			_content.put(key, new PsDataWrapper(value, PsType.TYPE_INTEGER));
		}
		
		public function setString(key:String, value:String):void
		{
			_content.put(key, new PsDataWrapper(value, PsType.TYPE_STRING));
		}
		
		public function setPsObject(key:String, value:PsObject):void
		{
			_content.put(key, new PsDataWrapper(value, PsType.TYPE_PSOBJECT));
		}
		
		public function setNumber(key:String, value:Number):void
		{
			_content.put(key, new PsDataWrapper(value, PsType.TYPE_NUMBER));
		}
		
		public function setPsArray(key:String, value:PsArray):void
		{
			_content.put(key, new PsDataWrapper(value, PsType.TYPE_PSARRAY));
		}
		
		public function setBoolean(key:String, value:Boolean):void
		{
			_content.put(key, new PsDataWrapper(value, PsType.TYPE_BOOLEAN));
		}
		
		public function toObject():Object
		{
			trace("TO JSON!");
			var out:Object = new Object(); //represents this ps object
			var iterator:Iterator  = _content.keySet().iterator();
			while (iterator.hasNext())
			{
				var key:String = String(iterator.next());
				var obj:PsDataWrapper = PsDataWrapper(_content.getValue(key));
				if (obj.getValue() is PsObject)
				{
					var psObject:Object = new Object();
					psObject[PsType.TYPE_FLAG] = PsType.TYPE_PSOBJECT;
					psObject[PsType.VALUE_FLAG] = PsObject(obj.getValue()).toObject();
					out[key] = psObject;
				}
				else if (obj.getValue() is PsArray)
				{
					var psArray:Object = new Object();
					psArray[PsType.TYPE_FLAG] = PsType.TYPE_PSARRAY;
					psArray[PsType.VALUE_FLAG] = PsArray(obj.getValue()).toArrayObject();
					out[key] = psArray;
				}
				else
				{
					out[key] = obj;
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
						setBoolean(key, Boolean(value));
						break;
						
					case PsType.TYPE_STRING:
						setString(key, String(value));
						break;
						
					case PsType.TYPE_INTEGER:
						setInt(key, int(value));
						break;
						
					case PsType.TYPE_LONG:
						setNumber(key, Number(value));
						break;
						
					case PsType.TYPE_FLOAT:
						setNumber(key, Number(value));
						break;
						
					case PsType.TYPE_DOUBLE:
						setNumber(key, Number(value));
						break;
						
					case PsType.TYPE_PSOBJECT:
						var newObj:PsObject = new PsObject();
						newObj.fromObject(value);
						setPsObject(key, newObj);
						break;
						
					case PsType.TYPE_PSARRAY:
						var newArray:PsArray = new PsArray();
						newArray.fromObject(value);
						setPsArray(key, newArray);
						break;	
					
					case PsType.TYPE_NUMBER:
						setNumber(key, Number(value));
						break;
						
					default:
						throw new Error("TYPE NOT SUPPORTED!!!");
						break;
				}
			}
		}
		
	}
}