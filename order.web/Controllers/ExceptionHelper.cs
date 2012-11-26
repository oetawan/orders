using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace order.web.Controllers
{
    public static class ExceptionHelper
    {
        public static Exception GetInnerMostException(this Exception ex)
        {
            Exception innerEx = ex;
            while (innerEx.InnerException != null)
            {
                innerEx = innerEx.InnerException;
            }

            return innerEx;
        }
    }
}