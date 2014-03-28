package planetserver.channel;

import org.jboss.netty.channel.Channel;
import planetserver.network.PsObject;

public class PsChannelWriter implements ChannelWriter
{
	private Channel channel;
	
	public PsChannelWriter(Channel channel) 
        {	
            this.channel = channel;
	}
	
	public void send(PsObject params)
        {	
            String message = params.toJsonString();
            channel.write(message + '\0');			
	}
        
        public void sendPolicy(String policyString)
        {
            channel.write(policyString + '\0');	
        }
}
