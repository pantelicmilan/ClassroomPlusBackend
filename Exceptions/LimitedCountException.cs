namespace ClassroomPlus.Exceptions;

public class LimitedCountException: Exception
{
    public LimitedCountException(string msg) : base(msg) { }
}
