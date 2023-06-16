using System.Globalization;
using System.Text.RegularExpressions;

namespace MinimalAPIJWTAuth.Utils;

public static class EmailAddressValidator
{
	public static bool TryValidate(string email, out string errorMessage)
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			errorMessage = "Null, empty, or whitespace.";
			return false;
		}

		var timeSpan = TimeSpan.FromMilliseconds(250);

		try
		{
			// Normalize the domain
			email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, timeSpan);

			// Examines the domain part of the email and normalizes it.
			string DomainMapper(Match match)
			{
				// Use IdnMapping class to convert Unicode domain names.
				var idn = new IdnMapping();

				// Pull out and process domain name (throws ArgumentException on invalid)
				string domainName = idn.GetAscii(match.Groups[2].Value);

				return match.Groups[1].Value + domainName;
			}
		}
		catch (RegexMatchTimeoutException e)
		{
			errorMessage = e.Message;
			return false;
		}
		catch (ArgumentException e)
		{
			errorMessage = e.Message;
			return false;
		}

		try
		{
			var match = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, timeSpan);
			errorMessage = match ? string.Empty : "The pattern did not match.";
			return match;
		}
		catch (RegexMatchTimeoutException e)
		{
			errorMessage = e.Message;
			return false;
		}
	}
}
