namespace ClassroomPlus.Exceptions;

public class WrongPasswordException : Exception
{
    public WrongPasswordException(string msg) : base(msg) { }
}
