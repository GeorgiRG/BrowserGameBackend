using Quartz;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using BrowserGameBackend.Models;
using BrowserGameBackend.Data;
using System.Security.Policy;

namespace BrowserGameBackend.Jobs
{
    public class CollectSensorData : IJob
    {
        private readonly GameContext _db;
        public CollectSensorData(GameContext db)
        {
            _db = db;
        }

        public  Task Execute(IJobExecutionContext context)
        {
            for (long i = 0; i < 1000000000; i++)
            {
                continue;
            }
            return Task.CompletedTask;
            /*
            _db.Add(new User
            {
                Name = "sadf",
                Email = "sdafbb",
                Password = "bcvxb",
                Faction = "sdabb",
                Race = "dasfbvz",
                Class = "a class"
            });
            _db.SaveChanges();
            */
        }
    }
}