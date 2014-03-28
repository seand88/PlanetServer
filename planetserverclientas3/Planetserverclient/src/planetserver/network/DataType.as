package planetserver.network {

	import flash.utils.Dictionary;
	public class DataType {
		public static const Integer:DataType = new DataType('Integer');
		public static const EsString:DataType = new DataType('String');
		public static const Double:DataType = new DataType('Double');
		public static const Float:DataType = new DataType('Float');
		public static const Boolean:DataType = new DataType('Boolean');
		public static const Byte:DataType = new DataType('Byte');
		public static const Character:DataType = new DataType('Character');
		public static const Long:DataType = new DataType('Long');
		public static const Short:DataType = new DataType('Short');
		public static const EsObject:DataType = new DataType('EsObject');
		public static const EsObjectArray:DataType = new DataType('EsObjectArray');
		public static const IntegerArray:DataType = new DataType('IntegerArray');
		public static const StringArray:DataType = new DataType('StringArray');
		public static const DoubleArray:DataType = new DataType('DoubleArray');
		public static const FloatArray:DataType = new DataType('FloatArray');
		public static const BooleanArray:DataType = new DataType('BooleanArray');
		public static const ByteArray:DataType = new DataType('ByteArray');
		public static const CharacterArray:DataType = new DataType('CharacterArray');
		public static const LongArray:DataType = new DataType('LongArray');
		public static const ShortArray:DataType = new DataType('ShortArray');
		public static const EsNumber:DataType = new DataType('Number');
		public static const NumberArray:DataType = new DataType('NumberArray');

		private static const NAMES_TO_VALUES:Dictionary = new Dictionary;
		{
			NAMES_TO_VALUES[Integer.name] = Integer;
			NAMES_TO_VALUES[EsString.name] = EsString;
			NAMES_TO_VALUES[Double.name] = Double;
			NAMES_TO_VALUES[Float.name] = Float;
			NAMES_TO_VALUES[Boolean.name] = Boolean;
			NAMES_TO_VALUES[Byte.name] = Byte;
			NAMES_TO_VALUES[Character.name] = Character;
			NAMES_TO_VALUES[Long.name] = Long;
			NAMES_TO_VALUES[Short.name] = Short;
			NAMES_TO_VALUES[EsObject.name] = EsObject;
			NAMES_TO_VALUES[EsObjectArray.name] = EsObjectArray;
			NAMES_TO_VALUES[IntegerArray.name] = IntegerArray;
			NAMES_TO_VALUES[StringArray.name] = StringArray;
			NAMES_TO_VALUES[DoubleArray.name] = DoubleArray;
			NAMES_TO_VALUES[FloatArray.name] = FloatArray;
			NAMES_TO_VALUES[BooleanArray.name] = BooleanArray;
			NAMES_TO_VALUES[ByteArray.name] = ByteArray;
			NAMES_TO_VALUES[CharacterArray.name] = CharacterArray;
			NAMES_TO_VALUES[LongArray.name] = LongArray;
			NAMES_TO_VALUES[ShortArray.name] = ShortArray;
			NAMES_TO_VALUES[EsNumber.name] = EsNumber;
			NAMES_TO_VALUES[NumberArray.name] = NumberArray;
		}

		private var _name:String;

		public function DataType(name:String) {
			this._name = name;
		}

		public function get name():String {
			return this._name;
		}

		public static function valueOf(s:String):DataType {
			return NAMES_TO_VALUES[s];
		}

	}
}
