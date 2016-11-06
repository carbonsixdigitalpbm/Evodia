using System;

namespace Evodia.Voyager.Exceptions
{
    public class NoRootNodeException : Exception
    {
        public override string ToString()
        {
            return "Could not find 'Jobs' page. Check if it's not deleted or unpublished.";
        }
    }
}
