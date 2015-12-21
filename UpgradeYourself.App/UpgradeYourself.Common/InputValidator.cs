namespace UpgradeYourself.Common
{
    public static class InputValidator
    {
        public static bool ValidateRegularTextInput(string text)
        {
            if (text != null && text.Length >= 3)
            {
                return true;
            }

            return false;
        }

        public static bool ValidatePasswordTextInput(string password)
        {
            if (password != null && password.Length >= 6)
            {
                return true;
            }

            return false;
        }

        public static bool ValidateRepeatedPasswordTextInput(string password, string repeatedPassword)
        {
            return ValidatePasswordTextInput(password) && password == repeatedPassword;
        }

        //public static bool ValidateEmailTextInput(string email)
        //{
        //    if (email != null && email.Length >= 5 && email.IndexOf("@") != -1)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public static bool ValidatePhoneTextInput(string phone)
        //{
        //    if (phone != null && phone.Length >= 6)
        //    {
        //        for (int i = 0; i < phone.Length; i++)
        //        {
        //            if (!char.IsDigit(phone[i]))
        //            {
        //                return false;
        //            }
        //        }

        //        return true;
        //    }

        //    return false;
        //}
    }
}