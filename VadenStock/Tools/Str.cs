using System;
using System.Text;
using System.Text.RegularExpressions;



namespace VadenStock.Tools
{
    public static class Str
    {
        public static string Currency(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "0,00";

            string output = Sanitize(input, 3);
            char[] vecout = output.ToCharArray();

            Array.Reverse(vecout);

            string reversedOutput = new(vecout);
            int reversedOutLength = reversedOutput.Length;

            int i,
                dotCount = 0;

            for (i = 0; i < reversedOutLength; i++)
            {
                if (i == 2)
                    reversedOutput = reversedOutput.Insert(i, ",");
                else if (i == 5 || i == 8 || i == 11 || i == 14 || i == 17 || i == 20)
                {
                    dotCount++;
                    reversedOutput = reversedOutput.Insert(i + dotCount, ".");
                }
            }

            char[] subversedOutput = reversedOutput.ToCharArray();

            Array.Reverse(subversedOutput);

            return new string(subversedOutput);
        }



        public static string Sanitize(string value, int fill = 3)
        {
            string input = Number(value);
            char[] output = new char[(input.Length >= fill) ? input.Length : fill];

            if (input.Length < output.Length)
            {
                int pos;

                for (int i = output.Length - 1; i >= 0; i--)
                {
                    pos = (i - (output.Length - input.Length));
                    output[i] = (pos >= 0) ? input[pos] : '0';
                }
            }
            else
            {
                for (int i = output.Length - 1; i >= 0; i--)
                    output[i] = input[i];
            }

            return new string(output);
        }



        public static string Number(string dirty)
        {
            string clean = string.Empty;

            for (int i = 0; i < dirty.Length; i++)
            {
                if (Char.IsDigit(dirty[i]))
                    clean += dirty[i];
            }

            return clean;
        }



        public static bool IsNumber(string text)
        {
            int crrAscii;
            char[] chars = text.ToCharArray();

            for (int c = 0; c < chars.Length; c++)
            {
                crrAscii = (int)chars[c];

                if (crrAscii < 48 || crrAscii > 57)
                    return false;
            }

            return true;
        }



        public static bool IsText(string text)
        {
            int crrAscii;
            char[] chars = text.ToCharArray();

            for (int c = 0; c < chars.Length; c++)
            {
                crrAscii = (int)chars[c];

                if ((crrAscii < 65 && crrAscii != 32) || (crrAscii > 90 && crrAscii < 97) || crrAscii > 122)
                    return false;
            }

            return true;
        }



        public static string ZeroFill(int number, string? concat = "")
        {
            string zero = (number > 0 && number < 10)
                ? $"0{ number }"
                : number.ToString();

            return string.Concat(zero, concat);
        }



        public static string Pluralize(this string subject, int number, string replacement = "")
        {
            if (number != 1)
            {
                int subjectLen = subject.Length - 1;
                string last = subject[subjectLen].ToString();

                return (string.IsNullOrEmpty(replacement))
                    ? string.Concat(subject, "s")
                    : string.Concat(subject.Replace(last, replacement), "s");
            }

            return subject;
        }



        static public string ToBase64(this string encode)
        {
            byte[] toEncodeAsBytes  = Encoding.ASCII.GetBytes(encode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }



        public static string FromBase64(this string decode)
        {
            if (string.IsNullOrEmpty(decode))
                return string.Empty;

            byte[] valueBytes = Convert.FromBase64String(decode);

            return Encoding.UTF8.GetString(valueBytes);
        }



        public static string? MAC(string input)
        {
            if (input.Length == 12)
                return Regex.Replace(input, "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", "$1:$2:$3:$4:$5:$6");

            return null;
        }



        public static string Phone(string input)
        {
            if (input == null)
                return string.Empty;

            string output = input.ToString()
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty);

            if (output.Length > 11)
                output = output[..11];

            return output.Length switch
            {
                8 => Regex.Replace(output, @"(\d{4})(\d{4})", "$1-$2"),
                10 => Regex.Replace(output, @"(\d{2})(\d{4})(\d{4})", "($1) $2-$3"),
                11 => Regex.Replace(output, @"(\d{2})(\d{1})(\d{4})(\d{4})", "($1) $2 $3-$4"),
                _ => output,
            };
        }



        public static string CNPJ(string input)
        {
            if (input == null)
                return string.Empty;

            string output = input.ToString()
                .Replace(".", string.Empty)
                .Replace("/", string.Empty)
                .Replace("-", string.Empty);

            if (output.Length > 14)
                output = output[..14];

            return output.Length switch
            {
                5 => Regex.Replace(output, @"(\d{2})(\d{3})", "$1.$2"),
                8 => Regex.Replace(output, @"(\d{2})(\d{3})(\d{3})", "$1.$2.$3"),
                12 => Regex.Replace(output, @"(\d{2})(\d{3})(\d{3})(\d{4})", "$1.$2.$3/$4"),
                14 => Regex.Replace(output, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5"),
                _ => output
            };
        }
    }
}
