using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineJobPortal
{
    public class Utils
    {
        public static bool IsValidExtention(string fileName)
        {
            bool isValid = false;
            string[] fileExtention = { ".jpg", ".png", "jpeg" };
            foreach (string E in fileExtention)
            {
                if (fileName.Contains(E))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }

        public static bool IsValidExtentionResume(string fileName)
        {
            bool isValid = false;
            string[] fileExtention = { ".doc", ".docx", "pdf" };
            foreach (string E in fileExtention)
            {
                if (fileName.Contains(E))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }
    }
}