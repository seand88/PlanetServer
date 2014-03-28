package planetserver.channel;
import planetserver.network.PsObject;

/**
 *
 * @author Sean
 */
public interface ChannelWriter
{
    public void send(PsObject params);
    public void sendPolicy(String policyString);
}
