using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common.EFCoreCommon
{
    public class CommandListener: IObserver<DiagnosticListener>
    {
        private readonly DbCommandInterceptor _dbCommandInterceptor = new DbCommandInterceptor();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == DbLoggerCategory.Name)
            {
                listener.Subscribe(_dbCommandInterceptor);
            }
        }
    }
}
