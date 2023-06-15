using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TournamentTrackerLibrary.Utility;

public static class TournamentTrackerValidations
{
    public static bool IsValidEmail(string email)
    {
        var validEmail = new Regex("^[\\w!#$%&'*+/=?^`{|}~-]+(\\.[\\w!#$%&'*+/=?^`{|}~-]+)*@(?:[\\w-]+\\.)+[a-zA-Z]{2,63}$");
        return validEmail.IsMatch(email);
    }

    public static bool IsValidName(string name)
    {
        if (name.Contains(',') || name.Contains('|') || name.Contains('^'))
        {
            return false;
        }

        return true;
    }
}
