package util;

import java.util.List;

import planetserver.room.Room;
import planetserver.session.UserSession;

/**
 *
 * @author Mike
 */
public class UserHelper
{
    public static List<UserSession> getRecipientsList(Room room, UserSession exceptUser)
    {
	List<UserSession> users = room.getPeopleInRoom();
	if (exceptUser != null)
            users.remove(exceptUser);

	return users;
    }

    public static List<UserSession> getRecipientsList(Room room) { return getRecipientsList(room, null); }
}
