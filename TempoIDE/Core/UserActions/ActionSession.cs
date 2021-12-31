using System.Collections.Generic;

namespace TempoIDE.Core.UserActions;

public class ActionSession
{
    private LinkedListNode<IUserAction>? CurrentActionNode { get; set; }
    private IUserAction? CurrentAction => CurrentActionNode?.Value;
        
    public object Id { get; }
    public LinkedList<IUserAction> Actions { get; } = new();

    public ActionSession(object id)
    {
        Id = id;
        CurrentActionNode = Actions.AddFirst(new SessionCreatedAction());
    }

    public void AddAction(IUserAction action)
    {
        if (CurrentActionNode == null)
            return;
        
        CurrentActionNode = Actions.AddAfter(CurrentActionNode, action);
    }

    public ActionResult Undo()
    {
        if (CurrentAction is SessionCreatedAction or null)
            return new ActionResult(false, "Nothing to undo.");

        var result = CurrentAction.Undo();
        CurrentActionNode = CurrentActionNode?.Previous;

        return result;
    }

    public ActionResult Redo()
    {
        if (CurrentActionNode?.Next == null)
            return new ActionResult(false, "Nothing to redo.");

        var result = CurrentActionNode.Next.Value.Redo();
        CurrentActionNode = CurrentActionNode.Next;

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