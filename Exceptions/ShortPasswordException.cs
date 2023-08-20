namespace ClassroomPlus.Exceptions;

public class ShortPasswordException : Exception
{
    public ShortPasswordException(string msg) : base(msg) { }
}
