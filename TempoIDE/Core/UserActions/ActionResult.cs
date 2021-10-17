namespace TempoIDE.Core.UserActions
{
    public readonly struct ActionResult
    {
        public readonly bool Success;
        public readonly string Message;

        public ActionResult(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}