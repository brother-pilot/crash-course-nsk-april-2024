using System.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace Market.MiddleWare;

public class MyException:Exception
{
    public MyException(int v1, string v2)
    {
        throw new NotImplementedException();
    }

    /*public void OnException(ExecutionContext context) 
    {
        if (context.Exception is not DbException exception)
            return;

        context.Result = new JsonResult(new
        {
            StarusCode= exception.StatusCode,
            Message= exception.Message
        });

        StatusCode = exception.StatusCode;
    }*/
}