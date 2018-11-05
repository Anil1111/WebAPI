using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace WebAPI.Models
{
    public class Grouper
    {
        public Grouper(String TestString)
        {
            this.testStr = TestString;
        }

        /* Customizable Settings */
        /// <summary> The character(s) that constitute the start of a group. </summary>
        public String open { get; set; } = "{";
        /// <summary> The character(s) that constitute the closing of a group. </summary>
        public String close { get; set; } = "}";
        /// <summary> The character(s) to ignore any character immediately following unless it is
        /// another escape character. </summary>
        public String escapes { get; set; } = "!";
        /// <summary> The character(s) that constitute the start of a section to ignore. </summary>
        public String commentStart { get; set; } = "<";
        /// <summary> The character(s) that constitute the end of a section to ignore. </summary>
        public String commentEnd { get; set; } = ">";
        /// <summary> The working string to perform the group counts on. </summary>
        public String testStr { get; set; }
        /// <summary> if and of the special characters are alpha, then this toggles case sensitivity. </summary>
        public bool ignoreCase { get; set; } = false;
        /// <summary> Private readonly regex options for case sensitivity </summary>
        private RegexOptions _regexOpt
        {
            get { return (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None); }
        }

        public override string ToString()
        {
            return (testStr == null ? "" : testStr);
        }

        /// <summary> Score the string based on groups of braces and their nested levels.
        /// Base level group) </summary>
        public long score()
        {
            long score = 0;

            if (String.IsNullOrEmpty(testStr))
                return score;
            else
                testStr = PurgeEscapedChars(testStr);

            if (String.IsNullOrEmpty(testStr))
                return score;
            else
            {
                testStr = testStr.Replace("\r", "").Replace("\n", "");
                testStr = PurgeCommentSections(testStr);
            }
            if (String.IsNullOrEmpty(testStr))
                return score;
            else
                testStr = PurgeAllOtherNonessentialChars(testStr);

            Stack<int> stack = new Stack<int>();
            int adj = (open.Length > close.Length ? open.Length : close.Length) - 1; // prevent index out of range 
            for (int i = 0; i < (testStr.Length - adj); i++)
            {
                if (testStr.IndexOf(open, i, open.Length) > -1)
                {
                    stack.Push(0);
                }
                if (testStr.IndexOf(close, i, close.Length) > -1)
                {
                    if (stack.Count > 0)
                    {   // only credit if closed and ignore extra occurances of close char
                        score += stack.Count(); 
                        stack.Pop();
                    }
                }
            }

            return score;
        }


        /// <summary> Escape Characters work left to right only.  Remove escape chars
        /// and their immediate next char. </summary>
        public String PurgeEscapedChars(string sTemp)
        {
            string expr = Regex.Escape(escapes);
            Match m = Regex.Match(sTemp, expr, _regexOpt);
            if (m.Success)
            {
                return PurgeEscapedChars(sTemp.Remove(m.Index, ((m.Index + 1) == sTemp.Length ? 1 : 2)));
            }
            else
            {
                return sTemp;
            }
        }

        /// <summary> Remove any commented sections (use only after escape characters
        /// have been removed.) </summary>
        public String PurgeCommentSections(string sTemp)
        {
            // match string starting with opening, then 0 or more any chars, and then ending.
            string expr = String.Format("{0}.*?{1}",
                Regex.Escape(commentStart), Regex.Escape(commentEnd));
            Match m = Regex.Match(sTemp, expr);
            if (m.Success)
            {
                return PurgeCommentSections(sTemp.Remove(m.Index, m.Value.Length));
            }
            else
            {
                // No more whole matches, check for one last partial.
                expr = String.Format("[{0}].*", Regex.Escape(commentStart));
                m = Regex.Match(sTemp, expr, _regexOpt);
                if (m.Success)
                {
                    // remove everything after the match.
                    return PurgeCommentSections(sTemp.Remove(m.Index));
                }
                else
                {
                    return sTemp;
                }
            }
        }

        public string PurgeAllOtherNonessentialChars(string sTemp)
        {
            string expr = String.Format("[^{0}]+", Regex.Escape(open + close));
            sTemp = Regex.Replace(sTemp, expr, String.Empty, _regexOpt);

            return sTemp;
        }


        /// <summary>
        /// Test Method for this class.
        /// </summary>
        /// <param name="argv"></param>
        public static void Main(string[] argv)
        {
            Grouper g = new Grouper(GrouperTestStrings.getTestStr(0));
            System.Console.WriteLine("score = " + g.score());

            g.testStr = GrouperTestStrings.getTestStr(1);
            System.Console.WriteLine("score = " + g.score());

            g.testStr = GrouperTestStrings.getTestStr(2);
            System.Console.WriteLine("score = " + g.score());


        }
    }

    public class GrouperTestStrings
    {
        /// <summary> Grab a test string to play with. </summary>
        /// <param name="num">Desired String (optional)</param>
        public static string getTestStr(int num = 0)
        {
            if (num == 0)
                return s[0];
            else if (num >= s.Length)
                return s[s.Length - 1];
            else
                return s[num];
        }

        public static readonly string[] s = new string[] {
@"This String Has a lot of comments<
>, empty garbage.
<random characters>, garbage containing random characters.
<<<<>, because the extra < are ignored.!
<{!>}>, because the first > is canceled.
<!!>, because the second ! is canceled, allowing the > to terminate the garbage.
<!!!>>, because the second ! and the first > are canceled.
<{o""i!a,<{i<a>, which ends at the first >.",

@"This string has a bunch of groups
Here are some examples of whole streams and the number of groups they contain:
{}, 1 group.
{{{}}}, 3 groups.
{{},{}}, also 3 groups.
{{{},{},{{}}}}, 6 groups.
{<{},{},{{}}>}, 1 group (which itself contains garbage).
{<a>,<a>,<a>,<a>}, 1 group.
{{<a>},{<a>},{<a>},{<a>}}, 5 groups.
{{<!>},{<!>},{<!>},{<a>}}, 2 groups (since all but the last > are canceled).",

@"This string has bunches of the same.
Your goal is to find the total score for all groups in your input. Each group is assigned a score which is one more than the score of the group that immediately contains it. (The outermost group gets a score of 1.)
{}, score of 1.
{{{}}}, score of 1 + 2 + 3 = 6.
{{},{}}, score of 1 + 2 + 2 = 5.
{{{},{},{{}}}}, score of 1 + 2 + 3 + 3 + 3 + 4 = 16.
{<a>,<a>,<a>,<a>}, score of 1.
{{<ab>},{<ab>},{<ab>},{<ab>}}, score of 1 + 2 + 2 + 2 + 2 = 9.
{{<!!>},{<!!>},{<!!>},{<!!>}}, score of 1 + 2 + 2 + 2 + 2 = 9.
{{<a!>},{<a!>},{<a!>},{<ab>}}, score of 1 + 2 = 3.",

@"Test case: too many closing characters.
{}}}}}",

"Test case: Unicode characters.{\u00B9}}}}} note this string isn't defined with the \"@\" Literal prefix" };

    } // end public class GrouperTestStrings
}