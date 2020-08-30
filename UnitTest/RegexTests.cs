using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
namespace UnitTest
{
    public class RegexTests
    {
        [Test]
        public void TestAtoi()
        {
            var ePlus = Regex.Escape("+");
            var p = $@"^\s*-*\+*[0-9]+.*$";
            var t = "       +42344????c34s##31233 dsfsdf";
            Assert.True(Regex.IsMatch(t, p));
            p = $@"^\s*(?<digits>-*\+*\d+)[^0-9].*$";
            var g = Regex.Match(t, p).Groups["digits"];
            Double.TryParse(g.ToString(), out var d);
            double INTMAX = Int32.MaxValue;
            double INTMIN = Int32.MinValue;
            int r = 0;
            if (r <= INTMAX && r >= INTMIN)
                r = (int)d;
            Assert.True(42344 == r);
        }

        [Test]
        public void RomanNumberValidation()
        {
            //{ "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            var s = "";
            //(I[V|X]|V?I{0,3}) - this is a repeated pattern for high numerals, this case is for units
            //                    covers  "" ,"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"
            //                    I[V|X] represents I followed by either V or X, [] represents set here
            //                    V?I{0,3} represents zero or one 'V' followed by 0 or 3 'I's
            string p = @"^(M{0,4}(C[M|D]|D?C{0,3})(X[L|C]|L?X{0,3})(I[V|X]|V?I{0,3}))$";
            Assert.True(Regex.IsMatch(s, p));
            s = "I";
            Assert.True(Regex.IsMatch(s, p));
            s = "IV";
            Assert.True(Regex.IsMatch(s, p));
            s = "IX";
            Assert.True(Regex.IsMatch(s, p));
            s = "VII";
            Assert.True(Regex.IsMatch(s, p));
            s = "IIII";
            Assert.False(Regex.IsMatch(s, p));
            s = "IVI";
            Assert.False(Regex.IsMatch(s, p));
            s = "VX";
            Assert.False(Regex.IsMatch(s, p));
            s = "XXXX";
            Assert.False(Regex.IsMatch(s, p));
            s = "VX";
            Assert.False(Regex.IsMatch(s, p));
            s = "LC";
            Assert.False(Regex.IsMatch(s, p));
            s = "DM";
            Assert.False(Regex.IsMatch(s, p));
            s = "MMMM";
            Assert.True(Regex.IsMatch(s, p));
            s = "MMMCMXCIX";
            Assert.True(Regex.IsMatch(s, p));
        }
    }
}
