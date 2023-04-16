using Quartz;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using BrowserGameBackend.Models;
using BrowserGameBackend.Data;
using System.Security.Policy;

namespace BrowserGameBackend.Jobs
{
    public class CollectSensorData2 : IJob
    {

        public Task Execute(IJobExecutionContext context)
        {
            for (long i = 0; i < 1000000000; i++)
            {
                continue;
            }
            Console.WriteLine("task2");
            return Task.CompletedTask;
        }
    }
}