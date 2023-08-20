namespace ClassroomPlus.Exceptions;

public class UnauthorizedContentException : Exception
{
    public UnauthorizedContentException(string msg) : base(msg) {}
}
