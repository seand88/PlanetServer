package planetserver.handler;

import net.sf.json.JSONObject;
import planetserver.channel.PsChannelWriter;
import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.channel.ChannelStateEvent;
import org.jboss.netty.channel.ExceptionEvent;
import org.jboss.netty.channel.MessageEvent;
import org.jboss.netty.channel.SimpleChannelHandler;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import planetserver.core.PSExtension;
import planetserver.network.PsObject;
import planetserver.room.RoomManager;
import planetserver.session.SessionManager;
import planetserver.session.UserSession;


public class PsChannelHandler extends SimpleChannelHandler 
{
        private static final Logger logger = LoggerFactory.getLogger( PsChannelHandler.class );
        private RoomManager roomManager;
        private SessionManager sessionManager;
        private PSExtension extension;
         
	public PsChannelHandler(PSExtension extension, SessionManager sessionManager, RoomManager roomManager) 
        {
            this.extension = extension;
            this.roomManager = roomManager;
            this.sessionManager = sessionManager;
	}

	@Override
	public void channelConnected(ChannelHandlerContext ctx, ChannelStateEvent e)
        {
            UserSession session = new UserSession(e.getChannel(), new PsChannelWriter(e.getChannel()));
            this.sessionManager.registerSession(session);
            logger.debug("Session Added, Total Sessions: " + sessionManager.getNumberOfSessions());
	}

	@Override
	public void channelDisconnected(ChannelHandlerContext ctx, ChannelStateEvent e)
        {
            UserSession session = this.sessionManager.getSession(e.getChannel());
            //remove the user from any rooms they are in!
            if (session.getCurrentRoom().length() > 1)
            {
                roomManager.getRoom(session.getCurrentRoom()).removeUserFromRoom(session);
            }
            //disconnect the session!
            this.sessionManager.unregisterSession(session);
           // logger.debug("Session Removed, Total Sessions: " + sessionManager.getNumberOfSessions());
	}
        
	@Override
	public void channelClosed(ChannelHandlerContext ctx, ChannelStateEvent e)
        {   
            /**
            UserSession session = this.sessionManager.getSession(e.getChannel());
            //remove the user from any rooms they are in!
            if (session.getCurrentRoom().length() > 1)
            {
                logger.debug("REMOVING USER FROM ROOM WITH ID: " + session.getId());
                roomManager.getRoom(session.getCurrentRoom()).removeUserFromRoom(session);
            }
            //disconnect the session!
            this.sessionManager.unregisterSession(session);
            logger.debug("Channel Closed, Total Sessions: " + sessionManager.getNumberOfSessions());
            */
	}        
        
	
	@Override
	public void messageReceived(ChannelHandlerContext ctx, MessageEvent e) throws Exception
        {
            String msg = (String) e.getMessage();
            String policyRequest = "<policy-file-request/>";
            
            UserSession session = sessionManager.getSession(e.getChannel());
            
            if (msg.equalsIgnoreCase(policyRequest.trim()))
            {
                extension.handlePolicyRequest(session); 
            }
            else
            {
               JSONObject jsonObject = JSONObject.fromObject(msg);
               PsObject messageIn = new PsObject();
               messageIn.fromJsonObject(jsonObject);
                
               extension.handleClientRequest(session, messageIn); 
            }
            //since this should be the last handler, no need to worry about other handlers upstream
            //super.messageReceived(ctx, e);
	}

	@Override
	public void exceptionCaught(ChannelHandlerContext ctx, ExceptionEvent e) {
             
            e.getCause().printStackTrace();
          
	}

}
