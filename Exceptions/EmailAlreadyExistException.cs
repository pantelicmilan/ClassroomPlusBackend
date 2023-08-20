namespace ClassroomPlus.Exceptions;

public class EmailAlreadyExistException : Exception
{
    public EmailAlreadyExistException(string msg): base(msg) { }
}
