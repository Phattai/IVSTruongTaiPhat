using Core.Common;
using DTO.Product;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Common.CommonMethod;

namespace DAL.Dao.Product
{
    public class CategoryDAO
    {
        private static string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        public static CommonData.ReturnCode SearchData(CategoryDTO inputDto, out List<CategoryDTO> list)
        {
            list = new List<CategoryDTO>();
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = "SELECT c.`id`, c.`parent_id`, c.`code`, c.`name`, cpp.`name` AS parent_name, c.`description` FROM `product_category` c " +
                                 " LEFT JOIN (SELECT cp.`name`,  cp.`id` FROM product_category cp ) cpp on cpp.`id` = c.`parent_id` WHERE TRUE ";
                    #region Clause
                    if (inputDto.id != null)
                    {
                        sql += " AND c.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", inputDto.id);
                    }

                    if (inputDto.parent_id != null)
                    {
                        sql += " AND c.`parent_id` = @Parent_ID ";
                        cmd.Parameters.AddWithValue("@Parent_ID", inputDto.parent_id);
                    }

                    if (inputDto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", inputDto.code);
                    }

                    if (inputDto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", inputDto.name);
                    }
                    #endregion

                    sql += " ORDER BY c.`updated_datetime` DESC";
                    sql += " LIMIT  @start,20";
                    if (inputDto.page > 1)
                    {
                        cmd.Parameters.AddWithValue("@start", 20 * (inputDto.page - 1));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@start", 0);
                    }

                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            foreach (DataRow category in dt.Rows)
                            {
                                CategoryDTO dto = new CategoryDTO();

                                dto.id = int.Parse(category["id"].ToString());
                                dto.parent_id = int.Parse(category["parent_id"].ToString());
                                dto.parent_name = category["parent_name"].ToString();
                                dto.code = category["code"].ToString();
                                dto.name = category["name"].ToString();
                                dto.decription = category["description"].ToString();

                                list.Add(dto);
                            }
                            returnCode = 0;
                        }
                        else
                        {
                            returnCode = CommonData.ReturnCode.UnSuccess;
                        }
                    }
                }
            }
            return returnCode;
        }

        public static CommonData.ReturnCode SelectSimpleData(out List<CategoryDetailDTO> list)
        {
            list = new List<CategoryDetailDTO>();

            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT id, name FROM product_category", con))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            foreach (DataRow category in dt.Rows)
                            {
                                CategoryDetailDTO dto = new CategoryDetailDTO();

                                dto.id = int.Parse(category["id"].ToString());
                                dto.name = category["name"].ToString();

                                list.Add(dto);
                            }
                        }
                    }
                }
            }
            return returnCode;
        }

        public static CommonData.ReturnCode InsertData(CategoryDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"INSERT INTO product_category(`code`, `parent_id`, `name`,
                                                    `description`, `created_datetime`, `created_by`,
                                                    `updated_datetime`, `updated_by`)
                                                    VALUES 
                                                    (@code, @parentID, @name, @description,
                                                     SYSDATE(), @created_by,
                                                     SYSDATE(), @updated_by)", connect))
                {
                    command.Parameters.AddWithValue("@code", dto.code);
                    command.Parameters.AddWithValue("@parentID", dto.parent_id);
                    command.Parameters.AddWithValue("@name", dto.name);
                    command.Parameters.AddWithValue("@description", dto.decription);
                    command.Parameters.AddWithValue("@created_by", dto.created_by);
                    command.Parameters.AddWithValue("@updated_by", dto.updated_by);

                    connect.Open();
                    command.ExecuteNonQuery();
                }
            }
            return returnCode;
        }

        public static CommonData.ReturnCode UpdateData(CategoryDTO dto)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"UPDATE product_category pc SET pc.`code` = @code,
							pc.`parent_id` = @parentID,
                            pc.`name` = @name,
                            pc.`description` = @description,
                            pc.`updated_datetime` = SYSDATE(),
                            pc.`updated_by` = @updated_by WHERE pc.`id` = @ID", connect))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@code", dto.code);
                        command.Parameters.AddWithValue("@parentID", dto.parent_id);
                        command.Parameters.AddWithValue("@name", dto.name);
                        command.Parameters.AddWithValue("@description", dto.decription);
                        command.Parameters.AddWithValue("@updated_by", dto.updated_by);
                        command.Parameters.AddWithValue("@ID", dto.id);

                        connect.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        returnCode = CommonData.ReturnCode.UnSuccess;
                    }
                }
            }
            return returnCode;
        }

        public static CommonData.ReturnCode DeleteData (int id)
        {
            CommonData.ReturnCode returnCode = CommonData.ReturnCode.Success;
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (MySqlConnection connect = new MySqlConnection(constr))
            {
                using (MySqlCommand command = new MySqlCommand(
                    @"DELETE FROM product_category WHERE id = @ID", connect))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        connect.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        returnCode = CommonData.ReturnCode.UnSuccess;
                    }
                }
            }
            return returnCode;
        }

        public static int CountData(CategoryDTO dto)
        {
            int count = 0;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = " SELECT COUNT(*) FROM product_category c WHERE TRUE ";

                    #region Where Clause
                    if (dto.id != null)
                    {
                        sql += " AND c.`id` = @ID ";
                        cmd.Parameters.AddWithValue("@ID", dto.id);
                    }

                    if (dto.parent_id != null)
                    {
                        sql += " AND c.`parent_id` = @Parent_ID ";
                        cmd.Parameters.AddWithValue("@Parent_ID", dto.parent_id);
                    }

                    if (dto.code.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`code` LIKE CONCAT('%',@Code,'%') ";
                        cmd.Parameters.AddWithValue("@Code", dto.code);
                    }

                    if (dto.name.IsNotNullOrEmpty())
                    {
                        sql += " AND c.`name` LIKE CONCAT('%',@Name,'%') ";
                        cmd.Parameters.AddWithValue("@Name", dto.name);
                    }
                    #endregion

                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    count = int.Parse(cmd.ExecuteScalar().ToString());
                }
            }

            return count;
        }
    }
}
