using System.Collections.Generic;

namespace TempoIDE.Core.UserActions
{
    public class ActionSession
    {
        private LinkedListNode<IUserAction> currentActionNode;
        private IUserAction CurrentAction => currentActionNode?.Value;
        
        public readonly string Id;
        public readonly LinkedList<IUserAction> Actions = new();

        public ActionSession(string id)
        {
            Id = id;
        }

        public void AddAction(IUserAction action)
        {
            currentActionNode = currentActionNode == null ? Actions.AddFirst(action) : Actions.AddAfter(currentActionNode, action);
        }

        public ActionResult? Undo()
        {
            if (CurrentAction == null)
                return null;

            var result = CurrentAction.Undo();
            currentActionNode = currentActionNode.Previous;

            return result;
        }

        public ActionResult? Redo()
        {
            if (CurrentAction == null)
                return null;

            var result = CurrentAction.Redo();
            currentActionNode = currentActionNode.Next;

            return result;
        }
    }
}