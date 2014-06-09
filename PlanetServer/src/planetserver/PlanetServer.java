package planetserver;

import planetserver.core.PSExtension;
import planetserver.core.CoreServer;
import planetserver.room.RoomManager;
import planetserver.session.SessionManager;
import java.io.File;
import java.io.FileReader;
import java.io.FilenameFilter;
import java.util.Properties;

import org.apache.log4j.PropertyConfigurator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import planetserver.handler.BasicServerEventHandler;
import planetserver.util.JarLoader;

/**
 *
 * @author Sean
 */
public class PlanetServer
{
    private static final Logger logger = LoggerFactory.getLogger(PlanetServer.class);
    private static PlanetServer instance;
    private RoomManager roomManager;
    private SessionManager sessionManager;
    private PSExtension extension;
    private Properties serverProperties;
    private Properties extensionProperties;
    private CoreServer coreServer;

    private PlanetServer() throws Exception
    {
        //load any properties here before we initialize!
        PropertyConfigurator.configure("./conf/log4j.properties");
    }

    public static PlanetServer getInstance()
    {
        if (instance == null)
        {
            try
            {
                instance = new PlanetServer();
                instance.init();
            }
            catch (Exception e)
            {
                e.printStackTrace();
                System.exit(-1);
            }
        }
        return instance;
    }

    private void init()
    {
        roomManager = new RoomManager();
        sessionManager = new SessionManager();
        loadResources();
    }

    private void loadResources()
    {
        try
        {
            //first load the properties file for the server properties
            loadServerPropertiesFile();

            //load the jars in the extensions folder
            File extensionDir = new File("extension");
            loadJars(extensionDir);
            loadExtension(extensionDir);
        }
        catch (Exception ex)
        {
            logger.error(ex.toString());
        }
    }

    private void loadExtension(File extensionDir) throws Exception
    {

        File[] propertyFiles = extensionDir.listFiles(new FilenameFilter()
        {
            @Override
            public boolean accept(File dir, String name)
            {
                return name.endsWith(".properties");
            }
        });

        for (File propertyFile : propertyFiles)
        {
            logger.debug("Loading property file: " + propertyFile.getAbsoluteFile());

            extensionProperties = new Properties();
            extensionProperties.load(new FileReader(propertyFile));

            String extensionClass = extensionProperties.getProperty("extension.class.name");
            try
            {
                Class<?> cls = Class.forName(extensionClass);
                extension = (PSExtension)cls.newInstance();
                extension.setRoomManager(roomManager);
                extension.setProperties(extensionProperties);
            }
            catch (Exception e)
            {
                logger.warn("Could not load module " + extensionClass);
                throw e;
            }

            return;//only one properties file!
        }
        //if we didnt find a file there is a problem! throw an error
        throw new Exception("no extension propety file found!");
    }

    private void loadJars(File extensionDir) throws Exception
    {
        File[] jarFiles = extensionDir.listFiles(new FilenameFilter()
        {
            @Override
            public boolean accept(File dir, String name)
            {
                return name.endsWith(".jar");
            }
        });

        for (File jarFile : jarFiles)
        {
            JarLoader.loadJar(jarFile);
        }
    }

    private void loadServerPropertiesFile() throws Exception
    {
        serverProperties = new Properties();
        serverProperties.load(new FileReader(new File("./conf/config.properties")));
    }

    public void start()
    {
        coreServer = new CoreServer(serverProperties, extension, sessionManager, roomManager);
        coreServer.start();
    }

    public void stop()
    {
        coreServer.stop();
    }

    /**
     * @param args
     */
    public static void main(String[] args) throws Exception
    {
        PlanetServer.getInstance().start();
    }
}
