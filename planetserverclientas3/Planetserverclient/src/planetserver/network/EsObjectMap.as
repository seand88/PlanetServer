package planetserver.network {
	
    /**
     * This class is used internally to map a name to an EsObject value..
     */
    public class EsObjectMap {

        private var value:EsObject;
        private var key:String;
		
        /**
         * Creates a new instance of the EsObjectMap class.
         * @param    Name of the EsObject.
         * @param    The EsObject.
         */
        public function EsObjectMap(nm:String, mp:EsObject) {
            key = nm;
            value = mp;
        }
		
        /**
         * Retunrs the EsObject.
         * @return Returns the EsObject.
         */
        public function getValue():EsObject {
            return value;
        }
		
        /**
         * Gets the name of the EsObject.
         * @return The name of the EsObject.
         */
        public function getName():String {
            return key;
      }
    }
}
