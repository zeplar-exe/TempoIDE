using System.Collections.Generic;
using System.Linq;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.UserActions
{
    public static class ActionHelper
    {
        private static readonly List<IUserAction> actionBuffer = new();
        private static readonly List<ActionSession> sessions = new();

        public static void ProcessActionResult(ActionResult result)
        {
            if (result.Success)
                return;
            
            ApplicationHelper.MainWindow.Notify($"Action failed to execute\n{result.Message}", NotificationIcon.Error);
        }

        public static bool LogAction(object id, IUserAction action)
        {
            actionBuffer.Add(action);
            
            var session = GetSession(id);

            if (session == null)
                return false;

            session.AddAction(action);
            
            return true;
        }

        public static ActionResult? Undo(object id)
        {
            return GetSession(id)?.Undo();
        }

        public static ActionResult? Redo(object id)
        {
            return GetSession(id)?.Redo();
        }
        
        public static ActionSession GetSession(object id) => sessions.FirstOrDefault(s => s.Id == id);

        public static ActionSession GetOrCreateSession(object id)
        {
            var session = GetSession(id);

            if (session == null)
            {
                session = new ActionSession(id);
                sessions.Add(session);
            }

            return session;
        }
    }
}