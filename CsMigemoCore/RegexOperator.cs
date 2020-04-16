using System;
using System.Collections.Generic;
using System.Text;

namespace CsMigemo
{
    public class RegexOperator
    {
        public readonly string Or;
        public readonly string BeginGroup;
        public readonly string EndGroup;
        public readonly string BeginClass;
        public readonly string EndClass;
        public readonly string Newline;

        public RegexOperator(string or, string beginGroup, string endGroup, string beginClass, string endClass, string newline)
        {
            Or = or;
            BeginGroup = beginGroup;
            EndGroup = endGroup;
            BeginClass = beginClass;
            EndClass = endClass;
            Newline = newline;
        }

        public static readonly RegexOperator DEFAULT = new RegexOperator("|", "(", ")", "[", "]", null);
        public static readonly RegexOperator VIM_NONEWLINE = new RegexOperator("\\|", "\\%(", "\\)", "[", "]", null);
        public static readonly RegexOperator VIM_NEWLINE = new RegexOperator("\\|", "\\%(", "\\)", "[", "]", "\\_s*");
        public static readonly RegexOperator EMACS_NONEWLINE = new RegexOperator("\\|", "\\(", "\\)", "[", "]", null);
        public static readonly RegexOperator EMACS_NEWLINE = new RegexOperator("\\|", "\\(", "\\)", "[", "]", "\\s-*");
    }
}
