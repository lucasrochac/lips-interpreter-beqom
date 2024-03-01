namespace LispInterpreter.Domain
{
    public class Function
    {
        public List<string> Parameters { get; set; }

        public string Body
        {
            get
            {
                return  $"({string.Join(" ", Parameters)})";
            }
        }

        public Function(List<string> parameters)
        {
            Parameters = parameters;
        }

        public override string ToString()
        {
            return Body;
        }
    }


}