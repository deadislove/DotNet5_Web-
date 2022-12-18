using DotNet5_web.Infra.DBContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNet5_web.Infra.DBContext
{
    public class DemoDbContext : DbContext
    {
        private static Uri _url = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static FileStream _fileStream;
        private readonly StreamWriter _logStream;
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
            string filePath = _url.LocalPath;

            if (filePath.Contains("bin"))
            {
                filePath = filePath.Substring(0, filePath.IndexOf("bin")) + "DbLogs";
            }

            var fileName = filePath + $"\\Log-{DateTime.Now:yyyyMMdd}.txt";

            if(!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);


            _fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            _logStream = new StreamWriter(_fileStream, Encoding.UTF8, 4096, true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder
            .ConfigureWarnings(b => b.Log(
                (RelationalEventId.ConnectionOpened, LogLevel.Information),
                (RelationalEventId.ConnectionClosed, LogLevel.Information)
                ))
            .LogTo(
                    _logStream.WriteLine, 
                    LogLevel.Debug, 
                    DbContextLoggerOptions.UtcTime | DbContextLoggerOptions.SingleLine)
            .EnableSensitiveDataLogging();

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();
        }

        public DbSet<Enterprise_MVC_Core> Enterprise_MVC_Cores { get; set; }

    }
}
