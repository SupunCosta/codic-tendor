using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MickiesoftMuiltitenant.Dapper;

/// <summary>
/// Bulk inserts for Dapper
/// </summary>
public static class DapperBulk
{
    /// <summary>
    /// Inserts entities into table <typeparamref name="T"/>s (by default).
    /// </summary>
    /// <typeparam name="T">The type being inserted.</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="data">Entities to insert</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="batchSize">Number of bulk items inserted together, 0 (the default) if all</param>
    /// <param name="bulkCopyTimeout">Number of seconds before bulk command execution timeout, 30 (the default)</param>
    /// <param name="identityInsert">Usage of db generated ids. By default DB generated IDs are used (identityInsert=false)</param>
    public static void BulkInsert<T>(this SqlConnection connection, IEnumerable<T> data,
        SqlTransaction transaction = null, int batchSize = 0, int bulkCopyTimeout = 30, bool identityInsert = false)
    {
        var type = typeof(T);
        BulkInsert(connection, type, data.Cast<object>(), transaction, batchSize, bulkCopyTimeout, identityInsert);
    }

    /// <summary>
    /// Inserts entities into table.
    /// by default, the table is named after the data type specified.
    /// </summary>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="type">The type being inserted.</param>
    /// <param name="data">Entities to insert</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="batchSize">Number of bulk items inserted together, 0 (the default) if all</param>
    /// <param name="bulkCopyTimeout">Number of seconds before bulk command execution timeout, 30 (the default)</param>
    /// <param name="identityInsert">Usage of db generated ids. By default DB generated IDs are used (identityInsert=false)</param>
    public static void BulkInsert(this SqlConnection connection, Type type, IEnumerable<object> data,
        SqlTransaction transaction = null, int batchSize = 0, int bulkCopyTimeout = 30, bool identityInsert = false)
    {
        var tableName = TableMapper.GetTableName(type);
        var allProperties = PropertiesCache.TypePropertiesCache(type);
        var keyProperties = PropertiesCache.KeyPropertiesCache(type);
        var computedProperties = PropertiesCache.ComputedPropertiesCache(type);
        var columns = PropertiesCache.GetColumnNamesCache(type);

        var insertProperties = allProperties.Except(computedProperties).ToList();

        if (!identityInsert)
            insertProperties = insertProperties.Except(keyProperties).ToList();

        var (identityInsertOn, identityInsertOff, sqlBulkCopyOptions) =
            GetIdentityInsertOptions(identityInsert, tableName);

        var insertPropertiesString = GetColumnsStringSqlServer(insertProperties, columns);
        var tempToBeInserted = $"#TempInsert_{tableName}".Replace(".", string.Empty);

        connection.Execute(
            $@"SELECT TOP 0 {insertPropertiesString} INTO {tempToBeInserted} FROM {FormatTableName(tableName)} target WITH(NOLOCK);",
            null, transaction);

        using (var bulkCopy = new SqlBulkCopy(connection, sqlBulkCopyOptions, transaction))
        {
            bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
            bulkCopy.BatchSize = batchSize;
            bulkCopy.DestinationTableName = tempToBeInserted;
            bulkCopy.WriteToServer(ToDataTable(data, insertProperties).CreateDataReader());
        }

        connection.Execute($@"
                {identityInsertOn}
                INSERT INTO {FormatTableName(tableName)}({insertPropertiesString}) 
                SELECT {insertPropertiesString} FROM {tempToBeInserted}
                {identityInsertOff}
                DROP TABLE {tempToBeInserted};", null, transaction);
    }

    /// <summary>
    /// Inserts entities into table <typeparamref name="T"/>s (by default) returns inserted entities.
    /// </summary>
    /// <typeparam name="T">The element type of the array</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="data">Entities to insert</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="batchSize">Number of bulk items inserted together, 0 (the default) if all</param>
    /// <param name="bulkCopyTimeout">Number of seconds before bulk command execution timeout, 30 (the default)</param>
    /// <param name="identityInsert">Usage of db generated ids. By default DB generated IDs are used (identityInsert=false)</param>
    /// <returns>Inserted entities</returns>
    public static IEnumerable<T> BulkInsertAndSelect<T>(this SqlConnection connection, IEnumerable<T> data,
        SqlTransaction transaction = null, int batchSize = 0, int bulkCopyTimeout = 30, bool identityInsert = false)
    {
        var type = typeof(T);
        var tableName = TableMapper.GetTableName(type);
        var allProperties = PropertiesCache.TypePropertiesCache(type);
        var keyProperties = PropertiesCache.KeyPropertiesCache(type);
        var computedProperties = PropertiesCache.ComputedPropertiesCache(type);
        var columns = PropertiesCache.GetColumnNamesCache(type);

        if (keyProperties.Count == 0)
        {
            var dataList = data.ToList();
            connection.BulkInsert(dataList, transaction, batchSize, bulkCopyTimeout);
            return dataList;
        }

        var insertProperties = allProperties.Except(computedProperties).ToList();

        if (!identityInsert)
            insertProperties = insertProperties.Except(keyProperties).ToList();

        var (identityInsertOn, identityInsertOff, sqlBulkCopyOptions) =
            GetIdentityInsertOptions(identityInsert, tableName);

        var keyPropertiesString = GetColumnsStringSqlServer(keyProperties, columns);
        var keyPropertiesInsertedString = GetColumnsStringSqlServer(keyProperties, columns, "inserted.");
        var insertPropertiesString = GetColumnsStringSqlServer(insertProperties, columns);
        var allPropertiesString = GetColumnsStringSqlServer(allProperties, columns, "target.");

        var tempToBeInserted = $"#TempInsert_{tableName}".Replace(".", string.Empty);
        var tempInsertedWithIdentity = $"@TempInserted_{tableName}".Replace(".", string.Empty);

        connection.Execute(
            $"SELECT TOP 0 {insertPropertiesString} INTO {tempToBeInserted} FROM {FormatTableName(tableName)} target WITH(NOLOCK);",
            null, transaction);

        using (var bulkCopy = new SqlBulkCopy(connection, sqlBulkCopyOptions, transaction))
        {
            bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
            bulkCopy.BatchSize = batchSize;
            bulkCopy.DestinationTableName = tempToBeInserted;
            bulkCopy.WriteToServer(ToDataTable(data, insertProperties).CreateDataReader());
        }

        var table = string.Join(", ",
            keyProperties.Select(k => $"[{(columns.ContainsKey(k.Name) ? columns[k.Name] : k.Name)}] bigint"));
        var joinOn = string.Join(" AND ",
            keyProperties.Select(k =>
                $"target.[{(columns.ContainsKey(k.Name) ? columns[k.Name] : k.Name)}] = ins.[{(columns.ContainsKey(k.Name) ? columns[k.Name] : k.Name)}]"));


        //data.Select(s=>s.)

        connection.Query<T>($@"
                INSERT INTO {FormatTableName(tableName)}({insertPropertiesString}) 
                SELECT {insertPropertiesString} FROM {tempToBeInserted}
                DROP TABLE {tempToBeInserted};", null, transaction);
        return data;
    }

    /// <summary>
    /// Inserts entities into table <typeparamref name="T"/>s (by default) asynchronously.
    /// </summary>
    /// <typeparam name="T">The type being inserted.</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="data">Entities to insert</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="batchSize">Number of bulk items inserted together, 0 (the default) if all</param>
    /// <param name="bulkCopyTimeout">Number of seconds before bulk command execution timeout, 30 (the default)</param>
    /// <param name="identityInsert">Usage of db generated ids. By default DB generated IDs are used (identityInsert=false)</param>
    public static async Task BulkInsertAsync<T>(this SqlConnection connection, IEnumerable<T> data,
        SqlTransaction transaction = null, int batchSize = 0, int bulkCopyTimeout = 30, bool identityInsert = false)
    {
        var type = typeof(T);
        var tableName = TableMapper.GetTableName(type);
        var allProperties = PropertiesCache.TypePropertiesCache(type);
        var keyProperties = PropertiesCache.KeyPropertiesCache(type);
        var computedProperties = PropertiesCache.ComputedPropertiesCache(type);
        var columns = PropertiesCache.GetColumnNamesCache(type);

        var insertProperties = allProperties.Except(computedProperties).ToList();

        if (!identityInsert)
            insertProperties = insertProperties.Except(keyProperties).ToList();

        var (identityInsertOn, identityInsertOff, sqlBulkCopyOptions) =
            GetIdentityInsertOptions(identityInsert, tableName);

        var insertPropertiesString = GetColumnsStringSqlServer(insertProperties, columns);
        var tempToBeInserted = $"#TempInsert_{tableName}".Replace(".", string.Empty);

        await connection.ExecuteAsync(
            $@"SELECT TOP 0 {insertPropertiesString} INTO {tempToBeInserted} FROM {FormatTableName(tableName)} target WITH(NOLOCK);",
            null, transaction);

        using (var bulkCopy = new SqlBulkCopy(connection, sqlBulkCopyOptions, transaction))
        {
            bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
            bulkCopy.BatchSize = batchSize;
            bulkCopy.DestinationTableName = tempToBeInserted;
            await bulkCopy.WriteToServerAsync(ToDataTable(data, insertProperties).CreateDataReader());
        }

        await connection.ExecuteAsync($@"
                {identityInsertOn}
                INSERT INTO {FormatTableName(tableName)}({insertPropertiesString}) 
                SELECT {insertPropertiesString} FROM {tempToBeInserted}
                {identityInsertOff}

                DROP TABLE {tempToBeInserted};", null, transaction);
    }

    /// <summary>
    /// Inserts entities into table <typeparamref name="T"/>s (by default) asynchronously and returns inserted entities.
    /// </summary>
    /// <typeparam name="T">The type being inserted.</typeparam>
    /// <param name="connection">Open SqlConnection</param>
    /// <param name="data">Entities to insert</param>
    /// <param name="transaction">The transaction to run under, null (the default) if none</param>
    /// <param name="batchSize">Number of bulk items inserted together, 0 (the default) if all</param>
    /// <param name="bulkCopyTimeout">Number of seconds before bulk command execution timeout, 30 (the default)</param>
    /// <param name="identityInsert">Usage of db generated ids. By default DB generated IDs are used (identityInsert=false)</param>
    /// <returns>Inserted entities</returns>
    public static async Task<IEnumerable<T>> BulkInsertAndSelectAsync<T>(this SqlConnection connection,
        IEnumerable<T> data, SqlTransaction transaction = null, int batchSize = 0, int bulkCopyTimeout = 30,
        bool identityInsert = false)
    {
        var type = typeof(T);
        var tableName = TableMapper.GetTableName(type);
        var allProperties = PropertiesCache.TypePropertiesCache(type);
        var keyProperties = PropertiesCache.KeyPropertiesCache(type);
        var computedProperties = PropertiesCache.ComputedPropertiesCache(type);
        var columns = PropertiesCache.GetColumnNamesCache(type);

        if (keyProperties.Count == 0)
        {
            var dataList = data.ToList();
            await connection.BulkInsertAsync(dataList, transaction, batchSize, bulkCopyTimeout);
            return dataList;
        }

        var insertProperties = allProperties.Except(computedProperties).ToList();

        if (!identityInsert)
            insertProperties = insertProperties.Except(keyProperties).ToList();

        var (identityInsertOn, identityInsertOff, sqlBulkCopyOptions) =
            GetIdentityInsertOptions(identityInsert, tableName);

        var keyPropertiesString = GetColumnsStringSqlServer(keyProperties, columns);
        var keyPropertiesInsertedString = GetColumnsStringSqlServer(keyProperties, columns, "inserted.");
        var insertPropertiesString = GetColumnsStringSqlServer(insertProperties, columns);
        var allPropertiesString = GetColumnsStringSqlServer(allProperties, columns, "target.");

        var tempToBeInserted = $"#TempInsert_{tableName}".Replace(".", string.Empty);
        var tempInsertedWithIdentity = $"@TempInserted_{tableName}".Replace(".", string.Empty);

        await connection.ExecuteAsync(
            $@"SELECT TOP 0 {insertPropertiesString} INTO {tempToBeInserted} FROM {FormatTableName(tableName)} target WITH(NOLOCK);",
            null, transaction);

        using (var bulkCopy = new SqlBulkCopy(connection, sqlBulkCopyOptions, transaction))
        {
            bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
            bulkCopy.BatchSize = batchSize;
            bulkCopy.DestinationTableName = tempToBeInserted;
            await bulkCopy.WriteToServerAsync(ToDataTable(data, insertProperties).CreateDataReader());
        }

        var table = string.Join(", ",
            keyProperties.Select(k => $"[{(columns.ContainsKey(k.Name) ? columns[k.Name] : k.Name)}] bigint"));
        var joinOn = string.Join(" AND ",
            keyProperties.Select(k =>
                $"target.[{(columns.ContainsKey(k.Name) ? columns[k.Name] : k.Name)}] = ins.[{(columns.ContainsKey(k.Name) ? columns[k.Name] : k.Name)}]"));
        return await connection.QueryAsync<T>($@"
                {identityInsertOn}
                DECLARE {tempInsertedWithIdentity} TABLE ({table})
                INSERT INTO {FormatTableName(tableName)}({insertPropertiesString}) 
                OUTPUT {keyPropertiesInsertedString} INTO {tempInsertedWithIdentity} ({keyPropertiesString})
                SELECT {insertPropertiesString} FROM {tempToBeInserted}
                {identityInsertOff}
                SELECT {allPropertiesString}
                FROM {FormatTableName(tableName)} target INNER JOIN {tempInsertedWithIdentity} ins ON {joinOn}

                DROP TABLE {tempToBeInserted};", null, transaction);
    }

    public static async Task<string> CreateTable<T>(this SqlConnection connection, T data,
        SqlTransaction transaction = null, int batchSize = 0, int bulkCopyTimeout = 30, bool identityInsert = false)
    {
        var type = typeof(T);
        var tableName = TableMapper.GetTableName(type);
        var allProperties = PropertiesCache.TypePropertiesCache(type);
        var keyProperties = PropertiesCache.KeyPropertiesCache(type);
        var computedProperties = PropertiesCache.ComputedPropertiesCache(type);
        var columns = PropertiesCache.GetColumnNamesCache(type);

        return tableName;
    }

    public static TConvert ConvertTo<TConvert>(this object entity) where TConvert : new()
    {
        var convertProperties = TypeDescriptor.GetProperties(typeof(TConvert)).Cast<PropertyDescriptor>();
        var entityProperties = TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>();

        var convert = new TConvert();

        foreach (var entityProperty in entityProperties)
        {
            var property = entityProperty;
            var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == property.Name);
            if (convertProperty != null)
            {
                convertProperty.SetValue(convert,
                    Convert.ChangeType(entityProperty.GetValue(entity), convertProperty.PropertyType));
            }
        }

        return convert;
    }

    private static string GetColumnsStringSqlServer(IEnumerable<PropertyInfo> properties,
        IReadOnlyDictionary<string, string> columnNames, string tablePrefix = null)
    {
        if (tablePrefix == "target.")
        {
            return string.Join(", ",
                properties.Select(property => $"{tablePrefix}[{columnNames[property.Name]}] as [{property.Name}] "));
        }

        return string.Join(", ", properties.Select(property => $"{tablePrefix}[{columnNames[property.Name]}] "));
    }

    private static DataTable ToDataTable<T>(IEnumerable<T> data, IList<PropertyInfo> properties)
    {
        var typeCasts = new Type[properties.Count];
        for (var i = 0; i < properties.Count; i++)
        {
            if (properties[i].PropertyType.IsEnum)
            {
                typeCasts[i] = Enum.GetUnderlyingType(properties[i].PropertyType);
            }
            else
            {
                typeCasts[i] = null;
            }
        }

        var dataTable = new DataTable();
        for (var i = 0; i < properties.Count; i++)
        {
            // Nullable types are not supported.
            var propertyNonNullType =
                Nullable.GetUnderlyingType(properties[i].PropertyType) ?? properties[i].PropertyType;
            dataTable.Columns.Add(properties[i].Name, typeCasts[i] ?? propertyNonNullType);
        }

        foreach (var item in data)
        {
            var values = new object[properties.Count];
            for (var i = 0; i < properties.Count; i++)
            {
                var value = properties[i].GetValue(item, null);
                values[i] = typeCasts[i] == null ? value : Convert.ChangeType(value, typeCasts[i]);
            }

            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

    internal static string FormatTableName(string table)
    {
        if (string.IsNullOrEmpty(table))
        {
            return table;
        }

        var parts = table.Split('.');

        if (parts.Length == 1)
        {
            return $"[{table}]";
        }

        var tableName = "";
        for (int i = 0; i < parts.Length; i++)
        {
            tableName += $"[{parts[i]}]";
            if (i + 1 < parts.Length)
            {
                tableName += ".";
            }
        }

        return tableName;
    }


    public static IEnumerable<string> GetColumns<T>()
    {
        var type = typeof(T);
        var tableName = TableMapper.GetTableName(type);
        var columns = PropertiesCache.GetColumnNamesCache(type).Keys;

        return columns;
    }

    public static string GetColumns<T>(T data)
    {
        var type = typeof(T);
        var tableName = TableMapper.GetTableName(type);
        var allProperties = PropertiesCache.TypePropertiesCache(type);
        var columns = PropertiesCache.GetColumnNamesCache(type);

        List<PropertyInfo> insertProperties;
        insertProperties = allProperties;
        //  IEnumerable<T> data, IList<PropertyInfo > propertie;
        var typeCasts = new Type?[insertProperties.Count];
        for (var i = 0; i < insertProperties.Count; i++)
        {
            if (insertProperties[i].PropertyType.IsEnum)
            {
                typeCasts[i] = Enum.GetUnderlyingType(insertProperties[i].PropertyType);
            }
            else
            {
                typeCasts[i] = null;
            }
        }

        var values = new object[columns.Count];
        var propertys = new object[columns.Count];
        var columnsString = "Select ";
        var where = " where ";
        for (var i = 0; i < columns.Count; i++)
        {
            var property = insertProperties[i].Name;
            var value = insertProperties[i].GetValue(data, null);
            if (value != null)
            {
                Type? typeX = insertProperties[i].GetValue(data, null)?.GetType();
                if (typeX == typeof(DateTime))
                {
                    continue;
                }

                if (typeX == typeof(String))
                {
                    columnsString += property + ",";
                    where += "" + property + " like \'%" + value + "%\' AND ";
                    propertys[i] = property;
                    values[i] = value;
                }
            }

            //values[i] = insertProperties[i] == null ? value : Convert.ChangeType(value, typeCasts[i]);
        }


        // dataTable.Rows.Add(values);
        var s = "";
        if (where.Length > 7)
        {
            //where.Remove(columnsString.Length - 4);
            s = columnsString.Remove(columnsString.Length - 1) + " From " + tableName +
                where.Substring(0, where.Length - 4);
            ;
        }
        else
        {
            s = columnsString.Remove(columnsString.Length - 1) + " * From " + tableName;
        }
        //where.Remove(where.Length-1);

        return s;
    }

    private static (string identityInsertOn, string identityInsertOff, SqlBulkCopyOptions bulkCopyOptions)
        GetIdentityInsertOptions(bool identityInsert, string tableName)
        => identityInsert
            ? ($"SET IDENTITY_INSERT {FormatTableName(tableName)} ON",
                $"SET IDENTITY_INSERT {FormatTableName(tableName)} OFF", SqlBulkCopyOptions.KeepIdentity)
            : (string.Empty, string.Empty, SqlBulkCopyOptions.Default);
}