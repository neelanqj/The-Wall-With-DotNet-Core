using System;
using System.ComponentModel.DataAnnotations;

namespace Extensions
{
    public class PastDateAttribute : ValidationAttribute
    {
        public PastDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            if(dt <= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}