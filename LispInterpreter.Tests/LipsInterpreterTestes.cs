using LispInterpreter.Service;

namespace LispInterpreter.Tests
{
    public class Tests
    {
        private ILispInterpreterService _interpreterService;

        [SetUp]
        public void Setup()
        {
            _interpreterService = new LispInterpreterService();
        }

        [Test]
        public void BasicSumTest()
        {
            var result = _interpreterService.InterpretExpression(new List<string> { "(+ 2 3)" });
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void BasicMultiplicationTest()
        {
            var result = _interpreterService.InterpretExpression(new List<string> { "(* 4 5)" });
            Assert.That(result, Is.EqualTo(20));
        }

        [Test]
        public void NestedExpression()
        {
            var result = _interpreterService.InterpretExpression(new List<string> { "(* (+ 2 3) (- 5 2))" });
            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void VariableDefinition()
        {
            var result = _interpreterService.InterpretExpression(new List<string> { "(define a 10)", "(+ a 5)" });
            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void ConditionalLogic()
        {
            var result = _interpreterService.InterpretExpression(new List<string> { "(if (> 10 5) (+ 1 1) (+ 2 2))" });
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void FunctionDefinitionAndExecution()
        {
            var result = _interpreterService.InterpretExpression(new List<string> { "(define (add x y) (+ x y))", "(add 3 4)" });
            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        public void FunctionDefinitionAndExecution_Subtract()
        {
            var result = _interpreterService.InterpretExpression(new List<string> { "(define (sub x y) (- x y))", "(sub 10 4)" });
            Assert.That(result, Is.EqualTo(6));
        }
    }
}