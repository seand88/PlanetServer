package util 
{
	import flash.utils.ByteArray;
	/**
	 * ...
	 * @author sean
	 */
	public class NetUtil 
	{
		
			public static function compress( str:String ) :String
			{
			   var b:ByteArray = new ByteArray();
			   b.writeObject( str );
			   b.compress();
			   return Base64.Encode( b );
			}

			public static function uncompress( str:String ) :String
			{
			   var b:ByteArray = Base64.Decode( str );
			   b.uncompress();
			   return b.toString();
			}
		
	}

}