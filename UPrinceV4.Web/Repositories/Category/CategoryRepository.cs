using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.Category;
using UPrinceV4.Web.Repositories.Interfaces.Category;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.Category;

public class CategoryRepository : ICategoryRepository
{
    public async Task<IEnumerable<CategoryLevelCreateDto>> GetCategory(CategoryParameter categoryParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(categoryParameter.ContractingUnitSequenceId,
            "ProjectTest", categoryParameter.TenantProvider);

        IEnumerable<CategoryLevelCreateDto> result = null;
        if (categoryParameter.CategoryDto.ParentId != null)
        {
            var sql = @"SELECT * FROM CategoryLevel WHERE ParentId = @ParentId";
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<CategoryLevelCreateDto>(sql,
                    new { categoryParameter.CategoryDto.ParentId });
            }
        }
        else
        {
            var sql = @"SELECT * FROM CategoryLevel WHERE ParentId IS NULL";
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<CategoryLevelCreateDto>(sql);
            }
        }

        foreach (var cl in result)
        {
            CategoryLevelCreateDto data;
            var selectSql = @"SELECT * FROM CategoryLevel WHERE ParentId = @Id";

            await using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<CategoryLevelCreateDto>(selectSql, new { cl.Id }).FirstOrDefault();
            }

            cl.hasChildren = data != null ? "1" : "0";
        }

        return result;
    }

    public async Task<string> CreateCategory(CategoryParameter categoryParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(categoryParameter.ContractingUnitSequenceId,
            "ProjectTest", categoryParameter.TenantProvider);
        var sql =
            @"INSERT INTO CategoryLevel VALUES (@Id, @Name, @LevelId, @LanguageCode, @DisplayOrder, @IsChildren, @Image, @ParentId)";
        if (categoryParameter.CategoryDto.Id == null)
        {
            var param = new
            {
                Id = Guid.NewGuid().ToString(),
                categoryParameter.CategoryDto.Name,
                categoryParameter.CategoryDto.LevelId,
                LanguageCode = categoryParameter.Lang,
                categoryParameter.CategoryDto.DisplayOrder,
                categoryParameter.CategoryDto.IsChildren,
                categoryParameter.CategoryDto.Image,
                categoryParameter.CategoryDto.ParentId
            };

            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, param);
            }
        }
        else
        {
            var updateSql =
                @"UPDATE CategoryLevel SET Name = @Name , DisplayOrder = @DisplayOrder , Image = @Image WHERE Id = @Id";

            var param = new
            {
                categoryParameter.CategoryDto.Id,
                categoryParameter.CategoryDto.Name,
                categoryParameter.CategoryDto.DisplayOrder,
                categoryParameter.CategoryDto.Image
            };
            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(updateSql, param);
            }
        }

        return null;
    }

    public async Task<string> CreatePost(CategoryParameter categoryParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            categoryParameter.ContractingUnitSequenceId, "ProjectTest", categoryParameter.TenantProvider);

        Pictures docData;
        string newId = null;


        if (categoryParameter.PostDto.Id == null)
        {
            var insertQuery =
                @"INSERT INTO Post ([Id], [Title], [PostType], [Description], [Address], [Location], [FishDetailtype]) VALUES (@Id, @Title, @PostType, @Description, @Address, @Location, @FishDetailtype)";
            newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                categoryParameter.PostDto.Title,
                categoryParameter.PostDto.PostType,
                categoryParameter.PostDto.Description,
                categoryParameter.PostDto.Address,
                categoryParameter.PostDto.Location,
                categoryParameter.PostDto.FishDetailtype
            };

            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(insertQuery, parameters);
            }

            if (categoryParameter.PostDto.Pictures != null)
                if (categoryParameter.PostDto.Pictures.FirstOrDefault() != null)
                    foreach (var doc in categoryParameter.PostDto.Pictures)
                    {
                        var docInsert =
                            @"INSERT INTO PostPictures ([Id], [Link], [PostId]) VALUES (@Id, @Link, @PostId)";

                        var param = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            Link = doc,
                            PostId = newId
                        };

                        await using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.ExecuteAsync(docInsert, param);
                        }
                    }
        }
        else
        {
            var updateQuery =
                @"UPDATE Post SET  Title = @Title, PostType = @PostType, Description = @Description, Address = @Address, Location = @Location, FishDetailtype = @FishDetailtype WHERE Id = @Id";

            var parameters = new
            {
                categoryParameter.PostDto.Id,
                categoryParameter.PostDto.Title,
                categoryParameter.PostDto.PostType,
                categoryParameter.PostDto.Description,
                categoryParameter.PostDto.Address,
                categoryParameter.PostDto.Location,
                categoryParameter.PostDto.FishDetailtype
            };

            await using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(updateQuery, parameters);
                docData = connection.Query<Pictures>("SELECT * FROM PostPictures WHERE PostId =@PostId",
                    new { PostId = categoryParameter.PostDto.Id }).FirstOrDefault();
            }

            if (categoryParameter.PostDto.Pictures != null)
                if (categoryParameter.PostDto.Pictures.FirstOrDefault() != null)
                {
                    if (docData == null)
                        foreach (var doc in categoryParameter.PostDto.Pictures)
                        {
                            var docInsert =
                                @"INSERT INTO PostPictures ([Id], [Link], [PostId]) VALUES (@Id, @Link, @PostId)";

                            var param = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                Link = doc,
                                PostId = categoryParameter.PostDto.Id
                            };

                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Execute(docInsert, param);
                            }
                        }
                    else
                        foreach (var doc in categoryParameter.PostDto.Pictures)
                        {
                            var docInsert =
                                @"INSERT INTO PostPictures ([Id], [Link], [PostId]) VALUES (@Id, @Link, @PostId)";

                            var param = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                Link = doc,
                                PostId = categoryParameter.PostDto.Id
                            };

                            string docLink;

                            using (var connection = new SqlConnection(connectionString))
                            {
                                docLink = connection
                                    .Query<string>("SELECT Id FROM PostPictures WHERE Link =@doc",
                                        new { doc }).FirstOrDefault();
                            }

                            if (docLink == null)
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    connection.Execute(docInsert, param);
                                }
                        }
                }
        }


        return newId;
    }

    public async Task<PostDto> GetPostById(CategoryParameter categoryParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(categoryParameter.ContractingUnitSequenceId,
            "ProjectTest", categoryParameter.TenantProvider);

        PostDto data;
        List<string> doc;

        await using (var connection = new SqlConnection(connectionString))
        {
            data = connection
                .Query<PostDto>("SELECT * FROM Post WHERE Id =@Id", new { categoryParameter.Id })
                .FirstOrDefault();
        }

        if (data != null)
        {
            var documents = @"SELECT Link FROM dbo.PostPictures WHERE PostId =@Id";
            await using (var connection = new SqlConnection(connectionString))
            {
                doc = connection.Query<string>(documents, new { categoryParameter.Id }).ToList();
            }


            if (doc.FirstOrDefault() != null) data.Pictures = doc;
        }

        return data;
    }
}