package planetserver.session;

import java.util.Collection;
import java.util.concurrent.ConcurrentHashMap;

import org.jboss.netty.channel.Channel;

public class SessionManager {
	

	private ConcurrentHashMap<Integer, UserSession> sessions;
	
	public SessionManager() {
		this.sessions = new ConcurrentHashMap<Integer, UserSession>();
	}
	
	public void registerSession(UserSession session) {
                this.sessions.put(session.getChannel().getId(), session);
	}
	
	public void unregisterSession(UserSession session) {
                this.sessions.remove(session.getChannel().getId());
	}
	
	public UserSession getSession(Channel channel) {
                return this.sessions.get(channel.getId());
	}
	
	public UserSession getSession(int id) {
		return this.sessions.get(id);
	}
	
	public Collection<UserSession> getSessions() {
		return this.sessions.values();
	}
	
	public int getNumberOfSessions() {
		return this.sessions.size();
	}

}
