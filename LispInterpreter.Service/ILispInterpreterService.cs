namespace LispInterpreter.Service
{
    public interface ILispInterpreterService
    {
        int InterpretExpression(List<string> expressions);
    }
}
