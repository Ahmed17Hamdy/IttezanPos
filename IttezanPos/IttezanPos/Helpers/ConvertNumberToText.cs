using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IttezanPos.Helpers
{
    public class ConvertNumberToText
    {
        static string[] digit_1 = new string[]
         { "", AppResources.One, AppResources.Two, AppResources.Three, AppResources.Four,
            AppResources.Five, AppResources.Six, AppResources.Seven, AppResources.Eight, AppResources.Nine};
        static string[] digit_2 = new string[]
         { AppResources.ZeroText,AppResources.Ten,AppResources.Twenty,AppResources.Thirty,AppResources.Fourty,AppResources.Fifty,
             AppResources.Sixty,AppResources.Seventy,AppResources.Eighty,AppResources.Ninety };
        static string[] digit_3 = new string[]
         { AppResources.ZeroText,AppResources.Hundred,AppResources._2Hundred,AppResources._3Hundred,AppResources._4Hundred,
             AppResources._5Hundred,AppResources._6Hundred,AppResources._7Hundred,AppResources._8Hundred,AppResources._9Hundred };

        static public string ConvertToArabic(double number)
        {
            string num = string.Format("{0:N}", number);
            num = num.Remove(num.IndexOf('.'));
            string[] parts = num.Split(',');
            num = " ";
            for (int i = 0; i < parts.Length; i++)
            {
                string t = " ";
                ulong current_part = ulong.Parse(parts[i]);
                // الأعداد المفردة تأخذ تمييز الجمع وهي الأعداد من ثلاثة الي عشرة 
                bool b = (current_part > 2 && current_part < 11);
                //
                switch (parts.Length - i)
                {
                    case 2: t = (b ? AppResources.Thousands : AppResources.Thousand); break;
                    case 3: t = (b ? AppResources.Millions :AppResources.Million); break;
                    case 4: t = (b ? AppResources.Billions :AppResources.Billion); break;
                    case 5: t = (b ?AppResources.Trillions : AppResources.Trillion); break;
                    case 6: t = (b ?AppResources.Quadrillions : AppResources.Quadrillion); break;
                    
                }
                num += (i != 0 && current_part != 0 ? AppResources.And : " ");
                if (current_part == 1 && parts.Length - i > 1)
                {
                    num += t;
                }
                else if (current_part == 2 && parts.Length - i > 1)
                {
                    num += t + AppResources.AAn;
                }
                else
                {
                    switch (current_part.ToString().Length)
                    {
                        case 1:
                            num += convertOneDigits(current_part);
                            break;
                        case 2:
                            num += convertTwoDigits(current_part);
                            break;
                        case 3:
                            num += convertThreeDigit(current_part);
                            break;
                    }
                    //اضافة التمييز للعدد (هل هو الاف أم ملايين أم غير ذلك
                    num += (current_part != 0 ? t : " " + " ");
                }

            }
            return num;
        }

        static string convertOneDigits(ulong OneDigits)
        {
            return digit_1[OneDigits];
        }
        static string convertTwoDigits(ulong x)
        {
            if (x == 10)
                return AppResources.Ten;
            if (x == 11)
                return AppResources.Eleven;
            if (x == 12)
                return AppResources.Twelve;
            else
            {
                if (x / 10 == 1)
                    return digit_1[x % 10] + " عشر";
                else
                    return digit_1[x % 10] + (x % 10 == 0 ? " " : AppResources.And) + digit_2[x / 10];
            }
        }
        static string convertThreeDigit(ulong x)
        {
            ulong last_two = x % 100;
            return digit_3[x / 100] + (last_two == 0 ? " " :AppResources.And) +
                (last_two < 10 ? convertOneDigits(last_two) : convertTwoDigits(last_two));
        }
        static bool custom(ulong x)
        {
            //you can extend this حتة
            return new ulong[] { 1, 2 }.Contains(x);
        }
    }
}


