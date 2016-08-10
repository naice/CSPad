using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSPad.Execution;

namespace CSPadUnitTest
{
    [TestClass]
    public class TestCSharpExpressionEvaluator
    {
        const string DateTimeNowExpression = "DateTime.Now";
        const string StringArrayExpression = "new string[] {\"a\",\"b\" }[1]";

        private CSharpExpressionEvaluator CreateEvaluator(string expression)
        {
            CSharpExpressionEvaluator evaluator = new CSharpExpressionEvaluator();
            evaluator.Expression = expression;

            return evaluator;
        }

        [TestMethod]
        public void DateTimeNowEvalutaionIsTypeDateTime()
        {
            var evaluator = CreateEvaluator(DateTimeNowExpression);

            var result = evaluator.Execute();

            Assert.IsTrue(result is DateTime);
        }
        [TestMethod]
        public void DateTimeNowEvalutaionIsWithinExpectedRange()
        {
            var evaluator = CreateEvaluator(DateTimeNowExpression);

            var min = DateTime.Now.AddMilliseconds(-500);
            var max = DateTime.Now.AddMilliseconds(500);
            var result = (DateTime)evaluator.Execute();

            Assert.IsTrue(result > min && result < max);
        }
        [TestMethod]
        public void RunningTwoEvaluationsWithOneEvaluator()
        {
            var evaluator = CreateEvaluator(DateTimeNowExpression);

            var min = DateTime.Now.AddMilliseconds(-500);
            var max = DateTime.Now.AddMilliseconds(500);
            var result1 = (DateTime)evaluator.Execute();

            Assert.IsTrue(result1 > min && result1 < max, "First failed.");

            evaluator.Expression = StringArrayExpression;
            var result2 = evaluator.Execute() as string;

            Assert.IsTrue(result2 == "b");
        }


    }
}
