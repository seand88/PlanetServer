package planetserver.core;


import java.net.InetSocketAddress;
import java.util.Properties;
import java.util.concurrent.Executors;


import org.jboss.netty.bootstrap.ServerBootstrap;
import org.jboss.netty.channel.Channel;
import org.jboss.netty.channel.ChannelFactory;

import org.jboss.netty.channel.group.ChannelGroup;
import org.jboss.netty.channel.group.ChannelGroupFuture;
import org.jboss.netty.channel.group.DefaultChannelGroup;
import org.jboss.netty.channel.socket.nio.NioServerSocketChannelFactory;
import org.jboss.netty.handler.execution.ExecutionHandler;
import org.jboss.netty.handler.execution.OrderedMemoryAwareThreadPoolExecutor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import planetserver.PlanetServer;


import planetserver.handler.PsChannelHandler;
import planetserver.room.RoomManager;
import planetserver.session.SessionManager;

public class CoreServer {
        private static final Logger logger = LoggerFactory.getLogger( CoreServer.class );
    
	static final ChannelGroup allChannels = new DefaultChannelGroup(
			"Planet-Server");

	private Properties properties;
	private PSExtension extension;
	private SessionManager sessionManager;
        private RoomManager roomManager;

	private ChannelFactory factory;
	private Channel channelJSon;
        
        ExecutionHandler executionHandler;
        ServerBootstrap bootstrap;
        
        private int port;
        public  int executorThreads;

	public CoreServer(Properties properties, PSExtension commandProcessor, SessionManager sessionManager, RoomManager roomManager)
        {
            this.roomManager = roomManager;
            this.properties = properties;
            this.extension = commandProcessor;
            this.sessionManager = sessionManager;
            this.port = Integer.parseInt(properties.getProperty("server.tcp.port"));
            this.executorThreads = Integer.parseInt(properties.getProperty("server.threads.executor"));
	}

	public void start() 
        {
		factory = new NioServerSocketChannelFactory(
				Executors.newCachedThreadPool(),
				Executors.newCachedThreadPool());

                bootstrap = new ServerBootstrap(factory);

                //setup our xml business logic handler
                PsChannelHandler handler = new PsChannelHandler(
                                extension, sessionManager, roomManager);

                //set up our execution handler with the thread pool
                executionHandler = new ExecutionHandler(new OrderedMemoryAwareThreadPoolExecutor(executorThreads, 1048576, 1048576));
                
                bootstrap.setPipelineFactory(new PsPipelineFactory(handler, executionHandler));

                bootstrap.setOption("child.tcpNoDelay", true);
                bootstrap.setOption("child.keepAlive", true);

                channelJSon = bootstrap.bind(new InetSocketAddress(port));

                allChannels.add(channelJSon);
                
                String startString = "\n";
                startString +=        "   _____  _                  _    _____  " + "\n";                        
                startString +=        "  |  __ \\| |                | |  / ____| " + "\n";                          
                startString +=        "  | |__) | | __ _ _ __   ___| |_| (___   __ _ ____   _____ _ __  " + "\n";   
                startString +=        "  |  ___/| |/ _` | '_ \\ / _ \\ __|\\___ \\ / _ \\ '__\\ \\ / / _ \\ '__| " + "\n";   
                startString +=        "  | |    | | (_| | | | |  __/ |_ ____) |  __/ |   \\ V /  __/ |    "  + "\n";   
                startString +=        "  |_|    |_|\\__,_|_| |_|\\___|\\__|_____/ \\___|_|    \\_/ \\___|_|    " + "\n";                                  
                logger.info(startString);                   
                logger.info("Listening on port " +  port);
                
                //init the extension!!
                extension.init();
        }
	

	public void stop()
        {
            ChannelGroupFuture future = allChannels.close();
            future.awaitUninterruptibly();
            factory.releaseExternalResources();
            executionHandler.releaseExternalResources();
            bootstrap.releaseExternalResources();
            //any code needed for destroying the extension..
            extension.destroy();
	}
	
}