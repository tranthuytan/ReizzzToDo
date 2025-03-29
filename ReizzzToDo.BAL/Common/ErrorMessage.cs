namespace ReizzzToDo.BAL.Common
{
    public static class ErrorMessage
    {
        public const string WrongUserAccessToDo = "You don't have permission to access this ToDo";
        public const string ToDoCanBeUpdatedWithNameOrCompleteStateChanged = "ToDo can be updated with name or complete state changed";
        public const string NoToDoWithThatId = "There's no ToDo with that Id {0}";
        public const string UserIdNotMatchWithAnyUser = "Critical error. The user is deleted while process this function";
        public const string JwtClaimCantReached = "No access to jwt claims";
    }
}
