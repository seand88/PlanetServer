package planetserver.session;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import org.jboss.netty.channel.Channel;

public class SessionManager 
{
    private ConcurrentHashMap<Integer, UserSession> _sessions;

    public SessionManager()
    {
        _sessions = new ConcurrentHashMap<Integer, UserSession>();
    }

    public void registerSession(UserSession session)
    {
        _sessions.put(session.getChannel().getId(), session);
    }

    public void unregisterSession(UserSession session)
    {
        _sessions.remove(session.getChannel().getId());
    }

    public UserSession getSession(Channel channel)
    {
        return _sessions.get(channel.getId());
    }

    public UserSession getSession(int id)
    {
        return _sessions.get(id);
    }

    public List<UserSession> getSessions()
    {
        return new ArrayList(_sessions.values());
    }

    public int getNumberOfSessions()
    {
        return _sessions.size();
    }
}
