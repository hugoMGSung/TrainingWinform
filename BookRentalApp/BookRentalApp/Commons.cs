﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookRentalApp
{
    public static class Commons
    {
        public static string USERID = string.Empty;  // 사용자 아이디

        public static string CONNECTIONSTRING =
            "Data Source=127.0.0.1;Initial Catalog=BookRentalshopDB;Persist Security Info=True;User ID=sa;Password=p@ssw0rd!";

        public static string GetMd5Hash(MD5 md5Hash, string input)
        { 
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input)); 
            // Create a new Stringbuilder to collect the bytes 
            // and create a string. 
            StringBuilder sBuilder = new StringBuilder(); 
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++) { 
                sBuilder.Append(data[i].ToString("x2")); 
            } 
            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
    }
}
