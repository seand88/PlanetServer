package planetserver.network 
{
	/**
	 * ...
	 * @author sean
	 */
	public class PsDataWrapper 
	{
		private var _value:Object;
		private var _type:int; //type is stored as an integer
		
		public function PsDataWrapper(value:Object, type:int) 
		{
			this._value = value;
			this._type = type;
		}
		
		public function getValue():Object
		{
			return _value;
		}
		
		public function getType():int
		{
			return _type;
		}
		
		//used by the json parse to store type as t
		public function get t():int 
		{
			return _type;
		}
		
		//used by json parser to store the value
		public function get v():Object 
		{
			return _value;
		}
		
	}
}