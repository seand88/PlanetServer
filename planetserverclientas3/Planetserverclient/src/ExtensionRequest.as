package 
{
	import com.adobe.serialization.json.JSON;
	import planetserver.network.EsObject;
	import planetserver.util.PSConstants;
	public class ExtensionRequest 
	{
		private var _command:String;
		private var _params:EsObject;

		public function ExtensionRequest (extCmd:String, params:EsObject, room:Room = null)
		{
			_command = extCmd;
			_params = params;
		}
		
		public function generateMessage():String
		{
			_params.setString(PSConstants.COMMAND, _command);
			return _params.toXML();
		}
	}
}
