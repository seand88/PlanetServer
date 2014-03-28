package planetserver.util;

import java.io.File;
import java.lang.reflect.Method;
import java.net.URL;
import java.net.URLClassLoader;

public class JarLoader 
{
        public static void loadJar(File file) throws Exception 
        {        
            URL jarFile = new URL("jar","","file:" + file.getAbsolutePath() + "!/");       
            URLClassLoader systemLoader = (URLClassLoader)ClassLoader.getSystemClassLoader();
            Class<?> systemClass = URLClassLoader.class;
            Method systemMethod = systemClass.getDeclaredMethod("addURL",new Class[] {URL.class});
            systemMethod.setAccessible(true);
            systemMethod.invoke(systemLoader, new Object[]{jarFile});
        }
}