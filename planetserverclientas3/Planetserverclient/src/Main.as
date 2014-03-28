package 
{
	import adobe.utils.CustomActions;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.events.SecurityErrorEvent;
	import flash.net.Socket;
	import flash.utils.ByteArray;
	import planetserver.network.EsObject;
	import planetserver.network.PsArray;
	import planetserver.network.PsDataWrapper;
	import planetserver.network.PsObject;

	/**
	 * ...
	 * @author sean
	 */
	[Frame(factoryClass="Preloader")]
	public class Main extends Sprite 
	{
		private var _server:PlanetServer;
		
		public function Main():void 
		{
			if (stage) init();
			else addEventListener(Event.ADDED_TO_STAGE, init);
		}

		private function init(e:Event = null):void 
		{
			removeEventListener(Event.ADDED_TO_STAGE, init);
			/**
			_server = new PlanetServer();
			var loginObj:EsObject = new EsObject();
			loginObj.setString("username", "login");
			var loginRequest:ExtensionRequest = new ExtensionRequest("login", loginObj);
			_server.send(loginRequest);
			
			
			var obj:EsObject = new EsObject();
			obj.setInteger("hello", 1);
			obj.setInteger("hello", 2);
			obj.setInteger("hello", 3);
			obj.setInteger("hello", 4);
			obj.setInteger("hello", 5);
			obj.setInteger("hello", 6);
			for (var i:int = 0; i < 10000; i++) 
			{
				var request2:ExtensionRequest = new ExtensionRequest("character", obj);
				_server.send(request2);
			}
			*/
			
			//testing ps object classes
			var psObject:PsObject = new PsObject();
			//psObject.setString("hello", "hello from ps object!")
	
			//psObject.setInt("best number", 555550);
			
			//psObject.setBoolean("thegame", false);
			
		    var newps:PsObject = new PsObject();
			newps.setInt("new object", 10);
			
			//psObject.setPsObject("psobj", newps);
			
			var array:PsArray = new PsArray();
			array.addInt(2);
			array.addInt(3);
			array.addInt(4);
			array.addString("hello1");
			array.addString("hello2");
			array.addPsObject(newps);
			psObject.setPsArray("psarray", array);
			
			var jsonString:String = JSON.stringify(psObject.toObject());
			trace("JSON STRING OUT: " + jsonString);
			
			var outPs:PsObject = new PsObject();
			var jsonObject:Object = JSON.parse(jsonString);
			outPs.fromObject(jsonObject);
			
			trace("JSON STRING IN : " + JSON.stringify(outPs.toObject()));
			
			var outArray:Array = outPs.getArray("psarray")
			trace(outArray);
			/**
			PsObject psObject = new PsObject();
			psObject.setString("hello", "hello from ps object!");
			psObject.setBoolean("thegame", true);
			PsObject newps = new PsObject();
			newps.setInt("new object", 10);
			psObject.setPsObject("psobj", newps);
			PsArray array = new PsArray();
			array.addInt(10);
			array.addInt(20);
			array.addInt(30);
			psObject.setPsArray("array", array);
			String psOut = psObject.toJSON().toString();
			logger.debug("PS OUT : " + psOut);
			JSONObject jsonObject = JSONObject.fromObject(psOut);
			PsObject psIn = new PsObject(jsonObject);
			String hello = psObject.getString("hello");
			logger.debug("HELLO! VARIABLE : " + hello);
			logger.debug("PS IN : " + psIn.toJSON().toString());			
			*/
			
		}
		
	}
}