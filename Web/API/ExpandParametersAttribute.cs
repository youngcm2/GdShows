using System;

namespace GdShows.API
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExpandParametersAttribute : Attribute 
    {
        public ExpandParametersAttribute()
        {

        }
    }
}
