using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using CrudOperation.Models;

namespace CrudOperation.Controllers
{
    public class ProductController : Controller
    {
        private static JsonResult JsonResult;
        private static string ConnectionString;

        List<Demo> list = new List<Demo>();
        Demo demo = new Demo();
        public ProductController()
        {
            GetAppSettingsFile();
        }
        public static void GetAppSettingsFile()
        {
            IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();
            ConnectionString = configuration["ConnectionStrings:ConnString"];
        }
        public IActionResult Index()
        {
            list = GetList(-1);
            return View(list);
        }
        public IActionResult AddEdit(Int32 id)
        {
            list = GetList(id);
            if (list.Count > 0)
                demo = list[0];
            return View(demo);
        }
        public List<Demo> GetList(Int32 id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetProduct", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@ID", id);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Demo demo = new Demo
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                Name = reader["ProductName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                StatusID = Convert.ToInt32(reader["StatusID"])
                            };
                            list.Add(demo);
                        }
                    }
                }
            }
            return list;
        }

        public JsonResult Save(IFormCollection objFrom)
        {
            Int32 ID = Convert.ToInt32(objFrom["id"]);
            string Name = objFrom["name"];
            string Category = objFrom["selectCategory"];
            string SPName = "SaveProduct";
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SPName, sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ID", ID);
                    sqlCommand.Parameters.AddWithValue("@Name", Name);
                    sqlCommand.Parameters.AddWithValue("@Category", Category);
                    sqlCommand.ExecuteNonQuery();

                }
            }
            return Json(new { success = true, massage = "User saved successfully!" });
        }
        public JsonResult Delete(Int32 id, int StatusId)
        {
            if (StatusId == 1)
                StatusId = 0;
            else
                StatusId = 1;

            string Query = "update Product set statusid = @StatusId where id = @ID";
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(Query, sqlConnection);
            sqlConnection.Open();
            sqlCommand.Parameters.AddWithValue("@ID", id);
            sqlCommand.Parameters.AddWithValue("@StatusId", StatusId);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Json(new { success = true, massage = "Deleted SuccessFully!" });
        }

        public JsonResult GetCategory()
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetCategoty", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Demo demo = new Demo
                            {
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                CategoryName = reader["CategoryName"].ToString(),
                            };
                            list.Add(demo);
                        }
                    }
                }
            }
            return Json(new { success = true, data = list });
        }

        public IActionResult ProductList(int pageNumber = 1, int pageSize = 10)
        {
            List<Demo> list = new List<Demo>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetProductList", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@PageNumber", pageNumber);
                    sqlCommand.Parameters.AddWithValue("@PageSize", pageSize);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Demo demo = new Demo
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                Name = reader["ProductName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString()
                            };
                            list.Add(demo);
                        }
                    }
                    sqlConnection.Close();
                }

                // Calculate the total number of items for pagination
                int totalItems = 0;
                sqlConnection.Open();
                using (SqlCommand countCommand = new SqlCommand("GetProductCount", sqlConnection))
                    {
                        countCommand.CommandType = CommandType.StoredProcedure;
                        totalItems = (int)countCommand.ExecuteScalar();
                    }
                

                int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // Pass pagination data to the view
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalItems = totalItems;
            }

            return View(list);
        }

        //public IActionResult ProductList()
        //{
        //    using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        //    {
        //        using (SqlCommand sqlCommand = new SqlCommand("GetProductList", sqlConnection))
        //        {
        //            sqlCommand.CommandType = CommandType.StoredProcedure;
        //            sqlConnection.Open();
        //            SqlDataReader reader = sqlCommand.ExecuteReader();
        //            if (reader != null)
        //            {
        //                while (reader.Read())
        //                {
        //                    Demo demo = new Demo
        //                    {
        //                        ID = Convert.ToInt32(reader["ID"]),
        //                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
        //                        Name = reader["ProductName"].ToString(),
        //                        CategoryName = reader["CategoryName"].ToString()
        //                    };
        //                    list.Add(demo);
        //                }
        //            }
        //        }
        //    }

        //    return View(list); 
        //}
    }

}
