using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace MvcSandbox
{
    public class ZalgoParameterTransformer : IOutboundParameterTransformer
    {
        private readonly Random _random = new Random();

        public string TransformOutbound(object value)
        {
            // Zalgo value
            return value == null
                ? null
                : Convert(value.ToString(), false, true, true);
        }

        //those go UP
        private readonly char[] _zalgoUp = new[] {
              '\u030d', /*     ̍     */		'\u030e', /*     ̎     */		'\u0304', /*     ̄     */		'\u0305', /*     ̅     */
            '\u033f', /*     ̿     */		'\u0311', /*     ̑     */		'\u0306', /*     ̆     */		'\u0310', /*     ̐     */
            '\u0352', /*     ͒     */		'\u0357', /*     ͗     */		'\u0351', /*     ͑     */		'\u0307', /*     ̇     */
            '\u0308', /*     ̈     */		'\u030a', /*     ̊     */		'\u0342', /*     ͂     */		'\u0343', /*     ̓     */
            '\u0344', /*     ̈́     */		'\u034a', /*     ͊     */		'\u034b', /*     ͋     */		'\u034c', /*     ͌     */
            '\u0303', /*     ̃     */		'\u0302', /*     ̂     */		'\u030c', /*     ̌     */		'\u0350', /*     ͐     */
            '\u0300', /*     ̀     */		'\u0301', /*     ́     */		'\u030b', /*     ̋     */		'\u030f', /*     ̏     */
            '\u0312', /*     ̒     */		'\u0313', /*     ̓     */		'\u0314', /*     ̔     */		'\u033d', /*     ̽     */
            '\u0309', /*     ̉     */		'\u0363', /*     ͣ     */		'\u0364', /*     ͤ     */		'\u0365', /*     ͥ     */
            '\u0366', /*     ͦ     */		'\u0367', /*     ͧ     */		'\u0368', /*     ͨ     */		'\u0369', /*     ͩ     */
            '\u036a', /*     ͪ     */		'\u036b', /*     ͫ     */		'\u036c', /*     ͬ     */		'\u036d', /*     ͭ     */
            '\u036e', /*     ͮ     */		'\u036f', /*     ͯ     */		'\u033e', /*     ̾     */		'\u035b', /*     ͛     */
            '\u0346', /*     ͆     */		'\u031a' /*     ̚     */
        };

        //those go DOWN
        private readonly char[] _zalgoDown = new[] {
              '\u0316', /*     ̖     */		'\u0317', /*     ̗     */		'\u0318', /*     ̘     */		'\u0319', /*     ̙     */
            '\u031c', /*     ̜     */		'\u031d', /*     ̝     */		'\u031e', /*     ̞     */		'\u031f', /*     ̟     */
            '\u0320', /*     ̠     */		'\u0324', /*     ̤     */		'\u0325', /*     ̥     */		'\u0326', /*     ̦     */
            '\u0329', /*     ̩     */		'\u032a', /*     ̪     */		'\u032b', /*     ̫     */		'\u032c', /*     ̬     */
            '\u032d', /*     ̭     */		'\u032e', /*     ̮     */		'\u032f', /*     ̯     */		'\u0330', /*     ̰     */
            '\u0331', /*     ̱     */		'\u0332', /*     ̲     */		'\u0333', /*     ̳     */		'\u0339', /*     ̹     */
            '\u033a', /*     ̺     */		'\u033b', /*     ̻     */		'\u033c', /*     ̼     */		'\u0345', /*     ͅ     */
            '\u0347', /*     ͇     */		'\u0348', /*     ͈     */		'\u0349', /*     ͉     */		'\u034d', /*     ͍     */
            '\u034e', /*     ͎     */		'\u0353', /*     ͓     */		'\u0354', /*     ͔     */		'\u0355', /*     ͕     */
            '\u0356', /*     ͖     */		'\u0359', /*     ͙     */		'\u035a', /*     ͚     */		'\u0323' /*     ̣     */
        };

        //those always stay in the middle
        private readonly char[] _zalgoMid = new[] {
              '\u0315', /*     ̕     */		'\u031b', /*     ̛     */		'\u0340', /*     ̀     */		'\u0341', /*     ́     */
            '\u0358', /*     ͘     */		'\u0321', /*     ̡     */		'\u0322', /*     ̢     */		'\u0327', /*     ̧     */
            '\u0328', /*     ̨     */		'\u0334', /*     ̴     */		'\u0335', /*     ̵     */		'\u0336', /*     ̶     */
            '\u034f', /*     ͏     */		'\u035c', /*     ͜     */		'\u035d', /*     ͝     */		'\u035e', /*     ͞     */
            '\u035f', /*     ͟     */		'\u0360', /*     ͠     */		'\u0362', /*     ͢     */		'\u0338', /*     ̸     */
            '\u0337', /*     ̷     */		'\u0361', /*     ͡     */		'\u0489' /*     ҉_     */
        };

        private bool IsZalgoChar(char c)
        {
            if (_zalgoUp.Contains(c) ||
                _zalgoDown.Contains(c) ||
                _zalgoMid.Contains(c))
            {
                return true;
            }

            return false;
        }

        private string Convert(string txt, bool zalgoUp, bool zalgoMiddle, bool zalgoDown)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < txt.Length; i++)
            {
                if (IsZalgoChar(txt[i]))
                {
                    continue;
                }

                sb.Append(txt[i]);

                var countUp = _random.Next(8);
                var countMid = _random.Next(2);
                var countDown = _random.Next(8);

                if (zalgoUp)
                {
                    for (var j = 0; j < countUp; j++)
                    {
                        sb.Append(GetRandomZalgoChar(_zalgoUp));
                    }
                }
                if (zalgoMiddle)
                {
                    for (var j = 0; j < countMid; j++)
                    {
                        sb.Append(GetRandomZalgoChar(_zalgoMid));
                    }
                }
                if (zalgoDown)
                {
                    for (var j = 0; j < countDown; j++)
                    {
                        sb.Append(GetRandomZalgoChar(_zalgoDown));
                    }
                }
            }

            return sb.ToString();
        }

        private char GetRandomZalgoChar(char[] array)
        {
            var i = System.Convert.ToInt32(Math.Floor(_random.NextDouble() * array.Length));
            return array[i];
        }
    }
}
