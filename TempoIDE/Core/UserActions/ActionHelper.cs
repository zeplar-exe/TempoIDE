using System.Collections.Generic;
using System.Linq;

namespace TempoIDE.Core.UserActions
{
    public static class ActionHelper
    {
        private static readonly List<IUserAction> actionBuffer = new();
        private static readonly List<ActionSession> sessions = new();

        public static bool LogAction(string id, IUserAction action)
        {
            actionBuffer.Add(action);
            
            var session = GetSession(id);

            if (session == null)
                return false;

            session.AddAction(action);
            
            return true;
        }

        public static ActionResult? Undo(string id)
        {
            return GetSession(id)?.Undo();
        }

        public static ActionResult? Redo(string id)
        {
            return GetSession(id)?.Redo();
        }
        
        public static ActionSession GetSession(string id) => sessions.FirstOrDefault(s => s.Id == id);
    }
}