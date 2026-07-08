using MO.Common;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BL.Common
{
    public class InputValidator : IInputValidator
    {
        #region Properties

        private static readonly string ScriptPattern = @"<script\b[^>]*>[\s\S]*?</script>|<iframe\b[^>]*>|javascript\s*:[\s\S]*|on\w+\s*=\s*(['""])?[\s\S]*?\1?";
        // Optional: Match inputs containing special characters if needed
        private static readonly string SpecialCharPattern = @"[<>{[\]\\|^`~!@#$%&*()_=+""';:?/]";
        //private static readonly string WhitelistPattern = @"^[a-zA-Z0-9\s\-,.()/: ]+$";
        private static readonly string WhitelistPattern = @"^[a-zA-Z0-9\s\-.,()/:""{}[\]null\r\n]+$";

        #endregion

        //private static readonly string WhitelistPattern = @"^[a-zA-Z0-9\s\-.,()/]*$";
        //private static readonly string ScriptPattern = @"<script\b[^>]*>[\s\S]*?</script>|<iframe\b[^>]*>|javascript\s*:[\s\S]*|on\w+\s*=\s*(['""])?[\s\S]*?\1?";

        public result ValidateInput(dynamic modelValue)
        {
            string json = JsonSerializer.Serialize(modelValue);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            bool isValid = true;
            string message = "";

            try
            {
                var inputList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json, options);

                foreach (var item in inputList)
                {
                    string baseValue = item.ContainsKey("Basevalue") && item["Basevalue"] != null ? item["Basevalue"].ToString() : "";
                    string targetValue = item.ContainsKey("TargetValue") && item["TargetValue"] != null ? item["TargetValue"].ToString() : "";

                    // Check for dangerous script content (XSS)
                    if (Regex.IsMatch(baseValue, ScriptPattern, RegexOptions.IgnoreCase))
                    {
                        isValid = false;
                        message = $"Basevalue contains disallowed script/XSS content: {baseValue}";
                        break;
                    }

                    if (Regex.IsMatch(targetValue, ScriptPattern, RegexOptions.IgnoreCase))
                    {
                        isValid = false;
                        message = $"TargetValue contains disallowed script/XSS content: {targetValue}";
                        break;
                    }

                    // Check for allowed characters only
                    if (!string.IsNullOrEmpty(baseValue) && !Regex.IsMatch(baseValue, WhitelistPattern))
                    {
                        isValid = false;
                        message = $"Invalid Basevalue characters: {baseValue}";
                        break;
                    }

                    if (!string.IsNullOrEmpty(targetValue) && !Regex.IsMatch(targetValue, WhitelistPattern))
                    {
                        isValid = false;
                        message = $"Invalid TargetValue characters: {targetValue}";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = $"Deserialization/Validation error: {ex.Message}"
                };
            }

            return new result
            {
                status = isValid,
                message = message
            };
        }


        public result ContainsScriptTags(dynamic modelValue)
        {
            // Convert the dynamic value to a string
            //string input = "{\"GoalId\":null,\"Guid\":null,\"GoalName\":\"\\u003Cscript\\u003Ealert(\\u0027test\\u0027)\\u003C/script\\u003E\",\"GoalNumber\":\"19\",\"TagLine\":\"yy\",\"GoalColorCode\":\"\\u003Cscript\\u003Ealert(\\u0027test tr\\u0027)!\\u003C/script\\u003E\",\"Displayordernumber\":\"19\",\"IsActive\":false}";
            // Decode Unicode escape sequences
            string input = JsonSerializer.Serialize(modelValue);
            string decodedInput = DecodeUnicodeSequences(input);

            //string decodedInput = HttpUtility.HtmlDecode(input);
            string message = "";
            bool _res = Regex.IsMatch(decodedInput, ScriptPattern, RegexOptions.IgnoreCase);
            if (_res)
            {
                _res = false;
                message = "Invalid input detected: Contains script tags.";
            }
            else if (!Regex.IsMatch(decodedInput, WhitelistPattern))
            {
                _res = false;
                message = "Input contains restricted special characters.";
            }
            else
            {
                _res = true;
            }
            return new result
            {
                status = _res,
                message = message
            };
        }

        public bool ContainsScriptTags(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            // If it matches the "dangerous" pattern → XSS attempt
            if (System.Text.RegularExpressions.Regex.IsMatch(input, ScriptPattern,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return true;
            }

            // If it doesn't match the whitelist → suspicious
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, WhitelistPattern))
            {
                return true;
            }

            return false;
        }
        private static string DecodeUnicodeSequences(string input)
        {
            return Regex.Replace(
                input,
                @"\\u(?<Value>[a-fA-F0-9]{4})",
                m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
        }

    }
}












