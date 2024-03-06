using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Quartz;

namespace UPrinceV4.Web.Jobs;

public class DatabaseCopyJob: IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        JobDataMap dataMap = context.JobDetail.JobDataMap;
        var SequenceCode =    dataMap.GetString("SequenceCode");
        var ConnectionString = dataMap.GetString("ConnectionString");
        await Console.Out.WriteLineAsync("Greetings from HelloJob!" + SequenceCode +" ConnectionString "+ ConnectionString);
        var projectTemplateDbName = "UPrinceV4ProjectTemplate";
        var copyQuery = "CREATE DATABASE " + SequenceCode + " AS COPY OF " + projectTemplateDbName;
        await using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        var command = new SqlCommand(copyQuery, connection);
        command.CommandTimeout = 600;
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        //Console.Write("ssss");
        // await Console.Out.WriteLineAsync("Greetings from HelloJob!" + SequenceCode);
    }
}
