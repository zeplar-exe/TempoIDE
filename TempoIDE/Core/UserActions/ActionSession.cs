using System.Collections.Generic;

namespace TempoIDE.Core.UserActions
{
    public class ActionSession
    {
        private LinkedListNode<IUserAction> currentActionNode;
        private IUserAction CurrentAction => currentActionNode?.Value;
        
        public readonly object Id;
        public readonly LinkedList<IUserAction> Actions = new();

        public ActionSession(object id)
        {
            Id = id;
            currentActionNode = Actions.AddFirst(new SessionCreatedAction());
        }

        public void AddAction(IUserAction action)
        {
            currentActionNode = Actions.AddAfter(currentActionNode, action);
        }

        public ActionResult Undo()
        {
            if (CurrentAction is SessionCreatedAction)
                return new ActionResult(false, "Nothing to undo.");

            var result = CurrentAction.Undo();
            currentActionNode = currentActionNode.Previous;

            return result;
        }

        public ActionResult Redo()
        {
            if (currentActionNode.Next == null)
                return new ActionResult(false, "Nothing to redo.");

            var result = currentActionNode.Next.Value.Redo();
            currentActionNode = currentActionNode.Next;

            return result;
        }
    }
    
    public class SessionCreatedAction : IUserAction
    {
        public ActionResult Undo()
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Redo()
        {
            throw new System.NotImplementedException();
        }
    }
}