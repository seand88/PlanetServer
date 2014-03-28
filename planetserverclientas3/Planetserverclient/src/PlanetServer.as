package  
{
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.events.SecurityErrorEvent;
	import flash.net.Socket;
	import flash.utils.ByteArray;
	import flash.xml.XMLDocument;
	import flash.xml.XMLNode;
	import planetserver.network.EsObject;
	/**
	 * ...
	 * @author sean
	 */
	public class PlanetServer 
	{
		private var _socket:Socket;
		private var _debugEnabled:Boolean = true;
		
		private var _currentMessage:String = "";
		
		public function PlanetServer() 
		{
			var remoteHost:String = "127.0.0.1";
			var remotePort:int = 8000;
			
			_socket = new Socket(remoteHost, remotePort);
			_socket.addEventListener(Event.CONNECT, socketConnect);
			_socket.addEventListener(Event.CLOSE, socketClose);
			_socket.addEventListener(IOErrorEvent.IO_ERROR, socketError);
			_socket.addEventListener(SecurityErrorEvent.SECURITY_ERROR, securityError);
			_socket.addEventListener(ProgressEvent.SOCKET_DATA, socketData);
			
			try {
                	trace("Trying to connect to " + remoteHost + ":" + remotePort + "\n");
                	_socket.connect(remoteHost, remotePort);
			} catch (error:Error) {
			/*
			 *   Unable to connect to remote server, display error 
			 *   message and close connection.
			 */
			    trace(error.message + "\n");
			    _socket.close();
			    throw (error);
			}	
		}
		
		public function send(request:ExtensionRequest):void
		{
			//dtrace("SENDING MESSAGE : " + request.generateMessage());
			var message:ByteArray = new ByteArray();
			message.writeUTFBytes(request.generateMessage() + String.fromCharCode(0));
			//having problems with receiving compress messages not using for now
			//message.compress(); 
			_socket.writeBytes(message);
			_socket.flush();
		}
		
		private function dtrace(msg:String):void 
		{
			if (_debugEnabled)
				trace(msg);
		}
		
		private function socketConnect(event:Event):void 
		{

		}

		private function socketData(event:ProgressEvent):void
		{
			while (_socket.bytesAvailable) // while there is byte to read
			{ 
				var byte:int = _socket.readByte();
				if (byte == 0) // if we read the end byte
				{ 
					//trace("FOUND BYTE ZERO!");
					var msg:String = _currentMessage;
					_currentMessage = "";
					//convert the message and send it off
					convertMessage(msg)
				} 
				else
				{
					_currentMessage += String.fromCharCode(byte); // else, we add the byte to our buffer
				}
			}
		}
		
		private function convertMessage(msg:String):void 
		{
			trace("CONSTRUCTED MESSAGE : " + msg);
			//TODO: Convert Message into a PSObject...
			var xmlDocument:XMLDocument = new XMLDocument();
			xmlDocument.ignoreWhite = true;
			xmlDocument.parseXML(msg);
			var xmlNode:XMLNode = xmlDocument.firstChild;
			var esob:EsObject = new EsObject();
			esob.fromXML(xmlNode);	
			trace("COMMAND : " + esob.getString("command"));
			trace("ESOBJ SIZE : " + esob.getSize());
		}

		private function socketClose(event:Event):void
		{
			trace("Connection closed: " + event);
			//this.chatArea.appendText("Connection lost." + "\n");
		}

		private function socketError(event:IOErrorEvent):void
		{
			trace("Socket error: " + event);
		}

		private function securityError(event:SecurityErrorEvent):void
		{
			trace("Security error: " + event);
		}			
		
		
	}

}