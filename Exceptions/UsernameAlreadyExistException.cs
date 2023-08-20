namespace ClassroomPlus.Exceptions;

public class UsernameAlreadyExistException : Exception
{
    public UsernameAlreadyExistException(string msg) : base(msg) { }
}
