using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    public static class MoneyHelper
    {
        /// <summary>
        /// 小写数字转成大写数字
        /// </summary>
        /// <param name="num">小写数字</param>
        /// <returns></returns>
        public static string GetNumberCapitalized(double num) 
        {
            string[] Unit = new string[8] { "拾", "佰", "仟", "万", "亿", "圆", "角", "分" };
            Dictionary<char, string> NumList = new Dictionary<char, string>()
            {
                { '0', "零"},
                { '1', "壹"},
                { '2', "贰"},
                { '3', "叁"},
                { '4', "肆"},
                { '5', "伍"},
                { '6', "陆"},
                { '7', "柒"},
                { '8', "捌"},
                { '9', "玖"}
            };
            string ChineseStr = "";
            string val = num.ToString();
            //if (Regex.IsMatch(val, @"^(([0 - 9] *) | (([0]\.\d{ 0,2}|[1 - 9][0 - 9] *\.\d{ 0,2})))$"))
            //{
            //    return ChineseStr;
            //}
            int PointIndex = val.IndexOf('.');
            string intNum = "0";
            string PointNum = "0";
            if (PointIndex == -1)
            {
                intNum = val;
            }
            else
            {
                intNum = val.Substring(0, PointIndex);
                PointNum = val.Substring(PointIndex + 1, val.Length-(PointIndex + 1));
            }
            int Strlen = intNum.Length;
            if (Strlen < 6)
            {
                for (int i = 0; i < Strlen; i++)
                {
                    if (ChineseStr!=""&&ChineseStr[ChineseStr.Length - 1] == '零' && intNum[i] == '0')
                    {
                        continue;
                    }
                    ChineseStr += NumList[intNum[i]];
                    if (Strlen - 2 - i >= 0)
                    {
                        if (intNum[i] != '0')
                        {
                            ChineseStr += Unit[Strlen - 2 - i];
                        }
                    }
                }
                if (ChineseStr.EndsWith("零"))
                {
                    ChineseStr = ChineseStr.Substring(0, ChineseStr.Length - 1);
                }
            }
            if (Strlen >= 6)
            {
                string str1 = intNum.Substring(0, intNum.Length - 4);
                string str2 = intNum.Substring(intNum.Length - 4, intNum.Length-(intNum.Length - 4));
                string heightStr = "";
                string lowStr = "";
                for (int i = 0; i < str1.Length; i++)
                {
                    heightStr += NumList[str1[i]];
                    if (str1.Length - i - 2 >= 0)
                    {
                        if (str1[i] != '0')
                        {
                            heightStr += Unit[str1.Length - 2 - i];
                        }
                    }
                }
                if (heightStr.EndsWith("零"))
                {
                    heightStr = heightStr.Substring(0, heightStr.Length - 1);
                }
                if (heightStr.EndsWith("拾") || str2.StartsWith("0"))
                {
                    heightStr += "万零";
                }
                else
                {
                    heightStr += "万";
                }
                for (int i = 0; i < str2.Length; i++)
                {
                    if (lowStr!=""&&lowStr[lowStr.Length - 1] == '零' && str2[i] == '0')
                    {
                        continue;
                    }
                    lowStr += NumList[str2[i]];
                    if (str2.Length - 2 - i >= 0)
                    {
                        if (str2[i] != '0')
                        {
                            lowStr += Unit[str2.Length - 2 - i];
                        }
                    }
                }
                if (lowStr.EndsWith("零"))
                {
                    lowStr = lowStr.Substring(0, lowStr.Length - 1);
                }
                if (lowStr.StartsWith("零"))
                {
                    lowStr = lowStr.Substring(1, lowStr.Length);
                }
                ChineseStr = heightStr + lowStr;
            }
            if (PointNum != "0")
            {
                string pointStr = "";
                PointNum = PointNum.Substring(0, 2);
                for (int i = 0; i < PointNum.Length; i++)
                {
                    if (PointNum[i] != '0')
                    {
                        pointStr +=NumList[PointNum[i]]+Unit[6 + i];
                    }
                    else
                    {
                        continue;
                    }
                }
                if (
                  ChineseStr.EndsWith("万") ||
                  ChineseStr.EndsWith("仟") ||
                  ChineseStr.EndsWith("佰")
                )
                {
                    ChineseStr += "圆零" + pointStr;
                }
                else
                {
                    if (ChineseStr != "")
                    {
                        ChineseStr += "圆" + pointStr;
                    }
                    else
                    {
                        ChineseStr +=pointStr;
                    }
                    
                }
            }
            else
            {
                if (ChineseStr == "")
                {
                    ChineseStr += "零圆整";
                }
                else
                {
                    ChineseStr += "圆整";
                }
            }
            return ChineseStr;
        }
    }
}
