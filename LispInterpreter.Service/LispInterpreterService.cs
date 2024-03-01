using LispInterpreter.Domain;

namespace LispInterpreter.Service
{
    public class LispInterpreterService : ILispInterpreterService
    {
        private Dictionary<string, int> variables = new Dictionary<string, int>();
        private Dictionary<string, Function> functions = new Dictionary<string, Function>();

        public int InterpretExpression(List<string> expressions)
        {
            var result = 0;

            foreach (var expression in expressions)
            {
                result += Evaluate(expression);
            }

            return result;
        }

        private int Evaluate(string expression)
        {
            if (IsNumeric(expression))
            {
                return int.Parse(expression);
            }
            else if (expression.StartsWith("("))
            {
                var tokens = Tokenize(expression.Substring(1, expression.Length - 2));
                var op = tokens[0];

                if (op == "define")
                {
                    if (IsNumeric(tokens[2]))
                    {
                        DefineVariable(tokens[1], int.Parse(tokens[2]));
                    }
                    else
                    {
                        DefineFunction(tokens);
                    }

                    return 0;
                }
                else if (op == "if")
                {
                    var args = tokens.Skip(1).Select(Evaluate).ToList();
                    return EvaluateIf(args);
                }
                else if (IsFunctionCall(tokens))
                {
                    return CallFunction(tokens);
                }
                else
                {
                    var args = tokens.Skip(1).Select(Evaluate).ToList();
                    return PerformOperation(op, args);
                }
            }
            else
            {
                return variables[expression];
            }
        }

        private List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            var token = "";
            var parenCount = 0;

            foreach (var c in expression)
            {
                if (c == '(')
                {
                    parenCount++;
                }
                else if (c == ')')
                {
                    parenCount--;
                }

                if (c == ' ' && parenCount == 0)
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        tokens.Add(token);
                        token = "";
                    }
                }
                else
                {
                    token += c;
                }
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                tokens.Add(token);
            }

            return tokens;
        }

        private bool IsNumeric(string str)
        {
            return int.TryParse(str, out _);
        }

        private int PerformOperation(string op, List<int> args)
        {
            switch (op)
            {
                case "+":
                    return args.Sum();
                case "-":
                    return args.Aggregate((x, y) => x - y);
                case "*":
                    return args.Aggregate((x, y) => x * y);
                case "/":
                    return args.Aggregate((x, y) => x / y);
                case ">":
                    return args[0] > args[1] ? 1 : 0;
                case "<":
                    return args[0] < args[1] ? 1 : 0;
                case "==":
                    return args[0] == args[1] ? 1 : 0;
                case "!=":
                    return args[0] != args[1] ? 1 : 0;
                default:
                    throw new ArgumentException("Invalid operation");
            }
        }

        private void DefineVariable(string name, int value)
        {
            if (variables.ContainsKey(name))
            {
                variables[name] = value;
            }
            else
            {
                variables.Add(name, value);
            }
        }

        private int EvaluateIf(List<int> args)
        {
            var condition = args[0];
            var trueBranch = args[1];
            var falseBranch = args[2];

            return condition != 0 ? trueBranch : falseBranch;
        }

        private bool IsFunctionCall(List<string> tokens)
        {
            return functions.ContainsKey(tokens[0]);
        }

        private int CallFunction(List<string> tokens)
        {
            var functionName = tokens[0];
            var args = tokens.Skip(1).Select(Evaluate).ToList();

            if (!functions.ContainsKey(functionName))
            {
                throw new ArgumentException($"Function {functionName} not defined.");
            }

            var function = functions[functionName];

            if (args.Count != function.Parameters.Count - 1)
            {
                throw new ArgumentException($"Incorrect number of arguments for function {functionName}.");
            }

            var parameters = new List<string>(function.Parameters);
            parameters.RemoveAt(0);

            for (int i = 0; i < parameters.Count; i++)
            {
                variables[parameters[i]] = args[i];
            }

            var bodyWithValues = function.Body;
            for (int i = 0; i < parameters.Count; i++)
            {
                bodyWithValues = bodyWithValues.Replace(parameters[i], args[i].ToString());
            }

            return Evaluate(bodyWithValues);
        }

        private void DefineFunction(List<string> tokens)
        {
            var tokenizedFunction = tokens[1].Substring(1).Split(' ').ToList();
            var parameters = tokens[2].Substring(1).Split(' ').ToList();

            functions[tokenizedFunction[0]] = new Function(parameters);
        }
    }

}
