
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Weather.MobileCore.Converters
{
    public class TempToColorConverter : IMarkupExtension, IValueConverter
    {
        public static bool UseCelcius { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int temp)
            {
                if(temp < 29) return Color.FromHex("#0022ff");

                switch (temp)
                {
                    case 29:
                    case 30: return Color.FromHex("#0032ff");
                    case 31: 
                    case 32: return Color.FromHex("#0044ff");
                    case 33: 
                    case 34: return Color.FromHex("#0054ff");
                    case 35: 
                    case 36: return Color.FromHex("#0064ff");
                    case 37: 
                    case 38: return Color.FromHex("#0074ff");
                    case 39: 
                    case 40: return Color.FromHex("#0084ff");

                    case 41:
                    case 42: return Color.FromHex("#0094ff");
                    case 43:
                    case 44: return Color.FromHex("#00a4ff");
                    case 45:
                    case 46: return Color.FromHex("#00b4ff");
                    case 47:
                    case 48: return Color.FromHex("#00c4ff");
                    case 49:
                    case 50: return Color.FromHex("#00d4ff");

                    case 51:
                    case 52: return Color.FromHex("#00e4ff");
                    case 53:
                    case 54: return Color.FromHex("#00fff4");
                    case 55:
                    case 56: return Color.FromHex("#00ffd0");
                    case 57:
                    case 58: return Color.FromHex("#00ffa8");
                    case 59:
                    case 60: return Color.FromHex("#00ff83");

                    case 61:
                    case 62: return Color.FromHex("#00ff5c");
                    case 63:
                    case 64: return Color.FromHex("#00ff36");
                    case 65:
                    case 66: return Color.FromHex("#00ff10");
                    case 67:
                    case 68: return Color.FromHex("#17ff00");
                    case 69:
                    case 70: return Color.FromHex("#3eff00");

                    case 71:
                    case 72: return Color.FromHex("#65ff00");
                    case 73:
                    case 74: return Color.FromHex("#8aff00");
                    case 75:
                    case 76: return Color.FromHex("#b0ff00");
                    case 77:
                    case 78: return Color.FromHex("#d7ff00");
                    case 79:
                    case 80: return Color.FromHex("#fdff00");

                    case 81:
                    case 82: return Color.FromHex("#FFfa00");
                    case 83:
                    case 84: return Color.FromHex("#FFf000");
                    case 85:
                    case 86: return Color.FromHex("#FFe600");
                    case 87:
                    case 88: return Color.FromHex("#FFdc00");
                    case 89:
                    case 90: return Color.FromHex("#FFd200");

                    case 91:
                    case 92: return Color.FromHex("#FFc800");
                    case 93:
                    case 94: return Color.FromHex("#FFbe00");
                    case 95:
                    case 96: return Color.FromHex("#FFb400");
                    case 97:
                    case 98: return Color.FromHex("#FFaa00");
                    case 99:

                    case 100:
                    default:
                        return Color.FromHex("#FFa000");
                }
                
            }

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
