using NUnit.Framework;
using Calculi.Shared;
using System.Collections.Generic;
using System;

namespace Calculi.Tests
{
    public class Tests
    {

        CalculatorIO io;

        [SetUp]
        public void Setup()
        {
            io = new CalculatorIO();
        }

        [Test]
        public void Addition()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.ONE);
            io.InsertSymbol(Symbol.ZERO);
            Assert.AreEqual(Convert.ToDouble(15), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void AdditionWithDecimal()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.FIVE);
            Assert.AreEqual(Convert.ToDouble(6.0), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Subtraction()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.SUBTRACT);
            io.InsertSymbol(Symbol.ONE);
            io.InsertSymbol(Symbol.ZERO);
            Assert.AreEqual(Convert.ToDouble(-5), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void SubtractionWithDecimal()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.SUBTRACT);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.EIGHT);
            Assert.AreEqual(Convert.ToDouble(4.7), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Multiplication()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.FIVE);
            Assert.AreEqual(Convert.ToDouble(100), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Multiplication2()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.TWO);
            Assert.AreEqual(Convert.ToDouble(200), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Division()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.DIVIDE);
            io.InsertSymbol(Symbol.FIVE);
            Assert.AreEqual(Convert.ToDouble(4), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Division2()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.DIVIDE);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.DIVIDE);
            io.InsertSymbol(Symbol.TWO);
            Assert.AreEqual(Convert.ToDouble(2), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void DivisionMultiplication()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.DIVIDE);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.TWO);
            Assert.AreEqual(Convert.ToDouble(8), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Modulo()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.DIVIDE);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.TWO);
            Assert.AreEqual(Convert.ToDouble(8), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void ModuloWithAddition()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.MODULO);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.TWO);
            Assert.AreEqual(Convert.ToDouble(4), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void ModuloWithMultiplication()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.MODULO);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.TWO);
            Assert.AreEqual(Convert.ToDouble(4), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Parenthesis()
        {
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Convert.ToDouble(5), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Parenthesis2()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Convert.ToDouble(10), io.ToCalculation().Compute().ToDouble());
        }

        [Test]
        public void ParenthesisWithAddition1()
        {
            io.InsertSymbol(Symbol.FOUR);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Convert.ToDouble(14), io.ToCalculation().Compute().ToDouble());
        }

        [Test]
        public void ParenthesisWithAddition2()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.FOUR);
            Assert.AreEqual(Convert.ToDouble(14), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void ParenthesisWithAddition3()
        {
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.FOUR);
            Assert.AreEqual(Convert.ToDouble(14), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void ParenthsisNested()
        {
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.ONE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Convert.ToDouble( 2*(2*(3+1)) ), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void ParenthesisMadness()
        {
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.DIVIDE);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.SUBTRACT);
            io.InsertSymbol(Symbol.ONE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.SUBTRACT);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.EIGHT);
            io.InsertSymbol(Symbol.SUBTRACT);
            io.InsertSymbol(Symbol.FOUR);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.DIVIDE);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.FIVE);
            Assert.AreEqual(Convert.ToDouble( (2+3.3)*(2/(3-1))-2*(8-4)/2*5 ), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void LongExpression()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.MULTIPLY);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.SUBTRACT);
            io.InsertSymbol(Symbol.FOUR);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.DIVIDE);
            io.InsertSymbol(Symbol.FOUR);
            Assert.AreEqual(Convert.ToDouble(5.5+0.5+(2+3*3.5)*2-40/4), io.ToCalculation().Compute().ToDouble()); // should be 21
        }
        [Test]
        public void LeadingPoint()
        {
            io.InsertSymbol(Symbol.POINT);
            io.InsertSymbol(Symbol.FIVE);
            Assert.AreEqual(Convert.ToDouble(0.5), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void LeadingMinus()
        {
            io.InsertSymbol(Symbol.SUBTRACT);
            io.InsertSymbol(Symbol.FIVE);
            Assert.AreEqual(Convert.ToDouble(-5), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void LeadingMinusSquared()
        {
            io.InsertSymbol(Symbol.SUBTRACT);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.SQR);
            Assert.AreEqual(Convert.ToDouble(-25), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Squared()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.SQR);
            Assert.AreEqual(Convert.ToDouble(25), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void SquaredBrackets()
        {
            io.InsertSymbol(Symbol.LEFT_PARENTHESIS);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ADD);
            io.InsertSymbol(Symbol.THREE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            io.InsertSymbol(Symbol.SQR);
            Assert.AreEqual(Convert.ToDouble(25), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Logarithm()
        {
            io.InsertSymbol(Symbol.LOGARITHM);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.ZERO);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Math.Log10(20), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void NaturalLogarithm()
        {
            io.InsertSymbol(Symbol.NATURAL_LOGARITHM);
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Math.Log(5), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Exponent()
        {
            io.InsertSymbol(Symbol.EXP);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Math.Exp(2), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Power()
        {
            io.InsertSymbol(Symbol.FIVE);
            io.InsertSymbol(Symbol.POWER);
            io.InsertSymbol(Symbol.THREE);
            Assert.AreEqual(Math.Pow(5,3), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Sine()
        {
            io.InsertSymbol(Symbol.SINE);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Math.Sin(2), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Cosine()
        {
            io.InsertSymbol(Symbol.COSINE);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Math.Cos(2), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Tangent()
        {
            io.InsertSymbol(Symbol.TANGENT);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(Math.Tan(2), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Secant()
        {
            io.InsertSymbol(Symbol.SECANT);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(1/Math.Cos(2), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Cosecant()
        {
            io.InsertSymbol(Symbol.COSECANT);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(1/Math.Sin(2), io.ToCalculation().Compute().ToDouble());
        }
        [Test]
        public void Cotangent()
        {
            io.InsertSymbol(Symbol.COTANGENT);
            io.InsertSymbol(Symbol.TWO);
            io.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
            Assert.AreEqual(1/ Math.Tan(2), io.ToCalculation().Compute().ToDouble());
        }
    }
}